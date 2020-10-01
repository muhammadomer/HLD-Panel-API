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
    [ApiController]
    public class QuotationController : ControllerBase
    {
        QuotationDataAccess _quotationDataAccess = null;
        public QuotationController(IConnectionString connectionString)
        {
            _quotationDataAccess = new QuotationDataAccess(connectionString);
        }
        [HttpPost ("api/Quotation/SaveMainQoute")]
       
        public int SaveMainQoute(SaveQuotationMainVM model)
        {
            SaveQuotationSubVM subqoutVM = new SaveQuotationSubVM();
            int last_insert_id = _quotationDataAccess.SaveMainQoute(model);
          
             //subqoutVM.latestqouteId = last_insert_id;
            //int last_SubQoute_id = _quotationDataAccess.SaveSubQoute(subqoutVM);
            QuotationImagesVM quoteImg = new QuotationImagesVM();
            //quoteImg.LastSubQouteId = last_SubQoute_id;
            //_quotationDataAccess.SaveQouteImage(quoteImg);

            return last_insert_id;
        }

        [HttpDelete("DeleteMainQoute")]
        public int DeleteMainQoute(int Id)
        {
            _quotationDataAccess.DeleteMainQoute(Id);
            return 0;
        }
        [HttpDelete("DeleteSubQoute")]
        public int DeleteSubQoute(int Id)
        {
            _quotationDataAccess.DeleteSubQoute(Id);
            return 0;
        }
        [HttpDelete("DeleteQouteImage")]
        public int DeleteQouteImage(int Id)
        {
            _quotationDataAccess.DeleteQouteImage(Id);
            return 0;
        }
        [HttpPost("GenerateMainSku")]
        public IActionResult GenerateMainSku()
        {
            _quotationDataAccess.GenerateMainSku();
            return Ok();
        }
        [HttpPost("CreateSubSku")]
        public IActionResult CreateSubSku(string _sku, int _mainSkuId)
        {
            _quotationDataAccess.CreateSubSku(_sku, _mainSkuId);
            return Ok();
        }

        [HttpGet ("api/Quotation/list")]
       
        public IActionResult QuotationList()
        {
            var list = _quotationDataAccess.QuotationList();
            {
                return Ok(
                    list
                );
            }
        }
    }
}