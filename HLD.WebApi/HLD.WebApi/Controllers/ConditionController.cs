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
    public class ConditionController : ControllerBase
    {
        ConditionDataAccess _conditionDataAccess;
        public ConditionController(IConnectionString connectionString)
        {
            _conditionDataAccess = new ConditionDataAccess(connectionString);
        }

        [HttpGet]
        [Authorize]
        [Route("api/condition")]
        public IActionResult Get()
        {
            List<ConditionViewModel> _conditionViewModels = null;

            _conditionViewModels = _conditionDataAccess.GetAllCondition();

            if (_conditionViewModels == null)
            {
                return Ok(new List<ConditionViewModel>());
            }
            else
            {
                return Ok(_conditionViewModels);
            }
        }

        [HttpGet()]
        [Authorize]
        [Route("api/condition/ById/{id}")]
        public IActionResult GetConditionById(int id)
        {
            ConditionViewModel _conditionViewModels = null;

            _conditionViewModels = _conditionDataAccess.GetConditionById(id);

            if (_conditionViewModels == null)
            {
                return Ok(new ConditionViewModel());
            }
            else
            {
                return Ok(_conditionViewModels);
            }
        }

        [HttpPost]
        [Authorize]
        [Route("api/condition/save")]
        public IActionResult Post(ConditionViewModel conditionViewModel)
        {
            bool status = false;
            if (_conditionDataAccess.SaveCondition(conditionViewModel))
            {
                status = true;
                return Ok(new { Status = status, Message = "Save Successfully" });
            }
            else
            {
                return Ok(new { Status = status, Message = "Some Error Occured" });
            }
        }

        [HttpPut]
        [Authorize]
        [Route("api/condition/Update")]
        public IActionResult Put(ConditionViewModel conditionViewModel)
        {
            bool status = false;
            if (_conditionDataAccess.UpdateCondition(conditionViewModel))
            {
                status = true;
                return Ok(new { Status = status, Message = "Save Successfully" });
            }
            else
            {
                return Ok(new { Status = status, Message = "Some Error Occured" });
            }
        }


        [HttpGet]
        [Authorize]
        [Route("api/condition/CheckExists/{name}")]
        public IActionResult CheckConditionExists(string name)
        {
            bool status = false;
            if (_conditionDataAccess.CheckConditionExists(name))
            {
                status = true;
                return Ok(new { Status = status, Message = "Condition exists ,please select another" });
            }
            else
            {
                return Ok(new { Status = status, Message = "Condition Not Exists" });
            }
        }
    }
}