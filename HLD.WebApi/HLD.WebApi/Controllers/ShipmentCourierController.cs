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
            _DataAccess.SaveAndEditShipmentCourier(model);
            return Ok();
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
    }
}