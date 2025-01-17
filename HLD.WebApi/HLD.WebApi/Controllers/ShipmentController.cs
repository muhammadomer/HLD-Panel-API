﻿using System;
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
    public class ShipmentController : ControllerBase
    {
        ShipmentDataAccess _DataAccess;
        public ShipmentController(IConnectionString connectionString)
        {
            _DataAccess = new ShipmentDataAccess(connectionString);
        }

        [HttpPost]
        //[Authorize]
        [Route("api/Shipment/save")]
        public IActionResult Post(ShipmentViewModel ViewModel)
        {
            int Id = 0;
            string status = "";
            //ViewModel.CreatedOn = DateTime.Now.AddDays(1);
            ViewModel.ShipmentId = ViewModel.CreatedOn.ToString("yyMMdd");

            status = _DataAccess.SaveShipment(ViewModel);
            if (status !=null)
            {
                return Ok(new { Status = status, Message = "Save Successfully" });
            }
            else
            {
                return Ok(new { Status = status, Message = "Some Error Occured" });
            }
        }

        [HttpGet]
        //[Authorize]
        [Route("api/Shipments")]
        public IActionResult Get(int VendorId, int limit, int offSet, string CurrentDate, string PreviousDate, string ShipmentId, string TrakingNumber, string Status, string Type)
        {
            var list = _DataAccess.GetShipmentsList(VendorId, limit, offSet, CurrentDate, PreviousDate, ShipmentId, TrakingNumber, Status, Type);
            {
                return Ok(
                    list
                );
            }

        }
        [HttpGet]
        //[Authorize]
        [Route("api/ShipmentByShipmentId")]
        public IActionResult Get(string ShipmentId)
        {
            var Item = _DataAccess.GetShipmentByShipmentId(ShipmentId);
            {
                return Ok(
                    Item
                );
            }

        }

        [HttpGet]
        //[Authorize]
        [Route("api/Shipment/GetCounter")]
        public IActionResult GetCounter(int VendorId, string CurrentDate, string PreviousDate, string ShipmentId, string TrakingNumber, string Status, string Type)
        {
            long Count = 0;
            bool status = false;
            Count = _DataAccess.GetShipmentsListCount(VendorId, CurrentDate, PreviousDate, ShipmentId, TrakingNumber, Status, Type);
            if (Count > 0)
            {
                status = true;
                return Ok(new { status = status, counter = Count, Message = "Success" });
            }
            else
            {
                return Ok(new { status = status, counter = Count, Message = "Some Error Occured" });
            }
        }

        [HttpPut]
        //[Authorize]
        [Route("api/Shipment/Update")]
        public IActionResult Update(ShipmentViewModel ViewModel)
        {
            bool status = false;
            status = _DataAccess.UpdateShipment(ViewModel);
            return Ok(new { status = status, Message = "Success" });
        }

        [HttpDelete]
        //[Authorize]
        [Route("api/Shipment/Delete")]
        public IActionResult Delete(string ShipmentId)
        {
            bool status = false;
            status = _DataAccess.DeleteShipment(ShipmentId);
            if (status)
            {
                return Ok(new { Status = status, Message = "Save Successfully" });
            }
            else
            {
                return Ok(new { Status = status, Message = "Some Error Occured" });
            }
        }

       

        [HttpGet]
        [Route("api/Shipment/Detail")]
        public IActionResult GetDetail(string ShipmentId)
        {
            var Item = _DataAccess.GetShipmentViewHeaderdetail(ShipmentId);
            {
                return Ok(
                    Item
                );
            }

        }

        [HttpGet]
        [Route("api/Shipment/ShipmentViewProductlist")]
        public IActionResult GetShipmentViewProductlist(string ShipmentId, int limit, int offSet, int POID, string SKU = "", string Title = "", string OpenItem = "", string ReceivedItem = "", string OrderdItem = "")
        {
            var list = _DataAccess.GetShipmentViewProductsList(ShipmentId, limit, offSet, POID, SKU, Title, OpenItem, ReceivedItem, OrderdItem);
            {
                return Ok(
                    list
                );
            }

        }

        [HttpGet]
        [Route("api/Shipment/ShipmentViewProductlistcount")]
        public IActionResult GetShipmentViewProductsListCount(string ShipmentId, int POID, string SKU = "", string Title = "", string OpenItem = "", string ReceivedItem = "", string OrderdItem = "")
        {
            var count = _DataAccess.GetShipmentViewProductsListCount(ShipmentId, POID, SKU, Title, OpenItem, ReceivedItem, OrderdItem);
            return Ok(
                count
            );

        }

        [HttpDelete]
        [Route("api/ShipmentCourier/Delete")]
        public IActionResult DeleteShipmentCourier(String ShipmentId)
        {
            bool status = false;
            status = _DataAccess.DeleteShipmentCourier(ShipmentId);
            if (status)
            {
                return Ok(new { Status = status, Message = "Delete Successfully" });
            }
            else
            {
                return Ok(new { Status = status, Message = "Delete Error Occured" });
            }
        }

        [HttpGet]
        [Route("api/Shipment/GetAllSKUByName/{name}/{ShipmentId}")]
        public IActionResult GetAllSKUByName(string name, string ShipmentId)
        {
            List<ProductSKUViewModel> _ViewModels = null;
            _ViewModels = _DataAccess.GetAllSKUForAutoComplete(name, ShipmentId);
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
        [Route("api/Shipment/GetShipmentHistoryCount")]
        public IActionResult GetShipmentHistoryCount(string DateTo, string DateFrom, int VendorId, string ShipmentId, string SKU = "", string Title = "", string Status = "")
        {
            long count = 0;
            GetShipedAndRecQtyViewModel model = new GetShipedAndRecQtyViewModel();
            model = _DataAccess.GetShipmentHistoryCount(DateTo, DateFrom, VendorId, ShipmentId, SKU, Title, Status);
            return Ok(model);
        }

        [HttpGet]
        [Route("api/Shipment/GetShipmentHistoryList")]
        public IActionResult GetShipmentHistoryList(string DateTo, string DateFrom, int VendorId, string ShipmentId, string SKU, string Title, int limit, int offset, string Status)
        {
            var list = _DataAccess.GetShipmentHistoryList(DateTo, DateFrom, VendorId, ShipmentId, SKU, Title, limit, offset, Status);
            return Ok(list);
        }

        [HttpGet]
        [Route("api/Shipment/GetShipmentHistoryBySKU")]
        public IActionResult GetShipmentHistoryBySKU(int POID, string SKU)
        {
            var list = _DataAccess.GetShipmentHistoryBySKU(POID, SKU);
            return Ok(list);
        }
        [HttpGet]
        //[Authorize]
        [Route("api/Shipment/GetShipmentCourierInfo")]
        public IActionResult GetShipmentCourierInfo(string ShipmentId)
        {

            var item = _DataAccess.GetShipmentCourierInfo(ShipmentId);
            return Ok(item);
        }
        [HttpPut]
        //[Authorize]
        [Route("api/Shipment/UpdateShipmentCourierInfo")]
        public IActionResult UpdateShipmentCourierInfo(ShipmentCourierInfoViewModel ViewModel)
        {
            bool status = false;
            status = _DataAccess.UpdateShipmentCourierInfo(ViewModel);
            return Ok(new { status = status, Message = "Success" });
        }
        [HttpPut]
        [Authorize]
        [Route("api/Shipment/UpdateExpectedDelivery")]
        public IActionResult Put(Expected_Delivery_Shipped_POViewModel ViewModel)
        {
            bool status = false;
            if (_DataAccess.UpdateExpectedDelivery(ViewModel))
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
      
        [Route("api/Shipment/GetById")]
        public IActionResult GetDeliveryDateById(string id)
        {
            Expected_Delivery_Shipped_POViewModel ViewModels = null;
            ViewModels = _DataAccess.GetDeliveryDateById(id);
            if (ViewModels == null)
            {
                return Ok(new Expected_Delivery_Shipped_POViewModel());
            }
            else
            {
                return Ok(ViewModels);
            }

        }
        [HttpPost]
        //[Authorize]
        [Route("api/Shipment/BBtrackingCodes")]
        public IActionResult BBtrackingCodes(BBtrackingCodesViewModel ViewModel)
        {
           
            string status = "false";         
            status = _DataAccess.BBtrackingCodes(ViewModel);
            return Ok(status);
        }
        [HttpGet]      
        [Route("api/Shipment/GetBBtrackingCodesList")]
        public IActionResult GetBBtrackingCodesList()
        {


            var list = _DataAccess.GetBBtrackingCodesList();
            {
                return Ok(list);
            }
        }
        [HttpGet]
        
        [Route("api/Shipment/{id}")]
        public IActionResult EditBBtrackingCodes(int id)
        {
            BBtrackingCodesViewModel ViewModel = new BBtrackingCodesViewModel();
            try
            {
                ViewModel = _DataAccess.EditBBtrackingCodesById(id);
                return Ok(ViewModel);
            }
            catch (Exception)
            {

                throw;
            }

        }

        [HttpGet]
        [Authorize]
        [Route("api/Shipment/CheckTrackingNumberExists/{name}")]
        public IActionResult CheckCategorySub3Exists(string name)
        {
            bool status = false;
            if (_DataAccess.CheckTrackingNumberExists(name))
            {
                status = true;
                return Ok(new { Status = status, Message = "Tracking Number exists ,please select another" });
            }
            else
            {
                return Ok(new { Status = status, Message = "Tracking Number Not Exists" });
            }
        }
        [HttpGet]
        [Authorize]
        [Route("api/Shipment/GetTrackingNumberCount")]
        public IActionResult GetTrackingNumberCount()
        {
            int count = 0;
            try
            {
                count = _DataAccess.GetTrackingNumberCount();
                return Ok(count);
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpGet]
        [Authorize]
        [Route("api/Shipment/GetBBtrackingRulesList/{offset}")]
        public IActionResult GetBBtrackingRulesList(int offset)
        {
            List<BBtrackingCodesViewModel> list = new List<BBtrackingCodesViewModel>();
            try
            {
                list = _DataAccess.GetBBtrackingRulesList(offset);
                return Ok(list);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}