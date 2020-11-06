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
    public class BulkUpdateController : Controller
    {
        BulkUpdateDataAccess DataAccess;
        IConnectionString _connectionString = null;

        public BulkUpdateController(IConnectionString connectionString)
        {
            DataAccess = new BulkUpdateDataAccess(connectionString);
            _connectionString = connectionString;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        [Authorize]
        [Route("api/BulkUpdate/GetBulkUpdate")]
        public List<BulkUpdateViewModel> GetBulkUpdate(List<string> shadowSku)
        {
            List<BulkUpdateViewModel> viewlList = new List<BulkUpdateViewModel>();
            try
            {
                viewlList = DataAccess.GetBulkUpdate(shadowSku);

                return viewlList;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        [HttpPut]
        [Authorize]
        [Route("api/BulkUpdate/EditBulkUpdate")]
        public IActionResult EditBulkUpdate([FromBody] EditBulkUpdateViewModel ViewModel)
        {
            bool status = false;
            status = DataAccess.EditBulkUpdate(ViewModel);
            return Ok(status);

        }
    }
}
