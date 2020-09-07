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
    public class ProductStatusController : Controller
    {
        ProductStatusDataAccess _DataAccess;
        public ProductStatusController(IConnectionString connectionString)
        {
            _DataAccess = new ProductStatusDataAccess(connectionString);
        }

        [HttpGet]
        [Authorize]
        [Route("api/ProductStatus/GetAllProductStatus")]
        public IActionResult Get()
        {
            List<ProductStatusViewModel> _list = null;

            _list = _DataAccess.GetAllProductStatus();
            if (_list == null)
            {
                return Ok(new List<ProductStatusViewModel>());
            }
            else
            {
                return Ok(_list);
            }
        }

        
        
    }
}