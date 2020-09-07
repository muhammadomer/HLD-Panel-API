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
    [ApiController]
    public class DropshipEnableDisableLogController : Controller
    {
        DropShipEnableDisableLogDataAccess _DS_ED_sDataAccess;
        public DropshipEnableDisableLogController(IConnectionString connectionString)
        {
            _DS_ED_sDataAccess = new DropShipEnableDisableLogDataAccess(connectionString);
        }

        [HttpGet]
        [Authorize]
        [Route("api/DropshipEnableDisableLog/{sku}")]
        public IActionResult Get(string sku)
        {
            List<DropShipEnableDisableLogViewModel> _DS_ED_ViewModels = null;

            _DS_ED_ViewModels = _DS_ED_sDataAccess.GetAll_DS_Log(sku);
            if (_DS_ED_ViewModels == null)
            {
                return Ok(new List<DropShipEnableDisableLogViewModel>());
            }
            else
            {
                return Ok(_DS_ED_ViewModels);
            }
        }
    }
}