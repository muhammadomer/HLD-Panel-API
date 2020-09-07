using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using DataAccess.DataAccess;
using DataAccess.Helper;
using DataAccess.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HLD.WebApi.Controllers
{

    //[ApiController]
    public class ProductController : ControllerBase
    {

        ProductDataAccess DataAccess;
        ProductWarehouseQtyDataAccess ProductWHQtyDataAccess;
        public ProductController(IConnectionString connectionString)
        {
            DataAccess = new ProductDataAccess(connectionString);
            ProductWHQtyDataAccess = new ProductWarehouseQtyDataAccess(connectionString);
        }



        [HttpGet]
        [Authorize]
        [Route("api/Product/updateSCImageStatusInProductTable/{Sku}/{status}")]
        public IActionResult Get(string Sku, bool status)
        {
            bool _status = false;
            _status = DataAccess.updateSCImageStatusInProductTable(Sku, status);
            {
                return Ok(_status);
            }
        }


        [HttpGet]
        [Authorize]
        [Route("api/CheckSKUImageExists/{productSku}/{imageURL}")]
        public IActionResult Get(string productSku, string imageURL)
        {
            int count = 0;
            count = DataAccess.CheckSKUImageExists(productSku, imageURL);
            {
                return Ok(count);
            }
        }
        [HttpGet]
        [Authorize]
        [Route("api/Product/SKU/{name}")]
        public IActionResult GetAllSKUByName(string name)
        {
            List<ProductSKUViewModel> _ViewModels = null;

            _ViewModels = ProductWHQtyDataAccess.GetAllSKUForAutoComplete(name);

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
        [Route("api/Product/{sku}")]
        public IActionResult GetProductDetailBySKU(string sku)
        {
            ProductDisplayViewModel _ViewModels = null;
            _ViewModels = DataAccess.GetProductDetailBySKU(sku);

            if (_ViewModels == null)
            {
                return Ok(new ProductDisplayViewModel());
            }
            else
            {
                return Ok(_ViewModels);
            }
        }

        [HttpGet]
        [Authorize]
        [Route("api/Product/ProductDeatil")]
        public IActionResult GetProductDetailSKU(string sku)
        {
            ProductDisplayViewModel _ViewModels = null;
            _ViewModels = DataAccess.GetProductDetailBySKU(sku);

            if (_ViewModels == null)
            {
                return Ok(new ProductDisplayViewModel());
            }
            else
            {
                return Ok(_ViewModels);
            }
        }

        [HttpGet]
        [Authorize]
        [Route("api/Product/GetProductBySKuAmazoneprice/{sku}")]
        public List<AsinAmazonePriceViewModel> GetProductBySKuAmazoneprice(string sku)
        {
            List<AsinAmazonePriceViewModel> viewlList = new List<AsinAmazonePriceViewModel>();
            try
            {
                viewlList = DataAccess.GetProductBySKuAmazoneprice(sku);
                return viewlList;
            }
            catch (Exception)
            {

                throw;
            }

        }

        [HttpGet]
        [Authorize]
        [Route("api/Product/{startLimit}/{endLimit}/{sort}/{dropship}/{dropshipsearch}/{sku}/{asin}/{Producttitle}/{DSTag}/{TypeSearch}")]
        public IActionResult Get(int startLimit, int endLimit, string sort, string dropship, string dropshipsearch, string sku, string asin, string Producttitle, string DSTag, string TypeSearch)
        {
            List<ProductDisplayInventoryViewModel> _ViewModels = null;
            _ViewModels = DataAccess.GetAllProducts(startLimit, endLimit, sort, dropshipsearch, dropship, sku, asin, Producttitle, DSTag, TypeSearch);
            if (_ViewModels == null)
            {
                return Ok(new List<ConditionViewModel>());
            }
            else
            {
                return Ok(_ViewModels);
            }
        }

        [HttpGet]
        [Authorize]
        [Route("api/Product/Export/{dropship}/{dropshipstatusSearch}/{sku}")]
        public IActionResult GetDataforExport(string dropship, string dropshipstatusSearch, string sku)
        {
            List<ExportProductDataViewModel> _ViewModels = new List<ExportProductDataViewModel>();

            _ViewModels = DataAccess.GetAllProductsForExport(dropship, dropshipstatusSearch, sku);

            return Ok(_ViewModels);

        }

        [HttpPost]
        [Authorize]
        [Route("api/Product")]
        public IActionResult GetAllProductWithoutPageLimit([FromBody] ProductInventorySearchViewModel viewModel)
        {
            List<ProductDisplayInventoryViewModel> _ViewModels = null;

            _ViewModels = DataAccess.GetAllProductsWithoutPageLimit(viewModel.dropshipstatus, viewModel.dropshipstatusSearch, viewModel.Sku, viewModel.SearchFromSkuList);

            if (_ViewModels == null)
            {
                return Ok(new List<ConditionViewModel>());
            }
            else
            {
                return Ok(_ViewModels);
            }
        }

        [HttpPost]
        [Authorize]
        [Route("api/Product/TotalCountProductIn_inventory")]
        public IActionResult GetTotalCount([FromBody] ProductInventorySearchViewModel viewModel)
        {
            return Ok(DataAccess.GetAllProductsCount(viewModel.dropshipstatus, viewModel.dropshipstatusSearch, viewModel.Sku, viewModel.SearchFromSkuList, viewModel.asin, viewModel.Producttitle, viewModel.DSTag, viewModel.TypeSearch));
        }

        [HttpGet]
        [Authorize]
        [Route("api/Product/GetProductId/{SKU}")]
        public IActionResult GetProductIdBySKU(string SKU)
        {
            int productId = 0;
            productId = DataAccess.GetProductIdBySKU(SKU.Trim());
            return Ok(new { product_Id = productId });
        }

        [HttpGet]
        [Authorize]
        [Route("api/Product/CheckProductId/{SKU}")]
        public IActionResult GetProductIdByBBSKU(string SKU)
        {
            int productId = 0;
            productId = DataAccess.GetProductIdByBBSKU(SKU.Trim());
            return Ok(new { product_Id = productId });
        }

        [HttpGet]
        [Authorize]
        [Route("api/ProductImages/{id}")]
        public IActionResult GetProductImageById(string id)
        {
            List<ProductImagesViewModel> _ViewModels = null;

            _ViewModels = DataAccess.GetAllProductsImagesByProductId(id);

            if (_ViewModels == null)
            {
                return Ok(new List<ProductImagesViewModel>());
            }
            else
            {
                return Ok(_ViewModels);
            }
        }

        [HttpGet]
        [Authorize]
        [Route("api/Product/ProductById/{id}")]
        public IActionResult GetProductDetailById(string id)
        {
            ProductInsertUpdateViewModel ViewModel = new ProductInsertUpdateViewModel();
            ViewModel = DataAccess.GetProductByProductID(id);
            if (ViewModel == null)
            {
                return Ok(new ProductInsertUpdateViewModel());
            }
            else
            {
                return Ok(ViewModel);
            }
        }

        [HttpPost]
        [Authorize]
        [Route("api/Product/save")]
        public IActionResult Post([FromBody]ProductInsertUpdateViewModel ViewModel)
        {
            int product_id = 0;
            bool status = false;
            product_id = DataAccess.SaveProduct(ViewModel);
            if (product_id > 0)
            {
                status = true;
                return Ok(new { Status = status, ProductId = product_id, Message = "Save Successfully" });
            }
            else
            {
                return Ok(new { Status = status, ProductId = product_id, Message = "Some Error Occured" });
            }
        }

        [HttpPost]
        [Authorize]
        [Route("api/Product/savenew")]
        public IActionResult savenew([FromBody]ProductSaveViewModel ViewModel)
        {
            int product_id = 0;
            bool status = false;
            product_id = DataAccess.SaveProductnew(ViewModel);
            ProductWHQtyDataAccess.GetWarahouseQty(ViewModel.SKU);
            if (product_id > 0)
            {

                status = true;
                return Ok(new { Status = status, ProductId = product_id, Message = "Save Successfully" });
            }
            else
            {
                return Ok(new { Status = status, ProductId = product_id, Message = "Some Error Occured" });
            }
        }


        [HttpPost]
        [Authorize]
        [Route("api/Product/Delete")]
        public IActionResult Post([FromBody]DeleteProductViewModel ViewModel)
        {
            int statusID = 0;
            bool status = false;
            statusID = DataAccess.DeleteProduct(ViewModel);
            if (statusID > 0)
            {
                status = true;
                return Ok(new { Status = status, Message = "Can't be deleted,exists in orders" });
            }
            else
            {
                return Ok(new { Status = status, Message = "Deleted successfully" });
            }
        }

        [HttpPost]
        [Authorize]
        [Route("api/Product/Update")]
        public IActionResult UpdateProduct([FromBody] ProductInsertUpdateViewModel data)
        {
            int product_id = 0;
            bool status = false;
            product_id = DataAccess.UpdateProduct(data);
            if (product_id > 0)
            {
                status = true;
                return Ok(new { Status = status, ProductId = product_id, Message = "Update Successfully" });
            }
            else
            {
                return Ok(new { Status = status, ProductId = product_id, Message = "Some Error Occured" });
            }
        }

        [HttpPost]
        [Authorize]
        [Route("api/Product/UpdateDropshipStatusAndQty")]
        public IActionResult UpdateProduct([FromBody]BBProductViewModel ViewModel)
        {
            bool status = false;
            status = DataAccess.UpdateProductDropshipStatusAndQty(ViewModel);
            return Ok(status);

        }

        [HttpPost]
        [Authorize]
        [Route("api/Product/updateProductStatus/{SKU}/{ProductStatusId}")]
        public IActionResult UpdateProductStatus(string SKU, string ProductStatusId)
        {
            bool status = false;
            status = DataAccess.updateProductStatus(SKU, ProductStatusId);
            return Ok(status);

        }


        [HttpPost]
        [Authorize]
        [Route("api/Product/saveImage")]
        public IActionResult Post(ProductImagesViewModel ViewModel)
        {
            int product_id = 0;
            bool status = false;
            product_id = DataAccess.SaveProductImages(ViewModel);
            if (product_id > 0)
            {
                status = true;
                return Ok(new { Status = status, ProductId = product_id, Message = "Save Successfully" });
            }
            else
            {
                return Ok(new { Status = status, ProductId = product_id, Message = "Some Error Occured" });
            }
        }



        [HttpGet]
        [Authorize]
        [Route("api/Product/Delete/{id}")]
        public IActionResult DeleteProductImage(int id)
        {
            bool status = false;
            status = DataAccess.DeleteProductImage(id);
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
        [Route("api/Product/CheckExistsSKU/{name}")]
        public IActionResult CheckProductSKUExists(string name)
        {
            bool status = false;
            if (DataAccess.CheckProductSKUExists(name))
            {
                status = true;
                return Ok(new { Status = status, Message = "SKU exists ,please select another" });
            }
            else
            {
                return Ok(new { Status = status, Message = "SKU Not Exists" });
            }
        }


        [HttpGet]
        [Authorize]
        [Route("api/Product/CheckExistsUPC/{name}")]
        public IActionResult CheckProductUPCExists(string name)
        {
            bool status = false;
            if (DataAccess.CheckProductUPCExists(name))
            {
                status = true;
                return Ok(new { Status = status, Message = "UPC exists ,please select another" });
            }
            else
            {
                return Ok(new { Status = status, Message = "UPC Not Exists" });
            }
        }

        [HttpGet]
        [Authorize]
        [Route("api/Product/UpdateProductAverageCost/{sku}/{averageCost}")]
        public IActionResult UpdateProductAverageCost(string sku, string averageCost)
        {
            string status = "";
            status = DataAccess.updateProductAverageCost(sku, averageCost);

            return Ok(new { Status = status });
        }


        [HttpPost]
        [Authorize]
        [Route("api/Product/UpdateProductDetailFromExcelFile")]
        public IActionResult UpdateProductDetailFromExcelFile([FromBody] ProductDisplayViewModel model)
        {
            string status = "";
            status = DataAccess.UpdateProductDetailFromExcelFile(model);

            return Ok(new { Status = status });
        }

        [HttpPost]
        [Authorize]
        [Route("api/Product/SaveProductDetailFromExcelFile")]
        public IActionResult SaveProductDetailFromExcelFile([FromBody] ProductDisplayViewModel model)
        {
            string status = "";
            status = DataAccess.SaveProductDetailFromExcelFile(model);
            return Ok(new { Status = status });
        }
        [HttpGet]
        [Authorize]
        [Route("api/product/{ProductSKU}")]
        public IActionResult GetWhQtyData(string ProductSKU)
        {
            List<ProductWarehouseQtyViewModel> _ViewModels = new List<ProductWarehouseQtyViewModel>();
            _ViewModels = ProductWHQtyDataAccess.GetProductQtyBySKU_ForOrdersPagefOriNVENTORY(ProductSKU);
            return Ok(_ViewModels);
        }

        [HttpGet]
        [Authorize]
        [Route("api/GetProductDetailForAPBySKU/{sku}")]
        public IActionResult GetProductDetailForAPBySKU(string sku)
        {
            var _ViewModels = DataAccess.GetProductDetailsForAPBySKU(sku);
            if (_ViewModels == null)
            {
                return Ok(new ProductDisplayViewModel());
            }
            else
            {
                return Ok(_ViewModels);
            }
        }

        [HttpGet]
        [Authorize]
        [Route("api/GetTagforSkuEmail/{sku}")]
        public IActionResult GetTagforSkuEmail(string sku)
        {
            var _ViewModels = DataAccess.GetTagforSkuEmail(sku);
            return Ok(_ViewModels);
        }

        [HttpGet]
        [Route("api/GetCatalog/{sku}")]
        public IActionResult GetCatalog(string sku)
        {
            var _ViewModels = DataAccess.GetCatalog(sku);
            ProductWHQtyDataAccess.GetWarahouseQty(sku);
            return Ok(_ViewModels);
        }

        [HttpGet]
        [Authorize]
        [Route("api/GetAllSKUsForCatalog")]
        public IActionResult GetAllSKUsForCatalog(string sku)
        {
            var list = DataAccess.GetAllSKUsForCatalog();
            return Ok(list);
        }

        [HttpPost]
        [Authorize]
        [Route("api/Product/ContinueDisContinue")]
        public IActionResult ContinueDisContinue([FromBody] List<ProductContinueDisContinueViewModel> ListViewModel)
        {


            var list = DataAccess.ContinueDisContinue(ListViewModel);
            return Ok(list);

        }

        [HttpPost]
        [Authorize]
        [Route("api/Product/KitOrShadow")]
        public IActionResult KitOrShadow([FromBody] List<ProductSetAsKitOrShadowViewModel> ListViewModel)
        {
            var list = DataAccess.KitOrShadow(ListViewModel);
            return Ok(list);
        }
        [HttpPost]
        [Authorize]
        [Route("api/Product/GetStausFromZinc")]
        public IActionResult GetStausFromZinc([FromBody] List<GetStatusFromZincViewModel> ListViewModel)
        {
            var list = DataAccess.GetStausFromZinc(ListViewModel);
            return Ok(list);
        }
    }


}


