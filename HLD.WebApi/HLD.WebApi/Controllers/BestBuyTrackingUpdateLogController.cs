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
    }
}