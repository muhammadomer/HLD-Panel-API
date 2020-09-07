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
    [Route("api/[controller]")]
    [ApiController]
    public class AutoControlController : ControllerBase
    {
        AutoControlDataAccess _dataAccess;


        public AutoControlController(IConnectionString connectionString)
        {
            _dataAccess = new AutoControlDataAccess(connectionString);
        }
        [HttpPost]
        [Authorize]
        [Route("EnableDisable")]
        public IActionResult EnableDisableZinc([FromBody] AutoControlViewModel ViewModel)
        {
            bool status = false;
            try
            {

                status = _dataAccess.EnableDisableZincJobs(ViewModel);
                return Ok(status);
            }
            catch (Exception)
            {

                throw;
            }

        }


        [Authorize]
        [HttpGet]
        public IActionResult GetControlstatus()
        {
            List<AutoControlViewModel> ViewModel = new List<AutoControlViewModel>();
            try
            {
                ViewModel = _dataAccess.GetControls();
                return Ok(ViewModel);
            }
            catch (Exception)
            {

                throw;
            }

        }
    }
}