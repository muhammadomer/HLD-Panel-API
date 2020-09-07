using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ViewModels
{
    public class BestBuyUpdateQty_Update_ViewModel
    {
        public class Range
        {
            public int? price { get; set; }
            public int? quantity_threshold { get; set; }
        }

        public class Discount
        {
            public DateTime? end_date { get; set; }
            public double? price { get; set; }           
            public DateTime? start_date { get; set; }
        }

        public class OfferAdditionalField
        {
            public string code { get; set; }
            public string value { get; set; }
        }

        public class Offer
        {
            public DateTime? available_ended { get; set; }
            public DateTime? available_started { get; set; }
            public string description { get; set; }
            public Discount discount { get; set; }
            
            public int min_quantity_alert { get; set; }
            public List<OfferAdditionalField> offer_additional_fields { get; set; }
            public double? price { get; set; }
            public string price_additional_info { get; set; }
            public string product_id { get; set; }
            public string product_id_type { get; set; }
            public int quantity { get; set; }
            public string shop_sku { get; set; }
            public string state_code { get; set; }
            public string update_delete { get; set; }
        }

        public class RootObject
        {
            public List<Offer> offers { get; set; }
        }
    }
}
