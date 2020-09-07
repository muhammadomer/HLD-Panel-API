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
    public class CatageorySub4Controller : ControllerBase
    {
        CatageorySub4DataAccess _dataAccess;
        public CatageorySub4Controller(IConnectionString connectionString)
        {
            _dataAccess = new CatageorySub4DataAccess(connectionString);
        }

        [HttpGet]
        [Authorize]
        [Route("api/CatageorySub4/ById/{id}")]
        public IActionResult GetByID(int id)
        {
            CatageorySub4ViewModel _ViewModels = null;

            _ViewModels = _dataAccess.GetAllCatageorySub4_ByID(id);

            if (_ViewModels == null)
            {
                return Ok(new List<CatageorySub4ViewModel>());
            }
            else
            {
                return Ok(_ViewModels);
            }
        }

        [HttpPut]
        [Authorize]
        [Route("api/CatageorySub4/Update")]
        public IActionResult Put(CatageorySub4ViewModel ViewModel)
        {
            bool status = false;
            if (_dataAccess.UpdateCatageorySub4(ViewModel))
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
        [Route("api/CatageorySub4/Delete")]
        public IActionResult Delete(int id)
        {
            bool status = false;
            if (_dataAccess.DeleteCatageorySub4(id))
            {
                status = true;
                return Ok(new { Status = status, Message = "Delete Successfully" });
            }
            else
            {
                return Ok(new { Status = status, Message = "Error Occured" });
            }
        }


        [HttpGet]
        [Authorize]
        [Route("api/CatageorySub4/CheckExists/{name}")]
        public IActionResult CheckCategorySub4Exists(string name)
        {
            bool status = false;
            if (_dataAccess.CheckCategorySub4Exists(name))
            {
                status = true;
                return Ok(new { Status = status, Message = "Catgeory 4 exists ,please select another" });
            }
            else
            {
                return Ok(new { Status = status, Message = "Catgeory 4 Not Exists" });
            }
        }


        [HttpGet]
        [Authorize]
        [Route("api/CatageorySub4/{id}")]
        public IActionResult Get(int id)
        {
            List<CatageorySub4ViewModel> _ViewModels = null;

            _ViewModels = _dataAccess.GetAllCatageorySub4_BySub3ID(id);

            if (_ViewModels == null)
            {
                return Ok(new List<CatageorySub4ViewModel>());
            }
            else
            {
                return Ok(_ViewModels);
            }
        }
        [HttpPost]
        [Authorize]
        [Route("api/CatageorySub4/save")]
        public IActionResult Post(CatageorySub4ViewModel ViewModel)
        {
            bool status = false;
            if (_dataAccess.SaveCatageorySub4(ViewModel))
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