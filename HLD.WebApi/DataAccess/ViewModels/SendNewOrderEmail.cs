using DataAccess.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ViewModels
{
    public class SendNewOrderEmail
    {

        public void SendrderEmail(List<BestBuyOrdersViewModel> mainmodel)
        {
            try
            {
                foreach (var model in mainmodel)
                {
                    // Credentials
                    var credentials = new NetworkCredential("AKIAJ2ZYJS2WHV3TBFYQ", "BO6Ht4m/+okdb40r13HNeQrGWOB82n6gvU1P3WtO9vDp");
                    // Mail message
                    StringBuilder stringBuilder = new StringBuilder();
                    stringBuilder.Append("The order detail is as follow");
                    stringBuilder.Append("<br><br>");
                    stringBuilder.Append("SC Order# &nbsp; <a style ='cursor:pointer;' href = 'https://lp.cwa.sellercloud.com/Orders/Orders_Details.aspx?ID=" + model.SellerCloudOrderID + "' target = '_blank' > " + model.SellerCloudOrderID + "</a> &nbsp;&nbsp; BBOrder#&nbsp;" +
                                   "<a style = 'cursor: pointer' href = 'https://marketplace.bestbuy.ca/mmp/shop/order/" + model.OrderNumber + "' target = '_blank' > " +
                                   model.OrderNumber + "</a>");
                    stringBuilder.Append("<br>");

                    //creating table 
                    stringBuilder.Append("<table style='width:70%;border-collapse: collapse;'>");
                    // table headers
                    stringBuilder.Append(@" <tr>
            <th style=' border: 1px solid black;width:30px'> Image </th>
            <th style='border: 1px solid black;width: 120px'> SKU </th>
            <th style=' border: 1px solid black'> Title </th>
            <th style=' border: 1px solid black;width:115px'> Price </th> 
            <th style=' border: 1px solid black;width:65px;'> WH Qty </th>
            </tr>");
                    //table rows

                    foreach (var item in model.BBProductDetail)
                    {
                        stringBuilder.Append("<tr >");
                        stringBuilder.Append("<td style=' border: 1px solid black'>" +
                           "<img src = 'https://s3.us-east-2.amazonaws.com/upload.hld.erp.images/" + item.ImageUrl + "' class='rounded' height='50' width='50'>" +
                            "</td>");
                        stringBuilder.Append("<td style=' border: 1px solid black;vertical-align:top'>" +
                            "<span>" + item.ProductSKU + "</span><br/><span>QTY :" + item.TotalQuantity + "</span>" +
                            "<div><a style='padding-left:5px; cursor: pointer' href='https://lp.cwa.sellercloud.com/Inventory/Product_Dashboard.aspx?Id=" + model.SellerCloudOrderID + "' target='_blank'> " +
                                     "SCloud</a>" +
                                     "<a style = 'padding-left:5px; cursor: pointer' href = 'https://marketplace.bestbuy.ca/mmp/shop/offer?productId=" + item.BestBuyPorductID + "' target = '_blank' > " +
                                     "BBLink</a>" +
                                     "<a style = 'padding-left:5px; cursor: pointer' href = 'https://erp.hldinc.net/BestBuyProduct/PropertyPage?ProductSKU=" + item.ProductSKU + "' target = '_blank' > " +
                                     "BBuy</a>" +
                                     " </div> " +
                            "</td>");
                        stringBuilder.Append("<td style=' border: 1px solid black;vertical-align:top'>" +
                           "<span>" + item.ProductTitle + "</span>" +
                           "</td>");

                        stringBuilder.Append("<td style=' border: 1px solid black;vertical-align:top'>" +
                          "<div> Price: " + item.calculation_TotalAmountOfUnitPrice + "</div>" +
                          "<div> Tax: " + item.calculation_TotalTax + "</div>" +
                          "<div> Fee: " + item.calculation_Comission + "</div>" +
                          "<div> AvgCost: " + item.caculation_TotalAvgCost + "</div>" +
                          "<div> P&L: " + item.calculation_ProfitLoss + "(" + item.calculation_ProfitLossPercentage + "%)" + "</div>" +
                          "</td>");

                        string checkboxValue = item.DropshipStatus == true ? "checked" : "";

                        stringBuilder.Append("<td style=' border: 1px solid black;vertical-align:top'>" +
                          "<div> DS: <input type='checkbox' " + checkboxValue + " ></div>" +
                          "<div> DSQ:" + item.DropshipQty + "</div>" +
                          "</td>");

                        stringBuilder.Append("</tr>");
                    }
                    stringBuilder.Append("</tr>");
                    stringBuilder.Append("</table>");

                    stringBuilder.Append("<div>" +
                        "<h3>Summary</h3>" +
                        "<div>Price: " + model.TotalPrice + "</div>" +
                        "<div>Fees: " + model.TotalComission + "</div>" +
                        "<div>Avg Cost: " + model.TotalAverageCost + "</div>" +
                        "<div>P&L: " + model.ProfitAndLossInDollar + "(" + model.ProfitAndLossInPercentage + "%)</div>" +
                        "<div>" +
                        "</div>");


                    stringBuilder.Append("&nbsp;&nbsp;&nbsp;&nbsp;");

                    var mail = new MailMessage()
                    {
                        From = new MailAddress("info@hldinc.net"),
                        Subject = "HLD Item Sold",
                        Body = stringBuilder.ToString()
                    };
                    mail.IsBodyHtml = true;
                    mail.To.Add(new MailAddress("amirjavaid06@gmail.com"));
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

            }
        }
    }

}

