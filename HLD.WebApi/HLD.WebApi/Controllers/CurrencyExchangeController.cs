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
    public class CurrencyExchangeController : Controller
    {
        CurrencyExchangeDataAccess _CurrencyDataAccess;
        public CurrencyExchangeController(IConnectionString connectionString)
        {
            _CurrencyDataAccess = new CurrencyExchangeDataAccess(connectionString);
        }
        
        [HttpPost]
        [Authorize]
        [Route("api/CurrencyExchange/UpdateCurrencyExchange")]
        public IActionResult UpdateDetail(CurrencyExchangeViewModel viewModels)
        {
            bool status = false;
            if (_CurrencyDataAccess.UpdateCurrencyExchange(viewModels))
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
        [Route("api/CurrencyExchange/SaveCurrencyExchange")]
        public IActionResult SaveDetail(CurrencyExchangeViewModel viewModels)
        {
            bool status = false;
            if (_CurrencyDataAccess.SaveCurrencyExchange(viewModels))
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
        [Route("api/CurrencyExchange/GetCurrencyExchangeList")]
        public IActionResult GetDetail()
        {
            List<CurrencyExchangeViewModel> list = new List<CurrencyExchangeViewModel>();
            list = _CurrencyDataAccess.GetAllCurrencyExchangeList();
            return Ok(list);

        }


        [HttpGet]
        [Authorize]
        [Route("api/CurrencyExchange/GetLatestCurrencyRate")]
        public IActionResult GetLatestCurrencyDetail()
        {
            double rate = _CurrencyDataAccess.GetLatestCurrencyRate();
            return Ok(rate);
        }



        [HttpGet]
        [Authorize]
        [Route("api/CurrencyExchange/GetCurrencyExchangeById/{id}")]
        public IActionResult Get(int id)
        {
            CurrencyExchangeViewModel viewModel = new CurrencyExchangeViewModel();
            viewModel = _CurrencyDataAccess.GetCurrencyExchangeById(id);
            return Ok(viewModel);
        }

    }
}