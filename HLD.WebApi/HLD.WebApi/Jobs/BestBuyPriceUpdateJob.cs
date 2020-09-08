using AutoMapper.Configuration;
using DataAccess.DataAccess;
using DataAccess.Helper;
using DataAccess.ViewModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
    public class BestBuyPriceUpdateJob: IJob
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
        public BestBuyPriceUpdateJob(IConnectionString connectionString)
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
            //_getChannelCredViewModel = new GetChannelCredViewModel();
            //ZincWatchListSummaryViewModal zincWatchListSummary = new ZincWatchListSummaryViewModal();
            //ZincWatchlistLogsViewModel zincWatchListlogs = new ZincWatchlistLogsViewModel();

            //List<BestBuyUpdatePriceJobViewModel> SKUsForJob = new List<BestBuyUpdatePriceJobViewModel>();
            //// get ASIN from local

            //// if (ASInForJob.Count > 0)
            //{
            //    // get zinc key
            //    _getChannelCredViewModel = _EncDecChannel.DecryptedData("Zinc");
            //    // set job as start
            //    int JobID = zincWathchlistDataAccess.GetBestBuyUpdateJobId();
            //    SKUsForJob = zincWathchlistDataAccess.P_GetBestBuyUpdateListForJob(JobID);
            //    if (JobID > 0)
            //    {
            //        zincWatchListSummary.JobID = JobID;
            //        zincWatchListlogs.jobID = JobID;
            //        zincWatchListSummary.Total_ASIN = SKUsForJob.Count;
            //    }
            //    foreach (var item in SKUsForJob)
            //    {
            //        Ranx ranx = new Ranx()
            //        {
            //            price = Convert.ToDouble(item.UpdateSelllingPrice),
            //            quantity_threshold = 10
            //        };
            //        List<Ranx> ranxes = new List<Ranx>();
            //        ranxes.Add(ranx);
            //        Discounts discounts = new Discounts()
            //        {
            //            end_date = DateTime.Now.AddDays(15),
            //            price = Convert.ToDouble(item.UpdateSelllingPrice),
            //            ranges = ranxes,
            //            start_date= DateTime.Now
            //        };
            //        OfferAdditionalField offerAdditionalFields = new OfferAdditionalField()
            //        {
            //            code = "",
            //            value = "",
            //        };
            //        List<OfferAdditionalField> additionalFields = new List<OfferAdditionalField>();
            //        additionalFields.Add(offerAdditionalFields);
            //        Offers offers = new Offers()
            //        {
            //            available_ended = DateTime.Now.AddDays(15),
            //            available_started = DateTime.Now,
            //            description = "",
            //            discount = discounts,
            //            internal_description = "",
            //            logistic_class = "",
            //            min_quantity_alert = 0,
            //            offer_additional_fields = additionalFields,
            //            price=Convert.ToInt32(Math.Round(item.MSRP>item.UpdateSelllingPrice? item.MSRP: item.UpdateSelllingPrice* Convert.ToDecimal(1.30))),
            //            price_additional_info="",
            //            product_id=item.ProductId,
            //            product_id_type="SKU",
            //            quantity=10,
            //            shop_sku=item.SKU,
            //            state_code="11",
            //            update_delete=""
            //        };

            //        List<Offers> offers1 = new List<Offers>();
            //        offers1.Add(offers);
            //        BestBuyPriceJobSCViewModel bestBuyPrice = new BestBuyPriceJobSCViewModel() { offers=offers1};
            //        // string ImportId= UpdatePriceOnBestBuy(_getChannelCredViewModel.Key, bestBuyPrice);
            //        string ImportId = "xyz";
            //        zincWathchlistDataAccess.SaveBestBuyUpdateLogs(item, JobID, ImportId);

            //    }
            //    zincWathchlistDataAccess.BestBuyUpdateJobUpdateEndTime(JobID);
            //}
        }


        public string UpdatePriceOnBestBuy(string ZincUserName, BestBuyPriceJobSCViewModel model)
        {
            var data = JsonConvert.SerializeObject(model);
            string strResponse = "";
            string importID = "";

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://marketplace.bestbuy.ca//api/offers");
                request.Method = "POST";
                request.Accept = "application/json;";
                request.ContentType = "application/json";
                request.Credentials = new NetworkCredential(ZincUserName, "");
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
