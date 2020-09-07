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
    public class CatageoryMainController : ControllerBase
    {
        CatageoryMainDataAccess _CatageoryMainDataAccess;
        public CatageoryMainController(IConnectionString connectionString)
        {
            _CatageoryMainDataAccess = new CatageoryMainDataAccess(connectionString);
        }

        [HttpGet]
        [Authorize]
        [Route("api/CatageoryMain")]
        public IActionResult Get()
        {
            List<CatageoryMainViewModel> _ViewModels = null;

            _ViewModels = _CatageoryMainDataAccess.GetAllCatageoryMain();

            if (_ViewModels == null)
            {
                return Ok(new List<CatageoryMainViewModel>());
            }
            else
            {
                return Ok(_ViewModels);
            }
        }


        [HttpGet]
        [Authorize]
        [Route("api/AllCategoriesForAutoComplete/{categoryName}")]
        public IActionResult GetAllCategoriesForAutoComplete(string categoryName)
        {
            List<CategoriesAutoCompleteViewModel> _ViewModels = null;

            _ViewModels = _CatageoryMainDataAccess.GetAllCatageoryForAutoComplete(categoryName);

            if (_ViewModels == null)
            {
                return Ok(new List<CategoriesAutoCompleteViewModel>());
            }
            else
            {
                return Ok(_ViewModels);
            }
        }

        [HttpGet]
        [Authorize]
        [Route("api/CatageoryMain/CheckExists/{name}")]
        public IActionResult CheckCategoryMainExists(string name)
        {
            bool status = false;
            if (_CatageoryMainDataAccess.CheckCategoryMainExists(name))
            {
                status = true;
                return Ok(new { Status = status, Message = "Catgeory 1 exists ,please select another " });
            }
            else
            {
                return Ok(new { Status = status, Message = "Catgeory 1 Not Exists" });
            }
        }


        [HttpPut]
        [Authorize]
        [Route("api/CatageoryMain/Update")]
        public IActionResult Put(CatageoryMainViewModel  ViewModel)
        {
            bool status = false;
            if (_CatageoryMainDataAccess.UpdateCatageoryMain(ViewModel))
            {
                status = true;
                return Ok(new { Status = status, Message = "Updated Successfully" });
            }
            else
            {
                return Ok(new { Status = status, Message = "Error Occured" });
            }
        }

        [HttpPost]
        [Authorize]
        [Route("api/CatageoryMain/save")]
        public IActionResult Post(CatageoryMainViewModel ViewModel)
        {
            bool status = false;
            if (_CatageoryMainDataAccess.SaveCatageoryMain(ViewModel))
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
        [Route("api/CatageoryMain/GetById/{id}")]
        public IActionResult GetCatageoryMainById(int id)
        {
            CatageoryMainViewModel ViewModels = null;
            ViewModels = _CatageoryMainDataAccess.GetCatageoryMainById(id);
            if (ViewModels == null)
            {
                return Ok(new CatageoryMainViewModel());
            }
            else
            {
                return Ok(ViewModels);
            }

        }
    }
}