using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccess;
using DataAccess.Helper;
using DataAccess.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HLD.WebApi.Controllers
{

    [ApiController]
    [Authorize]
    public class PredictionHistroyController : ControllerBase
    {
        PredictionHistroyDataAccess _predictionDataAccess;
        public PredictionHistroyController(IConnectionString connectionString)
        {
            _predictionDataAccess = new PredictionHistroyDataAccess(connectionString);
        }

        [HttpGet]
        [Route("api/PredictionSummaryCount")]
        public IActionResult PredictionSummaryCount(int VendorId, string SKU, string Title, bool Approved, bool Excluded, bool KitShadowStatus, bool Continue, string SearchFromSkuListPredict, int Type = 0 )  // get list of job summary
        {
            int count = 0;
            try
            {
                count = _predictionDataAccess.PredictionSummaryCount(VendorId, SKU, Title, Approved, Excluded, KitShadowStatus, Continue, SearchFromSkuListPredict, Type);
                return Ok(count);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        [Route("api/PredictionHistroy/SavePO")]
        public IActionResult SavePO(PurchaseOrderDataViewModel viewModel)// get list of job summary
        {
            int count = 0;
            try
            {
                count = _predictionDataAccess.SavePO(viewModel);
                return Ok(count);
            }
            catch (Exception)
            {
                throw;
            }
        }


        [HttpPost]
        [Route("api/PredictionHistroy/SavePOItem")]
        public IActionResult SavePOItem(PredictPOItemViewModel viewModel)// get list of job summary
        {
            int count = 0;
            try
            {
                count = _predictionDataAccess.SavePOItem(viewModel);
                return Ok(count);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        [Route("api/PredictionHistroy/GetPredictionDetailCopy")]
        public IActionResult GetCopyForMultipleskulist(int startLimit, int offset, string SearchFromSkuListPredict)
        {
            List<PredictionHistroyViewModel> _ViewModels = new List<PredictionHistroyViewModel>();
            try
            {
                _ViewModels = _predictionDataAccess.GetAllPredictionCopy(startLimit, offset, SearchFromSkuListPredict);
                return Ok(_ViewModels);
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpGet]
        [Route("api/PredictionHistroy")]
        public IActionResult Get(int startLimit, int offset, int VendorId, string SKU, string Title, bool Approved, bool Excluded,bool KitShadowStatus, bool Continue, string Sort, string SortedType, int Type = 0)
        {
            List<PredictionHistroyViewModel> _ViewModels = new List<PredictionHistroyViewModel>();
            try
            {
                _ViewModels = _predictionDataAccess.GetAllPrediction(startLimit, offset, VendorId, SKU, Title, Approved, Excluded, KitShadowStatus, Continue, Sort,SortedType,Type);
                return Ok(_ViewModels);
            }
            catch (Exception)
            {
                throw;
            }
        }


        [HttpGet]
        [Route("api/PredictionHistroy/Item")]
        public IActionResult GetbyId(int Id)
        {
            try
            {
                var _ViewModels = _predictionDataAccess.GetAllPredictionbyId(Id);
                return Ok(_ViewModels);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        [Route("api/PredictionHistroy/GetInternalPOIdBySku")]
        public IActionResult GetInternalPOIdBySku(string SKU)
        {
            try
            {
                var _ViewModels = _predictionDataAccess.GetInternalPOIdBySKU(SKU);
                return Ok(_ViewModels);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpDelete]
        [Route("api/PredictionHistroy/DeletePO")]
        public IActionResult DeletePO(int Id)
        {
            try
            {
                var _ViewModels = _predictionDataAccess.DeletePO(Id);
                return Ok(_ViewModels);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpDelete]
        [Route("api/PredictionHistroy/DeletePOItem")]
        public IActionResult DeletePOItem(int Id)
        {
            try
            {
                var _ViewModels = _predictionDataAccess.DeletePOItem(Id);
                return Ok(_ViewModels);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        [Route("api/PredictionHistroy/GetSoldQtyDays")]
        public IActionResult GetSoldQtyDays(string SKU)
        {
            try
            {
                var _ViewModels = _predictionDataAccess.GetSoldQtyDays(SKU);
                return Ok(_ViewModels);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        [Route("api/PredictionHistroy/GetSKUDetailBySku")]
        public IActionResult GetSKUDetailBySku(string SKU)
        {
            try
            {
                var _ViewModels = _predictionDataAccess.GetSKUDetailBySku(SKU);
                return Ok(_ViewModels);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        [Route("api/PredictionHistroy/DraftPOList")]
        public IActionResult DraftPOList()
        {
            try
            {
                var _ViewModels = _predictionDataAccess.DraftPOList();
                return Ok(_ViewModels);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        [Route("api/PredictionHistroy/GetWareHouseProductQuantitylistBySku")]
        public IActionResult GetWareHouseProductQuantitylistBySku(string SKU)
        {
            try
            {
                var _ViewModels = _predictionDataAccess.GetWareHouseProductQuantitylistBySku(SKU);
                return Ok(_ViewModels);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        [Authorize]
        [Route("api/PredictionHistroy/PredictIncludedExcluded")]
        public IActionResult PredictIncludedExcluded([FromBody] List<PredictIncludedExcludedViewModel> ListViewModel)
        {


            var list = _predictionDataAccess.PredictIncludedExcluded(ListViewModel);
            return Ok(list);

        }
        [HttpPost]
        [Route("api/PredictionHistroy/GetDataForPOCreation")]
        public IActionResult GetDataForPOCreation([FromBody] List<PredictionInternalSKUList> SKUlist)
        {
            try
            {
                var ViewModels = _predictionDataAccess.GetDataForPOCreation(SKUlist);
                return Ok(ViewModels);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}