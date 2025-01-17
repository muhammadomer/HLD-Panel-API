﻿using DataAccess.DataAccess;
using DataAccess.Helper;
using DataAccess.ViewModels;
using Newtonsoft.Json;
using Quartz;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using static DataAccess.ViewModels.GetOrdersFromBestBuyViewModel;

namespace HLD.WebApi.Jobs
{
    [DisallowConcurrentExecution]
    public class GetOrdersFromBestBuyJob : IJob
    {
        IConnectionString _connectionString = null;


        BestBuyOrderFromBBDataAccess _bestBuytDataAccess = null;
        EncDecChannel _EncDecChannel = null;
        GetChannelCredViewModel _getChannelCredViewModel = null;
        ChannelDecrytionDataAccess channelDecrytionDataAccess = null;
        BestBuyProductDataAccess _bestBuyProductDataAccess = null;
        public GetOrdersFromBestBuyJob(IConnectionString connectionString)
        {
            _connectionString = connectionString;

            _EncDecChannel = new EncDecChannel(_connectionString);
            _bestBuytDataAccess = new BestBuyOrderFromBBDataAccess(_connectionString);
            _EncDecChannel = new EncDecChannel(_connectionString);
            channelDecrytionDataAccess = new ChannelDecrytionDataAccess(_connectionString);
            _bestBuyProductDataAccess = new BestBuyProductDataAccess(_connectionString);
        }
        public async Task Execute(IJobExecutionContext context)
        {

            int status = channelDecrytionDataAccess.CheckZincJobsStatus("neworders");


            if (status == 1)
            {
                _getChannelCredViewModel = new GetChannelCredViewModel();
                _getChannelCredViewModel = _EncDecChannel.DecryptedData("bestbuy");
                GetOrdersFromBestBuyViewModel.BestBuyRootObjectBB bestBuyRootObject = new GetOrdersFromBestBuyViewModel.BestBuyRootObjectBB();

                bestBuyRootObject = GetBestBuyOrdersLasthundred(_getChannelCredViewModel.Key);

                string AllOrderCommaSeprate = string.Join(",", bestBuyRootObject.orders.Select(e => e.order_id));


                //For Testing
                //var result = bestBuyRootObject.orders.Where(e => e.order_id == "218382346-A").FirstOrDefault();
                //var AllSKUCommaSeprate = result.order_lines.Select(p => p.offer_sku).ToList();
                //List<DropShipAndQtyOrderViewModel> listqty = _bestBuytDataAccess.GeQtyAndDropShip(AllSKUCommaSeprate);
                //DropShipAndQtyOrderViewModel Qty = new DropShipAndQtyOrderViewModel();
                //Qty = listqty.Where(p => p.Status != "1" && p.Qty <= 0).FirstOrDefault();

                List<string> AllreadyExistOrder = _bestBuytDataAccess.GetOrderAlreadyExist(AllOrderCommaSeprate);
                //var NewOrders = bestBuyRootObject.orders.Select(e => e).Except(AllreadyExistOrder).ToList();
               // var NewOrders = bestBuyRootObject.orders.Select(e => e.order_id).Except(AllreadyExistOrder);
                var NewOrders = bestBuyRootObject.orders.Select(e => e.order_id).Except(AllreadyExistOrder);
                List<OrderBB> root = new List<OrderBB>();
                foreach (var item in NewOrders)
                {
                    var r = bestBuyRootObject.orders.Where(a => a.order_id == item).FirstOrDefault();
                    root.Add(r);
                }

                foreach (var item in NewOrders)
                {
                    var result = bestBuyRootObject.orders.Where(e => e.order_id == item).FirstOrDefault();

                    int bbOrderID = _bestBuytDataAccess.SaveBestBuyOrderINOrder(result);

                    if (bbOrderID != 0)
                    {
                        _bestBuytDataAccess.SaveBestBuyOrderINOrderLines(result, bbOrderID);
                        _bestBuytDataAccess.SaveBestBuyOrderINOrderLookUp(result, bbOrderID);
                        _bestBuytDataAccess.SaveBestBuyOrderINCustomerShipping(result, bbOrderID);
                        var AllSKUCommaSeprate = result.order_lines.Select(p => p.offer_sku).ToList();
                        List<DropShipAndQtyOrderViewModel> listqty = _bestBuytDataAccess.GeQtyAndDropShip(AllSKUCommaSeprate);
                        DropShipAndQtyOrderViewModel Qty = new DropShipAndQtyOrderViewModel();
                        Qty = listqty.Where(p => p.Status != "1" && p.Qty <= 0).FirstOrDefault();
                        if (listqty.Count == result.order_lines.Count && Qty == null)
                        {
                            AcceptBesyBuyOrderViewModel acceptBesyBuyOrder = new AcceptBesyBuyOrderViewModel();
                            List<OrderLine_accept> ListorderLine_Accept = new List<OrderLine_accept>();

                            foreach (var orderlinesdata in result.order_lines)
                            {
                                OrderLine_accept orderLine_Accept = new OrderLine_accept();
                                orderLine_Accept.id = orderlinesdata.order_line_id;
                                orderLine_Accept.accepted = true;
                                ListorderLine_Accept.Add(orderLine_Accept);
                            }
                            acceptBesyBuyOrder.order_lines = ListorderLine_Accept;
                            BestBuy_acceptOrder(_getChannelCredViewModel.Key, item, acceptBesyBuyOrder);
                        }
                        else
                        {
                            SendEmailForUnacceptedOrder(item, listqty);
                        }

                    }



                }

                foreach (var item in AllreadyExistOrder)
                {
                    var result = bestBuyRootObject.orders.Where(e => e.order_id == item).FirstOrDefault();
                    _bestBuytDataAccess.UpdateBestBuyOrderINOrder(result);
                    _bestBuytDataAccess.UpdateBestBuyOrderINOrderLines(result);

                    _bestBuytDataAccess.UpdateBestBuyOrderINCustomerShipping(result);

                }

                UpdateqtyinqryMovement(root);
            }
        }
        public GetOrdersFromBestBuyViewModel.BestBuyRootObjectBB GetBestBuyOrdersLasthundred(string token)
        {
            GetOrdersFromBestBuyViewModel.BestBuyRootObjectBB responses = new GetOrdersFromBestBuyViewModel.BestBuyRootObjectBB();
            try
            {


                //HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://marketplace.bestbuy.ca/api/orders?paginate=false&order_ids=218377639-A");
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://marketplace.bestbuy.ca/api/orders?order=desc&max=99");
                request.Method = "GET";
                request.Accept = "application/json;";
                request.ContentType = "application/json";
                request.Headers["Authorization"] = token;

                string strResponse = "";
                using (WebResponse webResponse = request.GetResponse())
                {
                    using (StreamReader stream = new StreamReader(webResponse.GetResponseStream()))
                    {
                        strResponse = stream.ReadToEnd();
                    }
                }

                responses = JsonConvert.DeserializeObject<GetOrdersFromBestBuyViewModel.BestBuyRootObjectBB>(strResponse);
            }
            catch (Exception ex)
            {

                throw;
            }
            return responses;
        }

