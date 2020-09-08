using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccess.DataAccess;
using DataAccess.Helper;
using DataAccess.ViewModels;
using Hld.WebApplication.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HLD.WebApi.Controllers
{

    [ApiController]
    public class BestBuyProductController : ControllerBase
    {
        BestBuyProductDataAccess dataAccess;
        public BestBuyProductController(IConnectionString connectionString)
        {
            dataAccess = new BestBuyProductDataAccess(connectionString);
        }
        [HttpPost]
        [Authorize]
        [Route("api/BBProduct/save")]
        public IActionResult Post(BBProductViewModel ViewModel)
        {
            bool status = false;
            if (dataAccess.SaveBBProduct(ViewModel))
            {
                status = true;
                return Ok(new { Status = status, Message = "Save Successfully" });
            }
            else
            {
                return Ok(new { Status = status, Message = "Some Error Occured" });
            }
        }





        [HttpGet]
        [Authorize]
        [Route("api/BBProduct/{id}")]
        public IActionResult GetBestBuyProductByProductId(string id)
        {
            BBProductViewModel _ViewModels = new BBProductViewModel();

            _ViewModels = dataAccess.GetBestBuyProductByProductID(id);

            if (_ViewModels == null)
            {
                return Ok(_ViewModels);
            }
            else
            {
                return Ok(_ViewModels);
            }
        } 
        [HttpGet]
        [Authorize]
        [Route("api/BBProduct/BBSKU/{id}")]
        public IActionResult GetBestBuyProductByBBSKU(string id)
        {
            BBProductViewModel _ViewModels = new BBProductViewModel();

            _ViewModels = dataAccess.GetBestBuyProductByBBSKU(id);

            if (_ViewModels == null)
            {
                return Ok(_ViewModels);
            }
            else
            {
                return Ok(_ViewModels);
            }
        }


        [HttpGet]
        [Authorize]
        [Route("api/BBProduct")]
        public IActionResult Get()
        {
            List<BBProductViewModel> _ViewModels = null;

            _ViewModels = dataAccess.GetAllBestBuyProducts();

            if (_ViewModels == null)
            {
                return Ok(new List<BrandViewModel>());
            }
            else
            {
                return Ok(_ViewModels);
            }
        }


        [HttpGet]
        [Authorize]
        [Route("api/BBProduct/OrdersListGlobalFilter/{FilterName}/{FilterValue}/{startLimit}/{endLimit}/{sort}")]
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
        [Route("api/BBProduct/BestBuyOrdersDetailSearchingTotalCount")]
        public IActionResult BestBuyOrdersDetailSearchingTotalCount(BestBuyOrderSearchTotalCountViewModel viewModel)
        {
            TotalCountWithBestBuyOrderViewModel model = new TotalCountWithBestBuyOrderViewModel();
            model = dataAccess.GetAllBestBuyOrdersSearchTotalCount(viewModel);
            return Ok(model);
        }





        [HttpGet]
        [Authorize]
        [Route("api/BBProduct/OrdersListGlobalFilterTotalCount/{FilterName}/{FilterValue}")]
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
        [Route("api/BBProduct/BestBuyOrdersDetailSearch")]
        public IActionResult GetBestBuyOrdersTotalCount(BestBuyOrderSearchTotalCountViewModel viewModel)
        {
            List<BestBuyOrdersViewModel> _ViewModels = null;
            _ViewModels = dataAccess.GetAllBestBuyOrdersSearch(viewModel);
            return Ok(_ViewModels);
        }







        //        {search_marketplace
        //    }/{search_shipmentOrderStatus
        //}

        //string search_marketplace,string search_shipmentOrderStatus
        [HttpGet]
        [Authorize]
        [Route("api/BBProduct/OrdersList/{startLimit}/{endLimit}/{sort}")]
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
        [Route("api/BestBuyProduct/GetBestBuyUpdate/")]
        public IActionResult GetBestBuyUpdate(int Offset)// get list of job summary
        {
            List<BestBuyUpdateViewModel> WatchlistSummary = new List<BestBuyUpdateViewModel>();
            try
            {
                WatchlistSummary = dataAccess.GetBestBuyUpdate(Offset);
                return Ok(WatchlistSummary);
            }
            catch (Exception)
            {
                throw;
            }

       
    }

        [HttpGet]
        [Route("api/BestBuyProduct/getcount")]
        public IActionResult GetWatchlistSummaryCountupdate()// get list of job summary
        {
            int count = 0;
            try
            {
                count = dataAccess.GetWatchlistSummaryCountupdate();
                return Ok(count);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}