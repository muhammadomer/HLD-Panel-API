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
    public class SellerCloudOrderController : ControllerBase
    {
        SellerCloudOrderDataAccess dataAccess;
        public SellerCloudOrderController(IConnectionString connectionString)
        {
            dataAccess = new SellerCloudOrderDataAccess(connectionString);
        }

        [HttpPost]
        [Authorize]
        [Route("api/SellerCloud/SaveOrder")]
        public IActionResult Post([FromBody] List<SellerCloudOrder_CustomerViewModel> ViewModel)
        {
            bool status = false;
            if (dataAccess.SaveOrderAndCustomerDetail(ViewModel))
            {
                status = true;
                return Ok(new { Status = status, Message = "Save Successfully" });
            }
            else
            {
                return Ok(new { Status = status, Message = "Some Error Occured" });
            }
        }


        [HttpPost]
        [Authorize]
        [Route("api/SellerCloud/SaveProductImages")]
        [RequestSizeLimit(524288000)]
        public IActionResult GetSellerCludOrders([FromBody]  ImagesSaveToDatabaseWithURLViewMOdel viewModel)
        {
            bool status = false;
            status = dataAccess.SaveProductImagesFromSellerCloudOrders(viewModel);
            //dataAccess.UpdateProductImages();
            return Ok(status);

        }



        [HttpPost]
        [Authorize]
        [Route("api/SellerCloud/GetSellerCloudOrders")]
        public IActionResult GetSellerCludOrders([FromBody] String SellerCloudOrderIds)
        {
            List<int> SellerCloudOrders = null;

            SellerCloudOrders = dataAccess.GetSellerCloudOrderWhichAreExists(SellerCloudOrderIds);


            if (SellerCloudOrders == null)
            {
                return Ok(null);
            }
            else
            {
                return Ok(SellerCloudOrders);
            }
        }

        [HttpPost]
        [Authorize]
        [Route("api/SellerCloud/GetSellerCloudOrderIdsForImagesImport")]
        public IActionResult GetSellerCludOrderIdsForImportImages()
        {
            List<int> SellerCloudOrders = null;

            SellerCloudOrders = dataAccess.GetSellerCloudOrderIdForImportImages(); ;

            if (SellerCloudOrders == null)
            {
                return Ok(null);
            }
            else
            {
                return Ok(SellerCloudOrders);
            }
        }

        [HttpPost]
        [Authorize]
        [Route("api/SellerCloud/SaveSellerCloudOrderStatus/{SellerCloudId}/{statusName}/{paymentStatus}")]
        public IActionResult SaveSellerCloudOrderStatus(string SellerCloudId, string statusName, string paymentStatus)
        {
            bool status = false;
            status = dataAccess.SaveSellerCloudOrderStatus(SellerCloudId, statusName, paymentStatus);
            return Ok(status);
        }


        [HttpPost]
        [Authorize]
        [Route("api/SellerCloud/UpdateSCOrderDropShipStatus")]
        public IActionResult UpdateSCOrderDropShipStatus(UpdateSCDropshipStatusViewModel viewModel)
        {
            bool status = false;
            status = dataAccess.UpdateSCOrderDropShipStatus(viewModel);
            return Ok(status);
        }



        [HttpPost]
        [Authorize]
        [Route("api/SellerCloud/SellerCloudOrderStatusLatestUpdate/{SellerCloudId}")]
        public IActionResult SellerCloudOrderStatusLatestUpdate(string SellerCloudId)
        {
            string status = "";
            status = dataAccess.SellerCloudOrderStatusLatestUpdate(SellerCloudId);
            return Ok(status);
        }

        [HttpGet]
        [Authorize]
        [Route("api/SellerCloud/InsertProductToBestBuy")]
        public IActionResult InsertProductDatafromSellerCloudToBestBuy()
        {

            bool status = dataAccess.InsertDataFromSellerCloudTableToBestBuyTable();

            return Ok(status);
        }

        [HttpGet]
        [Authorize]
        [Route("api/SellerCloud/GetSKUWhichImagesNotExists")]
        public IActionResult GetSKUWhichImagesNotExists()
        {
            List<string> list = new List<string>();
            list = dataAccess.GetSKUWhichImagesNotExists();

            return Ok(list);
        }

        [HttpGet]
        [Authorize]
        [Route("api/SellerCloud/GetSKUAndSellerCloudImageURLWhichImagesNotExists")]
        public IActionResult GetSKUAndSellerCloudImageURLWhichImagesNotExists()
        {
            List<SKUAndSellerCloudImageURLWhichImagesNotExistsViewModel> list = new List<SKUAndSellerCloudImageURLWhichImagesNotExistsViewModel>();
            list = dataAccess.GetSKUAndSellerCloudImageURLWhichImagesNotExists();
            return Ok(list);
        }

        [HttpPost]
        [Route("api/SellerCloud/GetproducTtitle")]
        public IActionResult GetproducTtitle(GetProductTitleViewModel saveWatchlistViewModel)
        {
            string status = "";
            status = dataAccess.GetproducTtitle(saveWatchlistViewModel);
            return Ok(status);
        }

        [HttpGet]
        [Route("api/SellerCloud/UpdateAccounts")]
        public IActionResult UpdateAccounts(int Id, int ZincAccountId, int CreditCardId)
        {

            bool status = dataAccess.UpdateAccounts(Id, ZincAccountId, CreditCardId);
            return Ok(status);
        }

    }
}