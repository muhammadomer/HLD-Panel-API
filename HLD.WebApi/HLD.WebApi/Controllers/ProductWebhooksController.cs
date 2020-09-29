using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper.Configuration;
using DataAccess.DataAccess;
using DataAccess.Helper;
using DataAccess.ViewModels;
using HLD.WebApi.Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace HLD.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductWebhooksController : ControllerBase
    {
        IConnectionString _connectionString = null;
        ServiceReference1.AuthHeader authHeader = null;
        PropductOrderLogAndDetailDataAccess _productOrderLogDataAccess = null;
        ZincDataAccess _zincDataAccess = null;
        SellerCloudOrderDataAccess _sellerCloudDataAccess = null;
        private IConfiguration _configuration = null;
        string ZincUserName = "";
        private readonly ILogger logger;
        EncDecChannel _EncDecChannel = null;

        GetChannelCredViewModel _getChannelCredSC = null;
        public ProductWebhooksController(IConnectionString connectionString, IConfiguration configuration, ILogger<ProductWebhooksController> _logger)
        {
            _configuration = configuration;
            _connectionString = connectionString;

            _getChannelCredSC = new GetChannelCredViewModel();
            _EncDecChannel = new EncDecChannel(_connectionString);
            _getChannelCredSC = _EncDecChannel.DecryptedData("sellercloud");
            authHeader = new ServiceReference1.AuthHeader();
            authHeader.ValidateDeviceID = false; ;
            authHeader.UserName = _getChannelCredSC.UserName;
            authHeader.Password = _getChannelCredSC.Key;
            _productOrderLogDataAccess = new PropductOrderLogAndDetailDataAccess(_connectionString);
            //ZincUserName = _configuration.GetValue<string>("ZincCredential:UserName");
            _zincDataAccess = new ZincDataAccess(_connectionString);
            _sellerCloudDataAccess = new SellerCloudOrderDataAccess(_connectionString);

            this.logger = _logger;
        }
        [HttpPost]
        [Route("failure")]
        public IActionResult ProductFailureWebhook([FromBody] object response)
        {
            try
            {
                logger.LogInformation("ProductFailureWebhook is called" + response);
                SendToZincProductViewModel model = new SendToZincProductViewModel();
                ProductOrderIDModelforWebhooks logidmodel = new ProductOrderIDModelforWebhooks();
                var X = JObject.Parse(response.ToString());
                var type = X["_type"];
                var code = X["code"];
                var message = X["message"];
                var extra = X["extra"];
                var request = X["request"];
                var sci = request.SelectToken("client_notes.our_internal_order_id");
                string commonMessage = "";


                if (message != null)
                {
                    commonMessage = message.ToString();
                }

                model.Message = commonMessage;

                //setting internal zinc order status
                if (type != null && type.ToString() != "error")
                {

                    model.ZincOrderStatusInternal = ZincOrderLogInternalStatus.Error.ToString();


                    model.Type = type.ToString();
                }

                if (sci != null)
                {

                    model.OurInternalOrderId = sci.ToString();
                  
                }

                if (type != null && code != null)
                {

                    if (type.ToString() == "error" && code.ToString() == "request_processing")
                    {
                        model.ZincOrderStatusInternal = ZincOrderLogInternalStatus.InProcess.ToString();
                    }
                    else
                    {
                        model.ZincOrderStatusInternal = ZincOrderLogInternalStatus.Error.ToString();
                    }

                    if (code.ToString() == "zma_temporarily_overloaded")
                    {
                        commonMessage = "We are experiencing a very high volume of orders and are temporarily unable to process this order. Please retry in a few minutes.";
                    }


                    model.Type = type.ToString();
                    model.Code = code.ToString();
                }


                model.OrderDatetime = DateTimeExtensions.ConvertToEST(DateTime.Now);

                _productOrderLogDataAccess.UpdateZincProductWebhookFailure(model);


                //if (model.ZincOrderStatusInternal == ZincOrderLogInternalStatus.Error.ToString())
                //{
                //    SendEmailWebhooks(model.OurInternalOrderId, model.Code);
                //}

                return Ok();
            }
            catch (Exception ex)
            {
                logger.LogInformation("ZincFailureWebhook exception" + ex);
                throw;
            }
        }

        [HttpPost]

        [Route("Success")]
        public IActionResult ProductSuccessWebhookAsync([FromBody] object response)
        {
            try
            {
                logger.LogInformation("ProductSuccessWebhookAsync is called" + response);
                SendToZincProductViewModel model = new SendToZincProductViewModel();
                ProductOrderIDModelforWebhooks logidmodel = new ProductOrderIDModelforWebhooks();
                var X = JObject.Parse(response.ToString());
                var type = X["_type"];

                string commonMessage = "";
                var delivery_dates = X["delivery_dates"];

                var request = X["request"];
                var sci = request.SelectToken("client_notes.our_internal_order_id");

                if (delivery_dates != null && delivery_dates.Count() != 0)
                {
                    commonMessage = delivery_dates[0].Value<string>("delivery_date").ToString();

                }


                model.Message = commonMessage;

                //setting internal zinc order status
                if (type != null)
                {
                    if (type.ToString() == "order_response" && delivery_dates != null)
                    {
                        model.ZincOrderStatusInternal = ZincOrderLogInternalStatus.InProgressSuccess.ToString();
                    }
                    else if (type.ToString() == "order_response" && delivery_dates == null)
                    {
                        model.ZincOrderStatusInternal = ZincOrderLogInternalStatus.InProcess.ToString();
                    }


                    model.Type = type.ToString();
                }

                if (sci != null)
                {

                    model.OurInternalOrderId = sci.ToString();
                    //logidmodel = _zincOrderLogDataAccess.GetLogid(model.OurInternalOrderId);
                    //model.ZincOrderLogID = logidmodel.zinc_order_log_id;

                }

                model.OrderDatetime = DateTimeExtensions.ConvertToEST(DateTime.Now);

                _productOrderLogDataAccess.UpdateZincProductWebhookSuccess(model);

                return Ok();
            }
            catch (Exception ex)
            {
                logger.LogInformation("ProductSuccessWebhookAsync exception" + ex);
                throw;
            }
        }
        [HttpPost]
        [Route("tracking")]
        // tracking webhooks
        public IActionResult ProducttrackingWebhookAsync([FromBody] object response)
        {
            try
            {
                logger.LogInformation("ProducttrackingWebhookAsync is called" + response);

                SendToZincProductViewModel model = new SendToZincProductViewModel();
                ProductOrderIDModelforWebhooks logidmodel = new ProductOrderIDModelforWebhooks();
                var X = JObject.Parse(response.ToString());
                var type = X["_type"];

                string commonMessage = "";
                var delivery_dates = X["delivery_dates"];
                var tracking = X["tracking"];

                var request = X["request"];
                var scid = request.SelectToken("client_notes.our_internal_order_id");
                if (delivery_dates != null && delivery_dates.Count() != 0)
                {
                    commonMessage = delivery_dates[0].Value<string>("delivery_date").ToString();

                }

                model.Message = commonMessage;

                if (scid != null)
                {

                    model.OurInternalOrderId = scid.ToString();

                }

                //checking tracking of an zinc order
                if (tracking != null)
                {
                    if (!string.IsNullOrEmpty(tracking[0].Value<string>("tracking_number")))
                    {
                        if (!string.IsNullOrEmpty(tracking[0].Value<string>("obtained_at")))
                        {
                            model.ShppingDate = tracking[0].Value<string>("obtained_at").ToString();
                        }
                        if (!string.IsNullOrEmpty(tracking[0].Value<string>("tracking_number")))
                        {
                            model.TrackingNumber = tracking[0].Value<string>("tracking_number").ToString();
                        }
                        if (!string.IsNullOrEmpty(tracking[0].Value<string>("carrier")))
                        {
                            model.Carrier = tracking[0].Value<string>("carrier").ToString();
                        }
                        if (!string.IsNullOrEmpty(tracking[0].Value<string>("merchant_order_id")))
                        {
                            model.MerchantOrderId = tracking[0].Value<string>("merchant_order_id").ToString();
                        }
                        if (!string.IsNullOrEmpty(tracking[0].Value<string>("tracking_url")))
                        {
                            model._17Tracking = tracking[0].Value<string>("tracking_url").ToString();
                        }
                        if (!string.IsNullOrEmpty(tracking[0].Value<string>("retailer_tracking_url")))
                        {
                            model.AmazonTracking = tracking[0].Value<string>("retailer_tracking_url").ToString();
                        }
                        if (type != null)
                        {
                            if (type.ToString() == "order_response")
                            {
                                model.ZincOrderStatusInternal = ZincOrderLogInternalStatus.Shipped.ToString();
                            }
                        }
                        //SKUQTYModelforWebhooks sKUQTYModelforWebhooks = new SKUQTYModelforWebhooks();
                        //sKUQTYModelforWebhooks = _zincDataAccess.GetQTyAndSKU(model.OurInternalOrderId);
                        //await SendTrackingToSCWebhooks(model.OurInternalOrderId, sKUQTYModelforWebhooks.SKU, sKUQTYModelforWebhooks.Qty.ToString(), model.ShppingDate, model.TrackingNumber);
                    }
                }

                model.OrderDatetime = DateTimeExtensions.ConvertToEST(DateTime.Now);


                _productOrderLogDataAccess.SaveProductOrderLogDetail(model);

                return Ok();
            }
            catch (Exception ex)
            {
                logger.LogInformation("ProducttrackingWebhookAsync exception" + ex);
                throw;
            }
        }

        [HttpGet]
        //[Authorize]
        [Route("GetZincProduct")]
        public IActionResult GetZincProduct()
        {
            SendToZincProductViewModel viewModels = null;

            viewModels = _productOrderLogDataAccess.GetZincProduct();

            if (viewModels == null)
            {
                return Ok(new SendToZincProductViewModel());
            }
            else
            {
                return Ok(viewModels);
            }
        }
    }
}