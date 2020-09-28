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
    public class ZincController : ControllerBase
    {
        ZincDataAccess _zincDataAccess;
        public ZincController(IConnectionString connectionString)
        {
            _zincDataAccess = new ZincDataAccess(connectionString);
        }

        [HttpPost]
        [Authorize]
        [Route("api/Zinc/UpdateZincProductASINDetail")]
        public IActionResult UpdateDetail(ZincProductSaveViewModel viewModels)
        {
            bool status = false;
            if (_zincDataAccess.UpdateZincProductASINDetail(viewModels))
            {
                status = true;
                return Ok(status);
            }
            else
            {
                return Ok(new { Status = status });
            }
        }

        [HttpPost]
        [Authorize]
        [Route("api/Zinc/UpdateZincProductASIN")]
        public IActionResult Update(ZincProductSaveViewModel viewModels)
        {
            bool status = false;
            if (_zincDataAccess.UpdateZincProductASIN(viewModels))
            {
                status = true;
                return Ok(status);
            }
            else
            {
                return Ok(new { Status = status });
            }
        }


        [HttpPost]
        [Authorize]
        [Route("api/Zinc/saveZincProductASIN")]
        public IActionResult Post(ZincProductSaveViewModel viewModels)
        {
            int status = 0;
            status = _zincDataAccess.SaveZincProductASIN(viewModels);
            return Ok(status);
        }


        [HttpPost]
        [Authorize]
        [Route("api/Zinc/SaveZincASINOffer")]
        public IActionResult Post(ZincASINOfferDetail viewModels)
        {
            int status = 0;
            status = _zincDataAccess.SaveZincASINOffer(viewModels);
            return Ok(status);
        }


        [HttpPost]
        [Authorize]
        [Route("api/Zinc/SaveASINProductDetail")]
        public IActionResult Post([FromBody] ASINDetailViewModel viewModels)
        {
            bool status = false;
            if (_zincDataAccess.SaveASINProductDetail(viewModels))
            {
                status = true;
                return Ok(status);
            }
            else
            {
                return Ok(new { Status = status });
            }
        }

        [HttpPost]
        [Authorize]
        [Route("api/Zinc/SaveASINProductImageDetail")]
        public IActionResult Post([FromBody] ASINProductImageViewModel viewModels)
        {
            bool status = false;
            if (_zincDataAccess.SaveASINProductImageDetail(viewModels))
            {
                status = true;
                return Ok(status);
            }
            else
            {
                return Ok(new { Status = status });
            }
        }



        [HttpGet]
        [Authorize]
        [Route("api/Zinc/GetZincDetailBySKU/{sku}")]
        public IActionResult Get(string sku)
        {
            List<ZincProductSaveViewModel> viewModels = null;

            viewModels = _zincDataAccess.GetProductASIN_Detail_bySKU(sku);

            if (viewModels == null)
            {
                return Ok(new List<ZincProductSaveViewModel>());
            }
            else
            {
                return Ok(viewModels);
            }
        }


        [HttpGet]
        [Authorize]
        [Route("api/Zinc/GetProductASINAlreadyExists/{sku}/{asin}")]
        public IActionResult Get(string sku, string asin)
        {
            int count = 0;

            count = _zincDataAccess.GetProductASINAlreadyExists(sku, asin);
            return Ok(count);

        }

        [HttpGet]
        [Authorize]
        [Route("api/Zinc/GetProductASINAlreadyExistsInASINProductDetail/{asin}")]
        public IActionResult GetASINProductDetail(string asin)
        {
            int count = 0;
            count = _zincDataAccess.GetProductASINAlreadyExistsInASINProductDetail(asin);
            return Ok(count);
        }

        [HttpGet]
        [Authorize]
        [Route("api/Zinc/CheckASINProductImageExist/{asin}")]
        public IActionResult CheckASINProductImageExists(string asin)
        {
            List<string> list = new List<string>();
            list = _zincDataAccess.CheckASINProductImageExists(asin);
            return Ok(list);
        }

        [HttpGet]
        [Authorize]
        [Route("api/Zinc/DeleteASINProductImageList/{asin}")]
        public IActionResult DeleteASINProductImageList(string asin)
        {
            bool status = false;
            status = _zincDataAccess.DeleteASINProductImageList(asin);
            return Ok(status);
        }

        [HttpGet]
        [Authorize]
        [Route("api/Zinc/DeleteBestBuyProductZinc_ByZincID/{asin}")]
        public IActionResult DeleteBestBuyProductZinc_ByZincID(string asin)
        {
            bool status = false;
            status = _zincDataAccess.DeleteBestBuyProductZinc_ByZincID(asin);
            return Ok(status);
        }




        [HttpGet]
        [Authorize]
        [Route("api/Zinc/GetCustomerDetailForSendOrderToZinc/{ASIN}/{MaxPrice}/{orderid}/{SellerOrderID}/{orderDetailID}")]
        public IActionResult GetCustomerDetailForSendOrderToZinc(string ASIN, string MaxPrice, string orderid, string SellerOrderID, string orderDetailID)
        {
            SaveZincOrders.RootObject detail = _zincDataAccess.GetCustomerDetailForSendOrderToZinc(ASIN, MaxPrice, orderid, SellerOrderID, orderDetailID);
            return Ok(detail);
        }

        [HttpGet]
        [Authorize]
        [Route("api/Zinc/GetActiveZincAccountsList")]
        public IActionResult GetActiveZincAccountsList()
        {
            SaveZincOrders.RootObject detail = _zincDataAccess.GetActiveZincAccountsList();
            return Ok(detail);
        }

        [HttpGet]
        [Authorize]
        [Route("api/Zinc/GetCustomerDetailForSendOrderToZincForOrderView/{ASIN}/{orderid}/{SellerOrderID}")]
        public IActionResult GetCustomerDetailForSendOrderToZincOrderView(string ASIN, string orderid, string SellerOrderID)
        {
            SaveZincOrders.RootObject detail = _zincDataAccess.GetCustomerDetailForSendOrderToZincForOrderView(ASIN, orderid, SellerOrderID);
            return Ok(detail);
        }

        [HttpGet]
        //[Authorize]
        [Route("api/Zinc/GetASINProductDetailCount")]
        public IActionResult GetASINProductDetailCount(string DateTo, string DateFrom, string ASIN, string Title)
        {
            long count = 0;
            bool status = false;
            count = _zincDataAccess.GetAsinProductDetailCount(DateTo, DateFrom, ASIN, Title);
            return Ok(count);
        }

        [HttpGet]
        //[Authorize]
        [Route("api/Zinc/GetASINProductDetailList")]
        public IActionResult GetASINProductDetailList(string DateTo, string DateFrom, string ASIN, string Title, int limit, int offset)
        {
            //long count = 0;
            bool status = false;
            var list = _zincDataAccess.GetAsinProductDetailList(DateTo, DateFrom, ASIN, Title, limit, offset);
            return Ok(list);
        }

        [HttpGet]
        [Authorize]
        [Route("api/Zinc/GetProductZincDetailBySKU/{sku}")]
        public IActionResult GetZincDetail(string sku)
        {
            List<ProductZinAsinDetail> detail = _zincDataAccess.GetProductZincDetailBySKU(sku);
            return Ok(detail);
        }



        [HttpGet]
        [Authorize]
        [Route("api/Zinc/GetASINProductDetail")]
        public IActionResult Get()
        {
            List<ASINDetailViewModel> viewModels = null;
            viewModels = _zincDataAccess.GetASINProductDetail();
            if (viewModels == null)
            {
                return Ok(new ASINDetailViewModel());
            }
            else
            {
                return Ok(viewModels);
            }
        }



        [HttpGet]
        [Authorize]
        [Route("api/Zinc/GetZincDetailByID/{ID}")]
        public IActionResult Get(int ID)
        {
            ZincProductSaveViewModel viewModels = null;

            viewModels = _zincDataAccess.GetProductASIN_Detail_byID(ID);

            if (viewModels == null)
            {
                return Ok(new ZincProductSaveViewModel());
            }
            else
            {
                return Ok(viewModels);
            }
        }


        [HttpGet]
        [Authorize]
        [Route("api/Zinc/DeleteZincDetailByID/{ID}")]
        public IActionResult Delete(int ID)
        {
            bool status = false;
            status = _zincDataAccess.DeleteProductASIN_Detail_byID(ID);
            return Ok(status);
        }

        [HttpPost]
        [Authorize]
        [Route("api/Zinc/SendToZinzProduct")]
        public IActionResult SendToZinzProduct([FromBody] SendToZincProductViewModel viewModels)
        {
            bool status = false;
            if (_zincDataAccess.SendToZinzProduct(viewModels))
            {
                status = true;
                return Ok(status);
            }
            else
            {
                return Ok(new { Status = status });
            }
        }

        [HttpGet]
        [Authorize]
        [Route("api/Zinc/GetSendToZincOrder/")]
        public List<GetSendToZincOrderViewModel> GetSendToZincOrder(int _offset,string Sku,string Asin,string FromDate = "", string ToDate = "")
        {
            List<GetSendToZincOrderViewModel> viewModels = null;

            viewModels = _zincDataAccess.GetSendToZincOrder(_offset,Sku,Asin, FromDate, ToDate);
            return viewModels;
        }
        [HttpGet]
        [Authorize]
        [Route("api/Zinc/GetSendToZincOrderCount")]
        public int GetSendToZincOrderCount()
        {
            int count = 0;
            count = _zincDataAccess.GetSendToZincOrderCount();
            return count;
        }

        [HttpGet]
        [Authorize]
        [Route("api/Zinc/GetAddress")]
        public IActionResult GetAddress()
        {
            List<GetAddressViewModel> viewModels = null;

            viewModels = _zincDataAccess.GetAddress();

            if (viewModels == null)
            {
                return Ok(new GetAddressViewModel());
            }
            else
            {
                return Ok(viewModels);
            }
        }


        [HttpPost]
        [Authorize]
        [Route("api/Zinc/SendToZincProduct")]
        public IActionResult SendToZincProduct([FromBody] SendToZincProductViewModel viewModels)
        {
            bool status = false;
            if (_zincDataAccess.SendToZincProduct(viewModels))
            {
                status = true;
                return Ok(status);
            }
            else
            {
                return Ok(new { Status = status });
            }
        }
    }
}