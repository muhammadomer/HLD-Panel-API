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
    public class ZincOrderLogAndDetailController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        ZincOrderLogAndDetailDataAccess _DataAccess;
        public ZincOrderLogAndDetailController(IConnectionString connectionString)
        {
            _DataAccess = new ZincOrderLogAndDetailDataAccess(connectionString);
        }

        [HttpPost]
        [Authorize]
        [Route("api/ZincOrderLog/SaveZincOrderLogDetail")]
        public IActionResult SaveZincOrderLogDetail([FromBody] ZincOrderLogDetailViewModel viewModel)
        {
            int zincOrderLogDetailID = 0;
            zincOrderLogDetailID = _DataAccess.SaveZincOrderLogDetail(viewModel);
            return Ok(zincOrderLogDetailID);
        }
        [HttpPost]
        [Authorize]
        [Route("api/ZincOrderLog/SaveZincOrderLog")]
        public IActionResult SaveZincOrderLog([FromBody] ZincOrderLogViewModel viewModel)
        {
            int zincOrderLogID = 0;
            // zincOrderLogID = _DataAccess.SaveZincOrderLog(viewModel);
            zincOrderLogID = _DataAccess.SaveZincOrderLogNew(viewModel);
            return Ok(zincOrderLogID);
        }

        [HttpGet]
        [Authorize]
        [Route("api/ZincOrderLog/GetZincOrderLogDetailById/{zincOrderLogDetailID}")]
        public IActionResult GetZincOrderLogDetailById(string zincOrderLogDetailID)
        {
            ZincOrderLogDetailViewModel model = new ZincOrderLogDetailViewModel();
            model = _DataAccess.GetZincOrderLogDetailById(zincOrderLogDetailID);
            return Ok(model);
        }



    }
}