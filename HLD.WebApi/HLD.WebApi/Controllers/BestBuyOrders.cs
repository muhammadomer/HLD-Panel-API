using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccess.DataAccess;
using DataAccess.Helper;
using DataAccess.ViewModels;
using Hld.WebApplication.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HLD.WebApi.Controllers
{
    [ApiController]
    public class BestBuyOrders : Controller
    {
        BestBuyOrdersDataAccessNew dataAccess;
        public BestBuyOrders(IConnectionString connectionString)
        {
            dataAccess = new BestBuyOrdersDataAccessNew(connectionString);
        }
        [HttpGet]
        [Authorize]
        [Route("api/BestBuyOrders/Index")]
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        [Authorize]
        [Route("api/BestBuyOrders/OrdersListGlobalFilterTotalCount/{FilterName}/{FilterValue}")]
        public IActionResult GetBestBuyOrdersTotalCount(string FilterName, string FilterValue)
        {
            TotalCountWithBestBuyOrderViewModel model = new TotalCountWithBestBuyOrderViewModel();

            if (!string.IsNullOrEmpty(FilterName) & !string.IsNullOrEmpty(FilterValue))
            {
                model = dataAccess.GetAllBestBuyOrdersTotalCount(FilterName, FilterValue);
            }
            if (FilterName == "NoFilter" && FilterValue == "NoValue")
            {
                model = dataAccess.GetAllBestBuyOrdersTotalCount(FilterName, FilterValue);
            }
            return Ok(model);
        }
        [HttpPost]
        [Authorize]
        [Route("api/BestBuyOrders/BestBuyOrdersDetailSearchingTotalCount")]
        public IActionResult BestBuyOrdersDetailSearchingTotalCount(BestBuyOrderSearchTotalCountViewModel viewModel)
        {
            TotalCountWithBestBuyOrderViewModel model = new TotalCountWithBestBuyOrderViewModel();
            model = dataAccess.GetAllBestBuyOrdersSearchTotalCount(viewModel);
            return Ok(model);
        }
        [HttpGet]
        [Authorize]
        [Route("api/BestBuyOrders/OrdersListGlobalFilter/{FilterName}/{FilterValue}/{startLimit}/{endLimit}/{sort}")]
        public IActionResult GetBestBuyOrders(string FilterName, string FilterValue, int startLimit, int endLimit, string sort)
        {
            List<BestBuyOrdersViewModel> _ViewModels = null;

            if (!string.IsNullOrEmpty(FilterName) & !string.IsNullOrEmpty(FilterValue))
            {
                _ViewModels = dataAccess.GetAllBestBuyOrdersWithGlobalFilter(FilterName, FilterValue, startLimit, endLimit, sort);
            }
            else
            {
                _ViewModels = dataAccess.GetAllBestBuyOrders(startLimit, endLimit, sort);
            }

            if (_ViewModels == null)
            {
                return Ok(new List<BestBuyOrdersViewModel>());
            }
            else
            {
                return Ok(_ViewModels);
            }
        }

        [HttpPost]
        [Authorize]
        [Route("api/BestBuyOrders/BestBuyOrdersDetailSearch")]
        public IActionResult GetBestBuyOrdersTotalCount(BestBuyOrderSearchTotalCountViewModel viewModel)
        {
            List<BestBuyOrdersViewModel> _ViewModels = null;
            _ViewModels = dataAccess.GetAllBestBuyOrdersSearch(viewModel);
            return Ok(_ViewModels);
        }
        //string search_marketplace,string search_shipmentOrderStatus
        [HttpGet]
        [Authorize]
        [Route("api/BestBuyOrders/OrdersList/{startLimit}/{endLimit}/{sort}")]
        public IActionResult GetBestBuyOrders(int startLimit, int endLimit, string sort)
        {
            List<BestBuyOrdersViewModel> _ViewModels = null;

            _ViewModels = dataAccess.GetAllBestBuyOrders(startLimit, endLimit, sort);

            if (_ViewModels == null)
            {
                return Ok(new List<BestBuyOrdersViewModel>());
            }
            else
            {
                return Ok(_ViewModels);
            }
        }

        [HttpGet]
        [Authorize]
        [Route("api/BestBuyOrders/BBOrderViewOrderDetail/{BBOrderID}")]
        public IActionResult GetBestBuyOrders(string BBOrderID)
        {
            BestBuyOrdersViewPageModel _ViewModels = null;

            _ViewModels = dataAccess.SCOrderPageViewOrderDetails(BBOrderID);

            return Ok(_ViewModels);

        }
    }
}
