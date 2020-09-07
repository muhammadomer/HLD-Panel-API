using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using DataAccess.DataAccess;
using DataAccess.Helper;
using DataAccess.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace HLD.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CreateOrderSCController : ControllerBase
    {


        IConnectionString _connectionString = null;

        string token = "";
      
        AddOrderToSCDataAccess _addOrderToSCDataAccess = null;
        string ApiURL = null;
        AuthenticteSCRestController ctrl = null;
        private readonly ILogger logger;
        private readonly IConfiguration _configuration;
        public CreateOrderSCController(IConfiguration configuration, IConnectionString connectionString, ILogger<CreateOrderSCController> _logger)
        {

            _connectionString = connectionString;
            this._configuration = configuration;
            ApiURL = _configuration.GetValue<string>("SCURL:URL");
            ctrl = new AuthenticteSCRestController(_configuration, connectionString);
            ctrl.ControllerContext = ControllerContext;
            _addOrderToSCDataAccess = new AddOrderToSCDataAccess(_connectionString);
            this.logger = _logger;

        }

        //DateTime dateTime = DateTime.Now;
        //logger.LogInformation("CreateOrderInSellerCloud => " + dateTime);
        //int status = 0;
        //    try
        //    {
        //        HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://localhost:44321/api/CreateOrderSC");
        //        request.Method = "GET";
        //        request.Accept = "application/json;";
        //        request.ContentType = "application/json";


        //        string strResponse = "";

        //        using (HttpWebResponse webResponse = (HttpWebResponse)request.GetResponse())
        //        {
        //            using (StreamReader stream = new StreamReader(webResponse.GetResponseStream()))
        //            {
        //                strResponse = stream.ReadToEnd();

        //            }
        //            status = (int)webResponse.StatusCode;
        //        }

        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }


        [HttpGet]
        public IActionResult SendOrderToSellerCloudForCreation()
        {
            try
            {
                List<UnCreatedOrderViewModel> unCreatedOrderViewModel = new List<UnCreatedOrderViewModel>();
                // get uncreated order
                unCreatedOrderViewModel = _addOrderToSCDataAccess.GetUncreatedOrder();

                if (unCreatedOrderViewModel.Count > 0)
                {
                    AuthenticateSCRestViewModel responses = new AuthenticateSCRestViewModel();
                    responses = ctrl.AuthenticateSC();
                    foreach (var item in unCreatedOrderViewModel)
                    {
                        // check payment status
                        bool status = _addOrderToSCDataAccess.CheckCityOrder(item.bbe2OrdersId);
                        if (status == true)
                        {
                            CreateOrderOnSCViewModel createOrderOnSCViewModel = new CreateOrderOnSCViewModel();
                            // get order data
                            createOrderOnSCViewModel = _addOrderToSCDataAccess.GetSCOrderData(item.Orderid);

                            SellerCloudOrderIdViewModel sellerCloudOrderIdViewModel = new SellerCloudOrderIdViewModel();
                            // create on sc
                          //  sellerCloudOrderIdViewModel =  SendToSCOrderCreateNew(createOrderOnSCViewModel,responses.access_token);

                            int code = 200;// for testing

                            //if (sellerCloudOrderIdViewModel.StatusCode == 200)
                            //{
                            //    logger.LogInformation("before Update on local => "+ "BBID"+item.Orderid  + "SCID => " + sellerCloudOrderIdViewModel.SellerCloudId);
                            //    if (sellerCloudOrderIdViewModel.SellerCloudId != 0)
                            //    {
                            //        logger.LogInformation("Update on local => " + sellerCloudOrderIdViewModel.SellerCloudId);
                            //        bool Updatedstatus = _addOrderToSCDataAccess.UpdateSellerID(item.Orderid, sellerCloudOrderIdViewModel.SellerCloudId);
                            //    }

                            //}
                            //else if (sellerCloudOrderIdViewModel.StatusCode == 401)
                            //{
                            //    responses = ctrl.AuthenticateSC();
                            //    continue;
                            //}

                            //else
                            //{
                            //    continue;
                            //}


                        }
                    }
                  
                }

            }
            catch (Exception ex)
            {

                logger.LogInformation("SendOrderToSellerCloudForCreation => " + ex);
            }
            return Ok();

        }


        public SellerCloudOrderIdViewModel SendToSCOrderCreateNew(CreateOrderOnSCViewModel createOrderOnSC,string token)
        {
            int SellerCloudId = 0;
            int status = 0;
            SellerCloudOrderIdViewModel sellerCloudOrderIdViewModel = new SellerCloudOrderIdViewModel();
            try
            {
              
                    var data = JsonConvert.SerializeObject(createOrderOnSC);
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(ApiURL + "/Orders");
                    request.Method = "POST";
                    request.Accept = "application/json;";
                    request.ContentType = "application/json";
                    request.Headers["Authorization"] = "Bearer " + token;

                    using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                    {
                        streamWriter.Write(data);
                        streamWriter.Flush();
                        streamWriter.Close();
                    }
                    var response = (HttpWebResponse)request.GetResponse();
                    string strResponse = "";
                    using (var sr = new StreamReader(response.GetResponseStream()))
                    {
                        strResponse = sr.ReadToEnd();
                        status = (int)response.StatusCode;
                    }
                    SellerCloudId = JsonConvert.DeserializeObject<int>(strResponse);


                    sellerCloudOrderIdViewModel.StatusCode = status;
                    sellerCloudOrderIdViewModel.SellerCloudId = SellerCloudId;

                
               
            }
            catch (WebException ex)
            {
                HttpWebResponse response = (HttpWebResponse)ex.Response;
                sellerCloudOrderIdViewModel.StatusCode = (int)response.StatusCode;
                logger.LogInformation("SendToSCOrderCreateNew => " + sellerCloudOrderIdViewModel.StatusCode + ex);
                return sellerCloudOrderIdViewModel;
            }
            return sellerCloudOrderIdViewModel;

        }



    }
}