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
    public class SCOrderPageViewController : ControllerBase
    {
        SCOrderPageViewDataAccess dataAccess;
        public SCOrderPageViewController(IConnectionString connectionString)
        {
            dataAccess = new SCOrderPageViewDataAccess(connectionString);
        }

        [HttpPost]
          [Authorize]
        [Route("api/SCOrderPageView/{BBOrderID}")]
        public SCOrderPaymentPageViewModel Post( string BBOrderID)
        {
            SCOrderPaymentPageViewModel sCOrderPaymentPageViewModel = new SCOrderPaymentPageViewModel();


            sCOrderPaymentPageViewModel =  dataAccess.GetSCOrderForOrderPageView(BBOrderID);

            return sCOrderPaymentPageViewModel;
        }

        [HttpGet]
        [Authorize]
        [Route("api/SCOrderPageViewOrderDetail/{BBOrderID}")]
        public IActionResult GetBestBuyOrders(string BBOrderID)
        {
             BestBuyOrdersViewPageModel  _ViewModels = null;
            
                _ViewModels = dataAccess.SCOrderPageViewOrderDetails(BBOrderID);
            
                return Ok(_ViewModels);
            
        }

    }
}