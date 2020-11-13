using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using DataAccess.DataAccess;
using DataAccess.Helper;
using DataAccess.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace HLD.WebApi.Controllers
{

    //[ApiController]
    public class ProductController : ControllerBase
    {

        ProductDataAccess DataAccess;
        ProductWarehouseQtyDataAccess ProductWHQtyDataAccess;

        IConnectionString _connectionString = null;

        // string ZincUserName = "";
        EncDecChannel _EncDecChannel = null;
        GetChannelCredViewModel _getChannelCredViewModel = null;
        ChannelDecrytionDataAccess channelDecrytionDataAccess = null;
        ZincWathchlistDataAccess zincWathchlistDataAccess = null;
        ProductWarehouseQtyDataAccess QtyDataAccess = null;
        ZincDataAccess zincDataAccess = null;
        ProductDataAccess productDataAccess = null;
        public ProductController(IConnectionString connectionString)
        {
            DataAccess = new ProductDataAccess(connectionString);
            ProductWHQtyDataAccess = new ProductWarehouseQtyDataAccess(connectionString);

            _connectionString = connectionString;

            _EncDecChannel = new EncDecChannel(_connectionString);
            channelDecrytionDataAccess = new ChannelDecrytionDataAccess(_connectionString);
            zincWathchlistDataAccess = new ZincWathchlistDataAccess(_connectionString);
            zincDataAccess = new ZincDataAccess(_connectionString);
            productDataAccess = new ProductDataAccess(_connectionString);
            QtyDataAccess = new ProductWarehouseQtyDataAccess(_connectionString);
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
            Thread emailThread = new Thread(() => TaskExecute(list));
            emailThread.Start();
            return Ok(list);
        }

        [HttpGet]
        [Authorize]
        [Route("api/Product/GetWareHousesQtyList")]
        public IActionResult GetWareHousesQtyList(string SKU)
        {
            var list = QtyDataAccess.GetWareHousesQtyList(SKU);
            return Ok(list);
        }

        public void TaskExecute(int Job_Id)
        {
            _getChannelCredViewModel = new GetChannelCredViewModel();
            ZincWatchListSummaryViewModal zincWatchListSummary = new ZincWatchListSummaryViewModal();
            ZincWatchlistLogsViewModel zincWatchListlogs = new ZincWatchlistLogsViewModel();

            List<SaveWatchlistForjobsViewModel> ASInForJob = new List<SaveWatchlistForjobsViewModel>();
            // get ASIN from local

            // if (ASInForJob.Count > 0)
            {
                // get zinc key
                _getChannelCredViewModel = _EncDecChannel.DecryptedData("Zinc");
                // set job as start
                //int JobID = zincWathchlistDataAccess.SaveWatchlistSummary();
                int JobID = Job_Id;
                ASInForJob = zincWathchlistDataAccess.GetWatchlistForJob(JobID);

                if (JobID > 0)
                {
                    zincWatchListSummary.JobID = JobID;
                    zincWatchListlogs.jobID = JobID;
                    zincWatchListSummary.Total_ASIN = ASInForJob.Count;
                }

            }
            foreach (var ASIN_List in ASInForJob)
            {
                int? minPriceOfOffer = null;
                string offerID = "";
                ZincProductSaveViewModel zincProductSaveViewModel = new ZincProductSaveViewModel();
                BBProductViewModel DropShipQtyViewModal = new BBProductViewModel();
                BestBuyDropShipQtyMovementViewModel qtyViewModel = new BestBuyDropShipQtyMovementViewModel();
                // get date from zinc
                try
                {
                    ZincProductOfferViewModel.RootObject model = GetInfoFromZinc(ASIN_List, _getChannelCredViewModel.Key);

                    if ((model.status != "processing" || model.status != "failed") && model.offers != null && model.offers.Count > 0)
                    {

                        string greytext = "";
                        // getting all those offers which have fulfilled true
                        var offerids = model.offers.Where(e => e.marketplace_fulfilled.Equals(true)).Select(
                        e => new
                        {
                            offerid = e.offer_id,
                            offerPrice = e.price
                        }).ToList();

                        // based on offerid's list getting minimum price and then select offer from offer's list

                        if (offerids != null && offerids.Count > 0)
                        {
                            minPriceOfOffer = offerids.Min(e => e.offerPrice);
                            offerID = offerids.Where(e => e.offerPrice.Value == minPriceOfOffer.Value).Select(e => e.offerid).FirstOrDefault();
                        }
                        var models = model.offers.Where(e => e.offer_id == offerID).ToList();
                        zincProductSaveViewModel = new ZincProductSaveViewModel();
                        zincProductSaveViewModel.timestemp = model.timestamp.HasValue ? model.timestamp.Value : 0;
                        zincProductSaveViewModel.status = model.status;
                        zincProductSaveViewModel.ASIN = model.asin;
                        zincProductSaveViewModel.Product_sku = ASIN_List.ProductSKU;
                        var status = model.status;
                        if (models != null && models.Count > 0)
                        {
                            foreach (var item in models)
                            {
                                zincProductSaveViewModel.sellerName = item.seller.name;
                                zincProductSaveViewModel.percent_positive = item.seller.percent_positive.HasValue ? item.seller.percent_positive.Value : 0;
                                zincProductSaveViewModel.itemprice = item.price.HasValue ? item.price.Value : 0;
                                zincProductSaveViewModel.itemavailable = item.available;
                                zincProductSaveViewModel.handlingday_min = item.handling_days.min.HasValue ? item.handling_days.min.Value : 0;
                                zincProductSaveViewModel.handlingday_max = item.handling_days.max.HasValue ? item.handling_days.max.Value : 0;
                                zincProductSaveViewModel.item_prime_badge = item.prime_badge;

                                foreach (var shippingOption in item.shipping_options)
                                {
                                    if (shippingOption.delivery_days != null)
                                    {
                                        zincProductSaveViewModel.delivery_days_max = shippingOption.delivery_days.max.HasValue ? shippingOption.delivery_days.max.Value : 0;
                                        zincProductSaveViewModel.delivery_days_min = shippingOption.delivery_days.min.HasValue ? shippingOption.delivery_days.min.Value : 0;
                                    }
                                }
                                zincProductSaveViewModel.item_condition = item.condition;
                                greytext = item.greytext;


                            }
                        }

                        zincWatchListlogs.Amz_Price = zincProductSaveViewModel.itemprice;
                        zincWatchListlogs.ASIN = zincProductSaveViewModel.ASIN;
                        zincWatchListlogs.ProductSKU = zincProductSaveViewModel.Product_sku;
                        zincWatchListlogs.SellerName = zincProductSaveViewModel.sellerName;
                        zincWatchListlogs.FulfilledBY = greytext;
                        zincWatchListlogs.ZincResponse = "Available";
                        zincWatchListlogs.IsPrime = zincProductSaveViewModel.item_prime_badge == true ? 1 : 0;
                    }
                    else
                    {
                        zincWatchListlogs.Amz_Price = 0;
                        zincWatchListlogs.ASIN = ASIN_List.ASIN;
                        zincWatchListlogs.ProductSKU = ASIN_List.ProductSKU;
                        zincWatchListlogs.SellerName = "";
                        zincWatchListlogs.FulfilledBY = "";
                        zincWatchListlogs.ZincResponse = "Currently Unavailable";
                        zincWatchListlogs.IsPrime = 0;

                        if (model.offers == null)
                        {
                            zincWatchListlogs.ZincResponse = "Listing Removed";
                        }
                    }

                    SaveWatchlistForjobsViewModel saveWatchlistForjobs = new SaveWatchlistForjobsViewModel();
                    // if available
                    if (model.offers == null || (model.offers.Count > 0 && zincProductSaveViewModel.item_prime_badge == true))
                    {
                        saveWatchlistForjobs.ValidStatus = 0;
                        saveWatchlistForjobs.ASIN = ASIN_List.ASIN;
                        saveWatchlistForjobs.Consumed_call = ASIN_List.Consumed_call + 1;
                        if (model.offers != null)
                        {
                            zincWatchListSummary.Available += 1;
                            zincWatchListSummary.Prime += 1;
                            zincWatchListlogs.ZincResponse = "Available";
                            // dropship enable disable
                            DropShipQtyViewModal.dropship_status = true;
                            DropShipQtyViewModal.dropship_Qty = 5;
                            DropShipQtyViewModal.DropshipComments = "Zinc Update";
                            DropShipQtyViewModal.ShopSKU_OfferSKU = ASIN_List.ProductSKU;




                        }
                        else
                        {
                            zincWatchListSummary.Unavailable += 1;
                            // dropship enable disable
                            DropShipQtyViewModal.dropship_status = false;
                            DropShipQtyViewModal.dropship_Qty = 0;
                            DropShipQtyViewModal.DropshipComments = "Zinc Update";
                            DropShipQtyViewModal.ShopSKU_OfferSKU = ASIN_List.ProductSKU;

                        }


                    }
                    else
                    {
                        saveWatchlistForjobs.ValidStatus = 1;
                        saveWatchlistForjobs.ASIN = ASIN_List.ASIN;
                        saveWatchlistForjobs.Consumed_call = ASIN_List.Consumed_call + 1;
                        // dropship enable disable
                        DropShipQtyViewModal.dropship_status = false;
                        DropShipQtyViewModal.dropship_Qty = 0;
                        DropShipQtyViewModal.DropshipComments = "Zinc Update";
                        DropShipQtyViewModal.ShopSKU_OfferSKU = ASIN_List.ProductSKU;

                        if (model.offers.Count > 0 && zincProductSaveViewModel.item_prime_badge == false)
                        {
                            zincWatchListSummary.Available += 1;
                            zincWatchListSummary.NoPrime += 1;
                        }
                        else
                        {
                            zincWatchListSummary.Unavailable += 1;
                        }
                    }

                    // update qty and dropship status

                    if (DropShipQtyViewModal.dropship_status == false) // if not available
                    {
                        // check is there any available ASIN
                        int count_available = zincDataAccess.GetAvailablePrimeDetail(ASIN_List.ProductSKU);
                        if (count_available == 0) // if not available set disable
                        {
                            bool isdone = productDataAccess.UpdateProductDropshipStatusAndQty(DropShipQtyViewModal);
                            if (isdone)
                            {
                                qtyViewModel.ProductSku = DropShipQtyViewModal.ShopSKU_OfferSKU;
                                qtyViewModel.DropShipQuantity = DropShipQtyViewModal.dropship_Qty;
                                qtyViewModel.DropShipStatus = DropShipQtyViewModal.dropship_status;
                                qtyViewModel.DropshipComments = DropShipQtyViewModal.DropshipComments;
                                qtyViewModel.OrderDate = DateTime.Now;

                                QtyDataAccess.SaveBestBuyQtyMovementForDropshipNone_SKU(qtyViewModel);
                            }
                        }

                    }
                    else // if available
                    {
                        CheckProductDropShipStatusViewModel dropshipStatus = productDataAccess.CheckSKuDropShipStatus(ASIN_List.ProductSKU); // enable or disable
                        if (dropshipStatus.dropship_status == false)// if disable set enable
                        {
                            bool isdone = productDataAccess.UpdateProductDropshipStatusAndQty(DropShipQtyViewModal);
                            if (isdone)
                            {
                                qtyViewModel.ProductSku = DropShipQtyViewModal.ShopSKU_OfferSKU;
                                qtyViewModel.DropShipQuantity = DropShipQtyViewModal.dropship_Qty;
                                qtyViewModel.DropShipStatus = DropShipQtyViewModal.dropship_status;
                                qtyViewModel.DropshipComments = DropShipQtyViewModal.DropshipComments;
                                qtyViewModel.OrderDate = DateTime.Now;

                                QtyDataAccess.SaveBestBuyQtyMovementForDropshipNone_SKU(qtyViewModel);
                            }
                        }


                    }

                    if (ASIN_List.ASIN != zincProductSaveViewModel.ASIN)
                    {
                        zincDataAccess.UpdateNewAsin(ASIN_List.ASIN, zincProductSaveViewModel.ASIN);
                    }

                    // Save logs
                    zincWathchlistDataAccess.SaveWatchlistLogs(zincWatchListlogs);
                    if (zincWatchListlogs.ZincResponse == "Available")
                    {

                        zincDataAccess.UpdateZincProductASINDetailWatchList(zincProductSaveViewModel);
                    }

                    // Set ASIN in watchLIst for next date
                    zincWathchlistDataAccess.UpdateWatchlistForJob(saveWatchlistForjobs);


                }
                catch (Exception ex)
                {

                    continue;
                }


            }

            // set job as completed
            zincWathchlistDataAccess.UpdateWatchlistSummary(zincWatchListSummary);


        }


        public ZincProductOfferViewModel.RootObject GetInfoFromZinc(SaveWatchlistForjobsViewModel watchASIN, string ZincUserName)
        {

            ZincProductOfferViewModel.RootObject model = null;
            try
            {
                string uri = " https://api.zinc.io/v1/products/" + watchASIN.ASIN + "/offers?retailer=amazon_ca";
                string response = "";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
                request.Credentials = new NetworkCredential(ZincUserName, "");
                using (var webResponse = request.GetResponse())
                {
                    using (var responseStream = webResponse.GetResponseStream())
                    {
                        response = new StreamReader(responseStream).ReadToEnd();
                    }
                }

                model = JsonConvert.DeserializeObject<ZincProductOfferViewModel.RootObject>(response);


            }
            catch (Exception)
            {

                throw;
            }
            return model;


        }
        //Below  API's of ParentSku Created
        [HttpPost]
        [Authorize]
        [Route("api/Product/SaveParentSKU")]
        public IActionResult SaveParentSKU([FromBody] SaveParentSkuVM model)
        {
            bool status = false;
            status = DataAccess.SaveParentSKU(model);
            return Ok(status);
        }
        [HttpGet]
        [Route("api/Product/GetAllParentSKUList")]
        public IActionResult GetAllParentSKU()
        {
            var list = DataAccess.GetAllParentSKU();
            return Ok(list);
        }

        [HttpGet("api/Product/DeleteParentSKU")]
        public int DeleteParentSKU(int Id)
        {
            DataAccess.DeleteParentSKU(Id);
            return 0;
        }
        [HttpGet("api/Product/GetParentSkuWithId")]
        public GetParentSkuById GetParentSkuWithId(int id)
        {

            GetParentSkuById viewModel = null;
            viewModel = DataAccess.GetParentSkuWithId(id);

            return viewModel;
        }
        //Below  API's of Child Sku Created
        //[HttpPost]
        //[Authorize]
        //[Route("api/Product/SaveChildSKUList")]
        //public List<SaveChildSkuVM> SaveChildSKU()
        //{
        //    List<SaveChildSkuVM> list = new List<SaveChildSkuVM>();
            
        //    var status = DataAccess.SaveChildSKU(list);
        //    return Ok(status);
        //}
        [HttpPost]
        [Authorize]
        [Route("api/Product/SaveChildSKUList")]
        public IActionResult SaveChildSKU([FromBody] List<SaveChildSkuVM> ListViewModel)
        {
            var list = DataAccess.SaveChildSKU(ListViewModel);
            return Ok(list);
        }

        [HttpGet]
        [Route("api/Product/GetAllChildSKUList")]
        public IActionResult GetAllChildSKU()
        {
            var list = DataAccess.GetAllChildSKU();
            return Ok(list);
        }

        [HttpGet]
        [Authorize]
        [Route("api/Product/DeleteChildSku")]
        public int DeleteChildSku(int child_id)
        {
            
            DataAccess.DeleteChildSku(child_id);
            return 0;
        }
       
        [HttpGet]
        [Authorize]
        [Route("api/Product/DeleteChildImage")]
        public int DeleteChildImage(int ChildImage)
        {

            DataAccess.DeleteChildImage(ChildImage);
            return 0;


        }
        [HttpGet]
        [Authorize]
        [Route("api/Product/GetChildSkuById")]
        public List<GetChildSkuVM> GetChildSkuById(int id)
        {
            List<GetChildSkuVM> viewlList = new List<GetChildSkuVM>();
            try
            {
                viewlList = DataAccess.GetChildSkuById(id);
                return viewlList;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        [HttpPut]
        [Authorize]
        [Route("api/Product/UpdateChildSKU")]
        public IActionResult UpdateChildSKU([FromBody] SaveChildSkuVM model)
        {
            bool status = false;
             status = DataAccess.UpdateChildSKU(model);
            return Ok(status);
        }

        [HttpGet]
        [Authorize]
        [Route("api/Product/GetMarketPlaceShadow")]
        public IActionResult GetMarketPlaceShadow()
        {
            var list = DataAccess.GetMarketPlaceShadow();
            return Ok(list);
        }

        [HttpPost]
        [Authorize]
        [Route("api/Product/SaveAndEditChildShadow")]
        public IActionResult SaveAndEditChildShadow([FromBody] SaveAndEditChildShadowViewModel model)
        {
            bool status = false;
            status = DataAccess.SaveAndEditChildShadow(model);
            return Ok(status);
        }

        [HttpPost]
        [Authorize]
        [Route("api/Product/SaveChildSkuShadow")]
        public IActionResult SaveChildSkuShadow([FromBody] List<SaveSkuShadowViewModel> model)
        {
            bool status = false;
            status = DataAccess.SaveChildSkuShadow(model);
            return Ok(status);
        }

        [HttpPost]
        [Authorize]
        [Route("api/Product/SaveProductImages")]
        [RequestSizeLimit(524288000)]
        public IActionResult GetSellerCludOrders([FromBody]  ImagesSaveToDatabaseWithURLViewMOdel viewModel)
        {
            bool status = false;
            status = DataAccess.SaveProductImagesFromSellerCloudOrders(viewModel);
            //dataAccess.UpdateProductImages();
            return Ok(status);

        }

        [HttpGet]
        [Authorize]
        [Route("api/Product/UpdateProductStatusWhenProductCreatedOnSC")]
        public IActionResult UpdateProductStatusWhenProductCreatedOnSC(string sku)
        {
            bool status = false;
            status = DataAccess.UpdateProductStatusWhenProductCreatedOnSC(sku);
            return Ok(status);
        }

        [HttpGet]
        [Route("api/Product/CheckChildOrShadowCreatedOnSC")]
        public IActionResult CheckChildOrShadowCreatedOnSC(string sku)
        {
            var list = DataAccess.CheckChildOrShadowCreatedOnSC(sku);
            return Ok(list);
        }

        [HttpGet]
        [Authorize]
        [Route("api/Product/GetShadowsOfChild")]
        public List<GetShadowsOfChildViewModel> GetShadowsOfChild(string childSku)
        {
            List<GetShadowsOfChildViewModel> viewlList = new List<GetShadowsOfChildViewModel>();
            try
            {
                viewlList = DataAccess.GetShadowsOfChild(childSku);
                return viewlList;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        [HttpPost]
        [Authorize]
        [Route("api/Product/GetShadowsOfChildForXls")]
        public List<FileContents> GetShadowsOfChildForXls([FromBody] List<CreateProductOnSallerCloudViewModel> dataSKU)
        {
            List<FileContents> viewlList = new List<FileContents>();
            try
            {
                viewlList = DataAccess.GetShadowsOfChildForXls(dataSKU);
                return viewlList;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        [HttpPost]
        [Authorize]
        [Route("api/Product/GetDataForBulkUpdate")]
        public List<BulkUpdateFileContents> GetDataForBulkUpdate([FromBody] List<GetBulkUpdateSkuViewModel> dataSKU)
        {
            List<BulkUpdateFileContents> viewlList = new List<BulkUpdateFileContents>();
            try
            {
                viewlList = DataAccess.GetDataForBulkUpdate(dataSKU);
                
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return viewlList;
        }

        [HttpGet]
        [Authorize]
        [Route("api/Product/GetChildSkuImageUrl")]
        public List<GetImageUrlOfChildSkuViewModel> GetChildSkuImageUrl(string childSku)
        {
            List<GetImageUrlOfChildSkuViewModel> viewlList = new List<GetImageUrlOfChildSkuViewModel>();
            try
            {
                viewlList = DataAccess.GetChildSkuImageUrl(childSku);
                return viewlList;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        [HttpGet]
        [Route("api/Product/CheckChildSkuImageUpdatedOnSC")]
        public IActionResult CheckChildSkuImageUpdatedOnSC(string sku)
        {
            var list = DataAccess.CheckChildSkuImageUpdatedOnSC(sku);
            return Ok(list);
        }

        [HttpGet]
        [Authorize]
        [Route("api/Product/UpdateImageStatusWhenImageUpdatedOnSC")]
        public IActionResult UpdateImageStatusWhenImageUpdatedOnSC(string sku)
        {
            bool status = false;
            status = DataAccess.UpdateImageStatusWhenImageUpdatedOnSC(sku);
            return Ok(status);
        }

        [HttpPut]
        [Authorize]
        [Route("api/Product/UpdateRelation")]
        public IActionResult UpdateRelation([FromBody] UpdateIsRelationViewModel relationViewModel )
        {
            bool status = false;
            status = DataAccess.UpdateRelation(relationViewModel);
            return Ok(status);
        }

        [HttpPut]
        [Authorize]
        [Route("api/Product/UpdateShadowSingleColoumn")]
        public IActionResult UpdateShadowSingleColoumn([FromBody] UpdateShadowSingleColoumnViewModel updateShadowSingleColoumnViewModel)
        {
            bool status = false;
            status = DataAccess.UpdateShadowSingleColoumn(updateShadowSingleColoumnViewModel);
            return Ok(status);
        }
        [HttpPost]
        [Authorize]
        [Route("api/Product/UpdateShadowSingleColoumnForistAsin")]
        public IActionResult UpdateShadowSingleColoumnForistAsin([FromBody] List<UpdateShadowSingleColoumnViewModel> ListViewModel)
        {
            var list = DataAccess.UpdateShadowSingleColoumnForistAsin(ListViewModel);
            return Ok(list);
        }

    }
}


