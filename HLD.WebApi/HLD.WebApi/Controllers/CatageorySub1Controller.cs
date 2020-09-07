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
    public class CatageorySub1Controller : ControllerBase
    {
        CatageorySub1DataAccess _dataAccess;
        public CatageorySub1Controller(IConnectionString connectionString)
        {
            _dataAccess = new CatageorySub1DataAccess(connectionString);
        }

        [HttpGet]
        [Authorize]
        [Route("api/CatageorySub1/{id}")]
        public IActionResult Get(int id)
        {
            List<CatageorySub1ViewModel> _ViewModels = null;

            _ViewModels = _dataAccess.GetAllCatageorySub1_ByMainID(id);

            if (_ViewModels == null)
            {
                return Ok(new List<CatageorySub1ViewModel>());
            }
            else
            {
                return Ok(_ViewModels);
            }
        }

        [HttpGet]
        [Authorize]
        [Route("api/CatageorySub1/ById/{id}")]
        public IActionResult GetById(int id)
        {
             CatageorySub1ViewModel  _ViewModels = null;

            _ViewModels = _dataAccess.GetAllCatageorySub1_ByID(id);

            if (_ViewModels == null)
            {
                return Ok(new List<CatageorySub1ViewModel>());
            }
            else
            {
                return Ok(_ViewModels);
            }
        }




        [HttpGet]
        [Authorize]
        [Route("api/CatageorySub1/Delete/{id}")]
        public IActionResult DeleteById(int id)
        {
            bool status = false;
            if (_dataAccess.DeleteCatageorySub1(id))
            {
                status = true;
                return Ok(new { Status = status, Message = "Deleted Successfully" });
            }
            else
            {
                return Ok(new { Status = status, Message = "Error Occured" });
            }
        }

        [HttpPut]
        [Authorize]
        [Route("api/CatageorySub1/Update")]
        public IActionResult Put(CatageorySub1ViewModel ViewModel)
        {
            bool status = false;
            if (_dataAccess.UpdateCatageorySub1(ViewModel))
            {
                status = true;
                return Ok(new { Status = status, Message = "Updated Successfully" });
            }
            else
            {
                return Ok(new { Status = status, Message = "Error Occured" });
            }
        }

        [HttpGet]
        [Authorize]
        [Route("api/CatageorySub1/CheckExists/{name}")]
        public IActionResult CheckCategorySub1Exists(string name)
        {
            bool status = false;
            if (_dataAccess.CheckCategorySub1Exists(name))
            {
                status = true;
                return Ok(new { Status = status, Message = "Catgeory 2 exists ,please select another" });
            }
            else
            {
                return Ok(new { Status = status, Message = "Catgeory 2 Not Exists" });
            }
        }


        [HttpPost]
        [Authorize]
        [Route("api/CatageorySub1/save")]
        public IActionResult Post(CatageorySub1ViewModel ViewModel)
        {
            bool status = false;
            if (_dataAccess.SaveCatageorySub1(ViewModel))
            {
                status = true;
                return Ok(status);
            }
            else
            {
                return Ok(new { Status = status });
            }
        }

    }
}