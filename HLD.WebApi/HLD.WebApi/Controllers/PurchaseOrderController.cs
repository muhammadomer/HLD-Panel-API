using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using DataAccess.DataAccess;
using DataAccess.Helper;
using DataAccess.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace HLD.WebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PurchaseOrderController : ControllerBase
    {
        PurchaseOrderDataAccess _DataAccess;
        public PurchaseOrderController(IConnectionString connectionString)
        {
            _DataAccess = new PurchaseOrderDataAccess(connectionString);
        }

        [HttpPost]

        [Route("save")]
        public bool Post(PurchaseOrderDataViewModel ViewModel)
        {
            bool status = false;
            status = _DataAccess.SavePurchaseOrders(ViewModel);
            return status;
        }

        [HttpPost]

        [Route("Update")]
        public bool UpdatePO(PurchaseOrderDataViewModel ViewModel)
        {
            bool status = false;
            status = _DataAccess.UpdatePurchaseOrders(ViewModel);

            string POOrderItemsCommaSeprate = string.Join(",", ViewModel.items.Select(e => e.ProductID));

            _DataAccess.DeleteRemovedPOItems(POOrderItemsCommaSeprate, ViewModel.POId);

            return status;
        }
        [HttpPost]
        [Route("POAsAccepted")]
        public bool SavePOAsAccepted(UpdatePOAcceptedViewModel ViewModel)
        {
            bool status = false;
            status = _DataAccess.SavePOAsAccepted(ViewModel);
            return status;
        }
        [HttpPost]
        [Route("UpdatePOShipDate")]
        public bool UpdatePOShipDate(UpdatePOAcceptedViewModel ViewModel)
        {
            bool status = false;
            status = _DataAccess.UpdatePOShipDate(ViewModel);
            return status;
        }
        [HttpPost]
        [Route("POcurrExchange")]
        public bool SaveCurrencyExchange(UpdatePOExchangeViewModel ViewModel)
        {
            bool status = false;
            status = _DataAccess.SaveCurrencyExchange(ViewModel);
            return status;
        }


        [HttpGet]
        [Route("GetPOCount")]
        public int Get(int VendorId, string CurrentDate, string PreviousDate, int POID, string OpenItem, string ReceivedItem, string OrderdItem, string NotShipped = "")
        {
            int data = 0;

            data = _DataAccess.GetAllPurchaseOrdersCount(VendorId, CurrentDate, PreviousDate, POID, OpenItem, ReceivedItem, OrderdItem, NotShipped);
            return data;
        }

        [HttpGet]
        [Route("GetShipdates/{POId}")]
        public UpdatePOAcceptedViewModel GetPOShiDatestoupdate(int POId)
        {
            UpdatePOAcceptedViewModel data = new UpdatePOAcceptedViewModel();
            data = _DataAccess.GetPOShiDatestoupdate(POId);
            return data;
        }
        //PURCHASE
        [HttpGet]
        [Route("GetPO")]
        public IActionResult Get(int VendorId, string CurrentDate, string PreviousDate, int limit, int offSet, int POID, string OpenItem = "", string ReceivedItem = "", string OrderdItem = "", string NotShipped = "")
        {
            var list = _DataAccess.GetAllPurchaseOrders(VendorId, CurrentDate, PreviousDate, limit, offSet, POID, OpenItem, ReceivedItem, OrderdItem, NotShipped);
            {
                return Ok(list);
            }

        }
        [HttpPost]
        [Route("GetPOitems")]
        public PurchaseOrderModel Getitem(searchPOitemViewModel searchPOView)
        {
            PurchaseOrderModel data = new PurchaseOrderModel();
            data = _DataAccess.GetAllPurchaseOrdersItems(searchPOView);
            return data;
        }
        [HttpGet]
        //[Authorize]
        [Route("UpdateCurrencyCode")]
        public IActionResult UpdateCurrency(int POId, int CurrencyCode)
        {
            var item = _DataAccess.UpdateCurrency(POId, CurrencyCode);
            return Ok(0);
        }
        [HttpPost]
        [Route("CheckPOOrder")]
        public MissingOrderReturnViewModel CheckPurchaseOrderOrderINDB([FromBody] List<CheckMissingOrderViewModel> OrderList)
        {
            MissingOrderReturnViewModel data = new MissingOrderReturnViewModel();
            data = _DataAccess.CheckPurchaseOrderOrderINDB(OrderList);
            return data;
        }
        //get po count gpd
        [Route("GetPOProductDetails")]
        public IActionResult GetPOProductDetails(int VendorId, string CurrentDate, string PreviousDate, int limit, int offSet, int POID, string SKU = "", string title = "", string OpenItem = "", string ReceivedItem = "", string OrderdItem = "", string NotShipped = "", string ShippedButNotReceived = "")
        {
            //bool _status = false;
            //_status = dataAccess.updateSCImageStatusInProductTable(Sku, status);

            var list = _DataAccess.GetPOProductDetails(VendorId, CurrentDate, PreviousDate, limit, offSet, POID, SKU, title, OpenItem, ReceivedItem, OrderdItem, NotShipped, ShippedButNotReceived);
            {
                return Ok(
                    list
                );
            }

        }
        [HttpGet]
        [Route("GetCount")]
        //get po count
        public IActionResult GetCount(int VendorId, string CurrentDate, string PreviousDate, int POID, string SKU, string title, string OpenItem, string ReceivedItem, string OrderdItem, string NotShipped, string ShippedButNotReceived = "")
        {
            long Count = 0;
            GetSummaryandCountPOViewModel status = new GetSummaryandCountPOViewModel();
            status = _DataAccess.GetCount(VendorId, CurrentDate, PreviousDate, POID, SKU, title, OpenItem, ReceivedItem, OrderdItem, NotShipped, ShippedButNotReceived);

            return Ok(status);

        }

        [HttpGet]
        [Authorize]
        [Route("SKU/{name}/{POID}")]
        public IActionResult GetAllSKUByNameAndPO(string name, int POID)
        {
            List<ProductSKUViewModel> _ViewModels = null;

            _ViewModels = _DataAccess.GetAllSKUForAutoCompleteFromPO(name, POID);

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
        [Route("deleteItem/{ID}")]
        public IActionResult DeletePOItems(int ID)
        {
            bool status = _DataAccess.DeletePOItems(ID);


            return Ok(status);


        }



        [HttpGet]
        //[Authorize]
        [Route("GetAllSKUByName/{name}/{VendorId}")]
        public IActionResult GetAllSKUByName(string name, int VendorId)
        {
            List<ProductSKUViewModel> _ViewModels = null;
            _ViewModels = _DataAccess.GetAllSKUForAutoComplete(name, VendorId);
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
        //[Authorize]
        [Route("GetPOIdBySku/{name}/{VendorId}")]
        public IActionResult GetPOIdBySku(string name, int VendorId)
        {
            List<POIdViewModel> list = null;
            list = _DataAccess.GetPOIdBySku(name, VendorId);
            if (list == null)
            {
                return Ok(null);
            }
            else
            {
                return Ok(list);
            }
        }
        [HttpGet]
        [Route("GetTableHeaderForPO")]
        public IActionResult GetTableHeaderForPO(int VendorId, string CurrentDate, string PreviousDate, int POID, string OpenItem, string ReceivedItem, string OrderdItem, string NotShipped)
        {
            var data = _DataAccess.GetTableHeaderForPO(VendorId, CurrentDate, PreviousDate, POID, OpenItem, ReceivedItem, OrderdItem, NotShipped);
            return Ok(data);
        }

        [HttpDelete]
        [Route("DeletePOByPOId")]
        public IActionResult DeletePO(int Id)
        {
            try
            {
                var _ViewModels = _DataAccess.DeletePO(Id);
                return Ok(_ViewModels);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        [Authorize]
        [Route("UpdateTitle")]
        public IActionResult UpdateTitle(string Sku, string Title)
        {
            bool status = false;

            status = _DataAccess.UpdateTitle(Sku, Title);


            return Ok(status);
        }

        [HttpGet]
        [Authorize]
        [Route("UpdatePODescription")]
        public IActionResult UpdatePODescription(string ProductPONotes, string PO)
        {
            bool status = false;

            status = _DataAccess.UpdatePODescription(ProductPONotes, PO);


            return Ok(status);
        }
    }
}