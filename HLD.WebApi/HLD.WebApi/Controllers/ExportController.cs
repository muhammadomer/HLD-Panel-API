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
    [Route("api/[controller]")]
    [ApiController]
    public class ExportController : ControllerBase
    {
        ExportSkuImgUrlDataAccess _ExportDataAccess;
        public ExportController(IConnectionString connectionString)
        {
            _ExportDataAccess = new ExportSkuImgUrlDataAccess(connectionString);
        }
        [HttpGet]
       
        [Route("ExportSkuImgUrl")]
        public IActionResult Get()
        {
            List<ExportSkuImgUrlViewModel> _list = null;
            _list = _ExportDataAccess.ExportSkuImgUrl();
            if (_list == null)
            {
                return Ok(new List<ExportSkuImgUrlViewModel>());
            }
            else
            {
                return Ok(_list);
            }
        }
    }
}
