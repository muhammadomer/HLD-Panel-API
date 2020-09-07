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
    }
}