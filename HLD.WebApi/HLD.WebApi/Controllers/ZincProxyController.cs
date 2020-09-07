using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper.Configuration;
using DataAccess.DataAccess;
using DataAccess.Helper;
using DataAccess.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HLD.WebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ZincProxyController : ControllerBase
    {
        IConnectionString _connectionString = null;

        ZincProxyDataAccess _ZincProxyDataAccess = null;

        public ZincProxyController(IConnectionString connectionString)
        {

            _connectionString = connectionString;

            _ZincProxyDataAccess = new ZincProxyDataAccess(_connectionString);


        }
        // GET: api/<ZincProxyController>
        [HttpGet]
        public IActionResult Get()
        {
            List<GetZincProxyViewModel> Data = _ZincProxyDataAccess.GetProxyList();

            return Ok(Data);
        }

        // POST api/<ZincProxyController>
        [HttpPost]
        public IActionResult Post([FromBody] ZincProxyViewModel _ZincProxyViewModel)
        {
            bool status = _ZincProxyDataAccess.SaveProxy(_ZincProxyViewModel);
            return Ok(status);

        }

        [HttpPut]
        [Route("SetActive")]
        public IActionResult SetActive([FromBody] ProxySettingViewModal _ZincProxyViewModel)
        {
            int status = _ZincProxyDataAccess.UpdateIsActive(_ZincProxyViewModel);
            return Ok(status);

        }
        [HttpPut]
        [Route("SetDefault")]
        public IActionResult SetDefault([FromBody] ProxySettingViewModal _ZincProxyViewModel)
        {
            int status = _ZincProxyDataAccess.UpdateIsDefault(_ZincProxyViewModel);
            return Ok(status);

        }

        [HttpGet]
        [Route("GetForZinc")]
        public IActionResult GetProxyForZinc()
        {
            GetZincProxyViewModel getZincProxy = _ZincProxyDataAccess.GetProxyDefaultForZinc();
            return Ok(getZincProxy);
        }

        [HttpPost]
        [Route("SaveProxyEmail")]
        public IActionResult SaveProxyEmail([FromBody] SaveZincProxyEmailViewModel _ZincProxyViewModel)
        {
            bool status = _ZincProxyDataAccess.SaveProxyEmail(_ZincProxyViewModel);
            return Ok(status);

        }
        [HttpGet]
        [Route("GetZincProxyEmail")]
        public IActionResult GetZincProxyEmail()
        {
            List<SaveZincProxyEmailViewModel> Data = _ZincProxyDataAccess.GetZincProxyEmail();
            return Ok(Data);
        }


        [HttpGet]
        [Route("Delete/{id}")]
        public IActionResult DeleteProxyEmail(int id)
        {
            bool status = false;
            status = _ZincProxyDataAccess.DeleteProxyEmail(id);
            if (status == false)
            {
                return Ok(new { Status = false, Message = "Some error occured" });
            }
            else
            {
                return Ok(new { Status = true, Message = "Delete Successfully" });
            }

        }
        [HttpGet]
        [Route("ProxyDelete/{id}")]
        public IActionResult DeleteProxy(int id)
        {
            bool status = false;
            status = _ZincProxyDataAccess.DeleteProxy(id);
            if (status == false)
            {
                return Ok(new { Status = false, Message = "Some error occured" });
            }
            else
            {
                return Ok(new { Status = true, Message = "Delete Successfully" });
            }

        }

    }
}
