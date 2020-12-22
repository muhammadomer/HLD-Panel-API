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
    //[Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class ZincAccountsController : ControllerBase
    {
        ZincAccountsDataAccess _DataAccess;
        public ZincAccountsController(IConnectionString connectionString)
        {
            _DataAccess = new ZincAccountsDataAccess(connectionString);
        }
        [HttpPost]
        //[Authorize]
        [Route("api/ZincAccount/save")]
        public IActionResult Post(ZincAccountsViewModel ViewModel)
        {
            int Id = 0;
            bool status = false;
         
            status = _DataAccess.Save(ViewModel);
            return Ok(status);
        }

        [HttpGet]
        //[Authorize]
        [Route("api/ZincAccount/list")]
        public IActionResult Get()
        {
            var list = _DataAccess.GetCreditCardsList();
            {
                return Ok(
                    list
                );
            }
        }

        [HttpGet]
        //[Authorize]
        [Route("api/ZincAccount/GetAccountDetailById")]
        public IActionResult GetAccountDetailById(int Id)
        {
            var list = _DataAccess.GetAccountDetailById(Id);
            {
                return Ok(
                    list
                );
            }
        }



        [HttpPut]
        [Route("api/ZincAccount/UpdateIsActive")]
        public IActionResult UpdateIsActive(ZincAccountsViewModel Obj)
        {
            int Id = 0;
            Id = _DataAccess.UpdateIsActive(Obj);
            return Ok(Id);
        }

        [HttpPut]
        [Route("api/ZincAccount/UpdateIsDefault")]
        public IActionResult UpdateIsDefault(ZincAccountsViewModel Obj)
        {
            int Id = 0;
            Id = _DataAccess.UpdateIsDefault(Obj);
            return Ok(Id);
        }
        [HttpGet]
        [Route("api/ZincAccount/ZincAccountDetailEdit")]
        public ZincAccountsViewModel ZincAccountDetailEdit(int id)
        {

            ZincAccountsViewModel viewModel = null;
            viewModel = _DataAccess.ZincAccountDetailEdit(id);

            return viewModel;
        }
    }
}