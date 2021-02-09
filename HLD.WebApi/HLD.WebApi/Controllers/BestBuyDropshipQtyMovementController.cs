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
    public class BestBuyDropshipQtyMovementController : ControllerBase
    {

        BestBuyProductQtyMovementDataAcces _dataAccess;
        public BestBuyDropshipQtyMovementController(IConnectionString connectionString)
        {
            _dataAccess = new BestBuyProductQtyMovementDataAcces(connectionString);
        }

        [Route("api/BestBuyDropshipQtyMovement/GetListOfAllSKU")]
        public IActionResult Get()
        {
            List<BestBuyQTYLogsDetailViewModel> model = _dataAccess.GetAllBestBuyQtyMovementDetail();
            return Ok(model);
        }
        [HttpPost]
        [Route("api/BestBuyDropshipQtyMovement/GetByQuery")]
        public IActionResult GetByQuery(SearchQueryViewModel query)
        {
            List<BestBuyQTYLogsDetailViewModel> model = _dataAccess.GetAllBestBuyQtyQuery(query.query);
            return Ok(model);
        }
        [HttpGet]
        [Route("GetCounter")]
        public IActionResult GetCounter(string product_sku, string ds_status, string BBProductID, string CurrentDate, string PreviousDate, string update_status)
        {
            long Count = 0;
            bool status = false;
            Count = _dataAccess.GetLogsCount(product_sku, ds_status, BBProductID, CurrentDate, PreviousDate, update_status);
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
        [Route("DropshipQtyList")]
        public IActionResult DropshipQtyList(string DateTo, string DateFrom, int limit, int offset, string product_sku, string ds_status, string BBProductID,string update_status)
        {
            List<BestBuyQTYLogsDetailViewModel> model = _dataAccess.DropshipQtyList(DateTo, DateFrom, limit, offset, product_sku, ds_status, BBProductID, update_status);
            return Ok(model);
        }
    }
}