using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading;
using System.Threading.Tasks;
using DataAccess.DataAccess;
using DataAccess.Helper;
using DataAccess.ViewModels;
using Newtonsoft.Json;
using Quartz;
using ServiceReference1;

namespace HLD.WebApi.Jobs
{
    [DisallowConcurrentExecution]
    public class QuartzJob : IJob
    {
        IConnectionString _connectionString = null;
        ServiceReference1.AuthHeader authHeader = null;
        SellerCloudOrderDataAccess _sellerCloudOrderDataAccess = null;
        BestBuyOrderDataAccess _bestBuyOrderDataAccess = null;
        BestBuyProductDataAccess _bestBuyProductDataAccess = null;
        EncDecChannel _EncDecChannel = null;
        GetChannelCredViewModel _getChannelCredViewModel = null;
        public QuartzJob(IConnectionString connectionString)
        {

            _connectionString = connectionString;
            _sellerCloudOrderDataAccess = new SellerCloudOrderDataAccess(_connectionString);
            _bestBuyOrderDataAccess = new BestBuyOrderDataAccess(_connectionString);
            _bestBuyProductDataAccess = new BestBuyProductDataAccess(_connectionString);
            _EncDecChannel = new EncDecChannel(_connectionString);

        }

        public ImportDataFromBestBuy.BestBuyRootObject GetBestBuyOrdersAgainstIDs(string apiurl, string token, List<string> OrderIds)
        {
            string orderIdsTobeSend = String.Join(",", OrderIds);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(apiurl + orderIdsTobeSend);
            request.Method = "GET";
            request.Accept = "application/json;";
            request.ContentType = "application/json";
            request.Headers["Authorization"] = token;

            string strResponse = "";
            using (WebResponse webResponse = request.GetResponse())
            {
                using (StreamReader stream = new StreamReader(webResponse.GetResponseStream()))
                {
                    strResponse = stream.ReadToEnd();
                }
            }

            ImportDataFromBestBuy.BestBuyRootObject responses = JsonConvert.DeserializeObject<ImportDataFromBestBuy.BestBuyRootObject>(strResponse);
            return responses;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            TimeSpan time = context.JobRunTime;
            string text = "";

            try
            {
                _getChannelCredViewModel = new GetChannelCredViewModel();
                _getChannelCredViewModel = _EncDecChannel.DecryptedData("sellercloud");
                authHeader = new AuthHeader();
                authHeader.ValidateDeviceID = false;
                authHeader.UserName = _getChannelCredViewModel.UserName;
                authHeader.Password = _getChannelCredViewModel.Key;
                ServiceReference1.SCServiceSoapClient sCServiceSoap =
                       new ServiceReference1.SCServiceSoapClient(ServiceReference1.SCServiceSoapClient.EndpointConfiguration.SCServiceSoap12);

                //HttpRequestMessageProperty requestMessagerr = new HttpRequestMessageProperty();
                //requestMessagerr.Headers["Content-Type"] = "application/soap+xml; charset=utf-8";

                List<string> bbOrderID = null;
                ServiceReference1.UpdateOrderDropShipStatusRequest request = new UpdateOrderDropShipStatusRequest(authHeader, null, 2345, DropShipStatusType.Requested);

                TimeSpan time1 = TimeSpan.FromHours(1); // my attempt to add 2 hours
                TimeSpan ts = DateTime.Now.TimeOfDay;
                var newTs = ts.Add(time1);
                sCServiceSoap.InnerChannel.OperationTimeout = newTs;
                ServiceOptions serviceOptions = new ServiceOptions();

                serviceOptions.AlwaysRecalculateWeight = false;
                serviceOptions.BulkDeleteShadows = false;
                serviceOptions.BulkWipeRelationships = false;
                serviceOptions.DontIncludePORMAImages = false;
                serviceOptions.FetchUserDefinedColumnsForOrder = false;
                serviceOptions.IncludeClientUserDocuments = false;
                serviceOptions.IncludePODetails = false;
                serviceOptions.SaveOrderPackageDimensions = false;
                serviceOptions.SkipBundleItemQtyUpdating = false;
                serviceOptions.UseCache = true;
                serviceOptions.DontNeedCompanyProfile = false;
                serviceOptions.FetchUserDefinedColumnsForProducts = false;
                serviceOptions.IncludeShippingSuggestions = false;
                serviceOptions.SkipCWAShippingRules = false;
                serviceOptions.AllowAnyProductShippingMethods = false;

                List<string> keys = new List<string>();
                List<string> values = new List<string>();
                //DateTime fromDate = new DateTime(2019, 5,15);
                //DateTime toDate = new DateTime(2019, 5, 16);
                keys.Add("Company");
                values.Add("513");

                //keys.Add("DateTo");
                //values.Add(toDate.ToBinary().ToString());

                SerializableDictionaryOfStringString filters = new SerializableDictionaryOfStringString();
                filters.Keys = keys.ToArray();
                filters.Values = values.ToArray();

                //get all those order from seller cloud which are unshipped and inprocess

                //using (OperationContextScope scope = new OperationContextScope(sCServiceSoap.InnerChannel))
                //{
                //    MessageHeader<string> header = new MessageHeader<string>("MyHttpHeaderValue");
                //    var untyped = header.GetUntypedHeader("Content-Type", "application/soap+xml; charset=utf-8");

                //    HttpRequestMessageProperty requestMessagerr = new HttpRequestMessageProperty();
                //    requestMessagerr.Headers["Content-Type"] = "application/soap+xml; charset=utf-8";

                //    OperationContext.Current.OutgoingMessageProperties[HttpResponseMessageProperty.Name]= requestMessagerr;

                //}

                var orderIds = await sCServiceSoap.Orders_GetAsync(authHeader, null, filters);


                string OrderIdToCheckInDatabase = string.Join(',', orderIds.Orders_GetResult);

                //check sellre cloud orders which exists in database
                List<int> orderWhichAreUnshippedDataExist = _sellerCloudOrderDataAccess.GetSellerCloudOrderWhichAreExists(OrderIdToCheckInDatabase);



                //get distinct orders and those which are not exist in database
                List<int> distinctOrders = orderIds.Orders_GetResult.Except(orderWhichAreUnshippedDataExist).ToList();

                //orderIds.Orders_GetResult.ToArray())    new int[] { 6167199 }
                List<SellerCloudOrder_CustomerViewModel> _mainOrderDetailCustomerList = new List<SellerCloudOrder_CustomerViewModel>();

                var ordersDetail = await sCServiceSoap.Orders_GetDatasAsync(authHeader, null, distinctOrders.ToArray());


                List<ImagesClass> imagesList = new List<ImagesClass>();

                #region commented code for download all images
                // commented code for download all images

                //List<int> sellerCloudOrderIdsToImportImages = sellerCloudApiAccess.GetSellerCloudOrderIdsForImageImport(ApiURL, token);
                // var ordersDetailofImages = await sCServiceSoap.Orders_GetDatasAsync(authHeader, null, sellerCloudOrderIdsToImportImages.ToArray());
                //foreach (var item in ordersDetailofImages.Orders_GetDatasResult)
                //{

                //    SerializableDictionaryOfStringString stringString = item.GalleryImagesURL;
                //    List<string> tempKeys = stringString.Keys.ToList();
                //    List<string> tempValues = stringString.Values.ToList();
                //    for (int i = 0; i < tempKeys.Count; i++)
                //    {
                //        ImagesClass images = new ImagesClass();
                //        images.Key = tempKeys[i].ToString();
                //        images.Value = tempValues[i].ToString();
                //        imagesList.Add(images);
                //    }
                //}

                #endregion

                //Prepare complete order and order detail object
                foreach (var item in ordersDetail.Orders_GetDatasResult)
                {
                    SellerCloudOrderViewModel sellerCloudOrder = new SellerCloudOrderViewModel();
                    SellerCloudCustomerDetail sellerCloudCustomer = new SellerCloudCustomerDetail();
                    SellerCloudOrder_CustomerViewModel order_orderDetail_customer = new SellerCloudOrder_CustomerViewModel();
                    List<SellerCloudOrderDetailViewModel> sellerCloudOrderDetailList = new List<SellerCloudOrderDetailViewModel>();


                    //SerializableDictionaryOfStringString stringString = item.GalleryImagesURL;
                    //List<string> tempKeys = stringString.Keys.ToList();
                    //List<string> tempValues = stringString.Values.ToList();
                    //for (int i = 0; i < tempKeys.Count; i++)
                    //{
                    //    ImagesClass images = new ImagesClass();
                    //    images.Key = tempKeys[i].ToString();
                    //    images.Value = tempValues[i].ToString();
                    //    imagesList.Add(images);
                    //}

                    sellerCloudOrder.totalCount = item.Order.ItemCount;
                    sellerCloudOrder.dropShipStatus = System.Enum.GetName(typeof(DropShipStatusType2), item.Order.DropShipStatus);
                    sellerCloudOrder.currencyRateFromUSD = item.Order.CurrencyRateFromUSD;
                    sellerCloudOrder.lastUpdate = item.Order.LastUpdated;
                    sellerCloudOrder.timeOfOrder = item.Order.TimeOfOrder;
                    sellerCloudOrder.taxTotal = item.Order.TaxTotal;

                    sellerCloudOrder.shippingStatus = System.Enum.GetName(typeof(OrderShippingStatus2), item.Order.ShippingStatus);
                    sellerCloudOrder.shippingWeightTotalOz = item.Order.ShippingWeightTotalOz;
                    sellerCloudOrder.orderCurrencyCode = System.Enum.GetName(typeof(CurrencyCodeType2), item.Order.OrderCurrencyCode);
                    sellerCloudOrder.orderSourceOrderId = item.Order.OrderSourceOrderId;
                    sellerCloudOrder.paymentDate = item.Order.PaymentDate;//Order Date in our case
                    sellerCloudOrder.sellerCloudID = item.Order.ID; //seller cloud order id
                    sellerCloudOrder.ClientID = item.Order.ClientId;


                    Address address = item.Order.BillingAddress;


                    sellerCloudCustomer.countryName = address.CountryName;
                    sellerCloudCustomer.firstName = address.FirstName;
                    sellerCloudCustomer.lastName = address.LastName;
                    sellerCloudCustomer.phoneNumber = address.PhoneNumber;
                    sellerCloudCustomer.postalCode = address.PostalCode;
                    sellerCloudCustomer.stateCode = address.StateCode;
                    sellerCloudCustomer.stateName = address.StateName;
                    sellerCloudCustomer.streetLine1 = address.StreetLine1;
                    if (sellerCloudCustomer.streetLine1.ToLower().Contains("Box")) {
                        sellerCloudCustomer.IsBox = 1;
                    }
                    sellerCloudCustomer.streetLine2 = address.StreetLine2;
                    sellerCloudCustomer.city = address.City;



                    foreach (var itemDetail in item.Order.Items)
                    {
                        SellerCloudOrderDetailViewModel sellerCloudOrderDetail = new SellerCloudOrderDetailViewModel();
                        sellerCloudOrderDetail.DropShippedOn = itemDetail.DropShippedOn;
                        sellerCloudOrderDetail.DropShippedStatus = System.Enum.GetName(typeof(DropShipStatusType), itemDetail.DropShippedStatus);
                        sellerCloudOrderDetail.OrderId = itemDetail.OrderID;
                        sellerCloudOrderDetail.MinQTY = itemDetail.MinimumQty;
                        sellerCloudOrderDetail.SKU = itemDetail.ProductID;
                        //change order status from string to int 1.8.2019 
                        //sellerCloudOrderDetail.StatusCode = System.Enum.GetName(typeof(OrderItemStatusCode), itemDetail.StatusCode);
                        sellerCloudOrderDetail.StatusCode = "5";
                        sellerCloudOrderDetail.Qty = itemDetail.Qty;
                        sellerCloudOrderDetail.ProductTitle = itemDetail.DisplayName;
                        sellerCloudOrderDetail.AdjustedSitePrice = itemDetail.AdjustedSitePrice;
                        sellerCloudOrderDetail.AverageCost = itemDetail.AverageCost;
                        sellerCloudOrderDetail.PricePerCase = itemDetail.PricePerCase;
                        sellerCloudOrderDetail.unitPrice = itemDetail.UnitPrice;
                        sellerCloudOrderDetail.UPC = itemDetail.UPC;

                        sellerCloudOrderDetailList.Add(sellerCloudOrderDetail);
                    }

                    //assign object to main object
                    order_orderDetail_customer.Customer = sellerCloudCustomer;
                    order_orderDetail_customer.Order = sellerCloudOrder;
                    order_orderDetail_customer.orderDetail = sellerCloudOrderDetailList;

                    //main object list
                    _mainOrderDetailCustomerList.Add(order_orderDetail_customer);
                }

                // add data into seller cloud tables like order ,order detail ,customer detail 
                List<ImagesSaveToDatabaseWithURLViewMOdel> listImagesUrl = new List<ImagesSaveToDatabaseWithURLViewMOdel>();



                if (_mainOrderDetailCustomerList.Count > 0)
                {
                    _sellerCloudOrderDataAccess.SaveOrderAndCustomerDetail(_mainOrderDetailCustomerList);
                    // save seller cloud order images to prorduct_images table    
                    //commented code for save images in seller cloud
                    //foreach (var item in imagesList)
                    //{
                    //    ImagesSaveToDatabaseWithURLViewMOdel databaseImagesURL = new ImagesSaveToDatabaseWithURLViewMOdel();
                    //    try
                    //    {


                    //        Image img = DownloadImageFromUrl(item.Value);
                    //        if (img != null)
                    //        {
                    //            string fileName = Guid.NewGuid().ToString() + "-" + item.Key + Path.GetExtension(item.Value);
                    //            await uploadFiles.uploadToS3(GetStream(img, ImageFormat.Jpeg), fileName);                                
                    //            databaseImagesURL.product_Sku = item.Key;
                    //            databaseImagesURL.FileName = fileName;

                    //            listImagesUrl.Add(databaseImagesURL);
                    //        }
                    //    }
                    //    catch (Exception ex)
                    //    {

                    //    }
                    //}
                }

                //getting all those seller cloud order ids whcih are not present in Best Buy Table(To get orders from best Buy )

                List<GetOrdersNotFromBBViewModel> OrderIds = _bestBuyOrderDataAccess.GetBestBuyOrderIdsFromSellerCloud();

                if (orderIds != null && OrderIds.Count > 0)
                {
                    GetChannelCredViewModel _getChannelCred = new GetChannelCredViewModel();
                    _getChannelCred = _EncDecChannel.DecryptedData("bestbuy");
                    List<string> BBList = OrderIds.Select(s => s.BBOrderID).ToList();
                    ImportDataFromBestBuy.BestBuyRootObject bestBuyRootObject = GetBestBuyOrdersAgainstIDs("https://marketplace.bestbuy.ca/api/orders?paginate=false&order_ids=", _getChannelCred.Key, BBList);

                    List<BestBuyOrdersImportMainViewModel> listBestBuyOrders = new List<BestBuyOrdersImportMainViewModel>();

                    if (bestBuyRootObject.orders.Count > 0)
                    {
                        foreach (var item in bestBuyRootObject.orders)
                        {

                            BestBuyOrderImportViewModel OrderViewModel = new BestBuyOrderImportViewModel();
                            List<BestBuyOrderDetailImportViewModel> ListorderDetailViewModel = new List<BestBuyOrderDetailImportViewModel>();
                            BestBuyCustomerDetailImportViewModel customerDetailOrderViewModel = new BestBuyCustomerDetailImportViewModel();
                            BestBuyOrdersImportMainViewModel mainModel = new BestBuyOrdersImportMainViewModel();
                            if (item.customer.shipping_address != null && item.customer.billing_address != null)
                            {


                                OrderViewModel.order_id = item.order_id;
                                OrderViewModel.commercial_id = item.commercial_id;
                                OrderViewModel.customer_id = item.customer.customer_id;
                                OrderViewModel.can_cancel = item.can_cancel;
                                OrderViewModel.order_state = item.order_state;
                                OrderViewModel.acceptance_decision_date = item.acceptance_decision_date;
                                OrderViewModel.created_date = item.created_date;
                                OrderViewModel.total_price = item.total_price;
                                OrderViewModel.total_commission = item.total_commission;
                                OrderViewModel.shipping_price = item.shipping_price;

                                OrderViewModel.sellerCloudID = Convert.ToInt32(OrderIds.Where(p => p.BBOrderID.Equals(item.order_id)).Select(p => p.SCOrderID).FirstOrDefault());

                                //
                                mainModel.OrderViewModel = OrderViewModel;

                                foreach (var orderDetail in item.order_lines)
                                {
                                    BestBuyOrderDetailImportViewModel orderDetailViewModel = new BestBuyOrderDetailImportViewModel();
                                    orderDetailViewModel.order_line_id = orderDetail.order_line_id;
                                    orderDetailViewModel.offer_sku = orderDetail.offer_sku;
                                    orderDetailViewModel.quantity = orderDetail.quantity;
                                    orderDetailViewModel.total_priceOrerLine = orderDetail.total_price;
                                    orderDetailViewModel.ShippingFee = orderDetail.shipping_price != null ? Convert.ToDecimal(orderDetail.shipping_price) : 0;
                                    orderDetailViewModel.total_commissionOrderLine = orderDetail.commission_fee;
                                    orderDetailViewModel.order_line_state = orderDetail.order_line_state;
                                    orderDetailViewModel.received_date = orderDetail.received_date;
                                    orderDetailViewModel.shipped_date = orderDetail.shipped_date;
                                    orderDetailViewModel.product_title = orderDetail.product_title;
                                    orderDetailViewModel.GST = Convert.ToDouble(orderDetail.taxes.Sum(e => e.amount).ToString());
                                    orderDetailViewModel.PST = 0;
                                    ListorderDetailViewModel.Add(orderDetailViewModel);
                                }

                                mainModel.orderDetailViewModel = ListorderDetailViewModel;


                                customerDetailOrderViewModel.firstname = item.customer.shipping_address.firstname;
                                customerDetailOrderViewModel.lastname = item.customer.shipping_address.lastname;
                                customerDetailOrderViewModel.state = item.customer.shipping_address.state;
                                
                                customerDetailOrderViewModel.street_1 = item.customer.shipping_address.street_1;
                                customerDetailOrderViewModel.street_2 = item.customer.shipping_address.street_2;
                                customerDetailOrderViewModel.zip_code = item.customer.shipping_address.zip_code;
                                customerDetailOrderViewModel.phone = item.customer.shipping_address.phone;
                                customerDetailOrderViewModel.phone_secondary = item.customer.shipping_address.phone_secondary;
                                customerDetailOrderViewModel.city = item.customer.shipping_address.city;
                                customerDetailOrderViewModel.country = item.customer.shipping_address.country;

                                mainModel.customerDetailOrderViewModel = customerDetailOrderViewModel;
                                listBestBuyOrders.Add(mainModel);
                            }
                        }
                    }

                    //var result = listBestBuyOrders.Select(e => new
                    //{
                    //    innerResult = e.orderDetailViewModel.Select(t => new { offersku = t.offer_sku, Qty = t.quantity }).ToList()
                    //});


                    if (listBestBuyOrders.Count > 0)
                    {
                        // Get single order against order id to send email
                        IEnumerable<BestBuyOrderDetailImportViewModel> result = listBestBuyOrders.SelectMany(e => e.orderDetailViewModel);
                        bbOrderID = listBestBuyOrders.Select(e => e.OrderViewModel.order_id).Distinct().ToList();

                        // sum quantity for specifu sku to update quantity on bb
                        var finalResult = result.GroupBy(e => e.offer_sku, (x, y) => new
                        {
                            totalQty = y.Sum(r => int.Parse(r.quantity)),
                            offersku = x,
                            date = y.Max(e => e.received_date)
                        }).ToList();

                        foreach (var item in finalResult)
                        {
                            BestBuyDropShipQtyMovementViewModel model = new BestBuyDropShipQtyMovementViewModel();
                            model.ProductSku = item.offersku;
                            model.OrderQuantity = item.totalQty.ToString();
                            model.OrderDate = DateTime.Now;
                            _bestBuyProductDataAccess.SaveBestBuyOrderDropShipMovement(model);
                        }
                        // save Order in bestBuy table order and orderslines
                        _bestBuyOrderDataAccess.SaveBestBuyOrders(listBestBuyOrders);
                    }
                }

                //insert product detail from seller cloud to product table
                _sellerCloudOrderDataAccess.InsertDataFromSellerCloudTableToBestBuyTable();

                foreach (var item in bbOrderID)
                {
                    SendNewOrderEmail sendNewOrderEmail = new SendNewOrderEmail(_connectionString);
                    sendNewOrderEmail.SendrderEmail(_bestBuyProductDataAccess.GetAllBestBuyOrdersWithGlobalFilter("order_id", item, 25, 0, "desc"));
                }


                //commented code for save seller cloud images
                //sellerCloudApiAccess.SaveSellerCloudImages(ApiURL, token, listImagesUrl);
            }
            catch (Exception ex)
            {

            }
            //SendNewOrderEmail sendNewOrderEmail = new SendNewOrderEmail(_connectionString);
            //sendNewOrderEmail.SendrderEmail(_bestBuyProductDataAccess.GetAllBestBuyOrdersWithGlobalFilter("order_id", "218382346-A", 25, 0, "desc"));
            await Task.CompletedTask;
        }
    }





}
