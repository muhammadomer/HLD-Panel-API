using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccess.ViewModels;
using DataAccess.DataAccess;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DataAccess.Helper;
using Microsoft.AspNetCore.Authorization;

namespace HLD.WebApi.Controllers
{
    [ApiController]
    public class ApprovedPriceController : ControllerBase
    {
        ApprovedPriceDataAccess dataAccess;
        ProductWarehouseQtyDataAccess ProductWHQtyDataAccess;
        public ApprovedPriceController(IConnectionString connectionString)
        {
            dataAccess = new ApprovedPriceDataAccess(connectionString);
            ProductWHQtyDataAccess = new ProductWarehouseQtyDataAccess(connectionString);
        }

        [HttpPost]
        [Authorize]
        [Route("api/ApprovedPrice/save")]
        public IActionResult Post(ApprovedPriceViewModel ViewModel)
        {
            int Id = 0;
            bool status = false;
            Id = dataAccess.SaveApprovedPrice(ViewModel);
            if (Id > 0)
            {
                status = true;
                return Ok(new { Status = status, ApprovedPriceID = Id, Message = "Save Successfully" });
            }
            else
            {
                return Ok(new { Status = status, ApprovedPriceID = Id, Message = "Some Error Occured" });
            }
        }

        [HttpGet]
        [Authorize]
        [Route("api/ApprovedPrice/GetCounter")]
        public IActionResult GetCounter(int VendorId, string SKU, string title, string skuList)
        {
            long Count = 0;
            bool status = false;
            Count = dataAccess.GetApprovedPriceCount(VendorId, SKU, title, skuList);
            if (Count > 0)
            {
                status = true;
                return Ok(new { Status = status, counter = Count, Message = "Success" });
            }
            else
            {
                return Ok(new { Status = status, counter = Count, Message = "Some Error Occured" });
            }
        }

        [HttpGet]
        [Authorize]
        [Route("api/ApprovedPrice")]
        public IActionResult Get(int VendorId, int limit, int offSet, string SKU, string Title, string skuList)
        {
            //bool _status = false;
            //_status = dataAccess.updateSCImageStatusInProductTable(Sku, status);
            var list = dataAccess.GetApprovedPricesList(VendorId, limit, offSet, SKU, Title, skuList);
            {
                return Ok(
                    list
                );
            }

        }
        [HttpGet]
        [Authorize]
        [Route("api/ApprovedPrice/Logs")]
        public IActionResult GetPriceLOgs(int VendorId, string SKU)
        {
            //bool _status = false;
            //_status = dataAccess.updateSCImageStatusInProductTable(Sku, status);
            var list = dataAccess.GetApprovedPricesLog(VendorId, SKU);
            {
                return Ok(
                    list
                );
            }

        }
        [HttpGet]
        [Authorize]
        [Route("api/ApprovedPrice/Vendor/{name}")]
        public IActionResult GetAllColorByName(string name)
        {
            List<GetVendorListViewModel> _ViewModels = null;

            _ViewModels = dataAccess.GetAllVendorForAutoComplete(name);

            if (_ViewModels == null)
            {
                return Ok(null);
            }
            else
            {
                return Ok(_ViewModels);
            }
        }

        [HttpGet]
        [Authorize]
        [Route("api/ApprovedPrice/Edit/{id}")]
        public IActionResult GetApprovedPricesForedit(int id)
        {
            ApprovedPriceViewModel _ViewModels = new ApprovedPriceViewModel();

            _ViewModels = dataAccess.GetApprovedPricesForedit(id);


            return Ok(_ViewModels);

        }

        [HttpPost]
        [Authorize]
        [Route("api/ApprovedPrice/Update")]
        public IActionResult UpdatePrice(ApprovedPriceViewModel ViewModel)
        {

            bool status = false;
            status = dataAccess.EditApprovedPrice(ViewModel);

            return Ok(status);
        }
        [HttpGet]
        [Authorize]
        [Route("api/ApprovedPrice/Vendor/")]
        public IActionResult GetAllVendorForAutoCompleteFocus()
        {
            List<GetVendorListViewModel> _ViewModels = null;

            _ViewModels = dataAccess.GetAllVendorForAutoCompleteFocus();

            if (_ViewModels == null)
            {
                return Ok(null);
            }
            else
            {
                return Ok(_ViewModels);
            }
        }

        [HttpPost]
        [Authorize]
        [Route("api/ApprovedPrice/AddNotesInApprovedPrice")]
        public IActionResult AddNotesInApprovedPrice(ApprovedPriceViewModel ViewModel)
        {
            
            bool status = false;
            status = dataAccess.AddNotesInApprovedPrice(ViewModel);
           
                return Ok(status);
            
           
        }
    }
}