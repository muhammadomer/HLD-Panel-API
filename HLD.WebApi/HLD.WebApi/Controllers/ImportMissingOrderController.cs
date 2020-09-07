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
    public class ImportMissingOrderController : ControllerBase
    {
        ImportMissingOrderDataAccess dataAccess;
        public ImportMissingOrderController(IConnectionString connectionString)
        {
            dataAccess = new ImportMissingOrderDataAccess(connectionString);
        }
        [HttpPost]
       
        public IActionResult CheckOrder([FromBody] List<CheckMissingOrderViewModel> OrderList)
        {
            MissingOrderReturnViewModel missing = new MissingOrderReturnViewModel();
               missing = dataAccess.CheckOrderINDB(OrderList);
            return Ok(missing);
        }
    }
}