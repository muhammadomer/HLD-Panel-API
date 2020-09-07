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
    public class CatageorySub2Controller : ControllerBase
    {
        CatageorySub2DataAccess _dataAccess;
        public CatageorySub2Controller(IConnectionString connectionString)
        {
            _dataAccess = new CatageorySub2DataAccess(connectionString);
        }

        [HttpGet]
        [Authorize]
        [Route("api/CatageorySub2/{id}")]
        public IActionResult Get(int id)
        {
            List<CatageorySub2ViewModel> _ViewModels = null;

            _ViewModels = _dataAccess.GetAllCatageorySub2_BySub1ID(id);

            if (_ViewModels == null)
            {
                return Ok(new List<CatageorySub2ViewModel>());
            }
            else
            {
                return Ok(_ViewModels);
            }
        }

        [HttpGet]
        [Authorize]
        [Route("api/CatageorySub2/ById/{id}")]
        public IActionResult GetById(int id)
        {
             CatageorySub2ViewModel  _ViewModels = null;

            _ViewModels = _dataAccess.GetAllCatageorySub2_ByID(id);

            if (_ViewModels == null)
            {
                return Ok(new List<CatageorySub2ViewModel>());
            }
            else
            {
                return Ok(_ViewModels);
            }
        }

        [HttpPut]
        [Authorize]
        [Route("api/CatageorySub2/Update")]
        public IActionResult Put(CatageorySub2ViewModel ViewModel)
        {
            bool status = false;
            if (_dataAccess.UpdateCatageorySub2(ViewModel))
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
        [Route("api/CatageorySub2/CheckExists/{name}")]
        public IActionResult CheckCategorySub2Exists(string name)
        {
            bool status = false;
            if (_dataAccess.CheckCategorySub2Exists(name))
            {
                status = true;
                return Ok(new { Status = status, Message = "Catgeory 2 exists ,please select another" });
            }
            else
            {
                return Ok(new { Status = status, Message = "Catgeory 2 Not Exists" });
            }
        }

        [HttpGet]
        [Authorize]
        [Route("api/CatageorySub2/Delete")]
        public IActionResult Delete(int id)
        {
            bool status = false;
            if (_dataAccess.DeleteCatageorySub2(id))
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
        [Route("api/CatageorySub2/save")]
        public IActionResult Post(CatageorySub2ViewModel ViewModel)
        {
            bool status = false;
            if (_dataAccess.SaveCatageorySub2(ViewModel))
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