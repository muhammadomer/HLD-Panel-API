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
   // [Authorize]
    [ApiController]
    public class ShipmentCourierController : Controller
    {
        ShipmentCourierDataAccess _DataAccess;
        public ShipmentCourierController(IConnectionString connectionString)
        {
            _DataAccess = new ShipmentCourierDataAccess(connectionString);
        }
        [HttpGet("api/ShipmentCourier/Index")]
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost("api/ShipmentCourier/SaveAndEditShipmentCourier")]
        public IActionResult SaveAndEditShipmentCourier(SaveAndEditShipmentCourierVM model)
        {
            bool status = false;
            status=_DataAccess.SaveAndEditShipmentCourier(model);
            return Ok(status);
        }
        [HttpGet("api/ShipmentCourier/GetShipmentCourierById")]
        public SaveAndEditShipmentCourierVM GetShipmentCourierById(int id)
        {

            SaveAndEditShipmentCourierVM viewModel = null;
            viewModel = _DataAccess.GetShipmentCourierById(id);

            return viewModel;
        }

        [HttpGet("api/ShipmentCourier/GetShipmentCourierList")]
        public IActionResult GetShipmentCourierList()
        {
            var list = _DataAccess.GetShipmentCourierList();
            {
                return Ok(list);
            }
        }
        [HttpPost]
        //[Authorize]
        [Route("api/ShipmentCourier/save")]
        public IActionResult ShipmentCourier(ShipmentCourierViewModel ViewModel)
        {
            bool status = false;
            status = _DataAccess.SaveShipmentCourier(ViewModel);
            if (status)
            {
                return Ok(new { Status = status, Message = "Save Successfully" });
            }
            else
            {
                return Ok(new { Status = status, Message = "Some Error Occured" });
            }
        }
    }
}