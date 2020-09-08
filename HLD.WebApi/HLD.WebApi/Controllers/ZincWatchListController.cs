using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DataAccess.DataAccess;
using DataAccess.Helper;
using DataAccess.ViewModels;
using HLD.WebApi.Jobs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HLD.WebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]

    public class ZincWatchListController : ControllerBase
    {
        ZincWathchlistDataAccess dataAccess;
        IConnectionString _connectionString = null;
        public ZincWatchListController(IConnectionString connectionString)
        {
            _connectionString = connectionString;
            dataAccess = new ZincWathchlistDataAccess(connectionString);
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

    }
}