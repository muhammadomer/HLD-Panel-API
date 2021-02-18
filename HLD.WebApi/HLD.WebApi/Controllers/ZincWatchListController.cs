using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using DataAccess.DataAccess;
using DataAccess.Helper;
using DataAccess.ViewModels;
using HLD.WebApi.Jobs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace HLD.WebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]

    public class ZincWatchListController : ControllerBase
    {
        ZincWathchlistDataAccess dataAccess;
        IConnectionString _connectionString = null;

        // string ZincUserName = "";
        EncDecChannel _EncDecChannel = null;
        GetChannelCredViewModel _getChannelCredViewModel = null;
        ChannelDecrytionDataAccess channelDecrytionDataAccess = null;
        ZincWathchlistDataAccess zincWathchlistDataAccess = null;
        ProductWarehouseQtyDataAccess QtyDataAccess = null;
        ZincDataAccess zincDataAccess = null;
        ProductDataAccess productDataAccess = null;
        public ZincWatchListController(IConnectionString connectionString)
        {
            _connectionString = connectionString;
            dataAccess = new ZincWathchlistDataAccess(connectionString);

            _EncDecChannel = new EncDecChannel(_connectionString);
            channelDecrytionDataAccess = new ChannelDecrytionDataAccess(_connectionString);
            zincWathchlistDataAccess = new ZincWathchlistDataAccess(_connectionString);
            zincDataAccess = new ZincDataAccess(_connectionString);
            productDataAccess = new ProductDataAccess(_connectionString);
            QtyDataAccess = new ProductWarehouseQtyDataAccess(_connectionString);
        }
       // [HttpPost]

        //public IActionResult SaveWatchlist(SaveWatchlistViewModel ViewModel)
        //{
        //    bool status = false;
        //    status = dataAccess.SaveWatchlist(ViewModel);

        //    return Ok(status);

        //}
        [HttpPost]
        public IActionResult SaveWatchlistNew(SaveWatchlistViewModel ViewModel)
        {
            bool status = false;
            status = dataAccess.SaveWatchlistNew(ViewModel);

            return Ok(status);

        }
        [HttpGet]
        [Route("{ASIN}")]
        public SaveWatchlistViewModel GetWatchlist(string ASIN)
        {
            SaveWatchlistViewModel status = new SaveWatchlistViewModel();
            status = dataAccess.GetWatchlist(ASIN);
            return status;


        }
        [HttpGet]
        [Route("GetSummaryCount")]
        public IActionResult GetWatchlistSummaryCount()// get list of job summary
        {
            int count = 0;
            try
            {
                count = dataAccess.GetWatchlistSummaryCount();
                return Ok(count);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        [Route("getsummary/{offset}")]
        public IActionResult GetWatchlistSummary(int offset)// get list of job summary
        {
            List<ZincWatchListSummaryViewModal> WatchlistSummary = new List<ZincWatchListSummaryViewModal>();
            try
            {
                WatchlistSummary = dataAccess.GetWatchlistSummary(offset);
                return Ok(WatchlistSummary);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet]
        [Route("GetlogsbyID/{JobID}")]
        public IActionResult GetWatchlistSummaryByID(int JobID)// get job summary by id
        {
            ZincWatchListSummaryViewModal WatchlistSummary = new ZincWatchListSummaryViewModal();
            try
            {
                WatchlistSummary = dataAccess.GetWatchlistSummaryByJobID(JobID);
                return Ok(WatchlistSummary);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        [Route("Getlogs")]
        public List<ZincWatchlistLogsViewModel> GetWatchlistLogs(ZincWatchLogsSearchViewModel searchViewModel)// get Logs 
        {
            List<ZincWatchlistLogsViewModel> status = new List<ZincWatchlistLogsViewModel>();
            status = dataAccess.GetWatchlistLogs(searchViewModel);
            return status;
        }

        [HttpPost]
        [Route("GetlogsCount")]
        public int GetWatchlistLogsCount(ZincWatchLogsSearchViewModel searchViewModel)// get Logs 
        {
            int status = 0;
            status = dataAccess.GetWatchlistLogsCount(searchViewModel);
            return status;
        }



        [HttpPost]
        [Route("ChangeStatus")]
        public IActionResult GetStatusResponce(ASINActiveStatusViewModel ViewModel)
        {
            bool status = false;
            status = dataAccess.GetStatusResponce(ViewModel);
            return Ok(status);
        }

        [HttpGet]
        [Route("UpdateZincJobSwitch/{Value}")]
        public IActionResult UpdateZincJobSwitch(string Value)
        {
            int Id = 0;
            Id = dataAccess.UpdateZincJobSwitch(Value);
            return Ok(Id);
        }

        [HttpGet]
        [Route("UpdateZincJobStatus/{Value}")]
        public IActionResult UpdateZincJobStatus(string Value)
        {
            int Id = 0;
            Id = dataAccess.UpdateZincJobStatus(Value);
            return Ok(Id);
        }

        [HttpGet]
        [Route("GetZincWatchListStatus")]
        public IActionResult GetZincWatchListStatus()
        {
            var item = dataAccess.GetZincWatchListStatus();
            return Ok(item);
        }
        [HttpPost]
        [Route("UpdateBestBuyForAllPages")]
        public List<ZincWatchlistLogsViewModel> UpdateBestBuyForAllPages(ZincWatchLogsSearchViewModel searchViewModel)// get Logs 
        {
            List<ZincWatchlistLogsViewModel> status = new List<ZincWatchlistLogsViewModel>();
            status = dataAccess.UpdateBestBuyForAllPages(searchViewModel);
            return status;
        }

        [HttpPost]
        [Authorize]
        [Route("SaveBestBuyUpdateList")]
        public IActionResult GetStausFromZinc([FromBody] List<ZincWatchlistLogsViewModel> ListViewModel)
        {

            var list = dataAccess.SaveBestBuyUpdateList(ListViewModel);
            Thread emailThread = new Thread(() => TaskExecute(list));
            emailThread.Start();
            return Ok(list);
        }


        [HttpGet]
        [Authorize]
        [Route("UpdateASINMaxPrice/{ASIN}/{MaxPrice}")]
        public IActionResult UpdateMaxPrice(string ASIN, double MaxPrice)
        {
            bool status = false;

            status = dataAccess.UpdateMaxPrice(ASIN, MaxPrice);


            return Ok(status);
        }

        [HttpPost]
        [Route("GetWatchlistLogsSelectAllCount")]
        public int GetWatchlistLogsSelectAllCount(ZincWatchLogsSearchViewModel searchViewModel)// get Logs 
        {
            int status = 0;
            status = dataAccess.GetWatchlistLogsSelectAllCountDA(searchViewModel);
            return status;
        }


        //[HttpGet]
        //[Route("StartZinWatchListJob")]
        //public IActionResult startJob()
        //{

        //    return Ok(0);
        //}

        //[HttpGet]
        //[Route("ZincStartWatchlistJob")]
        //public IActionResult startJobs()
        //{ 
        //    return Ok(0);
        //}


        public void TaskExecute(int job_Id)
        {

            _getChannelCredViewModel = new GetChannelCredViewModel();
            ZincWatchListSummaryViewModal zincWatchListSummary = new ZincWatchListSummaryViewModal();
            ZincWatchlistLogsViewModel zincWatchListlogs = new ZincWatchlistLogsViewModel();

            List<BestBuyUpdatePriceJobViewModel> SKUsForJob = new List<BestBuyUpdatePriceJobViewModel>();
            // get ASIN from local

            // if (ASInForJob.Count > 0)
            {
                // get zinc key
                _getChannelCredViewModel = _EncDecChannel.DecryptedData("bestbuy");
                // set job as start
                // int JobID = zincWathchlistDataAccess.GetBestBuyUpdateJobId();
                int JobID = job_Id;
                SKUsForJob = zincWathchlistDataAccess.P_GetBestBuyUpdateListForJob(JobID);
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
                            internal_description = "",
                            logistic_class = "",
                            min_quantity_alert = 0,
                            offer_additional_fields = additionalFields,
                            price = Math.Round(item.MSRP > item.UpdateSelllingPrice ? item.MSRP : item.UpdateSelllingPrice * Convert.ToDecimal(1.30),2),
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
                        model.ImportId =Convert.ToInt32( ImportId);
                        model.price = bestBuyPrice.offers.FirstOrDefault().discount.price;
                        model.JobID = JobID;
                        model.ZincJobID = SKUsForJob.FirstOrDefault().ZincJobID;
                        zincWathchlistDataAccess.UpdateImportIdInZincLog(model);
                    }

                    zincWathchlistDataAccess.BestBuyUpdateJobUpdateEndTime(JobID);
                }
                catch (Exception exp)
                {

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

        [HttpPost]
        [Route("GetCount")]
        public ZincWatchlistCountViewModel GetAllCount(ZincWatchLogsSearchViewModel searchViewModel)
        {
            ZincWatchlistCountViewModel obj = new ZincWatchlistCountViewModel();
            obj = dataAccess.GetCount(searchViewModel);
            return obj;
        }
        [HttpGet]
        [Route("GetZincCount")]
        public IActionResult GetCounter(string SC_Order_ID, string Amazon_AcName, string Zinc_Status, string CurrentDate, string PreviousDate)
        {
            long Count = 0;
            bool status = false;
            Count = zincWathchlistDataAccess.GetCounterLog(SC_Order_ID, Amazon_AcName, Zinc_Status, CurrentDate, PreviousDate);
            if (Count > 0)
            {
                status = true;
                return Ok(new { status = status, counter = Count, Message = "Success" });
            }
            else
            {
                return Ok(new { status = status, counter = Count, Message = "Some Error Occured" });
            }
        }
        [HttpGet]
        [Route("ZincOrdersLogList")]
        public IActionResult ZincOrdersLogList(string DateTo, string DateFrom, int limit, int offset, string SC_Order_ID, string Amazon_AcName, string Zinc_Status)
        {
            List<ZincOrdersLogViewModel> model = zincWathchlistDataAccess.ZincOrdersLogList(DateTo, DateFrom, limit, offset, SC_Order_ID, Amazon_AcName, Zinc_Status);
            return Ok(model);
        }
        [HttpGet]
        [Route("GetlogsCountForJob")]
        public IActionResult GetWatchlistLogsCount(string ASIN, string SKU, string available, string jobID,  string CurrentDate, string PreviousDate)// get Logs 
        {
            //int status = 0;
            //status = dataAccess.GetWatchlistLogsCount(ASIN, available, jobID, SKU, CurrentDate, PreviousDate);
            //return status;
            long Count = 0;
            bool status = false;
            Count = zincWathchlistDataAccess.GetWatchlistLogsCountForJob(ASIN, SKU, available, jobID, CurrentDate, PreviousDate);
            if (Count > 0)
            {
                status = true;
                return Ok(new { status = status, counter = Count, Message = "Success" });
            }
            else
            {
                return Ok(new { status = status, counter = Count, Message = "Some Error Occured" });
            }
        }

        [HttpGet]
        [Route("ZincOrdersLogListForJob")]
        public IActionResult ZincOrdersLogListForJob(string DateTo, string DateFrom, int limit, int offset, string ASIN, string SKU,string available, string jobID )
        {
            List<ZincWatchlistLogsForJobViewModel> model = zincWathchlistDataAccess.ZincOrdersLogListForJob(DateTo, DateFrom, limit, offset, ASIN, SKU, available, jobID);
            return Ok(model);
        }
        [HttpGet]
        [Route("GetZincWatchlistCount")]
        public IActionResult GetZincWatchlistCount(string ProductSKU, string ASIN, string Active_Inactive, string Enabled_Disabled, string CurrentDate, string PreviousDate)
        {
            long Count = 0;
            bool status = false;
            Count = zincWathchlistDataAccess.GetZincWatchlistCount(ProductSKU, ASIN, Active_Inactive, CurrentDate, PreviousDate, Enabled_Disabled);
            if (Count > 0)
            {
                status = true;
                return Ok(new { status = status, counter = Count, Message = "Success" });
            }
            else
            {
                return Ok(new { status = status, counter = Count, Message = "Some Error Occured" });
            }
        }
        [HttpGet]
        [Route("ZincWatchListDetail")]
        public IActionResult ZincWatchListDetail(string DateTo, string DateFrom, int limit, int offset, string ProductSKU, string ASIN, string Active_Inactive, string Enabled_Disabled)
        {
            List<ZincWatclistViewModel> model = zincWathchlistDataAccess.ZincWatchListDetail(DateTo, DateFrom, limit, offset, ProductSKU, ASIN, Active_Inactive, Enabled_Disabled);
            return Ok(model);
        }
        [HttpGet]      
        [Route("logHistory")]
        public IActionResult logHistory(string ProductSKU, string ASIN)
        {
            try
            {
                ZincWatclistLogsViewModel Listmodel = new ZincWatclistLogsViewModel();

                Listmodel = zincWathchlistDataAccess.logHistory(ProductSKU, ASIN);
                return Ok(Listmodel);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}