﻿
using DataAccess.DataAccess;
using DataAccess.Helper;
using DataAccess.ViewModels;
using HLD.WebApi.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Quartz;
using ServiceReference1;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace HLD.WebApi.Jobs
{
    [DisallowConcurrentExecution]
    public class CreateOrderInSellerCloud : IJob
    {

        IConnectionString _connectionString = null;
      
      
        AddOrderToSCDataAccess _addOrderToSCDataAccess = null;
        private readonly IConfiguration _configuration;
        private readonly ILogger logger;
        GetChannelCredViewModel _getChannelCredViewModel = null;
        ChannelDecrytionDataAccess channelDecrytionDataAccess = null;
        EncDecChannel _EncDecChannel = null;
        string ApiURL = null;
        ServiceReference1.AuthHeader authHeader = null;
        public CreateOrderInSellerCloud(IConnectionString connectionString, ILogger<CreateOrderInSellerCloud> _logger, IConfiguration configuration)
        {

            _connectionString = connectionString;
           
            this._configuration = configuration;
            ApiURL = _configuration.GetValue<string>("SCURL:URL");
            _EncDecChannel = new EncDecChannel(_connectionString);
            _addOrderToSCDataAccess = new AddOrderToSCDataAccess(_connectionString);
            channelDecrytionDataAccess = new ChannelDecrytionDataAccess(_connectionString);
            this.logger = _logger;


        }
        public async Task Execute(IJobExecutionContext context)
        {
              int status = channelDecrytionDataAccess.CheckZincJobsStatus("sellercloudadding");

           
            if (status == 1) 
            {
              await  SendOrderToSellerCloudForCreationAsync();
            
            }


            await Task.CompletedTask;

        }

        public async Task<bool> SendOrderToSellerCloudForCreationAsync()
        {
            bool Issent = false;
            _getChannelCredViewModel = new GetChannelCredViewModel();
            _getChannelCredViewModel = _EncDecChannel.DecryptedData("sellercloud");
            try
            {
                List<UnCreatedOrderViewModel> unCreatedOrderViewModel = new List<UnCreatedOrderViewModel>();
                // get uncreated order
                unCreatedOrderViewModel = _addOrderToSCDataAccess.GetUncreatedOrder();

                if (unCreatedOrderViewModel.Count > 0)
                {
                    AuthenticateSCRestViewModel responses = new AuthenticateSCRestViewModel();
                    responses = AuthenticateSC();
                    foreach (var item in unCreatedOrderViewModel)
                    {
                        // check payment status
                        bool status = _addOrderToSCDataAccess.CheckCityOrder(item.bbe2OrdersId);
                        CreateOrderOnSCViewModel createOrderOnSCViewModel = new CreateOrderOnSCViewModel();
                        if (status == true)
                        {
                          
                            // get order data
                            createOrderOnSCViewModel = _addOrderToSCDataAccess.GetSCOrderData(item.Orderid);
                            decimal am = 0;
                            foreach (var qty_price in createOrderOnSCViewModel.Products)
                            {
                                am += qty_price.SitePrice * qty_price.Qty;
                            }
                           //  am = createOrderOnSCViewModel.Products.Sum(p => p.SitePrice);
                            decimal tax = createOrderOnSCViewModel.Products.Sum(p => p.LineTaxTotal);
                            int shippingPrice = createOrderOnSCViewModel.ShippingMethodDetails.ShippingFee;
                            SellerCloudOrderIdViewModel sellerCloudOrderIdViewModel = new SellerCloudOrderIdViewModel();

                          int OrderID = await ConfirmOrderByOrderSourceIDFromSellerCloudAsync(item.Orderid, _getChannelCredViewModel);
                         

                            if (OrderID != 0)
                            {
                                bool Updatedstatus = _addOrderToSCDataAccess.UpdateSellerID(item.Orderid, OrderID);
                                //MaunualPaymentOnSellercloudViewCloud onSellercloudViewCloud = new MaunualPaymentOnSellercloudViewCloud();
                                //Random random = new Random();
                                //int transactionid = random.Next(11111111, 99999999);
                                //onSellercloudViewCloud.Amount = Convert.ToDouble(am + tax + shippingPrice);
                                //onSellercloudViewCloud.Notes = "Manual";
                                //onSellercloudViewCloud.PaymentMethod = "Cash";
                                //onSellercloudViewCloud.ReferenceNumber = transactionid.ToString();

                                //ReceiveManualPayment(responses.access_token, sellerCloudOrderIdViewModel.SellerCloudId, onSellercloudViewCloud);
                            }
                            else
                            {
                                // create on sc
                                sellerCloudOrderIdViewModel = SendToSCOrderCreateNew(createOrderOnSCViewModel, responses.access_token);

                                if (sellerCloudOrderIdViewModel.StatusCode == 200)
                                {
                                    logger.LogInformation("before Update on local => " + "BBID" + item.Orderid + "SCID => " + sellerCloudOrderIdViewModel.SellerCloudId);
                                    if (sellerCloudOrderIdViewModel.SellerCloudId != 0)
                                    {
                                        logger.LogInformation("Update on local => " + sellerCloudOrderIdViewModel.SellerCloudId);
                                        bool Updatedstatus = _addOrderToSCDataAccess.UpdateSellerID(item.Orderid, sellerCloudOrderIdViewModel.SellerCloudId);

                                        MaunualPaymentOnSellercloudViewCloud onSellercloudViewCloud = new MaunualPaymentOnSellercloudViewCloud();
                                        Random random = new Random();
                                        int transactionid = random.Next(11111111, 99999999);
                                        onSellercloudViewCloud.Amount = Convert.ToDouble(am + tax + shippingPrice);
                                        onSellercloudViewCloud.Notes = "Manual";
                                        onSellercloudViewCloud.PaymentMethod = "Cash";
                                        onSellercloudViewCloud.ReferenceNumber = transactionid.ToString();

                                        ReceiveManualPayment(responses.access_token, sellerCloudOrderIdViewModel.SellerCloudId, onSellercloudViewCloud);
                                        Issent = true;
                                    }

                                }
                                else if (sellerCloudOrderIdViewModel.StatusCode == 401)
                                {
                                    responses = AuthenticateSC();
                                    continue;
                                }

                                else
                                {
                                    SendEmailForMisssedOrder(createOrderOnSCViewModel);

                                    continue;
                                }
                            }

                        }
                    }

                }

            }
            catch (Exception ex)
            {

                logger.LogInformation("SendOrderToSellerCloudForCreation => " + ex);
            }
            return Issent;

        }
        public AuthenticateSCRestViewModel AuthenticateSC()
        {
            AuthenticateSCRestViewModel responses = new AuthenticateSCRestViewModel();
            try
            {
                _getChannelCredViewModel = new GetChannelCredViewModel();
                _getChannelCredViewModel = _EncDecChannel.DecryptedData("sellercloud");

                RestSCCredViewModel Data = new RestSCCredViewModel();
                Data.Username = _getChannelCredViewModel.UserName;
                Data.Password = _getChannelCredViewModel.Key;

                var data = JsonConvert.SerializeObject(Data);

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(ApiURL + "/token");
                request.Method = "POST";
                request.Accept = "application/json;";
                request.ContentType = "application/json";
                request.ContentLength = data.Length;
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

                responses = JsonConvert.DeserializeObject<AuthenticateSCRestViewModel>(strResponse);

            }
            catch (WebException ex)
            {
                return responses;
            }
            return responses;
        }

        public SellerCloudOrderIdViewModel SendToSCOrderCreateNew(CreateOrderOnSCViewModel createOrderOnSC, string token)
        {
            int SellerCloudId = 0;
            int status = 0;
            SellerCloudOrderIdViewModel sellerCloudOrderIdViewModel = new SellerCloudOrderIdViewModel();
            try
            {

                var data = JsonConvert.SerializeObject(createOrderOnSC);
                logger.LogInformation("SendToSCOrderCreateNew => data => " + data.ToString());

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

                logger.LogInformation("SendToSCOrderCreateNew => status => " + sellerCloudOrderIdViewModel.StatusCode + " Order =>"+ SellerCloudId);

            }
            catch (WebException ex)
            {
                HttpWebResponse response = (HttpWebResponse)ex.Response;
                sellerCloudOrderIdViewModel.StatusCode = 500;
                logger.LogInformation("SendToSCOrderCreateNew => " + sellerCloudOrderIdViewModel.StatusCode + ex);


                return sellerCloudOrderIdViewModel;
            }
            return sellerCloudOrderIdViewModel;

        }
        public int ReceiveManualPayment(string token, int orderid, MaunualPaymentOnSellercloudViewCloud maunualPayment)
        {
            int StatusCode = 0;
            var data = JsonConvert.SerializeObject(maunualPayment);
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(ApiURL+ "/Orders/" + orderid+ "/ReceiveManualPayment");
                request.Method = "PUT";
                request.Accept = "application/json;";
                request.ContentType = "application/json";
                request.Headers["Authorization"] = "Bearer "+token;

                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    streamWriter.Write(data);
                    streamWriter.Flush();
                    streamWriter.Close();
                }
                string strResponse = "";
                using (HttpWebResponse webResponse = (HttpWebResponse)request.GetResponse())
                {
                    using (StreamReader stream = new StreamReader(webResponse.GetResponseStream()))
                    {
                        strResponse = stream.ReadToEnd();
                    }
                    StatusCode = (int)webResponse.StatusCode;
                }

            }
            catch (WebException ex)
            {
                HttpWebResponse response = (HttpWebResponse)ex.Response;
                StatusCode = (int)response.StatusCode;

            }


            return StatusCode;
        }

        public async Task<int> ConfirmOrderByOrderSourceIDFromSellerCloudAsync(string OrderID,GetChannelCredViewModel _getChannel)
        {
            int orderID = 0;
            try
            {
               
                authHeader = new AuthHeader();
                authHeader.ValidateDeviceID = false;
                authHeader.UserName = _getChannel.UserName;
                authHeader.Password = _getChannel.Key;
                ServiceReference1.SCServiceSoapClient sCServiceSoap =
                       new ServiceReference1.SCServiceSoapClient(ServiceReference1.SCServiceSoapClient.EndpointConfiguration.SCServiceSoap12);
                ServiceReference1.UpdateOrderDropShipStatusRequest request = new UpdateOrderDropShipStatusRequest(authHeader, null, 2345, DropShipStatusType2.Requested);
               
                var data = await sCServiceSoap.Orders_GetByOrderSourceOrderIDAsync(authHeader, null, ServiceReference1.OrderSource.Website, 513,OrderID);
                orderID = data.Orders_GetByOrderSourceOrderIDResult;
                
               
            }
            catch (Exception ex)
            {
                orderID = 0;

                throw;
            }
            return orderID;


        }


        public void SendEmailForMisssedOrder(CreateOrderOnSCViewModel model)
        {
            try
            {

                // Credentials
                var credentials = new NetworkCredential("AKIAJ2ZYJS2WHV3TBFYQ", "BO6Ht4m/+okdb40r13HNeQrGWOB82n6gvU1P3WtO9vDp");
                string messageBody = "<br><font> "+ model.OrderDetails.OrderSourceOrderID + " This order got Remote Server Error from Sellercloud, please check this order and resolve the issue <br>"

                     + "<a style = 'cursor: pointer' href = 'https://lp.cwa.sellercloud.com/Orders/ManageOrders.aspx?DateRange=6&ShipDateRange=9&CompanyId=&Keywords="+model.CustomerDetails.Email+"&advancedMode=false&LocationNotesFilter=0&PromiseDateRange=-1' target = '_blank' > Click here </a>" ;


                string htmlTableStart = "<table style=\"border-collapse:collapse;width:300px; min-width:300px\" >";
                string htmlTableEnd = "</table>";
                string htmlHeaderRowStart = "<tr style =\"background-color:#5f9ea0; color:#ffffff;\">";
                string htmlHeaderRowEnd = "</tr>";
                string htmlTrStart = "<tr style =\"color:#00000;\">";
                string htmlTrEnd = "</tr>";
                string htmlTdStart = "<td style=\" border-color:#5f9ea0; border-style:solid; border-width:thin; padding: 5px;\">";


                string htmlTdEnd = "</td>";


                messageBody += htmlTableStart;
                messageBody += htmlHeaderRowStart;
                messageBody += htmlTdStart + "SKU" + htmlTdEnd + htmlTdStart + "Qty " + htmlTdEnd;
                messageBody += htmlHeaderRowEnd;
                foreach (var item in model.Products)
                {
                   

                    messageBody = messageBody + htmlTrStart;

                    messageBody = messageBody +
                       htmlTdStart + "<a style = 'padding-left:5px; cursor: pointer' href = 'https://erp.hldinc.net/BestBuyProduct/PropertyPage?ProductSKU=" + item.ProductID + "' target = '_blank' > " + item.ProductID + "</a><br>" + "<a style = 'padding-left:5px; cursor: pointer' href = 'https://lp.cwa.sellercloud.com/Inventory/Product_Dashboard.aspx?Id=" + item.ProductID + "' target = '_blank' >Seller-Cloud </a>" + htmlTdEnd

                    + htmlTdStart + "<span>" + item.Qty + "</span>" + htmlTdEnd;

                    messageBody = messageBody + htmlTrEnd;
                }
                messageBody = messageBody + htmlTableEnd;




                var mail = new MailMessage()
                {
                    From = new MailAddress("info@hldinc.net"),
                    Subject = "Seller cloud order creation issue for " + model.OrderDetails.OrderSourceOrderID ,
                    Body = messageBody.ToString()
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
            catch (System.Exception e)
            {
                //return e.Message;
            }

        }
    }
}
