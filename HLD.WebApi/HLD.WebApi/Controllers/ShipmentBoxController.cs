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
    //[Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class ShipmentBoxController : ControllerBase
    {
        ShipmentBoxDataAccess _DataAccess;
        public ShipmentBoxController(IConnectionString connectionString)
        {
            _DataAccess = new ShipmentBoxDataAccess(connectionString);
        }
        [HttpPost]
        [Route("api/ShipmentBox/save")]
        public IActionResult Post(ShipmentBoxViewModel ViewModel)
        {
            string Id = "";
            bool status = false;
            Id = _DataAccess.SaveShipmentBox(ViewModel);
            if (Id != "")
            {
                return Ok(new { Status = status, Id = Id, Message = "Save Successfully" });
            }
            else
            {
                return Ok(new { Status = status, Id = Id, Message = "Some Error Occured" });
            }
        }
        [HttpPut]
        [Route("api/ShipmentBox/Update")]
        public IActionResult Put(ShipmentBoxViewModel ViewModel)
        {
            int Id = 0;
            bool status = false;
            status = _DataAccess.UpdateShipmentBox(ViewModel);
            if (status)
            {
                return Ok(new { Status = status, Message = "Save Successfully" });
            }
            else
            {
                return Ok(new { Status = status, Message = "Some Error Occured" });
            }
        }


        [HttpGet]
        [Route("api/ShipmentBoxs")]
        public IActionResult Get(int VendorId, int limit, int offSet)
        {
            var list = _DataAccess.GetShipmentBoxList(VendorId, limit, offSet);
            {
                return Ok(
                    list
                );
            }

        }

        [HttpGet]
        [Route("api/ShipmentBoxs/GetCounter")]
        public IActionResult GetCounter(int VendorId)
        {
            long Count = 0;
            bool status = false;
            Count = _DataAccess.GetShipmentBoxListCount(VendorId);
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
        [HttpDelete]
        [Route("api/ShipmentBox/Delete")]
        public IActionResult Delete(string Id)
        {
            bool status = _DataAccess.DeleteBox(Id);
            if (status)
            {
                return Ok(new { Status = status, Message = "Save Successfully" });
            }
            else
            {
                return Ok(new { Status = status, Message = "Some Error Occured" });
            }
        }

        [HttpGet]
        [Route("api/ShipmentBox/GetBoxDetailById")]
        public IActionResult GetBoxDetailById(string BoxId)
        {
            var Item = _DataAccess.GetBoxDetailById(BoxId);
            {
                return Ok(
                    Item
                );
            }

        }
    }
}