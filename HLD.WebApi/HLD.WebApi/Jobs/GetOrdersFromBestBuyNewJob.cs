using DataAccess.DataAccess;
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
using System.Threading.Tasks;

namespace HLD.WebApi.Jobs
{
    public class GetOrdersFromBestBuyNewJob : IJob
    {
        IConnectionString _connectionString = null;
        BestBuyOrderFromBBDataAccessNew _bestBuytDataAccessNew = null;
        EncDecChannel _EncDecChannel = null;
        GetChannelCredViewModel _getChannelCredViewModel = null;
        ChannelDecrytionDataAccess channelDecrytionDataAccess = null;
        public GetOrdersFromBestBuyNewJob(IConnectionString connectionString)
        {
            _connectionString = connectionString;

            _EncDecChannel = new EncDecChannel(_connectionString);
            _bestBuytDataAccessNew = new BestBuyOrderFromBBDataAccessNew(_connectionString);
            _EncDecChannel = new EncDecChannel(_connectionString);
            channelDecrytionDataAccess = new ChannelDecrytionDataAccess(_connectionString);
        }
        public async Task Execute(IJobExecutionContext context)
        
        {

            //   int status = channelDecrytionDataAccess.CheckZincJobsStatus("neworders");
            int status = 1;
            if (status == 1)
            {
                _getChannelCredViewModel = new GetChannelCredViewModel();
                _getChannelCredViewModel = _EncDecChannel.DecryptedData("bestbuy");
                GetOrdersFromBestBuyViewModel.BestBuyRootObjectBB bestBuyRootObject = new GetOrdersFromBestBuyViewModel.BestBuyRootObjectBB();

                bestBuyRootObject = GetBestBuyOrdersLasthundred(_getChannelCredViewModel.Key);

                string AllOrderCommaSeprate = string.Join(",", bestBuyRootObject.orders.Select(e => e.order_id));


                List<string> AllreadyExistOrder = _bestBuytDataAccessNew.GetOrderAlreadyExist(AllOrderCommaSeprate);
                var NewOrders = bestBuyRootObject.orders.Select(e => e.order_id).Except(AllreadyExistOrder);

                foreach (var item in NewOrders)
                {
                    var result = bestBuyRootObject.orders.Where(e => e.order_id == item).FirstOrDefault();

                    int bbOrderID = _bestBuytDataAccessNew.SaveBestBuyOrderINOrder(result);

                    if (bbOrderID != 0)
                    {
                        _bestBuytDataAccessNew.SaveBestBuyOrderINOrderLines(result, bbOrderID);
                        _bestBuytDataAccessNew.SaveBestBuyOrderINOrderLookUp(result, bbOrderID);
                        _bestBuytDataAccessNew.SaveBestBuyOrderINCustomerShipping(result, bbOrderID);
                        var AllSKUCommaSeprate = result.order_lines.Select(p => p.offer_sku).ToList();
                        List<DropShipAndQtyOrderViewModel> listqty = _bestBuytDataAccessNew.GeQtyAndDropShip(AllSKUCommaSeprate);
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

                //foreach (var item in AllreadyExistOrder)
                //{
                //    var result = bestBuyRootObject.orders.Where(e => e.order_id == item).FirstOrDefault();
                //    _bestBuytDataAccessNew.UpdateBestBuyOrderINOrder(result);
                //    _bestBuytDataAccessNew.UpdateBestBuyOrderINOrderLines(result);

                //    _bestBuytDataAccessNew.UpdateBestBuyOrderINCustomerShipping(result);

                //}
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
              //  client.Send(mail);
                //return "Email Sent Successfully!";

            }
            catch (System.Exception e)
            {
                //return e.Message;
            }

        }
    }
}

