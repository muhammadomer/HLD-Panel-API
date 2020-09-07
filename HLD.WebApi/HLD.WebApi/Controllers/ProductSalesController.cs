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
    public class ProductSalesController : ControllerBase
    {
        ProductSalesDataAccess _DataAccess;
      
        public ProductSalesController(IConnectionString connectionString)
        {
            _DataAccess = new ProductSalesDataAccess(connectionString);
          
        }

        [HttpGet]
        
        [Route("PredictList/{dateFrom}/{dateTo}/{startLimit}/{endLimit}/{SortColumn}/{SortType}")]
        public IActionResult GetBestBuyOrders(string dateFrom, string dateTo, int startLimit, int endLimit, string SortColumn, string SortType)
        {
            List<ProductSalesViewModel> _ViewModels = new List<ProductSalesViewModel>();

          
                _ViewModels = _DataAccess.GetProductSalesPredict(dateFrom, dateTo, startLimit, endLimit, SortColumn, SortType);
          
                return Ok(_ViewModels);
            
        }

    }
}