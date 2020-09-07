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
    public class OrderRelationController : ControllerBase
    {
        OrderRelationDataAccess _DataAccess;
        public OrderRelationController(IConnectionString connectionString)
        {
            _DataAccess = new OrderRelationDataAccess(connectionString);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Post(OrderRelationViewModel ViewModels)
        {
            bool status = false;
            if (_DataAccess.InsertOrderRelation(ViewModels))
            {
                status = true;
                return Ok(status);
            }
            else
            {
                return Ok(new { Status = status });
            }
        }
        [HttpPost]
        [Authorize]
        [Route("ForJob")]
        public IActionResult SaveForJobs([FromBody] List<OrderRelationToSaveViewModel> ViewModels)
        {
            JobIdReturnViewModel status = new JobIdReturnViewModel();
            status= _DataAccess.InsertChildOrderForJob(ViewModels);
            return Ok(status);
           
        }
        [HttpGet]
        [Authorize]
        [Route("{OrderId}")]
        public IActionResult Get(int OrderId)
        {
            List<RelatedOrderModel> list = new List<RelatedOrderModel>();
            list = _DataAccess.GetChildOrderByParentID(OrderId);
            if (list == null)
            {
                return Ok(new List<RelatedOrderModel>());
            }
            else
            {
                return Ok(list);
            }
        }

    }
}