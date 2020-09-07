using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccess.DataAccess;
using DataAccess.Helper;
using DataAccess.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HLD.WebApi.Controllers
{
    [ApiController]
    public class HLDHistoryController : Controller
    {
        HLDHistoryDataAccess dataAccess;
        public HLDHistoryController(IConnectionString connectionString)
        {
            dataAccess = new HLDHistoryDataAccess(connectionString);
        }

        [HttpGet]
        [Authorize]
        [Route("api/HldHistory/GetSKU_OrderHistoryBy_SKU/{productSKU}")]
        public IActionResult Get(string productSKU)
        {
            List<SKUSalesHistoryFromOrders> _list = null;

            _list = dataAccess.GetSKU_OrderHistoryBy_SKU(productSKU);
            if (_list == null)
            {
                return Ok(new List<SKUSalesHistoryFromOrders>());
            }
            else
            {
                return Ok(_list);
            }
        }

        [HttpGet]
        [Authorize]
        [Route("api/HldHistory/GetSkuProfitHistory/{productSKU}")]
        public IActionResult GetSkuProfitHistory(string productSKU)
        {
            List<Order_SKU_ProfitHistory_CalculationViewmodel> _list = null;

            _list = dataAccess.GetProfitHistoryDetailByDate(productSKU);
            if (_list == null)
            {
                return Ok(new List<Order_SKU_ProfitHistory_CalculationViewmodel>());
            }
            else
            {
                return Ok(_list);
            }
        }

        [HttpGet]
        [Authorize]
        [Route("api/HldHistory/GetSlaesHistoryForDashBoardByDate")]
        public IActionResult GetSlaesHistoryForDashBoardByDate()
        {
            List<Order_SKU_ProfitHistory_CalculationViewmodel> _list = null;

            _list = dataAccess.GetSalesProfitHistoryDashBoard();
            if (_list == null)
            {
                return Ok(new List<Order_SKU_ProfitHistory_CalculationViewmodel>());
            }
            else
            {
                return Ok(_list);
            }
        }

        [HttpPost]
        [Authorize]
        [Route("api/HldHistory/GetSlaesHistoryForDashBoardCustomRange")]
        public IActionResult GetSlaesHistoryForDashBoardByDate([FromBody] SalesHistoryDashboardSearchViewModel model)
        {

            Order_SKU_ProfitHistory_CalculationViewmodel modelData = new Order_SKU_ProfitHistory_CalculationViewmodel();

            modelData = dataAccess.GetSalesProfitHistoryDashBoard(model.DateFrom, model.DateTo);
            return Ok(modelData);
        }

    }
}