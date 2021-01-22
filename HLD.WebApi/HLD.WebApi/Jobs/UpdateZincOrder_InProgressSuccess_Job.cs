using DataAccess.DataAccess;
using DataAccess.Helper;
using DataAccess.ViewModels;
using HLD.WebApi.Enum;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using Quartz;
using ServiceReference1;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace HLD.WebApi.Jobs
{
    public class UpdateZincOrder_InProgressSuccess_Job : IJob
    {
        IConnectionString _connectionString = null;
        ServiceReference1.AuthHeader authHeader = null;
        ZincOrderLogAndDetailDataAccess _zincOrderLogDataAccess = null;
        ZincDataAccess _zincDataAccess = null;
        SellerCloudOrderDataAccess _sellerCloudDataAccess = null;
        private IConfiguration _configuration = null;
        EncDecChannel _EncDecChannel = null;
        GetChannelCredViewModel _getChannelCredViewModel = null;
        GetChannelCredViewModel _getChannelCredSC = null;
        ChannelDecrytionDataAccess channelDecrytionDataAccess = null;
        public UpdateZincOrder_InProgressSuccess_Job(IConnectionString connectionString, IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = connectionString;
            _getChannelCredSC = new GetChannelCredViewModel();
            _EncDecChannel = new EncDecChannel(_connectionString);
            _getChannelCredSC = _EncDecChannel.DecryptedData("sellercloud");
            authHeader = new AuthHeader();
            authHeader.ValidateDeviceID = false; ;
            authHeader.UserName = _getChannelCredSC.UserName;
            authHeader.Password = _getChannelCredSC.Key;
            _zincOrderLogDataAccess = new ZincOrderLogAndDetailDataAccess(_connectionString);
           // ZincUserName = _configuration.GetValue<string>("ZincCredential:UserName");
            _zincDataAccess = new ZincDataAccess(_connectionString);
            channelDecrytionDataAccess = new ChannelDecrytionDataAccess(_connectionString);
            _sellerCloudDataAccess = new SellerCloudOrderDataAccess(_connectionString);
           

        }
        public async Task Execute(IJobExecutionContext context)
        {
           int status = channelDecrytionDataAccess.CheckZincJobsStatus("InProgessSuccessZinc");
            if (status == 1)
            {
                List<ZincOrderInProgressSuccessViewModel> list = _zincDataAccess.GetAllSCZincOrders_InProgressSuccess_ForJob();
                _getChannelCredViewModel = new GetChannelCredViewModel();
                _getChannelCredViewModel = _EncDecChannel.DecryptedData("Zinc");


                foreach (var item in list)
                {
                    try
                    {
                        ZincOrderLogDetailViewModel model = new ZincOrderLogDetailViewModel();
                        string uri = "https://api.zinc.io/v1/orders/" + item.RequestID;
                        string response = "";
                        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
                        request.Method = "GET";
                        request.Accept = "application/json;";
                        request.ContentType = "application/json";
                        request.Credentials = new NetworkCredential(_getChannelCredViewModel.Key, "");

                        using (var webResponse = request.GetResponse())
                        {
                            using (var responseStream = webResponse.GetResponseStream())
                            {
                                response = new StreamReader(responseStream).ReadToEnd();
                            }
                        }
                        var X = JObject.Parse(response);
                        var type = X["_type"];
                        var code = X["code"];
                        var message = X["message"];
                        var extra = X["extra"];
                        string commonMessage = "";
                        var delivery_dates = X["delivery_dates"];
                        var tracking = X["tracking"];

                        if (delivery_dates != null && delivery_dates.Count() != 0)
                        {
                            commonMessage = delivery_dates[0].Value<string>("delivery_date").ToString();

                        }

                        if (message != null)
                        {
                            commonMessage = message.ToString();
                        }



                        model.Message = commonMessage;

                        //setting internal zinc order status
                        if (type != null)
                        {
                            if (type.ToString() == "order_response" && delivery_dates != null)
                            {
                                model.ZincOrderStatusInternal = ZincOrderLogInternalStatus.InProgressSuccess.ToString();
                            }
                            else if (type.ToString() == "order_response")
                            {
                                model.ZincOrderStatusInternal = ZincOrderLogInternalStatus.InProcess.ToString();
                            }
                            else
                            {
                                model.ZincOrderStatusInternal = ZincOrderLogInternalStatus.Error.ToString();
                            }

                            model.Type = type.ToString();
                        }


                        if (type != null && code != null)
                        {
                            if (type.ToString() == "error" && code.ToString() == "request_processing")
                            {
                                model.ZincOrderStatusInternal = ZincOrderLogInternalStatus.InProcess.ToString();
                            }
                            else
                            {
                                model.ZincOrderStatusInternal = ZincOrderLogInternalStatus.Error.ToString();
                            }

                            if (code.ToString() == "zma_temporarily_overloaded")
                            {
                                commonMessage = "We are experiencing a very high volume of orders and are temporarily unable to process this order. Please retry in a few minutes.";
                            }


                            model.Type = type.ToString();
                            model.Code = code.ToString();
                        }


                        //checking tracking of an zinc order
                        if (tracking != null)
                        {
                            if (!string.IsNullOrEmpty(tracking[0].Value<string>("tracking_number")))
                            {
                                if (!string.IsNullOrEmpty(tracking[0].Value<string>("obtained_at")))
                                {
                                    model.ShppingDate = tracking[0].Value<string>("obtained_at").ToString();
                                }
                                if (!string.IsNullOrEmpty(tracking[0].Value<string>("tracking_number")))
                                {
                                    model.TrackingNumber = tracking[0].Value<string>("tracking_number").ToString();
                                }
                                if (!string.IsNullOrEmpty(tracking[0].Value<string>("carrier")))
                                {
                                    model.Carrier = tracking[0].Value<string>("carrier").ToString();
                                }
                                if (!string.IsNullOrEmpty(tracking[0].Value<string>("merchant_order_id")))
                                {
                                    model.MerchantOrderId = tracking[0].Value<string>("merchant_order_id").ToString();
                                }
                                if (!string.IsNullOrEmpty(tracking[0].Value<string>("tracking_url")))
                                {
                                    model._17Tracking = tracking[0].Value<string>("tracking_url").ToString();
                                }
                                if (!string.IsNullOrEmpty(tracking[0].Value<string>("retailer_tracking_url")))
                                {
                                    model.AmazonTracking = tracking[0].Value<string>("retailer_tracking_url").ToString();
                                }
                                if (type != null)
                                {
                                    if (type.ToString() == "order_response")
                                    {
                                        model.ZincOrderStatusInternal = ZincOrderLogInternalStatus.Shipped.ToString();
                                    }
                                }
                                await SendTrackingToSC(item.SCOrderId, item.ZincOrderLogDetailID, item.ProductSKU, item.Qty, model.ShppingDate, model.TrackingNumber);
                            }
                        }

                        model.OrderDatetime = DateTimeExtensions.ConvertToEST(DateTime.Now);
                        model.ZincOrderLogID = Convert.ToInt32(item.ZincOrderLogID);

                        _zincOrderLogDataAccess.SaveZincOrderLogDetail(model);
                    }
                    catch (Exception ex)
                    {
                        continue;
                    }
                }
                await Task.CompletedTask;
            }
        }

        public async Task SendTrackingToSC(string sellerCloudOrderId, string zincOrderLogDetailID, string productSku, string itemQuantity, string shippingDate, string trackingNo)
        {
            bool status = false;

            ZincOrderLogDetailViewModel model = new ZincOrderLogDetailViewModel();

            ServiceReference1.SCServiceSoapClient sCServiceSoap =
                      new ServiceReference1.SCServiceSoapClient(ServiceReference1.SCServiceSoapClient.EndpointConfiguration.SCServiceSoap12);

            var request = await sCServiceSoap.UpdateOrderDropShipStatusAsync(authHeader, null, int.Parse(sellerCloudOrderId), DropShipStatusType2.Processed);
            bool response = request.UpdateOrderDropShipStatusResult;
            status = response;
            if (status)
            {
                UpdateSCDropshipStatusViewModel updateSCViewModel = new UpdateSCDropshipStatusViewModel();
                updateSCViewModel.StatusName = "Processed";
                updateSCViewModel.SCOrderID = int.Parse(sellerCloudOrderId);
                updateSCViewModel.LogDate = DateTime.Now;
                updateSCViewModel.IsTrackingUpdate = true;


                status = _sellerCloudDataAccess.UpdateSCOrderDropShipStatus(updateSCViewModel);
                //ZincOrderLogDetailViewModel model = new ZincOrderLogDetailViewModel();
                //model = _zincOrderLogDataAccess.GetZincOrderLogDetailById(zincOrderLogDetailID);
                OrdersUpdateShippingForOrderRequest req = new OrdersUpdateShippingForOrderRequest();



                req.WarehouseName = "DropShip Canada";
                req.TrackingNumber = trackingNo;


                string[] splitString = shippingDate.Split('/');
                string[] yearSplit = splitString[2].Split(' ');
                string[] splitStringtime = shippingDate.Split(':');
                string[] splitTime = splitString[0].Split(' ');

                DateTime d = new DateTime(int.Parse(yearSplit[0]), int.Parse(splitString[0]), int.Parse(splitString[1]), int.Parse(splitTime[0]), int.Parse(splitStringtime[1]), int.Parse(splitStringtime[2]), 0);

                d =DateTimeExtensions.ConvertToEST(d);

                req.ShipDate = d;
                req.OrderID = int.Parse(sellerCloudOrderId);
                var result = await sCServiceSoap.Orders_UpdateShippingForOrderAsync(authHeader, null, req);
                bool shippingOrderStatus = result.Orders_UpdateShippingForOrderResult;
                var warehouseAdjustment = await sCServiceSoap.ProductWarehouse_AdjustQtyAsync(authHeader, null, productSku, 364, int.Parse(itemQuantity), "Dropship Adj " + sellerCloudOrderId, "");
                status = warehouseAdjustment.ProductWarehouse_AdjustQtyResult;
            }
            await Task.CompletedTask;
        }
    }

}

