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
    public class ChannelsController : ControllerBase
    {
        ChannelsDataAccess DataAccess;
        ChannelDecrytionDataAccess decDataAccess;

        public ChannelsController(IConnectionString connectionString)
        {
            DataAccess = new ChannelsDataAccess(connectionString);
            decDataAccess = new ChannelDecrytionDataAccess(connectionString);
        }
        [HttpPost]
        [Authorize]
        [Route("save")]
        public IActionResult Post(UpdateChannelsViewModel ViewModels)
        {
            bool status = false;
            if (DataAccess.UpdateChannelsCred(ViewModels))
            {
                status = true;
                return Ok(status);
            }
            else
            {
                return Ok(new { Status = status });
            }
        }

        [HttpGet]
        [Authorize]
        [Route("{Method}")]
        public IActionResult GetAllLogsByMethod(string Method)
        {
            List<ChannelLogs> ViewModels = null;

            ViewModels = DataAccess.GetChannelsLogs(Method);

            if (ViewModels == null)
            {
                return Ok(null);
            }
            else
            {
                return Ok(ViewModels);
            }
        }


        [HttpGet]
        [Authorize]
        [Route("dec/{Method}")]
        public IActionResult GetDECdata(string Method)
        {
            GetChannelCredViewModel ViewModels = null;

            ViewModels = decDataAccess.GetChannelDec(Method);

            if (ViewModels == null)
            {
                return Ok(null);
            }
            else
            {
                return Ok(ViewModels);
            }
        }

    }
}