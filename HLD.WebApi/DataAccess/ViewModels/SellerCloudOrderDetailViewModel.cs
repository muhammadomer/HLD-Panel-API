using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ViewModels
{
   public class SellerCloudOrderDetailViewModel
    {
        public DateTime DropShippedOn { get; set; }
        public string DropShippedStatus { get; set; }
        public int OrderId { get; set; }
        public int MinQTY { get; set; }
        public string SKU { get; set; }
        public string StatusCode { get; set; }
        public int Qty { get; set; }
        public decimal AdjustedSitePrice { get; set; }
        public decimal AverageCost { get; set; }
        public decimal PricePerCase { get; set; }
        public string unitPrice { get; set; }
        public string ProductTitle { get; set; }
        public string UPC { get; set; }
    }
}
