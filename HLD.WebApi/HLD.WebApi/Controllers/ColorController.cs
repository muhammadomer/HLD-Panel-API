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
    public class ColorController : ControllerBase
    {
        ColorDataAccess _colorDataAccess;
        public ColorController(IConnectionString connectionString)
        {
            _colorDataAccess = new ColorDataAccess(connectionString);
        }

        [HttpGet]
        [Authorize]
        [Route("api/Color")]
        public IActionResult Get()
        {
            List<ColorViewModel> _ViewModels = null;

            _ViewModels = _colorDataAccess.GetAllColor();
            if (_ViewModels == null)
            {
                return Ok(new List<ColorViewModel>());
            }
            else
            {
                return Ok(_ViewModels);
            }
        }

        [HttpPost]
        [Authorize]
        [Route("api/Color/save")]
        public IActionResult Post(ColorViewModel colorViewModel)
        {
            bool status = false;
            if (_colorDataAccess.SaveColor(colorViewModel))
            {
                status = true;
                return Ok(status);
            }
            else
            {
                return Ok(new { Status = status });
            }
        }

        [HttpPut]
        [Authorize]
        [Route("api/Color/Update")]
        public IActionResult Put(ColorViewModel colorViewModel)
        {
            bool status = false;
            if (_colorDataAccess.UpdateColor(colorViewModel))
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
        [Route("api/Color/CheckExists/{name}")]
        public IActionResult CheckColorExists(string  name)
        {
            bool status = false;
            if (_colorDataAccess.CheckColorExists(name))
            {
                status = true;
                return Ok(new { Status = status, Message = "Color exists ,please select another color name" });
            }
            else
            {
                return Ok(new { Status = status, Message = "Color Not Exists" });
            }
        }

        [HttpGet]
        [Authorize]
        [Route("api/Color/CheckColorAliasExists/{name}")]
        public IActionResult CheckColorAliasExists(string name)
        {
            bool status = false;
            if (_colorDataAccess.CheckColorAliasExists(name))
            {
                status = true;
                return Ok(new { Status = status, Message = "Color Alias exists ,please select another color name" });
            }
            else
            {
                return Ok(new { Status = status, Message = "Color Alias Not Exists" });
            }
        }

        [HttpGet]
        [Authorize]
        [Route("api/Color/GetById/{id}")]
        public IActionResult GetAliasById(int id)
        {
            ColorViewModel _colorViewModels = null;
            _colorViewModels = _colorDataAccess.GetColorById(id);
            if (_colorViewModels == null)
            {
                return Ok(new AliasViewModel());
            }
            else
            {
                return Ok(_colorViewModels);
            }

        }

        [HttpGet]
        [Authorize]
        [Route("api/Color/Delete/{id}")]
        public IActionResult DeleteBrand(int id)
        {
            bool status = false;
            status = _colorDataAccess.DeleteColor(id);
            if (status == false)
            {
                return Ok(new { Status = false, Message = "Some error occured" });
            }
            else
            {
                return Ok(new { Status = true, Message = "Delete Successfully" });
            }

        }

        [HttpGet]
        [Authorize]
        [Route("api/Color/ByName/{name}")]
        public IActionResult GetAllColorByName(string name)
        {
            List<ColorAutoCompleteViewModel> _ViewModels = null;

            _ViewModels = _colorDataAccess.GetAllColorForAutoComplete(name);

            if (_ViewModels == null)
            {
                return Ok(null);
            }
            else
            {
                return Ok(_ViewModels);
            }
        }
    }
}