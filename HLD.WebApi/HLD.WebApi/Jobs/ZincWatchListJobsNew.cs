using DataAccess.DataAccess;
using DataAccess.Helper;
using DataAccess.ViewModels;
using Newtonsoft.Json;
using Quartz;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace HLD.WebApi.Jobs
{
    [DisallowConcurrentExecution]
    public class ZincWatchListJobsNew : IJob
    {
        IConnectionString _connectionString = null;

        // string ZincUserName = "";
        EncDecChannel _EncDecChannel = null;
        GetChannelCredViewModel _getChannelCredViewModel = null;
        ChannelDecrytionDataAccess channelDecrytionDataAccess = null;
        ZincWathchlistDataAccess zincWathchlistDataAccess = null;
        ProductWarehouseQtyDataAccess QtyDataAccess = null;
        ZincDataAccess zincDataAccess = null;
        ProductDataAccess productDataAccess = null;
        ZincWatchlistJobsDataAccess zincWatchlist = null;
        public ZincWatchListJobsNew(IConnectionString connectionString)
        {

            _connectionString = connectionString;
            _EncDecChannel = new EncDecChannel(_connectionString);
            channelDecrytionDataAccess = new ChannelDecrytionDataAccess(_connectionString);
            zincWathchlistDataAccess = new ZincWathchlistDataAccess(_connectionString);
            zincDataAccess = new ZincDataAccess(_connectionString);
            productDataAccess = new ProductDataAccess(_connectionString);
            QtyDataAccess = new ProductWarehouseQtyDataAccess(_connectionString);
            zincWatchlist = new ZincWatchlistJobsDataAccess(_connectionString);
        }

        public async Task Execute(IJobExecutionContext context)
        {
            int Zincstatus = channelDecrytionDataAccess.CheckZincJobsStatus("ZincJob");
            if (Zincstatus == 1)
            {
                _getChannelCredViewModel = new GetChannelCredViewModel();
                ZincWatchListSummaryViewModal zincWatchListSummary = new ZincWatchListSummaryViewModal();
                ZincWatchlistLogsViewModel zincWatchListlogs = new ZincWatchlistLogsViewModel();
                List<SaveWatchlistForjobsViewModel> ASInForJob = new List<SaveWatchlistForjobsViewModel>();
                // get ASIN from local
                ASInForJob = zincWatchlist.GetWatchlist();
                if (ASInForJob.Count > 0)
                {
                    // get zinc key
                    _getChannelCredViewModel = _EncDecChannel.DecryptedData("Zinc");
                    // set job as start
                    int JobID = zincWathchlistDataAccess.SaveWatchlistSummary();
                    if (JobID > 0)
                    {
                        zincWatchListSummary.JobID = JobID;
                        zincWatchListlogs.jobID = JobID;
                        zincWatchListSummary.Total_ASIN = ASInForJob.Count;
                    }

                    foreach (var ASIN_List in ASInForJob)
                    {
                        int ZincSwitch = channelDecrytionDataAccess.CheckZincJobsSwitch("ZincJob");
                        if (ZincSwitch == 0)
                        {
                            break;
                        }
                        int? minPriceOfOffer = null;
                        string offerID = "";
                        ZincProductSaveViewModel zincProductSaveViewModel = new ZincProductSaveViewModel();
                        BBProductViewModel DropShipQtyViewModal = new BBProductViewModel();
                        BestBuyDropShipQtyMovementViewModel qtyViewModel = new BestBuyDropShipQtyMovementViewModel();
                        // get date from zinc
                        try
                        {
                            ZincProductOfferViewModel.RootObject model = GetInfoFromZinc(ASIN_List, _getChannelCredViewModel.Key);

                            if ((model.status != "processing" || model.status != "failed") && model.offers != null && model.offers.Count > 0)
                            {

                                string greytext = "";
                                // getting all those offers which have fulfilled true
                                var offerids = model.offers.Where(e => e.marketplace_fulfilled.Equals(true)).Select(
                                    e => new
                                    {
                                        offerid = e.offer_id,
                                        offerPrice = e.price
                                    }).ToList();

                                // based on offerid's list getting minimum price and then select offer from offer's list

                                if (offerids != null && offerids.Count > 0)
                                {
                                    minPriceOfOffer = offerids.Min(e => e.offerPrice);
                                    offerID = offerids.Where(e => e.offerPrice.Value == minPriceOfOffer.Value).Select(e => e.offerid).FirstOrDefault();
                                }
                                var models = model.offers.Where(e => e.offer_id == offerID).ToList();

                                if (models != null && models.Count > 0)
                                {
                                    zincProductSaveViewModel = new ZincProductSaveViewModel();
                                    zincProductSaveViewModel.timestemp = model.timestamp.HasValue ? model.timestamp.Value : 0;
                                    zincProductSaveViewModel.status = model.status;
                                    zincProductSaveViewModel.ASIN = model.asin;
                                    zincProductSaveViewModel.Product_sku = ASIN_List.ProductSKU;
                                    var status = model.status;
                                    foreach (var item in models)
                                    {
                                        zincProductSaveViewModel.sellerName = item.seller.name;
                                        zincProductSaveViewModel.percent_positive = item.seller.percent_positive.HasValue ? item.seller.percent_positive.Value : 0;
                                        zincProductSaveViewModel.itemprice = item.price.HasValue ? item.price.Value : 0;
                                        zincProductSaveViewModel.itemavailable = item.available;
                                        zincProductSaveViewModel.handlingday_min = item.handling_days.min.HasValue ? item.handling_days.min.Value : 0;
                                        zincProductSaveViewModel.handlingday_max = item.handling_days.max.HasValue ? item.handling_days.max.Value : 0;
                                        zincProductSaveViewModel.item_prime_badge = item.prime_badge;

                                        foreach (var shippingOption in item.shipping_options)
                                        {
                                            if (shippingOption.delivery_days != null)
                                            {
                                                zincProductSaveViewModel.delivery_days_max = shippingOption.delivery_days.max.HasValue ? shippingOption.delivery_days.max.Value : 0;
                                                zincProductSaveViewModel.delivery_days_min = shippingOption.delivery_days.min.HasValue ? shippingOption.delivery_days.min.Value : 0;
                                            }
                                        }
                                        zincProductSaveViewModel.item_condition = item.condition;
                                        greytext = item.greytext;


                                    }
                                }
                                zincWatchListlogs.Amz_Price = zincProductSaveViewModel.itemprice;
                                zincWatchListlogs.ASIN = zincProductSaveViewModel.ASIN;
                                zincWatchListlogs.ProductSKU = zincProductSaveViewModel.Product_sku;
                                zincWatchListlogs.SellerName = zincProductSaveViewModel.sellerName;
                                zincWatchListlogs.FulfilledBY = greytext;
                                zincWatchListlogs.ZincResponse = "Available";
                                zincWatchListlogs.IsPrime = zincProductSaveViewModel.item_prime_badge == true ? 1 : 0;
                            }
                            else
                            {
                                zincWatchListlogs.Amz_Price = 0;
                                zincWatchListlogs.ASIN = ASIN_List.ASIN;
                                zincWatchListlogs.ProductSKU = ASIN_List.ProductSKU;
                                zincWatchListlogs.SellerName = "";
                                zincWatchListlogs.FulfilledBY = "";
                                zincWatchListlogs.ZincResponse = "Currently Unavailable";
                                zincWatchListlogs.IsPrime = 0;

                                if (model.offers == null)
                                {
                                    zincWatchListlogs.ZincResponse = "Listing Removed";
                                }
                            }

                            SaveWatchlistForjobsViewModel saveWatchlistForjobs = new SaveWatchlistForjobsViewModel();
                            // if available
                            if (model.offers == null || (model.offers.Count > 0 && zincProductSaveViewModel.item_prime_badge == true))
                            {
                                saveWatchlistForjobs.ValidStatus = 0;
                                saveWatchlistForjobs.ASIN = ASIN_List.ASIN;
                                saveWatchlistForjobs.Consumed_call = ASIN_List.Consumed_call + 1;
                                if (model.offers != null)
                                {
                                    zincWatchListSummary.Available += 1;
                                    zincWatchListSummary.Prime += 1;
                                    zincWatchListlogs.ZincResponse = "Available";
                                    // dropship enable disable
                                    DropShipQtyViewModal.dropship_status = true;
                                    DropShipQtyViewModal.dropship_Qty = 5;
                                    DropShipQtyViewModal.DropshipComments = "Zinc Update";
                                    DropShipQtyViewModal.ShopSKU_OfferSKU = ASIN_List.ProductSKU;




                                }
                                else
                                {
                                    zincWatchListSummary.Unavailable += 1;
                                    // dropship enable disable
                                    DropShipQtyViewModal.dropship_status = false;
                                    DropShipQtyViewModal.dropship_Qty = 0;
                                    DropShipQtyViewModal.DropshipComments = "Zinc Update";
                                    DropShipQtyViewModal.ShopSKU_OfferSKU = ASIN_List.ProductSKU;

                                }


                            }
                            else
                            {
                                saveWatchlistForjobs.ValidStatus = 1;
                                saveWatchlistForjobs.ASIN = ASIN_List.ASIN;
                                saveWatchlistForjobs.Consumed_call = ASIN_List.Consumed_call + 1;
                                // dropship enable disable
                                DropShipQtyViewModal.dropship_status = false;
                                DropShipQtyViewModal.dropship_Qty = 0;
                                DropShipQtyViewModal.DropshipComments = "Zinc Update";
                                DropShipQtyViewModal.ShopSKU_OfferSKU = ASIN_List.ProductSKU;

                                if (model.offers.Count > 0 && zincProductSaveViewModel.item_prime_badge == false)
                                {
                                    zincWatchListSummary.Available += 1;
                                    zincWatchListSummary.NoPrime += 1;
                                }
                                else
                                {
                                    zincWatchListSummary.Unavailable += 1;
                                }
                            }

                            // update qty and dropship status

                            if (DropShipQtyViewModal.dropship_status == false) // if not available
                            {
                                // check is there any available ASIN
                                //int count_available = zincDataAccess.GetAvailablePrimeDetail(ASIN_List.ProductSKU);
                                //if (count_available == 0) // if not available set disable
                                //{
                                //    bool isdone = productDataAccess.UpdateProductDropshipStatusAndQty(DropShipQtyViewModal);
                                //    if (isdone)
                                //    {
                                //        qtyViewModel.ProductSku = DropShipQtyViewModal.ShopSKU_OfferSKU;
                                //        qtyViewModel.DropShipQuantity = DropShipQtyViewModal.dropship_Qty;
                                //        qtyViewModel.DropShipStatus = DropShipQtyViewModal.dropship_status;
                                //        qtyViewModel.DropshipComments = DropShipQtyViewModal.DropshipComments;
                                //        qtyViewModel.OrderDate = DateTime.Now;

                                //        QtyDataAccess.SaveBestBuyQtyMovementForDropshipNone_SKU(qtyViewModel);
                                //    }
                                //}

                            }
                            else // if available
                            {
                                CheckProductDropShipStatusViewModel dropshipStatus = productDataAccess.CheckSKuDropShipStatus(ASIN_List.ProductSKU); // enable or disable 
                                if (dropshipStatus.dropship_status == false)// if disable set enable
                                {
                                    bool isdone = productDataAccess.UpdateProductDropshipStatusAndQty(DropShipQtyViewModal);
                                    if (isdone)
                                    {
                                        qtyViewModel.ProductSku = DropShipQtyViewModal.ShopSKU_OfferSKU;
                                        qtyViewModel.DropShipQuantity = DropShipQtyViewModal.dropship_Qty;
                                        qtyViewModel.DropShipStatus = DropShipQtyViewModal.dropship_status;
                                        qtyViewModel.DropshipComments = DropShipQtyViewModal.DropshipComments;
                                        qtyViewModel.OrderDate = DateTime.Now;

                                        QtyDataAccess.SaveBestBuyQtyMovementForDropshipNone_SKU(qtyViewModel);
                                    }
                                }


                            }


                            // Save logs
                            zincWathchlistDataAccess.SaveWatchlistLogs(zincWatchListlogs);
                            if (zincWatchListlogs.ZincResponse == "Available")
                            {

                                zincDataAccess.UpdateZincProductASINDetailWatchList(zincProductSaveViewModel);
                            }

                            // Set ASIN in watchLIst for next date
                            //zincWathchlistDataAccess.UpdateWatchlistForJob(saveWatchlistForjobs);


                        }
                        catch (Exception ex)
                        {
                            //num = 0;
                        }


                    }
                    // set job as completed
                    zincWathchlistDataAccess.UpdateWatchlistSummary(zincWatchListSummary);
                }
            }
            await Task.CompletedTask;
        }

        public ZincProductOfferViewModel.RootObject GetInfoFromZinc(SaveWatchlistForjobsViewModel watchASIN, string ZincUserName)
        {

            ZincProductOfferViewModel.RootObject model = null;
            try
            {
                string uri = " https://api.zinc.io/v1/products/" + watchASIN.ASIN + "/offers?retailer=amazon_ca";
                string response = "";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
                request.Credentials = new NetworkCredential(ZincUserName, "");
                using (var webResponse = request.GetResponse())
                {
                    using (var responseStream = webResponse.GetResponseStream())
                    {
                        response = new StreamReader(responseStream).ReadToEnd();
                    }
                }

                model = JsonConvert.DeserializeObject<ZincProductOfferViewModel.RootObject>(response);


            }
            catch (Exception)
            {

                throw;
            }
            return model;

        }
    }
}
