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
using Microsoft.Extensions.Options;

namespace HLD.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HldPanelController : ControllerBase
    {
       
        HldPanelDataAccess hldPanelDataAccess;
        public HldPanelController(IConnectionString connectionString)
        {
            hldPanelDataAccess = new HldPanelDataAccess(connectionString);             
        }
         
        [HttpGet]
        [Authorize]
        public IActionResult Get()
        {
            return Ok(
            
                 hldPanelDataAccess.GetHldPanelOrderList(50)
             );
        }
    }
}