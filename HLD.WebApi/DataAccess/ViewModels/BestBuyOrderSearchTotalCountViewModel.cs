using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hld.WebApplication.ViewModel
{
    public class BestBuyOrderSearchTotalCountViewModel
    {
        public string ZincStatus { get; set; }
        public string OrderStatus { get; set; }
        public int MarketPlaces { get; set; }
        public int StartIndex { get; set; }
        public int EndIndex { get; set; }
        public string Sort { get; set; }
        public string DateFrom { get; set; }
        public string DateTo { get; set; }
        public string Sku { get; set; }
        public string DSStatus { get; set; }
        public string PaymentStatus { get; set; }
        public string ShippingTags { get; set; }
        public int ShippingBoxContain { get; set; }
        public string BBOrderStatus { get; set; }
        public int? WHQStatus { get; set; }
    }
}
