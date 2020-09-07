using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccess.DataAccess;
using DataAccess.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DataAccess.ViewModels;
using System.Data;

namespace HLD.WebApi.Controllers
{
    public class ProductWarehouseQtyController : Controller
    {
        ProductWarehouseQtyDataAccess _DataAccess;
        public ProductWarehouseQtyController(IConnectionString connectionString)
        {
            _DataAccess = new ProductWarehouseQtyDataAccess(connectionString);
        }
        [HttpPost]
        [Authorize]
        [Route("api/ProductWarehouseQty/SaveProductQty")]
        public IActionResult SaveDetail([FromBody] List<ProductWarehouseQtyViewModel> viewModels)
        {
            bool status = false;
            status=_DataAccess.SaveProductQty(viewModels);
            return Ok(status);
        }


        [HttpPost]
        [Authorize]
        [Route("api/ProductWarehouseQty/GetProductWarehouseQtyFromDatabase")]
        public IActionResult showDetail([FromBody] ProductWarehouseQtyViewModel viewModels)
        {
            List< ProductWarehouseQtyViewModel > models= _DataAccess.GetProductQtyBySKU(viewModels.ProductSku);
            return Ok(models);
        }


        [HttpPost]
        [Authorize]
        [Route("api/ProductWarehouseQty/SaveBestBuyQtyMovementForDropshipNone_SKU")]
        public IActionResult showDetail([FromBody] BestBuyDropShipQtyMovementViewModel viewModels)
        {
            bool status = false;
            status = _DataAccess.SaveBestBuyQtyMovementForDropshipNone_SKU(viewModels);
            return Ok(status);
        }

        

    }
}