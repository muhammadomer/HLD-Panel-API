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
    public class ZincASINWatchListNewJob : IJob
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
        public ZincASINWatchListNewJob(IConnectionString connectionString)
        {
            _connectionString = connectionString;

            _EncDecChannel = new EncDecChannel(_connectionString);
            channelDecrytionDataAccess = new ChannelDecrytionDataAccess(_connectionString);
            zincWathchlistDataAccess = new ZincWathchlistDataAccess(_connectionString);
            zincDataAccess = new ZincDataAccess(_connectionString);
            productDataAccess = new ProductDataAccess(_connectionString);
            QtyDataAccess = new ProductWarehouseQtyDataAccess(_connectionString);
        }
        public async Task Execute(IJobExecutionContext context)
        {
            _getChannelCredViewModel = new GetChannelCredViewModel();
            ZincWatchListSummaryViewModal zincWatchListSummary = new ZincWatchListSummaryViewModal();
            List<SaveWatchlistForjobsViewModel> ASInForJob = new List<SaveWatchlistForjobsViewModel>();
            // get ASIN from local
            int _JobID = 0;
            // set job as start
           
            ASInForJob = zincWathchlistDataAccess.GetWatchlistForJobNew();
            if (ASInForJob != null && ASInForJob.Count > 0)
            {
                _JobID = zincWathchlistDataAccess.SaveWatchlistSummaryNew();
                // get zinc key
                _getChannelCredViewModel = _EncDecChannel.DecryptedData("Zinc");
               
                if (_JobID > 0)
                {
                    zincWatchListSummary.JobID = _JobID;
                    zincWatchListSummary.Total_ASIN = ASInForJob.Count;
                }

            }
            foreach (var ASIN_List in ASInForJob)
            {
                int? minPriceOfOffer = null;
                string offerID = "";

                ZincProductSaveViewModel zincProductSaveViewModel = new ZincProductSaveViewModel();
                BBProductViewModel DropShipQtyViewModal = new BBProductViewModel();
                BestBuyDropShipQtyMovementViewModel qtyViewModel = new BestBuyDropShipQtyMovementViewModel();
                SaveWatchlistForjobsViewModel saveWatchlistForjobs = new SaveWatchlistForjobsViewModel();
                ZincWatchlistLogsViewModel zincWatchListlogs = new ZincWatchlistLogsViewModel();
                zincWatchListlogs.jobID = _JobID;
                // get date from zinc
                try
                {
                    ZincProductOfferViewModel.RootObject model = GetInfoFromZinc(ASIN_List, _getChannelCredViewModel.Key);
                    if (model.offers != null)
                    {
                        model.offers = model.offers.Where(s => s.condition.ToLower().Trim().Equals("new") && s.price > 0 && s.fba_badge.Equals(true)).ToList();
                    }
                    else
                    {
                        // update logs 
                        zincWatchListlogs.Amz_Price = 0;
                        zincWatchListlogs.ASIN = ASIN_List.ASIN;
                        zincWatchListlogs.ProductSKU = ASIN_List.ProductSKU;
                        zincWatchListlogs.SellerName = "";
                        zincWatchListlogs.FulfilledBY = "";
                        zincWatchListlogs.IsPrime = 0;
                        zincWatchListlogs.ZincResponse = "Listing Removed";
                        zincWatchListlogs.Remarks = "ASIN is Listing Removed";
                        zincWatchListlogs.UpdateOnHLD = "No changes on Panel";

                        zincWathchlistDataAccess.SaveWatchlistLogsNew(zincWatchListlogs);

                        // update watchlist 
                        saveWatchlistForjobs.ValidStatus = 0;
                        saveWatchlistForjobs.ASIN = ASIN_List.ASIN;
                        saveWatchlistForjobs.Consumed_call = ASIN_List.Consumed_call + 1;
                        // Set ASIN in watchLIst for next date
                        zincWathchlistDataAccess.UpdateWatchlistForJobNew(saveWatchlistForjobs);

                        // update in product zinc 
                        zincProductSaveViewModel = new ZincProductSaveViewModel();
                        zincProductSaveViewModel.timestemp = 0;
                        zincProductSaveViewModel.status = "";
                        zincProductSaveViewModel.ASIN = ASIN_List.ASIN;
                        zincProductSaveViewModel.Product_sku = ASIN_List.ProductSKU;
                        zincProductSaveViewModel.sellerName = "";
                        zincProductSaveViewModel.percent_positive = 0;
                        zincProductSaveViewModel.itemprice = 0;
                        zincProductSaveViewModel.itemavailable = false;
                        zincProductSaveViewModel.handlingday_min = 0;
                        zincProductSaveViewModel.handlingday_max = 0;
                        zincProductSaveViewModel.item_prime_badge = false;
                        zincProductSaveViewModel.delivery_days_max = 0;
                        zincProductSaveViewModel.delivery_days_min = 0;
                        zincProductSaveViewModel.item_condition = "";
                        zincProductSaveViewModel.IsListingRemove=true;
                        zincProductSaveViewModel.MessageWatchlist = ASIN_List.ASIN + " has been Listing removed on " + DateTime.Now + " by watchlist";
                        zincDataAccess.UpdateZincProductASINDetailWatchList(zincProductSaveViewModel);
                        zincWatchListSummary.Unavailable += 1;
                        zincWatchListSummary.NoPrime += 1;
                        continue;
                    }
                    if ((model.status != "processing" || model.status != "failed") && model.offers.Count > 0)
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
                                zincProductSaveViewModel.item_prime_badge = item.fba_badge;

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
                        if (zincProductSaveViewModel.item_prime_badge == true)
                        {
                            saveWatchlistForjobs.ValidStatus = 0;
                            saveWatchlistForjobs.ASIN = ASIN_List.ASIN;
                            saveWatchlistForjobs.Consumed_call = ASIN_List.Consumed_call + 1;
                            // Set ASIN in watchLIst for next date
                            zincWathchlistDataAccess.UpdateWatchlistForJobNew(saveWatchlistForjobs);

                            zincWatchListlogs.Amz_Price = zincProductSaveViewModel.itemprice;
                            zincWatchListlogs.ASIN = zincProductSaveViewModel.ASIN;
                            zincWatchListlogs.ProductSKU = zincProductSaveViewModel.Product_sku;
                            zincWatchListlogs.SellerName = zincProductSaveViewModel.sellerName;
                            zincWatchListlogs.FulfilledBY = greytext;
                            zincWatchListlogs.ZincResponse = "Available";
                            zincWatchListlogs.Remarks = "ASIN is Available";
                            zincWatchListlogs.UpdateOnHLD = "DS is enabled by watchlist";
                            zincWatchListlogs.IsPrime = 1;
                            zincWathchlistDataAccess.SaveWatchlistLogsNew(zincWatchListlogs);
                            // update in zinc Product table
                            zincProductSaveViewModel.MessageWatchlist = ASIN_List.ASIN + " is Available on " + DateTime.Now + " by watchlist";
                            zincDataAccess.UpdateZincProductASINDetailWatchList(zincProductSaveViewModel);
                            // dropship enable disable
                            DropShipQtyViewModal.dropship_status = true;
                            DropShipQtyViewModal.dropship_Qty = 5;
                            DropShipQtyViewModal.DropshipComments = "Zinc Update from watchlist";
                            DropShipQtyViewModal.ShopSKU_OfferSKU = ASIN_List.ProductSKU;

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
                            // summary
                            zincWatchListSummary.Available += 1;
                            zincWatchListSummary.Prime += 1;
                        }
                        else // if unavailable
                        {
                            // update logs 
                            zincWatchListlogs.Amz_Price = 0;
                            zincWatchListlogs.ASIN = ASIN_List.ASIN;
                            zincWatchListlogs.ProductSKU = ASIN_List.ProductSKU;
                            zincWatchListlogs.SellerName = "";
                            zincWatchListlogs.FulfilledBY = "";
                            zincWatchListlogs.IsPrime = 0;
                            zincWatchListlogs.ZincResponse = "Currently Unavailable";
                            zincWatchListlogs.Remarks = "ASIN is Currently Unavailable";
                            zincWatchListlogs.UpdateOnHLD = "No changes on Panel";
                            zincWathchlistDataAccess.SaveWatchlistLogsNew(zincWatchListlogs);

                            // update watchlist 
                            saveWatchlistForjobs.ValidStatus = 1;
                            saveWatchlistForjobs.ASIN = ASIN_List.ASIN;
                            saveWatchlistForjobs.Consumed_call = ASIN_List.Consumed_call + 1;
                            // Set ASIN in watchLIst for next date
                            zincWathchlistDataAccess.UpdateWatchlistForJobNew(saveWatchlistForjobs);

                            // update in product zinc 
                            zincProductSaveViewModel = new ZincProductSaveViewModel();
                            zincProductSaveViewModel.timestemp = 0;
                            zincProductSaveViewModel.status = "";
                            zincProductSaveViewModel.ASIN = ASIN_List.ASIN;
                            zincProductSaveViewModel.Product_sku = ASIN_List.ProductSKU;
                            zincProductSaveViewModel.sellerName = "";
                            zincProductSaveViewModel.percent_positive = 0;
                            zincProductSaveViewModel.itemprice = 0;
                            zincProductSaveViewModel.itemavailable = false;
                            zincProductSaveViewModel.handlingday_min = 0;
                            zincProductSaveViewModel.handlingday_max = 0;
                            zincProductSaveViewModel.item_prime_badge = false;
                            zincProductSaveViewModel.delivery_days_max = 0;
                            zincProductSaveViewModel.delivery_days_min = 0;
                            zincProductSaveViewModel.item_condition = "";
                            zincProductSaveViewModel.MessageWatchlist = ASIN_List.ASIN + " is unavailable on " + DateTime.Now + " by watchlist";
                            zincDataAccess.UpdateZincProductASINDetailWatchList(zincProductSaveViewModel);

                            // check is there any available ASIN
                            int count_available = zincDataAccess.GetAvailablePrimeDetail(ASIN_List.ProductSKU);
                            if (count_available == 0) // if not available set disable
                            {
                                DropShipQtyViewModal.dropship_status = false;
                                DropShipQtyViewModal.dropship_Qty = 0;
                                DropShipQtyViewModal.DropshipComments = "Zinc Update from watchlist";
                                DropShipQtyViewModal.ShopSKU_OfferSKU = ASIN_List.ProductSKU;

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
                            zincWatchListSummary.Unavailable += 1;
                            zincWatchListSummary.NoPrime += 1;
                            continue;

                        }
                    }
                    else // currently unavailable
                    {
                        // update logs 
                        zincWatchListlogs.Amz_Price = 0;
                        zincWatchListlogs.ASIN = ASIN_List.ASIN;
                        zincWatchListlogs.ProductSKU = ASIN_List.ProductSKU;
                        zincWatchListlogs.SellerName = "";
                        zincWatchListlogs.FulfilledBY = "";
                        zincWatchListlogs.IsPrime = 0;
                        zincWatchListlogs.ZincResponse = "Currently Unavailable";
                        zincWatchListlogs.Remarks = "ASIN is Currently Unavailable";
                        zincWatchListlogs.UpdateOnHLD = "No changes on Panel";
                        zincWathchlistDataAccess.SaveWatchlistLogsNew(zincWatchListlogs);
                        // update watchlist 
                        saveWatchlistForjobs.ValidStatus = 1;
                        saveWatchlistForjobs.ASIN = ASIN_List.ASIN;
                        saveWatchlistForjobs.Consumed_call = ASIN_List.Consumed_call + 1;
                        // Set ASIN in watchLIst for next date
                        zincWathchlistDataAccess.UpdateWatchlistForJobNew(saveWatchlistForjobs);
                        // update in product zinc 
                        zincProductSaveViewModel = new ZincProductSaveViewModel();
                        zincProductSaveViewModel.timestemp = 0;
                        zincProductSaveViewModel.status = "";
                        zincProductSaveViewModel.ASIN = ASIN_List.ASIN;
                        zincProductSaveViewModel.Product_sku = ASIN_List.ProductSKU;
                        zincProductSaveViewModel.sellerName = "";
                        zincProductSaveViewModel.percent_positive = 0;
                        zincProductSaveViewModel.itemprice = 0;
                        zincProductSaveViewModel.itemavailable = false;
                        zincProductSaveViewModel.handlingday_min = 0;
                        zincProductSaveViewModel.handlingday_max = 0;
                        zincProductSaveViewModel.item_prime_badge = false;
                        zincProductSaveViewModel.delivery_days_max = 0;
                        zincProductSaveViewModel.delivery_days_min = 0;
                        zincProductSaveViewModel.item_condition = "";
                        zincProductSaveViewModel.MessageWatchlist = ASIN_List.ASIN + " is unavailable on " + DateTime.Now + " by watchlist";
                        zincDataAccess.UpdateZincProductASINDetailWatchList(zincProductSaveViewModel);

                        // check is there any available ASIN
                        int count_available = zincDataAccess.GetAvailablePrimeDetail(ASIN_List.ProductSKU);
                        if (count_available == 0) // if not available set disable
                        {

                            DropShipQtyViewModal.dropship_status = false;
                            DropShipQtyViewModal.dropship_Qty = 0;
                            DropShipQtyViewModal.DropshipComments = "Zinc Update from watchlist";
                            DropShipQtyViewModal.ShopSKU_OfferSKU = ASIN_List.ProductSKU;

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

                        zincWatchListSummary.Unavailable += 1;
                        zincWatchListSummary.NoPrime += 1;
                        continue;

                    }
                }
                catch (Exception ex)
                {
                    continue;
                }
            }
            // set job as completed
            zincWathchlistDataAccess.UpdateWatchlistSummaryNew(zincWatchListSummary);
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
