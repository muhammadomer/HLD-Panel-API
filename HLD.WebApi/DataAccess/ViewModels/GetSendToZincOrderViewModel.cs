using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ViewModels
{
    public class GetAddressViewModel
    {
        public string WH_Name { get; set; }
    }
    public class GetSendToZincOrderViewModel
    {
        public int OrderId { get; set; }
        public bool internalStatus { get; set; }
        public string order_type { get; set; }
        public string amazon_tracking { get; set; }
        public string _tracking { get; set; }
        public string carrier { get; set; }
        public string shpping_date { get; set; }
        public string merchant_order_id { get; set; }
        public string RequestId { get; set; }
        public string Asin { get; set; }
        public string Sku { get; set; }
        public string AccountDetail { get; set; }
        public string name_on_card { get; set; }
        public string UserName { get; set; }
        public string OrderDate { get; set; }
        public decimal Tax { get; set; }
        public decimal Shipping { get; set; }
        public decimal AmzSubTotal { get; set; }
        public DateTime Date { get; set; }
        public string TrackingNumber { get; set; }
        public string Response { get; set; }
        public string CompressedImage { get; set; }
        public string ImageName { get; set; }
        public DateTime LastUpdate { get; set; }
        public int Qty { get; set; }
        public decimal Price { get; set; }
        public int RecievedOrderQty { get; set; }
        public DateTime RecievedOrderDate { get; set; }
        // public decimal MaxPrice { get; set; }
    }
}
