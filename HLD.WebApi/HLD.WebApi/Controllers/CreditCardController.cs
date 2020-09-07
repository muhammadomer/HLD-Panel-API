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
    public class CreditCardController : ControllerBase
    {
        CreditCardDetailDataAccess _DataAccess;
        public CreditCardController(IConnectionString connectionString)
        {
            _DataAccess = new CreditCardDetailDataAccess(connectionString);
        }
        [HttpPost]
        [Route("api/CreditCard/save")]
        public IActionResult Post(CreditCardDetailViewModel ViewModel)
        {
            int Id = 0;
            bool status = false;
            CreditCardDetailViewModel viewModel = new CreditCardDetailViewModel
            {
                name_on_card = "name_on_card",
                number = "number",
                security_code = "security_code",
                expiration_month = "expiration_month",
                expiration_year = "expiration_year",
                first_name = "first_name",
                last_name = "last_name",
                address_line1 = "address_line1",
                address_line2 = "address_line2",
                zip_code = "zip_code",
                city = "city",
                state = "state",
                country = "country",
                IsActive = false,
                IsDefault = false,
                name_on_cardShort = "name_on_cardShort",
                numberShort = "numberShort",
                security_codeShort = "security_codeShort",
                PhoneNo = "0123465",

            };
            status = _DataAccess.Save(ViewModel);
            return Ok(status);
        }

        [HttpGet]
        [Route("api/CreditCard/list")]
        public IActionResult Get()
        {
            var list = _DataAccess.GetCreditCardsList();
            {
                return Ok(
                    list
                );
            }
        }

        [HttpGet]
        [Route("api/CreditCard/GetCreditCardDetailById")]
        public IActionResult GetCreditCardDetailById(int Id)
        {
            var model = _DataAccess.GetCreditCardDetailById(Id);
            {
                return Ok(
                    model
                );
            }
        }

        [HttpPut]
        [Route("api/CreditCard/UpdateIsActive")]
        public IActionResult UpdateIsActive(CreditCardDetailViewModel Obj)
        {
            int Id = 0;
            Id = _DataAccess.UpdateIsActive(Obj);
            return Ok(Id);
        }

        [HttpPut]
        [Route("api/CreditCard/UpdateIsDefault")]
        public IActionResult UpdateIsDefault(CreditCardDetailViewModel Obj)
        {
            int Id = 0;
            Id = _DataAccess.UpdateIsDefault(Obj);
            return Ok(Id);
        }
    }
}