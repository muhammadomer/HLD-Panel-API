using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using DataAccess.DataAccess;
using DataAccess.Helper;
using DataAccess.ViewModels;
using HLD.WebApi.Enum;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SCOrderTrackingService;
using ServiceReference1;

namespace HLD.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ZincWebhooksController : ControllerBase
    {

        IConnectionString _connectionString = null;
        ServiceReference1.AuthHeader authHeader = null;
        ZincOrderLogAndDetailDataAccess _zincOrderLogDataAccess = null;
        ZincDataAccess _zincDataAccess = null;
        SellerCloudOrderDataAccess _sellerCloudDataAccess = null;
        private IConfiguration _configuration = null;
        string ZincUserName = "";
        private readonly ILogger logger;
        EncDecChannel _EncDecChannel = null;
        OrderNotesDataAccess _NotesDataAccess=null;
        GetChannelCredViewModel _getChannelCredSC = null;
        string SCRestURL = "";
        GetChannelCredViewModel _Selller = null;
        string ApiURL = "";
        string token = "";
        public ZincWebhooksController(IConnectionString connectionString, IConfiguration configuration,ILogger<ZincWebhooksController> _logger)
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
            _zincOrderLogDataAccess = new ZincOrderLogAndDetailDataAccess(_connectionString);
            ZincUserName = _configuration.GetValue<string>("ZincCredential:UserName");
            _zincDataAccess = new ZincDataAccess(_connectionString);
            _sellerCloudDataAccess = new SellerCloudOrderDataAccess(_connectionString);
            _NotesDataAccess = new OrderNotesDataAccess(connectionString);
            this.logger = _logger;
            SCRestURL = _configuration.GetValue<string>("SCURL:URL");
            ApiURL = _configuration.GetValue<string>("WebApiURL:URL");
        }
        // failure Webhooks
        [HttpPost]

        [Route("failure")]
        public IActionResult ZincFailureWebhook([FromBody] object response)
        {
            bool status;
            try
            {
                //_Selller = new GetChannelCredViewModel();
                _getChannelCredSC = _EncDecChannel.DecryptedData("sellercloud");
                AuthenticateSCRestViewModel authenticate = _zincOrderLogDataAccess.AuthenticateSCForIMportOrder(_getChannelCredSC, SCRestURL);
                logger.LogInformation("ZincFailureWebhook is called" + response);
                ZincOrderLogDetailViewModel model = new ZincOrderLogDetailViewModel();
                ZincOrderIDModelforWebhooks logidmodel = new ZincOrderIDModelforWebhooks();
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
                    //logidmodel = _zincOrderLogDataAccess.GetLogid(model.OurInternalOrderId);
                    //model.ZincOrderLogID = logidmodel.zinc_order_log_id;

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
                //CreateOrderNotesViewModel orderNotesViewModels = new CreateOrderNotesViewModel();
                CreateNotesViewModel notesViewModel = new CreateNotesViewModel();
                model.OrderDatetime = DateTimeExtensions.ConvertToEST(DateTime.Now);

                _zincOrderLogDataAccess.SaveZincOrderLogDetailNew(model);
               string acName= _zincOrderLogDataAccess.GetAcName(Convert.ToInt32(model.OurInternalOrderId));

                //orderNotesViewModels.Note = "MerchantId: " + model.MerchantOrderId + "Estimated Shipping: " + model.ShppingDate + "Notes: " + model.Message + "Amazon A/c Name: " + acName;
                notesViewModel.Message = " MerchantId: " + model.MerchantOrderId + " Estimated Shipping: " + model.ShppingDate + " Delivery Date: " + model.Message + " Amazon A/c Name: " + acName;
                notesViewModel.Category = "0";
              //  _NotesDataAccess.CreateOrderNotesFormSC(SCRestURL, authenticate.access_token, Convert.ToInt32(model.OurInternalOrderId), notesViewModel);
                List<CreateOrderNotesViewModel> getNotes = _NotesDataAccess.GetOrderNotesFormSC(SCRestURL, authenticate.access_token, Convert.ToInt32(model.OurInternalOrderId));

                if (getNotes.Count > 0)
                {
                    List<CreateOrderNotesViewModel> data = (List<CreateOrderNotesViewModel>)getNotes.Select(p => p).Where(p => p.NoteID != 0).ToList();
                    if (data.Count > 0)
                    {
                        status = _NotesDataAccess.SaveOrderNotes(data);
                        if (status == true)
                        {
                            _NotesDataAccess.UpdateOrderAsHavingNotes(Convert.ToInt32(model.OurInternalOrderId));
                        }
                    }
                }

                if (model.ZincOrderStatusInternal == ZincOrderLogInternalStatus.Error.ToString())
                {
                    SendEmailWebhooks(model.OurInternalOrderId, model.Code);
                }

                return Ok();
            }
            catch (Exception ex)
            {
                logger.LogInformation("ZincFailureWebhook exception" + ex);
                throw;
            }
        }

        // success webhooks
        [HttpPost]

        [Route("Success")]
        public IActionResult ZincSuccessWebhookAsync([FromBody] object response)
        {
            bool status;
            try
            {
                _getChannelCredSC = _EncDecChannel.DecryptedData("sellercloud");
                AuthenticateSCRestViewModel authenticate = _zincOrderLogDataAccess.AuthenticateSCForIMportOrder(_getChannelCredSC, SCRestURL);
                logger.LogInformation("ZincSuccessWebhookAsync is called" + response);
                ZincOrderLogDetailViewModel model = new ZincOrderLogDetailViewModel();
                ZincOrderIDModelforWebhooks logidmodel = new ZincOrderIDModelforWebhooks();
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
                CreateOrderNotesViewModel orderNotesViewModels = new CreateOrderNotesViewModel();
                CreateNotesViewModel notesViewModel = new CreateNotesViewModel();
                List<CreateOrderNotesViewModel> data = new List<CreateOrderNotesViewModel>();
                model.OrderDatetime = DateTimeExtensions.ConvertToEST(DateTime.Now);

                _zincOrderLogDataAccess.SaveZincOrderLogDetailNew(model);

                string acName = _zincOrderLogDataAccess.GetAcName(Convert.ToInt32(model.OurInternalOrderId));

                orderNotesViewModels.Note = "MerchantId: " + model.MerchantOrderId + "Estimated Shipping: " + model.ShppingDate + "Notes: " + model.Message + "Amazon A/c Name: " + acName;
                notesViewModel.Message = "MerchantId: " + model.MerchantOrderId + "Estimated Shipping: " + model.ShppingDate + "Notes: " + model.Message + "Amazon A/c Name: " + acName;
                notesViewModel.Category = "0";
                _NotesDataAccess.CreateOrderNotesFormSC(SCRestURL, authenticate.access_token, Convert.ToInt32(model.OurInternalOrderId), notesViewModel);
                List<CreateOrderNotesViewModel> getNotes = _NotesDataAccess.GetOrderNotesFormSC(SCRestURL, authenticate.access_token, Convert.ToInt32(model.OurInternalOrderId));

                if (getNotes.Count > 0)
                {
                    List<CreateOrderNotesViewModel> dataa = (List<CreateOrderNotesViewModel>)getNotes.Select(p => p).Where(p => p.NoteID != 0).ToList();
                    if (dataa.Count > 0)
                    {
                        status = _NotesDataAccess.SaveOrderNotes(dataa);
                        if (status == true)
                        {
                            _NotesDataAccess.UpdateOrderAsHavingNotes(Convert.ToInt32(model.OurInternalOrderId));
                        }
                    }
                }
                return Ok();
            }
            catch (Exception ex)
            {
                logger.LogInformation("ZincSuccessWebhookAsync exception" + ex);
                throw;
            }
        }
        [HttpPost]

        [Route("tracking")]
        // tracking webhooks
        public async Task<IActionResult> ZinctrackingWebhookAsync([FromBody] object response)
        {
            bool status;
            try
            {
                _getChannelCredSC = _EncDecChannel.DecryptedData("sellercloud");
                AuthenticateSCRestViewModel authenticate = _zincOrderLogDataAccess.AuthenticateSCForIMportOrder(_getChannelCredSC, SCRestURL);
                logger.LogInformation("ZinctrackingWebhookAsync is called" + response);

                ZincOrderLogDetailViewModel model = new ZincOrderLogDetailViewModel();
                ZincOrderIDModelforWebhooks logidmodel = new ZincOrderIDModelforWebhooks();
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

                    //logidmodel = _zincOrderLogDataAccess.GetLogid(model.OurInternalOrderId);
                    //model.ZincOrderLogID = logidmodel.zinc_order_log_id;

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
                        SKUQTYModelforWebhooks sKUQTYModelforWebhooks = new SKUQTYModelforWebhooks();
                        sKUQTYModelforWebhooks = _zincDataAccess.GetQTyAndSKU(model.OurInternalOrderId);
                        await SendTrackingToSCWebhooks(model.OurInternalOrderId, sKUQTYModelforWebhooks.SKU, sKUQTYModelforWebhooks.Qty.ToString(), model.ShppingDate, model.TrackingNumber);
                    }
                }
                CreateOrderNotesViewModel orderNotesViewModels = new CreateOrderNotesViewModel();
                CreateNotesViewModel notesViewModel = new CreateNotesViewModel();
                model.OrderDatetime = DateTimeExtensions.ConvertToEST(DateTime.Now);

                _zincOrderLogDataAccess.SaveZincOrderLogDetailNew(model);
                string acName = _zincOrderLogDataAccess.GetAcName(Convert.ToInt32(model.OurInternalOrderId));

                orderNotesViewModels.Note = "MerchantId: " + model.MerchantOrderId + "Estimated Shipping: " + model.ShppingDate + "Notes: " + model.Message + "Amazon A/c Name: " + acName;
                notesViewModel.Message = "MerchantId: " + model.MerchantOrderId + "Estimated Shipping: " + model.ShppingDate + "Notes: " + model.Message + "Amazon A/c Name: " + acName;
                notesViewModel.Category = "0";
                _NotesDataAccess.CreateOrderNotesFormSC(SCRestURL, authenticate.access_token, Convert.ToInt32(model.OurInternalOrderId), notesViewModel);
                List<CreateOrderNotesViewModel> getNotes = _NotesDataAccess.GetOrderNotesFormSC(SCRestURL, authenticate.access_token, Convert.ToInt32(model.OurInternalOrderId));

                if (getNotes.Count > 0)
                {
                    List<CreateOrderNotesViewModel> data = (List<CreateOrderNotesViewModel>)getNotes.Select(p => p).Where(p => p.NoteID != 0).ToList();
                    if (data.Count > 0)
                    {
                        status = _NotesDataAccess.SaveOrderNotes(data);
                        if (status == true)
                        {
                            _NotesDataAccess.UpdateOrderAsHavingNotes(Convert.ToInt32(model.OurInternalOrderId));
                        }
                    }
                }
                return Ok();
            }
            catch (Exception ex)
            {
                logger.LogInformation("ZinctrackingWebhookAsync exception" + ex);
                throw;
            }
        }

        // send to zinc if tracking is updated and tracking number is available
        public async Task SendTrackingToSCWebhooks(string sellerCloudOrderId, string productSku, string itemQuantity, string shippingDate, string trackingNo)
        {
            bool status = false;
            _getChannelCredSC = _EncDecChannel.DecryptedData("sellercloud");
            AuthenticateSCRestViewModel authenticate = _zincOrderLogDataAccess.AuthenticateSCForIMportOrder(_getChannelCredSC, SCRestURL);
            ZincOrderLogDetailViewModel model = new ZincOrderLogDetailViewModel();

            ServiceReference1.SCServiceSoapClient sCServiceSoap =
                      new ServiceReference1.SCServiceSoapClient(ServiceReference1.SCServiceSoapClient.EndpointConfiguration.SCServiceSoap12);

            //var request = await sCServiceSoap.UpdateOrderDropShipStatusAsync(authHeader, null, int.Parse(sellerCloudOrderId), ServiceReference1.DropShipStatusType2.Processed);
            //bool response = request.UpdateOrderDropShipStatusResult;
            var getOrderId = new UpdateScOrderToDs()
            {
                Orders = new List<int>
                        {
                            Convert.ToInt32(sellerCloudOrderId)

                        },
                DropshipStatus = "Processed"
            };
            status = UpdateDropShipStatus(SCRestURL, authenticate.access_token, getOrderId);
            if (status)
            {
                UpdateSCDropshipStatusViewModel updateSCViewModel = new UpdateSCDropshipStatusViewModel();
                updateSCViewModel.StatusName = "Processed";
                updateSCViewModel.SCOrderID = int.Parse(sellerCloudOrderId);
                updateSCViewModel.LogDate = DateTime.Now;
                updateSCViewModel.IsTrackingUpdate = true;
                 status = _sellerCloudDataAccess.UpdateSCOrderDropShipStatus(updateSCViewModel);
             
                //ZincOrderLogDetailViewModel model = new ZincOrderLogDetailViewModel();
                //model = _zincOrderLogDataAccess.GetZincOrderLogDetailById(zincOrderLogDetailID);
                OrdersUpdateShippingForOrderRequest req = new OrdersUpdateShippingForOrderRequest();

                req.WarehouseName = "DropShip Canada";
                req.TrackingNumber = trackingNo;
                string[] splitString = shippingDate.Split('/');
                string[] yearSplit = splitString[2].Split(' ');
                string[] splitStringtime = shippingDate.Split(':');
                string[] splitTime = splitString[0].Split(' ');

                DateTime d = new DateTime(int.Parse(yearSplit[0]), int.Parse(splitString[0]), int.Parse(splitString[1]), int.Parse(splitTime[0]), int.Parse(splitStringtime[1]), int.Parse(splitStringtime[2]), 0);

                d = DateTimeExtensions.ConvertToEST(d);

                req.ShipDate = d;
                req.OrderID = int.Parse(sellerCloudOrderId);
                var result = await sCServiceSoap.Orders_UpdateShippingForOrderAsync(authHeader, null, req);
                bool shippingOrderStatus = result.Orders_UpdateShippingForOrderResult;
                var warehouseAdjustment = await sCServiceSoap.ProductWarehouse_AdjustQtyAsync(authHeader, null, productSku, 364, int.Parse(itemQuantity), "Dropship Adj " + sellerCloudOrderId, "");
                status = warehouseAdjustment.ProductWarehouse_AdjustQtyResult;
            }
            await Task.CompletedTask;
        }

        // send email if error
        public void SendEmailWebhooks(string sellerCloudOrderId, string zincmessage)
        {
            try
            {
                List<EmailJobDetailViewModel> model = _sellerCloudDataAccess.GetDetailFromEmailJob(sellerCloudOrderId);
                foreach (var item in model)
                {
                    // Credentials
                    var credentials = new NetworkCredential("AKIAJ2ZYJS2WHV3TBFYQ", "BO6Ht4m/+okdb40r13HNeQrGWOB82n6gvU1P3WtO9vDp");
                    // Mail message
                    StringBuilder stringBuilder = new StringBuilder();
                    stringBuilder.Append("Please note below given order has error while sending to Zinc, please review & take necessary action to resolve the error.");
                    stringBuilder.Append("<br>");
                    stringBuilder.Append("<img src = 'https://s3.us-east-2.amazonaws.com/upload.hld.erp.images/" + item.ImageName + "' class='rounded' height='50' width='50'>");
                    stringBuilder.Append("&nbsp;&nbsp;&nbsp;&nbsp;");

                    stringBuilder.Append("<a target='_blank' href='https://dash.zinc.io/ZACiVJJ8DF3YEXg1GgyHj/orders/" + item.RequestID + "'>Zinc Order ID</a>"); stringBuilder.Append("&nbsp;&nbsp;&nbsp;&nbsp;");
                    stringBuilder.Append("<a target='_blank' href='https://erp.hldinc.net/BBOrderViewPage/OrderViewPage?SCorderid=" + item.SCOrderID + "'>Seller Cloud Order ID</a>"); stringBuilder.Append("&nbsp;&nbsp;&nbsp;&nbsp;");
                    stringBuilder.Append("Zinc Message: " + zincmessage);



                    var mail = new MailMessage()
                    {
                        From = new MailAddress("info@hldinc.net"),
                        Subject = "Zinc Error of SC Order " + item.SCOrderID,
                        Body = stringBuilder.ToString()
                    };
                    mail.IsBodyHtml = true;
                    mail.To.Add(new MailAddress("hfd1278@gmail.com"));
                    // Smtp client
                    var client = new SmtpClient()
                    {
                        Port = 587,
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        UseDefaultCredentials = true,
                        Host = "email-smtp.us-east-1.amazonaws.com",
                        EnableSsl = true,
                        Credentials = credentials
                    };
                    client.Send(mail);
                    //return "Email Sent Successfully!";
                }
            }

            catch (Exception ex)
            {
                throw ex;
                //return e.Message;
            }

        }
        public bool UpdateDropShipStatus(string ApiURL, string token, UpdateScOrderToDs scOrderToDs)
        {
            bool status;
            try
            {


                var data = JsonConvert.SerializeObject(scOrderToDs);

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(ApiURL + "/Orders/DropshipStatus");
                request.Method = "PUT";
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
                }
                status = true;
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return status;
        }
    }
}