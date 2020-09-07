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
    [Authorize]
    //[Route("api/[controller]")]
    [ApiController]
    public class ShipmentCasePackController : ControllerBase
    {
        ShipmentCasePackDataAccess _DataAccess;
        public ShipmentCasePackController(IConnectionString connectionString)
        {
            _DataAccess = new ShipmentCasePackDataAccess(connectionString);
        }

        [HttpPost]
        //[Authorize]
        [Route("api/ShipmentCasePackProduct/saveCasePack")]
        public IActionResult Post(ShipmentCasePackProductViewModel ViewModel)
        {
            int Id = 0;
            Id = _DataAccess.Save(ViewModel);
            return Ok(Id);
        }

        [HttpPut]
        //[Authorize]
        [Route("api/ShipmentCasePackProduct/Update")]
        public IActionResult Update(ShipmentCasePackProductViewModel ViewModel)
        {
            int Id = 0;
            Id = _DataAccess.Update(ViewModel);
            return Ok(Id);
        }

        [HttpGet]
        //[Authorize]
        [Route("api/ShipmentCasePackProduct/GetTemplateCasePack")]
        public IActionResult Get(int VendorId, string SKU)
        {
            var list = _DataAccess.GetTemplateCasePack(VendorId, SKU);
            {
                return Ok(
                    list
                );
            }

        }

        [HttpGet]
        //[Authorize]
        [Route("api/ShipmentCasePackProduct/GetShipmentCasePackProducts")]
        public IActionResult GetShipmentCasePackProducts(string ShipmentId)
        {
            var list = _DataAccess.GetShipmentCasePackProducts(ShipmentId);
            {
                return Ok(
                    list
                );
            }

        }

        [HttpGet]
        //[Authorize]
        [Route("api/ShipmentCasePackProduct/GetShipmentCasePackProductHeader")]
        public IActionResult GetShipmentCasePackProductHeader(string ShipmentId)
        {
            var Item = _DataAccess.GetShipmentCasePackProductHeader(ShipmentId);
            {
                return Ok(
                    Item
                );
            }
        }

        [HttpDelete]
        //[Authorize]
        [Route("api/ShipmentCasePackProduct/Delete")]
        public IActionResult Delete(int Id)
        {
            var list = _DataAccess.Delete(Id);
            {
                return Ok(
                    list
                );
            }

        }

        [HttpGet]
        //[Authorize]
        [Route("api/ShipmentCasePackProduct/GetShipmentViewCasePackHeader")]
        public IActionResult GetShipmentViewCasePackHeader(string ShipmentId)
        {
            var Item = _DataAccess.GetShipmentViewCasePackHeader(ShipmentId);
            {
                return Ok(
                    Item
                );
            }
        }

        [HttpGet]
        //[Authorize]
        [Route("api/ShipmentCasePackProduct/GetShipmentViewProductCasPackList")]
        public IActionResult GetShipmentViewProductCasPackList(string ShipmentId)
        {
            var Item = _DataAccess.GetShipmentViewProductCasPackList(ShipmentId);
            {
                return Ok(
                    Item
                );
            }
        }


        [HttpPost]
        //[Authorize]
        [Route("api/ShipmentCasePackProduct/saveCasePackTemplate")]
        public IActionResult PostSaveShipmentSKUCasePackTemplate(CasePackViewModel ViewModel)
        {
            int Id = 0;
            Id = _DataAccess.SaveShipmentSKUCasePackTemplate(ViewModel);
            return Ok(Id);
        }


        [HttpGet]
        //[Authorize]
        [Route("api/ShipmentCasePackProduct/GetTemplateCasePackCount")]
        public IActionResult GetTemplateCasePackCount(int VendorId, string SKU, string Title)
        {
            var list = _DataAccess.GetTemplateCasePackCount(VendorId, SKU, Title);
            {
                return Ok(
                    list
                );
            }
        }

        [HttpGet]
        //[Authorize]
        [Route("api/ShipmentCasePackProduct/GetTemplateCasePackList")]
        public IActionResult GetTemplateCasePackList(int VendorId, string SKU, string Title, int limit, int offset)
        {
            var list = _DataAccess.GetTemplateCasePackList(VendorId, SKU, Title, limit, offset);
            {
                return Ok(
                    list
                );
            }
        }

        [HttpDelete]
        //[Authorize]
        [Route("api/ShipmentCasePackProduct/DeleteCasePack")]
        public IActionResult DeleteCasePack(int Id)
        {
            var list = _DataAccess.DeleteCasePack(Id);
            {
                return Ok(
                    list
                );
            }

        }



    }
}