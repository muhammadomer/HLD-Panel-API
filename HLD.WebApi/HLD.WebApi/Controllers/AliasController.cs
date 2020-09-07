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
    public class AliasController : ControllerBase
    {
        AliasDataAccess _aliasDataAccess;
        public AliasController(IConnectionString connectionString)
        {
            _aliasDataAccess = new AliasDataAccess(connectionString);
        }

        [HttpGet]
        [Authorize]
        [Route("api/Alias")]
        public IActionResult Get()
        {
            List<AliasViewModel> _aliasViewModels = null;

            _aliasViewModels = _aliasDataAccess.GetAllAlias();
            if (_aliasViewModels == null)
            {
                return Ok(new List<AliasViewModel>());
            }
            else
            {
                return Ok(_aliasViewModels);
            }
        }

        [HttpPost]
        [Authorize]
        [Route("api/Alias/save")]
        public IActionResult Post(AliasViewModel aliasViewModels)
        {
            bool status = false;
            if (_aliasDataAccess.SaveAlias(aliasViewModels))
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
        [Route("api/Alias/Update")]
        public IActionResult Put(AliasViewModel aliasViewModel)
        {
            bool status = false;
            if (_aliasDataAccess.UpdateAlias(aliasViewModel))
            {
                status = true;
                return Ok(new {Status=status,Message="Updated Successfully" });
            }
            else
            {
                return Ok(new { Status = status, Message = "Error Occured" });
            }
        }

        [HttpGet]
        [Authorize]
        [Route("api/Alias/GetById/{id}")]
        public IActionResult GetAliasById(int id)
        {
            AliasViewModel _aliasViewModels = null;
            _aliasViewModels = _aliasDataAccess.GetAliasById(id);
            if (_aliasViewModels == null)
            {
                return Ok(new AliasViewModel());
            }
            else
            {
                return Ok(_aliasViewModels);
            }

        }

        [HttpGet]
        [Authorize]
        [Route("api/Alias/Delete/{id}")]
        public IActionResult DeleteBrand(int id)
        {
            bool status = false;
            status = _aliasDataAccess.DeleteAlias(id);
            if (status == false)
            {
                return Ok(new { Status = false, Message = "Some error occured" });
            }
            else
            {
                return Ok(new { Status = true, Message = "Delete Successfully" });
            }

        }

    }
}