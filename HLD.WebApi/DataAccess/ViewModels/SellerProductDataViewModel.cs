using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ViewModels
{
  public class SellerProductDataViewModel
  {
        public double FinalShippingFee { get; set; }
        public string ProductID { get; set; }
        public int Qty { get; set; }
        public double AdjustedSitePrice { get; set; }
        public DateTime TimeOfOrder { get; set; }
        public string OrderSourceOrderID { get; set; }
        public int OrderSource { get; set; }
        public int ShippingStatus { get; set; }
        public int PaymentStatus { get; set; }
        public int StatusCode { get; set; }
        public double GrandTotal { get; set; }
        public string CompanyName { get; set; }
        public int CompanyID { get; set; }
        public string DestinationCountry { get; set; }
        public int OrderID { get; set; }
        public string DisplayName { get; set; }
        public double FinalValueFee { get; set; }
        public double AverageCost { get; set; }
        public int OrderCurrencyCode { get; set; }
    }
}
