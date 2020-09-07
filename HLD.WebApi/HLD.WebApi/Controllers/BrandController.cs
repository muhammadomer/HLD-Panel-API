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
    public class BrandController : ControllerBase
    {
        BrandDataAccess _brandDataAccess;
        public BrandController(IConnectionString connectionString)
        {
            _brandDataAccess = new BrandDataAccess(connectionString);
        }

        [HttpGet]
        [Authorize]
        [Route("api/Brand")]
        public IActionResult Get()
        {
            List<BrandViewModel> _brandViewModels = null;

            _brandViewModels = _brandDataAccess.GetAllBrand();

            if (_brandViewModels == null)
            {
                return Ok(new List<BrandViewModel>());
            }
            else
            {
                return Ok(_brandViewModels);
            }
        }


        [HttpGet]
        [Authorize]
        [Route("api/Brand/ByName/{name}")]
        public IActionResult GetAllBrandByName(string name)
        {
            List<BrandViewModel> _brandViewModels = null;

            _brandViewModels = _brandDataAccess.GetAllBrandByName(name);

            if (_brandViewModels == null)
            {
                return Ok(null);
            }
            else
            {
                return Ok(_brandViewModels);
            }
        }

        [HttpPost]
        [Authorize]
        [Route("api/Brand/save")]
        public IActionResult Post(BrandViewModel brandViewModel)
        {
            bool status = false;
            if (_brandDataAccess.SaveBrand(brandViewModel))
            {
                status = true;
                return Ok( new { Status = status, Message = "Save Successfully" });
            }
            else
            {
                return Ok(new { Status = status, Message = "Some Error Occured" });
            }
        }

        [HttpPut]
        [Authorize]
        [Route("api/Brand/Update")]
        public IActionResult Put(BrandViewModel brandViewModel)
        {
            bool status = false;
            if (_brandDataAccess.UpdateBrand(brandViewModel))
            {
                status = true;
                return Ok(new { Status = status, Message = "Update Successfully" });
            }
            else
            {
                return Ok(new { Status = false, Message = "Some Error Occured" });
            }
        }

        [HttpGet]
        [Authorize]
        [Route("api/Brand/GetById/{id}")]
        public IActionResult GetBrandById(int id)
        {
            BrandViewModel _brandViewModels = null;
            _brandViewModels = _brandDataAccess.GetBrandById(id);
            if (_brandViewModels == null)
            {
                return Ok(new BrandViewModel());
            }
            else
            {
                return Ok(_brandViewModels);
            }

        }

        [HttpGet]
        [Authorize]
        [Route("api/Brand/Delete/{id}")]
        public IActionResult DeleteBrand(int id)
        {
            bool status = false;
            status = _brandDataAccess.DeleteBrand(id);
            if (status == false)
            {
                return BadRequest();
            }
            else
            {
                return Ok(status);
            }

        }


        [HttpGet]
        [Authorize]
        [Route("api/Brand/CheckExists/{name}")]
        public IActionResult CheckCategorySub3Exists(string name)
        {
            bool status = false;
            if (_brandDataAccess.CheckBrandExists(name))
            {
                status = true;
                return Ok(new { Status = status, Message = "Brand exists ,please select another" });
            }
            else
            {
                return Ok(new { Status = status, Message = "Brand Not Exists" });
            }
        }



    }
}