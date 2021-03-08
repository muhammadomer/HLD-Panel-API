using DataAccess.DataAccess;
using DataAccess.Helper;
using DataAccess.ViewModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Quartz;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace HLD.WebApi.Jobs
{
    [DisallowConcurrentExecution]
    public class UpdateLimitedRecordOnBestBuy : IJob
    {
        IConnectionString _connectionString = null;
        EncDecChannel _EncDecChannel = null;
        GetChannelCredViewModel _getChannelCredViewModel = null;
        ZincWathchlistDataAccess zincWathchlistDataAccess = null;
      
        public UpdateLimitedRecordOnBestBuy(IConnectionString connectionString)
        {
            _connectionString = connectionString;
            _EncDecChannel = new EncDecChannel(_connectionString);
            zincWathchlistDataAccess = new ZincWathchlistDataAccess(_connectionString);
          
        }

        public async Task Execute(IJobExecutionContext context)
        {
            _getChannelCredViewModel = new GetChannelCredViewModel();
            ZincWatchListSummaryViewModal zincWatchListSummary = new ZincWatchListSummaryViewModal();
            ZincWatchlistLogsViewModel zincWatchListlogs = new ZincWatchlistLogsViewModel();

            List<BestBuyUpdatePriceJobViewModel> SKUsForJob = new List<BestBuyUpdatePriceJobViewModel>();
            {
                // get zinc key
                _getChannelCredViewModel = _EncDecChannel.DecryptedData("bestbuy");
                // set job as start
                int JobID = 0;
                JobID = zincWathchlistDataAccess.GetNotCompletedTimeJobId();
                SKUsForJob = zincWathchlistDataAccess.GetNotCompletedTimeJobData(JobID);
                var list = SKUsForJob.GroupBy(s => s.SKU).Select(p => p.OrderBy(x => x.UpdateSelllingPrice).FirstOrDefault()).Distinct().ToList();
                //var list= .GroupBy()
                if (JobID > 0)
                {
                    zincWatchListSummary.JobID = JobID;
                    zincWatchListlogs.jobID = JobID;
                    zincWatchListSummary.Total_ASIN = SKUsForJob.Count;
                }
                try
                {
                    foreach (var item in SKUsForJob)
                    {
                        Ranxs ranx = new Ranxs()
                        {
                            price = item.UpdateSelllingPrice,
                            quantity_threshold = 10
                        };
                        List<Ranxs> ranxes = new List<Ranxs>();
                        ranxes.Add(ranx);

                        Discounts discounts = new Discounts()
                        {
                            end_date = DateTime.Now.AddDays(15).ToString("o", CultureInfo.CreateSpecificCulture("de-DE")),
                            price = item.UpdateSelllingPrice,
                            ranges = ranxes,
                            start_date = DateTime.Now.ToString("o", CultureInfo.CreateSpecificCulture("de-DE"))
                        };
                        OfferAdditionalFields offerAdditionalFields = new OfferAdditionalFields()
                        {
                            code = "",
                            value = "",
                        };
                        List<OfferAdditionalFields> additionalFields = new List<OfferAdditionalFields>();
                        additionalFields.Add(offerAdditionalFields);
                        Offers offers = new Offers()
                        {
                            available_ended = DateTime.Now.AddDays(15).ToString("o", CultureInfo.CreateSpecificCulture("de-DE")),
                            available_started = DateTime.Now.ToString("o", CultureInfo.CreateSpecificCulture("de-DE")),
                            description = "",
                            discount = discounts,
                            internal_description = item.ASIN,
                            logistic_class = "",
                            min_quantity_alert = 0,
                            offer_additional_fields = additionalFields,
                            price = Math.Round(item.MSRP > item.UpdateSelllingPrice ? item.MSRP : item.UpdateSelllingPrice * Convert.ToDecimal(1.30), 2),
                            price_additional_info = "",
                            product_id = item.ProductId,
                            product_id_type = "SKU",
                            quantity = 10,
                            shop_sku = item.SKU,
                            state_code = "11",
                            update_delete = ""
                        };
                        List<Offers> offers1 = new List<Offers>();
                        offers1.Add(offers);
                        BestBuyPriceJobSCViewModel bestBuyPrice = new BestBuyPriceJobSCViewModel() { offers = offers1 };
                        string ImportId = UpdatePriceOnBestBuy(_getChannelCredViewModel.Key, bestBuyPrice);
                        //string ImportId = "xyz";
                        zincWathchlistDataAccess.SaveBestBuyUpdateLogs(item, JobID, ImportId);
                        //Code here for submission sp
                        UpdateImportIdInZincLogViewModel model = new UpdateImportIdInZincLogViewModel();
                        model.SKU = SKUsForJob.FirstOrDefault().SKU;
                        model.ImportId = Convert.ToInt32(ImportId);
                        model.price = bestBuyPrice.offers.FirstOrDefault().discount.price;
                        model.JobID = JobID;
                        model.ZincJobID = SKUsForJob.FirstOrDefault().ZincJobID;
                        zincWathchlistDataAccess.UpdateImportIdInZincLog(model);
                    }
                    int jobDataCount = zincWathchlistDataAccess.GetNotCompletedTimeJobCount(JobID);
                    if (jobDataCount == 0) {
                        zincWathchlistDataAccess.BestBuyUpdateJobUpdateEndTime(JobID);
                    }
                    await Task.CompletedTask;
                }
                catch (Exception exp)
                {   
                    throw exp;
                }
            }
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
                request.Headers["Authorization"] = ZincUserName;
                //request.Credentials = new NetworkCredential(ZincUserName, "");
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
