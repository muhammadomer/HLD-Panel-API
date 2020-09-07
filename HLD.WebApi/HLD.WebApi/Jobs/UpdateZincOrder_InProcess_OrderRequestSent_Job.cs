using DataAccess.DataAccess;
using DataAccess.Helper;
using DataAccess.ViewModels;
using HLD.WebApi.Enum;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using Quartz;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace HLD.WebApi.Jobs
{
    [DisallowConcurrentExecution]
    public class UpdateZincOrder_InProcess_OrderRequestSent_Job : IJob
    {
        IConnectionString _connectionString = null;
        ZincOrderLogAndDetailDataAccess _zincOrderLogDataAccess = null;
        ZincDataAccess _zincDataAccess = null;
        SellerCloudOrderDataAccess _sellerCloudDataAccess = null;
        private IConfiguration _configuration = null;
       // string ZincUserName = "";
        EncDecChannel _EncDecChannel = null;
        GetChannelCredViewModel _getChannelCredViewModel = null;
        ChannelDecrytionDataAccess channelDecrytionDataAccess = null;
        public UpdateZincOrder_InProcess_OrderRequestSent_Job(IConnectionString connectionString, IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = connectionString;
            _zincOrderLogDataAccess = new ZincOrderLogAndDetailDataAccess(_connectionString);
           // ZincUserName = _configuration.GetValue<string>("ZincCredential:UserName");
            _zincDataAccess = new ZincDataAccess(_connectionString);
            _sellerCloudDataAccess = new SellerCloudOrderDataAccess(_connectionString);
            _EncDecChannel = new EncDecChannel(_connectionString);
            channelDecrytionDataAccess = new ChannelDecrytionDataAccess(_connectionString);

        }
        public async Task Execute(IJobExecutionContext context)
        {
            int status = channelDecrytionDataAccess.CheckZincJobsStatus("InprocessZincOrder");
            if (status == 1)
            {
                List<ZincOrderInProcess_OrderRequestSentViewModel> list = _zincDataAccess.GetAllSCZincOrders_InProcessAndRequestSentState_ForJob();
                try
                {
                    _getChannelCredViewModel = new GetChannelCredViewModel();
                    _getChannelCredViewModel = _EncDecChannel.DecryptedData("Zinc");
                    foreach (var item in list)
                    {
                        try
                        {
                            ZincOrderLogDetailViewModel model = new ZincOrderLogDetailViewModel();
                            string uri = "https://api.zinc.io/v1/orders/" + item.RequestID;
                            string response = "";
                            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
                            request.Method = "GET";
                            request.Accept = "application/json;";
                            request.ContentType = "application/json";
                            request.Credentials = new NetworkCredential(_getChannelCredViewModel.Key, "");

                            using (var webResponse = request.GetResponse())
                            {
                                using (var responseStream = webResponse.GetResponseStream())
                                {
                                    response = new StreamReader(responseStream).ReadToEnd();
                                }
                            }
                            var X = JObject.Parse(response);
                            var type = X["_type"];
                            var code = X["code"];
                            var message = X["message"];
                            var extra = X["extra"];
                            string commonMessage = "";
                            var delivery_dates = X["delivery_dates"];
                            var tracking = X["tracking"];

                            if (delivery_dates != null && delivery_dates.Count() != 0)
                            {
                                commonMessage = delivery_dates[0].Value<string>("delivery_date").ToString();

                            }

                            if (message != null)
                            {
                                commonMessage = message.ToString();
                            }



                            model.Message = commonMessage;

                            //setting internal zinc order status
                            if (type != null)
                            {
                                if (type.ToString() == "order_response" && delivery_dates != null)
                                {
                                    model.ZincOrderStatusInternal = ZincOrderLogInternalStatus.InProgressSuccess.ToString();
                                }
                                else if (type.ToString() == "order_response")
                                {
                                    model.ZincOrderStatusInternal = ZincOrderLogInternalStatus.InProcess.ToString();
                                }
                                else
                                {
                                    model.ZincOrderStatusInternal = ZincOrderLogInternalStatus.Error.ToString();
                                }

                                model.Type = type.ToString();
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


                            //checking tracking of an zinc order
                            if (tracking != null)
                            {
                                if (!string.IsNullOrEmpty(tracking[0].Value<string>("tracking_number")) && tracking[0].Value<string>("delivery_status") != "Canceled")
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
                                }
                                else if (tracking[0].Value<string>("delivery_status") == "Canceled")
                                {
                                    model.ZincOrderStatusInternal = ZincOrderLogInternalStatus.Canceled.ToString();
                                }
                            }

                            model.OrderDatetime = DateTimeExtensions.ConvertToEST(DateTime.Now);
                            model.ZincOrderLogID = Convert.ToInt32(item.ZincOrderLogID);

                            _zincOrderLogDataAccess.SaveZincOrderLogDetail(model);


                            if (model.ZincOrderStatusInternal == ZincOrderLogInternalStatus.Error.ToString())
                            {
                                SendEmail(item.SellerCloudOrderID, model.Code);
                            }

                        }
                        catch (Exception ex)
                        {
                            continue;
                        }

                    }
                }
                catch (Exception)
                {


                }
                await Task.CompletedTask;
            }
        }

        public void SendEmail(string sellerCloudOrderId, string zincmessage)
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
            catch (System.Exception e)
            {
                //return e.Message;
            }

        }

    }
}

