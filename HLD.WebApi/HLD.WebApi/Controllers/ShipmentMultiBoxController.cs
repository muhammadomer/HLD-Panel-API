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
    //[Route("api/[controller]")]
    [ApiController]
    public class ShipmentMultiBoxController : ControllerBase
    {
        ShipmentProductDataAccess dataAccess;
        public ShipmentMultiBoxController(IConnectionString connectionString)
        {
            dataAccess = new ShipmentProductDataAccess(connectionString);
        }

        [HttpPost]
        //[Authorize]
        [Route("api/ShipmentMultiBox/save")]
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
    }
}