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
        [HttpPost]

        public IActionResult SaveWatchlist(SaveWatchlistViewModel ViewModel)
        {
            bool status = false;
            status = dataAccess.SaveWatchlist(ViewModel);

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
                            price = Convert.ToDouble(item.UpdateSelllingPrice),
                            quantity_threshold = 10
                        };
                        List<Ranxs> ranxes = new List<Ranxs>();
                        ranxes.Add(ranx);



                        Discounts discounts = new Discounts()
                        {
                            end_date = DateTime.Now.AddDays(15).ToString("o", CultureInfo.CreateSpecificCulture("de-DE")),
                            price = Convert.ToDouble(item.UpdateSelllingPrice),
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
                            price = Convert.ToInt32(Math.Round(item.MSRP > item.UpdateSelllingPrice ? item.MSRP : item.UpdateSelllingPrice * Convert.ToDecimal(1.30))),
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

                    }

                    zincWathchlistDataAccess.BestBuyUpdateJobUpdateEndTime(JobID);
                }
                catch(Exception exp)
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

    }
}