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

           // status=_DataAccess.SaveProductQty(viewModels);

            ProductwareHousesViewModel viewModel = new ProductwareHousesViewModel();

            viewModel.SKU = viewModels[0].ProductSku;

            viewModel.DropShip_Canada = viewModels.Where(s => s.WarehouseID == 1).Select(s => s.AvailableQty).FirstOrDefault();
            viewModel.DropShip_USA = viewModels.Where(s => s.WarehouseID == 2).Select(s => s.AvailableQty).FirstOrDefault();
            viewModel.FBA_Canada = viewModels.Where(s => s.WarehouseID == 3).Select(s => s.AvailableQty).FirstOrDefault();
            viewModel.FBA_USA = viewModels.Where(s => s.WarehouseID == 4).Select(s => s.AvailableQty).FirstOrDefault();
            viewModel.HLD_CA1 = viewModels.Where(s => s.WarehouseID == 5).Select(s => s.AvailableQty).FirstOrDefault();
            viewModel.HLD_CA2 = viewModels.Where(s => s.WarehouseID == 6).Select(s => s.AvailableQty).FirstOrDefault();
            viewModel.HLD_CN1 = viewModels.Where(s => s.WarehouseID == 7).Select(s => s.AvailableQty).FirstOrDefault();
            viewModel.HLD_Interim = viewModels.Where(s => s.WarehouseID == 8).Select(s => s.AvailableQty).FirstOrDefault();
            viewModel.HLD_Tech1 = viewModels.Where(s => s.WarehouseID == 9).Select(s => s.AvailableQty).FirstOrDefault();
            viewModel.Interim_FBA_CA = viewModels.Where(s => s.WarehouseID == 10).Select(s => s.AvailableQty).FirstOrDefault();
            viewModel.Interim_FBA_USA = viewModels.Where(s => s.WarehouseID == 11).Select(s => s.AvailableQty).FirstOrDefault();
            viewModel.NY_14305 = viewModels.Where(s => s.WarehouseID == 12).Select(s => s.AvailableQty).FirstOrDefault();
            viewModel.Shipito = viewModels.Where(s => s.WarehouseID == 13).Select(s => s.AvailableQty).FirstOrDefault();
            _DataAccess.SaveProductWareHouses(viewModel);

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