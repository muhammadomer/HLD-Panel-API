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
    public class WarehouseAddressController : ControllerBase
    {
        WarehouseAddressDateAccess _wharehouseDataAccess;
        public WarehouseAddressController(IConnectionString connectionString)
        {
            _wharehouseDataAccess = new WarehouseAddressDateAccess(connectionString);
        }
        [HttpPost]
        [Authorize]
        [Route("api/WarehouseAddress/SaveWarehouseData")]
        public IActionResult Post([FromBody]WarehouseAddressViewModel viewModels)
        {
            bool status = false;
            try
            {
                status = _wharehouseDataAccess.SaveWarehouse(viewModels);
                return Ok(status);
            }
            catch (Exception)
            {

                throw;
            }
        }
        [HttpGet]
        [Authorize]
        [Route("api/WarehouseAddress/GetAddress")]
       
        public IActionResult GetWarehouseAddresslist()
        {
            List<WarehouseAddressViewModel> tagViewModel = new List<WarehouseAddressViewModel>();
            try
            {
                tagViewModel = _wharehouseDataAccess.GetWarehouseAddresslist();
                return Ok(tagViewModel);
            }
            catch (Exception)
            {

                throw;
            }

        }

       
        [HttpGet]
        [Authorize]
        [Route("api/WarehouseAddress/{id}")]
        public IActionResult GetWHAddressById(int id)
        {
            WarehouseAddressViewModel ViewModel = new WarehouseAddressViewModel();
            try
            {
                ViewModel = _wharehouseDataAccess.GetWHAddressById(id);
                return Ok(ViewModel);
            }
            catch (Exception)
            {

                throw;
            }

        }
        [HttpPut]
        [Authorize]
        [Route("api/WarehouseAddress/Update")]
        public IActionResult Put(WarehouseAddressViewModel ViewModel)
        {
            bool status = false;
            if (_wharehouseDataAccess.UpdateWHAddress(ViewModel))
            {
                status = true;
                return Ok(new { Status = status, Message = "Updated Successfully" });
            }
            else
            {
                return Ok(new { Status = status, Message = "Error Occured" });
            }
        }

    }
}