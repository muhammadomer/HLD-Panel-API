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
    // [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class ShipmentProductController : ControllerBase
    {
        ShipmentProductDataAccess dataAccess;
        public ShipmentProductController(IConnectionString connectionString)
        {
            dataAccess = new ShipmentProductDataAccess(connectionString);
        }

        [HttpPost]
        [Route("api/ShipmentProduct/save")]
        public IActionResult Post(ShipmentProductViewModel ViewModel)
        {

            int Id = 0;
            Id = dataAccess.SaveShipmentProduct(ViewModel);
            if (Id > 0)
            {
                return Ok(new { id = Id, Message = "Save Successfully" });
            }
            else
            {
                return Ok(new { id = Id, Message = "Some Error Occured" });
            }
        }

        [HttpGet]
        [Route("api/ShipmentProduct/GetCounter")]
        public IActionResult GetCounter(int VendorId)
        {
            long Count = 0;
            bool status = false;
            Count = dataAccess.GetShipmentProductListCount(VendorId);
            if (Count > 0)
            {
                status = true;
                return Ok(new { Status = status, counter = Count, Message = "Success" });
            }
            else
            {
                return Ok(new { Status = status, counter = Count, Message = "Some Error Occured" });
            }
        }

        [HttpGet]
        [Route("api/ShipmentProducts")]
        public IActionResult Get(int VendorId, int limit, int offSet)
        {
            var list = dataAccess.GetShipmentProductsList(VendorId, limit, offSet);
            {
                return Ok(
                    list
                );
            }
        }

        [HttpGet]
        [Route("api/ShipmentProducts/GetListByBoxId")]
        public IActionResult GetListByBoxId(string BoxId)
        {
            var Item = dataAccess.GetListByBoxId(BoxId);
            {
                return Ok(
                    Item
                );
            }
        }

        [HttpGet]
        [Route("api/ShipmentProduct/Edit/{id}")]
        public IActionResult GetApprovedPricesForedit(int id)
        {
            ShipmentProductViewModel _ViewModels = new ShipmentProductViewModel();
            _ViewModels = dataAccess.GetShipmentProductForedit(id);
            return Ok(_ViewModels);
        }

        [HttpPut]
        [Route("api/ShipmentProduct/Update")]
        public IActionResult UpdatePrice(ShipmentProductViewModel ViewModel)
        {
            var Id = dataAccess.UpdateShipmentProduct(ViewModel);
            return Ok(Id);
        }
        [HttpDelete]
        [Route("api/ShipmentProduct/Delete")]
        public IActionResult Delete(int id)
        {
            bool status = false;
            status = dataAccess.DeleteShipmentProduct(id);
            return Ok(status);
        }

        [HttpGet]
        [Route("api/ShipmentProduct/GetCounterByBarcode")]
        public IActionResult GetCounterByBarcode(string BoxId)
        {
            long Count = 0;
            bool status = false;
            Count = dataAccess.GetShipmentProductListCountByBarcode(BoxId);
            if (Count > 0)
            {
                status = true;
                return Ok(new { Status = status, counter = Count, Message = "Success" });
            }
            else
            {
                return Ok(new { Status = status, counter = Count, Message = "Some Error Occured" });
            }
        }

        [HttpGet]
        [Route("api/ShipmentProducts/GetByBarcode")]
        public IActionResult GetByBarcode(string BoxId, int limit, int offSet)
        {
            var list = dataAccess.GetShipmentProductsListByBarcode(BoxId, limit, offSet);
            {
                return Ok(
                    list
                );
            }
        }

        [HttpPut]
        [Route("api/ShipmentProduct/UpdateRecivedQty")]
        public IActionResult UpdateRecivedQty(ShipmentProductListViewModel Obj)
        {
            int Id = 0;
            Id = dataAccess.UpdateRecivedQty(Obj);
            return Ok(Id);
        }

        [HttpGet]
        [Route("api/ShipmentProduct/GetPOIID")]
        public IActionResult GetPOIID(int ShipmentProductId)
        {
            int Id = 0;
            Id = dataAccess.GetPOIID(ShipmentProductId);
            return Ok(Id);
        }

        [HttpPut]
        [Route("api/ShipmentProduct/UpdateShipmentProductInventory")]
        public IActionResult UpdateShipmentProductInventory(ShipmentProductListViewModel Obj)
        {
            int Id = 0;
            Id = dataAccess.UpdateShipmentProductInventory(Obj);
            return Ok(Id);
        }

        [HttpPut]
        [Route("api/ShipmentProduct/UpdateShipmentStatus")]
        public IActionResult UpdateShipmentStatus(ShipmentViewModel Obj)
        {
            int Id = 0;
            Id = dataAccess.UpdateShipmentStatus(Obj);
            return Ok(Id);
        }

        [HttpPut]
        [Route("api/ShipmentProduct/SetShipmentasReceived")]
        public IActionResult SetShipmentasReceived(ShipmentViewModel Obj)
        {
            int Id = 0;
            Id = dataAccess.SetShipmentasReceived(Obj);
            return Ok(Id);
        }

    }
}