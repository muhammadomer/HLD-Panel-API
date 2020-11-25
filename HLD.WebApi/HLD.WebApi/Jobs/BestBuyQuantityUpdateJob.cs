using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using DataAccess.DataAccess;
using DataAccess.Helper;
using DataAccess.ViewModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Quartz;
using ServiceReference1;

namespace HLD.WebApi.Jobs
{
    [DisallowConcurrentExecution]
    public class BestBuyQuantityUpdateJob : IJob
    {
        IConnectionString _connectionString = null;
        //ServiceReference1.AuthHeader authHeader = null;
        SellerCloudOrderDataAccess _sellerCloudOrderDataAccess = null;
        BestBuyOrderDataAccess _bestBuyOrderDataAccess = null;
        BestBuyProductDataAccess _bestBuyProductDataAccess = null;
        EncDecChannel _EncDecChannel = null;
        ChannelDecrytionDataAccess channelDecrytionDataAccess = null;
        public BestBuyQuantityUpdateJob(IConnectionString connectionString)
        {
            _connectionString = connectionString;
            _EncDecChannel = new EncDecChannel(_connectionString);
            _sellerCloudOrderDataAccess = new SellerCloudOrderDataAccess(_connectionString);
            _bestBuyOrderDataAccess = new BestBuyOrderDataAccess(_connectionString);
            _bestBuyProductDataAccess = new BestBuyProductDataAccess(_connectionString);
            channelDecrytionDataAccess = new ChannelDecrytionDataAccess(_connectionString);
        }


        public async Task Execute(IJobExecutionContext context)
        {
            int status = channelDecrytionDataAccess.CheckZincJobsStatus("qtyupdater");
            if (status == 1)
            {
                List<BestBuyDropShipQtyMovementViewModel> list = _bestBuyOrderDataAccess.GetBestBuyDropShipQtyForUpdateOnBB();
                if (list != null)
                {
                    GetChannelCredViewModel _getChannelCred = new GetChannelCredViewModel();
                    _getChannelCred = _EncDecChannel.DecryptedData("bestbuy");

                    foreach (var item in list)
                    {
                        try
                        {
                            RootObject rootObject = GetBestBuyOfferDetailBySKU("https://marketplace.bestbuy.ca/api/offers", _getChannelCred.Key, item.ProductSku, item.DropShipQuantity, item.DropShipQtyMovementID);
                        }
                        catch (Exception ex)
                        {
                            continue;
                        }
                    }
                }
            }
            else
            {
                _bestBuyOrderDataAccess.UpdateBestBuyQtyWhenJobDisabled();
            }
            await Task.CompletedTask;
        }


        public RootObject GetBestBuyOfferDetailBySKU(string apiurl, string token, string sku, int quantity, int dropshipQtyMovementID)
        {
            string strResponse = "";
            RootObject responses = null;
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(apiurl + "?sku=" + sku);
                request.Method = "GET";
                request.Accept = "application/json;";
                request.ContentType = "application/json";
                request.Headers["Authorization"] = token;

                using (WebResponse webResponse = request.GetResponse())
                {
                    using (StreamReader stream = new StreamReader(webResponse.GetResponseStream()))
                    {
                        strResponse = stream.ReadToEnd();
                    }
                }
                responses = JsonConvert.DeserializeObject<RootObject>(strResponse);

                List<BestBuyUpdateQty_Update_ViewModel.Offer> listOffers = new List<BestBuyUpdateQty_Update_ViewModel.Offer>();
                BestBuyUpdateQty_Update_ViewModel.Discount discount = new BestBuyUpdateQty_Update_ViewModel.Discount();
                // update best buy product from best buy offer detail
                BBProductViewModel bBProductViewModel = new BBProductViewModel();
                if (responses.offers.Count > 0)
                {
                    foreach (var item in responses.offers)
                    {

                        BestBuyUpdateQty_Update_ViewModel.Offer obj = new BestBuyUpdateQty_Update_ViewModel.Offer();
                        obj.discount = new BestBuyUpdateQty_Update_ViewModel.Discount();

                        obj.discount.end_date = DateTime.Now.AddDays(10);
                        obj.discount.price = item.discount.discount_price;
                        if (item.discount.start_date.HasValue)
                        {
                            obj.discount.start_date = item.discount.start_date.Value;
                        }
                        else
                        {
                            obj.discount.start_date = DateTime.Now.AddDays(-3);
                        }

                        obj.price = item.discount.origin_price;
                        obj.product_id = item.product_sku;
                        obj.quantity = quantity;
                        obj.shop_sku = sku;
                        obj.state_code = "11";
                        obj.update_delete = "update";
                        obj.available_ended = null;
                        obj.available_started = null;
                        obj.product_id_type = "SKU";
                        listOffers.Add(obj);

                        bBProductViewModel.DiscountEndDate = obj.discount.end_date;
                        bBProductViewModel.DiscountStartDate = obj.discount.start_date;
                        bBProductViewModel.UnitDiscountPrice_SellingPrice = obj.discount.price;
                        bBProductViewModel.UnitOriginPrice_MSRP = obj.price;
                        bBProductViewModel.CategoryCode = item.category_code;
                        bBProductViewModel.CategoryName = item.category_label;
                        bBProductViewModel.LogisticClass_Code = item.logistic_class.code;
                        bBProductViewModel.ShopSKU_OfferSKU = item.shop_sku;
                        bBProductViewModel.Product_Sku = item.product_sku;

                    }

                    BestBuyUpdateQty_Update_ViewModel.RootObject model = new BestBuyUpdateQty_Update_ViewModel.RootObject();
                    model.offers = new List<BestBuyUpdateQty_Update_ViewModel.Offer>();
                    model.offers = listOffers;

                    string response = UpdateQtyOnBestBuy(apiurl, token, model);

                    _bestBuyOrderDataAccess.UpdateBestBuyQtyMovementDropshipStatus(dropshipQtyMovementID, response);
                    _bestBuyProductDataAccess.UpdateBestBuyProductFromBestBuyOffer(bBProductViewModel);

                }
                else
                {
                    _bestBuyOrderDataAccess.UpdateBestBuyQtyMovementDropshipStatus(dropshipQtyMovementID, "-1");
                }




            }
            catch (Exception ex)
            {
                throw;
            }
            return responses;
        }

        public string UpdateQtyOnBestBuy(string apiurl, string token, BestBuyUpdateQty_Update_ViewModel.RootObject model)
        {
            var data = JsonConvert.SerializeObject(model);
            string strResponse = "";
            string importID = "";

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(apiurl);
                request.Method = "POST";
                request.Accept = "application/json;";
                request.ContentType = "application/json";
                request.Headers["Authorization"] = token;

                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    streamWriter.Write(data);
                    streamWriter.Flush();
                    streamWriter.Close();
                }

                using (WebResponse webResponse = request.GetResponse())
                {
                    using (StreamReader stream = new StreamReader(webResponse.GetResponseStream()))
                    {
                        strResponse = stream.ReadToEnd();
                    }
                }
                if (strResponse != string.Empty)
                {
                    JObject jObject = JObject.Parse(strResponse);
                    importID = jObject["import_id"].ToString();
                }

            }
            catch (Exception ex)
            {
                throw;

            }
            return importID;
        }
    }
}
