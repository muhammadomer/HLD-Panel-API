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
    public class BestBuyOrdersController : ControllerBase
    {
        BestBuyOrderDataAccess _DataAccess;
        BestBuyProductDataAccess _bestBuyProductDataAccess = null;
        public BestBuyOrdersController(IConnectionString connectionString)
        {
            _DataAccess = new BestBuyOrderDataAccess(connectionString);
            _bestBuyProductDataAccess = new BestBuyProductDataAccess(connectionString);
        }
        
        [HttpGet]
        [Authorize]
        [Route("api/BestBuyOrders/GetBestBuyOrderIdsFromSellerCloud")]
        public IActionResult Get()
        {
            List<string> _ViewModels = null;

            List<GetOrdersNotFromBBViewModel> OrderIds = _DataAccess.GetBestBuyOrderIdsFromSellerCloud();

            _ViewModels = OrderIds.Select(s => s.BBOrderID).ToList();

            if (_ViewModels == null)
            {
                return Ok(new List<string>());
            }
            else
            {
                return Ok(_ViewModels);
            }
        }

        [HttpGet]
        [Authorize]
        [Route("api/BestBuyOrders/GetBestBuyOrderIdsFromSellerCloudForImportImages")]
        public IActionResult GetSellerCloudIdsForImages()
        {
            List<string> _ViewModels = null;

            List<GetOrdersNotFromBBViewModel> OrderIds = _DataAccess.GetBestBuyOrderIdsFromSellerCloud();

            _ViewModels = OrderIds.Select(s => s.BBOrderID).ToList();
            if (_ViewModels == null)
            {
                return Ok(new List<string>());
            }
            else
            {
                return Ok(_ViewModels);
            }
        }


        


        [HttpPost]
        [Authorize]
        [Route("api/BestBuyOrders/SaveBestBuyOrders")]
        public IActionResult SaveBestBuyOrders([FromBody] List<BestBuyOrdersImportMainViewModel> viewModel)
        {
            bool status;

            status = _DataAccess.SaveBestBuyOrders(viewModel);
          //  status = _DataAccess.DeleteDuplicateBestBuyOrdes();
            return Ok(status);
        }

        [HttpPost]
        [Authorize]
        [Route("api/BestBuyOrders/SaveBestBuyOrderDropShipMovement")]
        public IActionResult SaveBestBuyOrdersQTY([FromBody] BestBuyDropShipQtyMovementViewModel viewModel)
        {
            bool status;

            status = _bestBuyProductDataAccess.SaveBestBuyOrderDropShipMovement(viewModel);
           
            return Ok(status);
        }
        

    }
}