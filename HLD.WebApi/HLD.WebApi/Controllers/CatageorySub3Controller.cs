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
    public class CatageorySub3Controller : ControllerBase
    {
        CatageorySub3DataAccess _dataAccess;
        public CatageorySub3Controller(IConnectionString connectionString)
        {
            _dataAccess = new CatageorySub3DataAccess(connectionString);
        }

        [HttpGet]
        [Authorize]
        [Route("api/CatageorySub3/{id}")]
        public IActionResult Get(int id)
        {
            List<CatageorySub3ViewModel> _ViewModels = null;

            _ViewModels = _dataAccess.GetAllCatageorySub3_BySub2ID(id);

            if (_ViewModels == null)
            {
                return Ok(new List<CatageorySub3ViewModel>());
            }
            else
            {
                return Ok(_ViewModels);
            }
        }

        [HttpGet]
        [Authorize]
        [Route("api/CatageorySub3/ById/{id}")]
        public IActionResult GetById(int id)
        {
             CatageorySub3ViewModel  _ViewModels = null;

            _ViewModels = _dataAccess.GetAllCatageorySub3_ByID(id);

            if (_ViewModels == null)
            {
                return Ok(new List<CatageorySub3ViewModel>());
            }
            else
            {
                return Ok(_ViewModels);
            }
        }

        [HttpPut]
        [Authorize]
        [Route("api/CatageorySub3/Update")]
        public IActionResult Put(CatageorySub3ViewModel ViewModel)
        {
            bool status = false;
            if (_dataAccess.UpdateCatageorySub3(ViewModel))
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
        [Route("api/CatageorySub3/CheckExists/{name}")]
        public IActionResult CheckCategorySub3Exists(string name)
        {
            bool status = false;
            if (_dataAccess.CheckCategorySub3Exists(name))
            {
                status = true;
                return Ok(new { Status = status, Message = "Catgeory 3 exists ,please select another" });
            }
            else
            {
                return Ok(new { Status = status, Message = "Catgeory 3 Not Exists" });
            }
        }


        [HttpGet]
        [Authorize]
        [Route("api/CatageorySub3/Delete")]
        public IActionResult Delete(int id)
        {
            bool status = false;
            if (_dataAccess.DeleteCatageorySub3(id))
            {
                status = true;
                return Ok(new { Status = status, Message = "Delete Successfully" });
            }
            else
            {
                return Ok(new { Status = status, Message = "Error Occured" });
            }
        }


        [HttpPost]
        [Authorize]
        [Route("api/CatageorySub3/save")]
        public IActionResult Post(CatageorySub3ViewModel ViewModel)
        {
            bool status = false;
            if (_dataAccess.SaveCatageorySub3(ViewModel))
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