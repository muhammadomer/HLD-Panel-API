using DataAccess.DataAccess;
using DataAccess.Helper;
using DataAccess.ViewModels;
using HLD.WebApi.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;

namespace HLD.WebApi.Jobs
{
    public class SendNewOrderEmailAfter: ISendEmailOfNewOrder
    {
        ProductDataAccess productData = null;
        BestBuyProductDataAccess _bestBuyProductDataAccess = null;
        public SendNewOrderEmailAfter(IConnectionString connectionString)
        {
            productData = new ProductDataAccess(connectionString);
            _bestBuyProductDataAccess = new BestBuyProductDataAccess(connectionString);
        }
        public async Task SendNewEmail(int SCOrderId)
        {
            try
            {
                List<BestBuyOrdersViewModel> mainmodel = new List<BestBuyOrdersViewModel>();
                mainmodel= _bestBuyProductDataAccess.GetAllBestBuyOrdersWithGlobalFilter("SCOrders.seller_cloud_order_id", SCOrderId.ToString(), 25, 0, "desc");
                foreach (var model in mainmodel)
                {
                    // Credentials
                    var credentials = new NetworkCredential("AKIAJ2ZYJS2WHV3TBFYQ", "BO6Ht4m/+okdb40r13HNeQrGWOB82n6gvU1P3WtO9vDp");
                    string messageBody = "<span style=\"font-size: 22px;\">Order View #&nbsp;" + "<a style = 'cursor: pointer' href = 'https://erp.hldinc.net/BBOrderViewPage/OrderViewPage?SCorderid=" + model.SellerCloudOrderID + "' target = '_blank' >" + model.SellerCloudOrderID + "</a>" +
                    " &nbsp;&nbsp;SC Order# &nbsp;" + "<a style ='cursor:pointer;' href = 'https://lp.cwa.sellercloud.com/Orders/Orders_Details.aspx?ID=" + model.SellerCloudOrderID + "' target = '_blank' > " + model.SellerCloudOrderID + "</a> &nbsp;&nbsp;" +
                    "BB&nbsp;Order#&nbsp;" + "<a style = 'cursor: pointer' href = 'https://marketplace.bestbuy.ca/mmp/shop/order/" + model.OrderNumber + "' target = '_blank' > " + model.OrderNumber + "</a>" + "</span><br>" +
                    "<hr style='width:85% margin-left: 0px; border-bottom: 1px solid cornflowerblue;' />";
                    //messageBody = messageBody + "<br><br>" + "<b>Customer Address: </b>" + model.ShipmentAddress;
                    string htmlTableStart = "<!DOCTYPE html><html><head><meta charset='UTF-8'> " +
                        "<meta name='viewport' content='width=device-width,initial-scale = 1.0'>" +
                        "</head><body>" +
                        "<div style='font-size:22px;margin-top: 5px;'><table border=" + 0 + " cellpadding=" + 0 + " cellspacing=" + 0 + " style=\"font-size:22px;line-height:1.2; font-family:Arial;margin 0 auto;display:block;margin-top: 5px;\">";
                    string htmlTableEnd = "</table></body></html></div>";
                    //string htmlHeaderRowStart = "<tr style =\"background-color:#5f9ea0; color:#ffffff;\">";
                    //string htmlHeaderRowEnd = "</tr>";
                    string htmlTrStart = "<tr style =\"color:#00000;\">";
                    string htmlTrEnd = "</tr>";
                    string htmlTdStart = "<td style=\" border:none;font-family:arial;line-height: 1.2;vertical-align:top;\">";

                    string htmlTdEnd = "</td>";
                    string htmlspanEnd = "</span>";

                    //messageBody += htmlHeaderRowStart;
                    //messageBody += htmlTdStart + "Image" + htmlTdEnd + htmlTdStart + "SKU " + htmlTdEnd + htmlTdStart + "Price" + htmlTdEnd + htmlTdStart + " WH Qty " + htmlTdEnd + htmlTd5Start + "Title" + htmlTdEnd;
                    ////messageBody += htmlHeaderRowEnd;
                    var num = model.BBProductDetail.Count;
                    var shippingCost = 10 / model.BBProductDetail.Count;
                    foreach (var item in model.BBProductDetail)
                    {


                        //var PNL = Convert.ToDecimal(item.calculation_ProfitLoss) - Convert.ToDecimal(10 / num);
                        var PNL = (item.calculation_TotalAmountOfUnitPrice+ item.ShippingFee )- (item.calculation_Comission + item.caculation_TotalAvgCost + shippingCost);
                        var PNLPer = (PNL / item.calculation_TotalAmountOfUnitPrice) * 100;
                        messageBody += htmlTableStart;

                        var product = productData.GetCatalog(item.ProductSKU);
                        var Tags = productData.GetTagforSkuEmail(item.ProductSKU);
                        var tagesContent = "";

                        foreach (var tag in Tags)
                        {
                            tagesContent += "<span style = \"padding:3px; border:none; border-radius:3px; color:white; margin-right:5px; background-color:" + tag.TagColor + ";\">" + tag.TagName + " </span>";
                        }
                        string checkboxValue = item.DropshipStatus == true ? "checked" : "";

                        string P_L = "<span >" + PNL + "&nbsp;" + "(" + (Math.Round(PNLPer)) + "%)" + htmlspanEnd;
                        if (item.calculation_ProfitLossPercentage < 15)
                        {
                            P_L = "<span style=\" color:red; font-weight: bold; width: 160px;float:right\">" + PNL + "&nbsp;" + "(" + (Math.Round(PNLPer)) + "%)" + htmlspanEnd;
                        }
                        string Fees = "<span class='pull-right'>" + item.calculation_Comission+" ("+(Math.Round(item.calculation_comissionPercentage)) + "% )" + htmlspanEnd;
                        if (item.calculation_comissionPercentage > 15)
                        {
                            Fees = "<span class='pull-right' style=\" \">" + item.calculation_Comission +" ("+(Math.Round(item.calculation_comissionPercentage)) + "% )" + htmlspanEnd;
                        }
                        var sale60 = Math.Round(((Convert.ToDecimal(product.QtySold60)) / Convert.ToDecimal(60)), 2);
                        var sale60velocity = "<span class='pull-right'>" + sale60 + "</span>";
                        product.PhysicalQty = product.PhysicalQty < 0 ? 0 : product.PhysicalQty;
                        var LowStock60 = Math.Round(((Convert.ToDecimal(product.QtySold60)) / Convert.ToDecimal(60)) * Convert.ToDecimal(60) - Convert.ToDecimal(product.PhysicalQty) - Convert.ToDecimal(product.OnOrder), 2);
                        var lowstock = "<span class='pull-right'>" + LowStock60 + "</span>";
                        if (LowStock60 > 5)
                        {

                            lowstock = "<span class='pull-right' style=\" color:red; font-weight: bold; \">" + LowStock60 + "</span>";
                        }
                        var WHQ = item.ProductrWarehouseQtyViewModel.Where(e => e.WarehouseName == "HLD-CA1").Select(e => e.AvailableQty).FirstOrDefault();
                        var days = sale60 > 0 ? Math.Round(WHQ / sale60) : 0;
                        messageBody = messageBody + htmlTrStart;
                        messageBody = messageBody + htmlTdStart +
                        "<span style=\'width:300px;max-width:350px;\'><a style='' target='_blank' href='https://s3.us-east-2.amazonaws.com/upload.hld.erp.images/" + item.ImageUrl + "'> <img src = 'https://s3.us-east-2.amazonaws.com/upload.hld.erp.images.thumbnail/" + item.ImageUrl + "' class='rounded'width='235'style='border:none;border-right:2px solid black;max-width:100;border-bottom:2px solid black'></a></span><br> " + "<div style=\"max-width:190px!important;width:190px!important;font-weight:100!important;margin-bottom:5px!important;\">" + item.ProductTitle + "</div> " + htmlTdEnd
                        + htmlTdStart + "<span style = 'display:inline;display:flex;'>" + item.ProductSKU + "<span >&nbsp;&nbsp;QTY: " + item.TotalQuantity + "</span></span>" +
                        "<span>Shadow Of: " + product.ShadowOf + " </span><br>" +

                        //"QTY :" + item.TotalQuantity + "<br>" +
                        "<span><a style='cursor: pointer;vertical-align: top;' href='https://lp.cwa.sellercloud.com/Inventory/Product_Dashboard.aspx?Id=" + item.ProductSKU + "' target='_blank'><img src = 'https://s3.us-east-2.amazonaws.com/icon.erp.email/sellercloud-gray.png' class='rounded'width='35' height='35'></a>&nbsp;&nbsp;" +
                        "<a style = 'padding-left:8px;vertical-align: top; cursor: pointer' href = 'https://www.bestbuy.ca/en-ca/search?search=+" + item.BestBuyPorductID + "' target = '_blank' ><img src = 'https://s3.us-east-2.amazonaws.com/icon.erp.email/openbb.png' class='rounded'width='35' height='28'></a>&nbsp;&nbsp;" +
                        "<a style = 'padding-left:8px;vertical-align: top; cursor: pointer' href = 'https://erp.hldinc.net/Zinc/ProductZinc?ProductSKU=" + item.ProductSKU + "' target = '_blank' ><img src = 'https://s3.us-east-2.amazonaws.com/icon.erp.email/zinc.png' class='rounded'width='35' height='35'></a>&nbsp;&nbsp;" +
                        "<a style = 'padding-left:8px;vertical-align: top; cursor: pointer' href = 'https://marketplace.bestbuy.ca/mmp/shop/offer?productId=" + item.BestBuyPorductID + "' target = '_blank' ><img src = 'https://s3.us-east-2.amazonaws.com/icon.erp.email/BestBuyImage.png' class='rounded'width='35' height='28'></a>&nbsp;&nbsp;" + "</span></span>" +
                        " <br>" +
                        "<table>" +
                        "<tr>" +
                        "<td>Sale Price: </td>" +
                        "<td>" +item.TotalQuantity+" * "+item.UnitPrice+" = "+ item.calculation_TotalAmountOfUnitPrice + " + " + item.ShippingFee + "</td>" +
                        "</tr>" +
                        "<tr>" +
                        "<td>Fee: </td>" +
                        "<td>" + Fees + "</td>" +
                        "</tr>" +
                        "<tr>" +
                        "<td>AvgCost: </td>" +
                        "<td>" + item.caculation_TotalAvgCost + "</td>" +
                        "</tr>" +
                        "<tr>" +
                        "<td>Ship Cost: </td>" +
                        "<td>" + 10 + "</td>" +
                        "</tr>" +
                        "<tr>" +
                        "<td>P&L: </td>" +
                        "<td>" + P_L + "</td>" +
                        "</tr>" +
                        "<tr>" +
                        "<td>WHQ: </td>" +
                        "<td>" + item.ProductrWarehouseQtyViewModel.Where(e => e.WarehouseName == "HLD-CA1").Select(e => e.AvailableQty).FirstOrDefault() + " (" + days + " days)" + "</td>" +
                        "</tr>" +
                        "<tr>" +
                        "<td>Agg Qty: </td>" +
                        "<td>" + product.AggregateQty + "</td>" +
                        "</tr>" +
                        //"<tr>" +
                        //"<td>Velocity: </td>" +
                        //"<td>" + sale60velocity + "</td>" +
                        //"</tr>" +
                        "<tr>" +
                        "<td>Sale60: </td>" +
                        "<td><span><a style = 'cursor: pointer;text-decoration:none' href = 'https://lp.cwa.sellercloud.com/Inventory/Product_Purchase.aspx?Id=" + item.ProductSKU + "' target = '_blank' > " + product.QtySold60 + "</a>" + " (" + sale60velocity + ")" + "</span></td>" +
                        "</tr>" +
                        "<tr>" +
                        "<td><a style = 'cursor: pointer' href ='https://erp.hldinc.net/PurchaseOrder/GetPOProduct?Vendor=&VendorID=&POID=&title=&ItemType=&orderDateTimeFrom=&orderDateTimeTo==&SKU=" + item.ProductSKU + "' target = '_blank' >On Order: </a></td>" +
                        "<td><span>" + "<a style = 'cursor: pointer;text-decoration:none' href = 'https://lp.cwa.sellercloud.com/Inventory/Product_OnOrderDetail.aspx?ID=" + item.ProductSKU + "' target = '_blank' > " + product.OnOrder + "</a>" + "</span></td>" +
                        "</tr>" +
                        "<tr>" +
                        "<td>LowStock60: </td>" +
                        "<td>" + lowstock + "</td>" +
                        "</tr>" +
                        "<tr>" +
                        "<td>DS: </td>" +
                        "<td><input style='width:23px;height:23px;' type='checkbox' " + checkboxValue + "/></td>" +
                        "</tr>" +
                        "<tr>" +
                        "<td>Location Notes: </td>" +
                        "<td>" + item.LocationNotes + "</td>" +
                        "</tr>" +
                        "<tr>" + tagesContent + "</tr>" +
                        "</table>" +



                        htmlTdEnd

                        ;
                        messageBody = messageBody + htmlTrEnd;
                        messageBody = messageBody + htmlTableEnd;

                        messageBody = messageBody + "<hr style='width: 556px; margin-left: 0px; border-bottom: 2px solid cornflowerblue;' />";

                    }
                    decimal sumtotalPrice = model.TotalPrice + (Convert.ToDecimal(model.ShippingPrice));

                    var FeePer = Math.Round(((model.TotalComission / model.TotalPrice) * 100));
                    var totalCommision = Math.Round(model.TotalComission, 2);
                    var PNLModel = sumtotalPrice - (totalCommision + model.TotalAverageCost + 10);
                    var PNLPerModel = Math.Round((PNLModel / model.TotalPrice) * 100);
                   
                    messageBody = messageBody +

                    "<div style='font-size:22px;'>" +
                    "<h3>Summary</h3>" +
                    "<table>" +
                    "<tr><td><b>Price: </b> </td><td>" + model.TotalPrice + " + " + model.ShippingPrice + "</td></tr>" +
                    "<tr><td><b>Fees: </b> </td><td>" + totalCommision + " (" + FeePer + "%)" + "</td></tr>" +
                    "<tr><td><b>Avg Cost: </b></td><td>" + model.TotalAverageCost + "</td></tr>" +
                    "<tr><td><b>P&L: </b></td><td>" + PNLModel + "&nbsp;(" + PNLPerModel + "%)" + "</td></tr>" +
                        "</table>" +
                        "</div>";

                    //var mail = new MailMessage()
                    //{
                    // From = new mailaddress("testcrmphenologix@gmail.com"),
                    // Subject = "HLD Item Sold " + model.SellerCloudOrderID,
                    // Body = messageBody.ToString()
                    //};


                    //mail.To.Add(new mailaddress("adeel.ahmad8000@gmail.com", "password"));
                    //// Smtp client
                    //var client = new SmtpClient()
                    //{
                    // Port = 587,
                    // DeliveryMethod = SmtpDeliveryMethod.Network,
                    // UseDefaultCredentials = true,
                    // Host = "smtp.gmail.com",
                    // //Host = "email-smtp.us-east-1.amazonaws.com",
                    // EnableSsl = true,
                    // Credentials = credentials
                    //};
                    //client.Send(mail);
                    //return "Email Sent Successfully!";




                    //using (MailMessage mail = new MailMessage())
                    //{
                    //    mail.From = (new MailAddress("muhammadmueen9692@gmail.com"));
                    //    mail.To.Add("muhammadmueen9691@gmail.com");
                    //    mail.Subject = "Please confirm your email address";
                    //    mail.Body = messageBody;
                    //    mail.IsBodyHtml = true;
                    //    using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                    //    {
                    //        smtp.UseDefaultCredentials = false;
                    //        smtp.Credentials = new NetworkCredential("muhammadmueen9692@gmail.com", "ASDF/1234");

                    //        smtp.EnableSsl = true;
                    //        smtp.Send(mail);
                    //    }
                    //}

                    var mail = new MailMessage()
                    {
                        From = new MailAddress("info@hldinc.net"),
                        Subject = "HLD Item Sold " + model.SellerCloudOrderID,
                        Body = messageBody,
                        IsBodyHtml = true
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
                    await client.SendMailAsync(mail);


                }
            }
            catch (Exception ex)
            {
            }


        }

       

       
    }
}