        public int BestBuy_acceptOrder(string token, string orderid, AcceptBesyBuyOrderViewModel besyBuyOrderViewModel)
        {
            int StatusCode = 0;
            var data = JsonConvert.SerializeObject(besyBuyOrderViewModel);
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://marketplace.bestbuy.ca/api/orders/" + orderid + "/accept");
                request.Method = "PUT";
                request.Accept = "application/json;";
                request.ContentType = "application/json";
                request.Headers["Authorization"] = token;

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

        public void SendEmailForUnacceptedOrder(string BBOrderID, List<DropShipAndQtyOrderViewModel> model)
        {
            try
            {

                // Credentials
                var credentials = new NetworkCredential("AKIAJ2ZYJS2WHV3TBFYQ", "BO6Ht4m/+okdb40r13HNeQrGWOB82n6gvU1P3WtO9vDp");
                string messageBody = "<br><font>Please note BestBuy Order ID " + "<a style = 'cursor: pointer' href = 'https://marketplace.bestbuy.ca/mmp/shop/order/" + BBOrderID + "' target = '_blank' > " +
                                     BBOrderID + "</a>" + " is on Pending Acceptance Status due to following reason ";


                string htmlTableStart = "<table style=\"border-collapse:collapse;width:500px; min-width:500px\" >";
                string htmlTableEnd = "</table>";
                string htmlHeaderRowStart = "<tr style =\"background-color:#5f9ea0; color:#ffffff;\">";
                string htmlHeaderRowEnd = "</tr>";
                string htmlTrStart = "<tr style =\"color:#00000;\">";
                string htmlTrEnd = "</tr>";
                string htmlTdStart = "<td style=\" border-color:#5f9ea0; border-style:solid; border-width:thin; padding: 5px;\">";


                string htmlTdEnd = "</td>";


                messageBody += htmlTableStart;
                messageBody += htmlHeaderRowStart;
                messageBody += htmlTdStart + "Image" + htmlTdEnd + htmlTdStart + "SKU " + htmlTdEnd + htmlTdStart + "DropShip" + htmlTdEnd + htmlTdStart + " WH Qty " + htmlTdEnd;
                messageBody += htmlHeaderRowEnd;
                foreach (var item in model)
                {
                    string checkboxValue = item.Status == "1" ? "checked" : "";

                    messageBody = messageBody + htmlTrStart;

                    messageBody = messageBody +
                        htmlTdStart +
                                       "<a target='_blank' href='https://s3.us-east-2.amazonaws.com/upload.hld.erp.images/" + item.image + "'> <img src = 'https://s3.us-east-2.amazonaws.com/upload.hld.erp.images.thumbnail/" + item.image + "' class='rounded' height='50' width='50'>  </a>" + htmlTdEnd
                                   + htmlTdStart + "<a style = 'padding-left:5px; cursor: pointer' href = 'https://erp.hldinc.net/BestBuyProduct/PropertyPage?ProductSKU=" + item.SKU + "' target = '_blank' > " + item.SKU + "</a><br>" + "<a style = 'padding-left:5px; cursor: pointer' href = 'https://lp.cwa.sellercloud.com/Inventory/Product_Dashboard.aspx?Id=" + item.SKU + "' target = '_blank' >Seller-Cloud </a>" + htmlTdEnd

                                   + htmlTdStart +
                                                "<span>DS:</span><span> <input type='checkbox' " + checkboxValue + "/></span>" + htmlTdEnd
                    + htmlTdStart + "<span>" + item.Qty + "</span>" + htmlTdEnd;

                    messageBody = messageBody + htmlTrEnd;
                }
                messageBody = messageBody + htmlTableEnd;




                var mail = new MailMessage()
                {
                    From = new MailAddress("info@hldinc.net"),
                    Subject = "BBuy Order ID " + BBOrderID + " Not Accepted Due to Stock Issue.",
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

        void UpdateqtyinqryMovement(List<GetOrdersFromBestBuyViewModel.OrderBB> NewOrders)
        {

           var result = NewOrders.SelectMany(e => e.order_lines);
            //bbOrderID = listBestBuyOrders.Select(e => e.OrderViewModel.order_id).Distinct().ToList();

            // sum quantity for specifu sku to update quantity on bb
            var finalResult = result.GroupBy(e => e.offer_sku, (x, y) => new
            {
                totalQty = y.Sum(r => int.Parse(r.quantity)),
                offersku = x,
                date = y.Max(e => e.received_date)
            }).ToList();

            foreach (var item in finalResult)
            {
                BestBuyDropShipQtyMovementViewModel model = new BestBuyDropShipQtyMovementViewModel();
                model.ProductSku = item.offersku;
                model.OrderQuantity = item.totalQty.ToString();
                model.OrderDate = DateTime.Now;
                _bestBuyProductDataAccess.SaveBestBuyOrderDropShipMovement(model);
            }
        }

    }
}
