using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccess.DataAccess;
using DataAccess.Helper;
using DataAccess.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HLD.WebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BestBuyTrackingUpdateLogController : ControllerBase
    {
        BestBuyTrackingUpdateLogDataAccess _dataAccess;
        public BestBuyTrackingUpdateLogController(IConnectionString connectionString)
        {
            _dataAccess = new BestBuyTrackingUpdateLogDataAccess(connectionString);
        }
       
        [HttpGet]
       
        public IActionResult Get()
        {
            List<BestBuyTrackingUpdate> model = _dataAccess.GetAllBestBuyUpdateLog();
            return Ok(model);
        }
       
        [HttpPost]
        
        public IActionResult GetByDynamicquery(SearchQueryViewModel query)
        {
            List<BestBuyTrackingUpdate> model = _dataAccess.GetByDynamicquery(query.query);
            return Ok(model);
        }
        [HttpGet]
        [Route("GetCounter")]
        public IActionResult GetCounter(string scOrderID,string bbOrderID,string TrakingNumber,string BBStatus,DateTime CurrentDate, DateTime PreviousDate)
        {
            long Count = 0;
            bool status = false;
            Count = _dataAccess.GetLogsCount(scOrderID, bbOrderID, TrakingNumber, BBStatus, CurrentDate, PreviousDate);
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
        [Route("getLogs")]
        public IActionResult getLogs(string DateTo, string DateFrom, int limit, int offset, string scOrderID, string bbOrderID , string TrakingNumber , string BBStatus)
        {
            List<BestBuyTrackingUpdate> model = _dataAccess.getLogs(DateTo, DateFrom, limit, offset, scOrderID, bbOrderID, TrakingNumber, BBStatus);
            return Ok(model);
        }
    }
}