using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccess.DataAccess;
using DataAccess.Helper;
using DataAccess.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HLD.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WareHouseQty : ControllerBase
    {
        ShipmentDataAccess _DataAccess;
        public WareHouseQty(IConnectionString connectionString)
        {
            _DataAccess = new ShipmentDataAccess(connectionString);
        }

        [HttpGet]
        //[Authorize]
        [Route("api/Shipment/save")]
        public IActionResult Post(ShipmentViewModel ViewModel)
        {
            int Id = 0;
            bool status = false;
            //ViewModel.CreatedOn = DateTime.Now.AddDays(1);
            ViewModel.ShipmentId = ViewModel.CreatedOn.ToString("yyMMdd");

            status = _DataAccess.SaveShipment(ViewModel);
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
