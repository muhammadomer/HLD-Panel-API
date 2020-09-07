using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ViewModels
{
  public class ZincProductOfferViewModel
    {
        public class Seller
        {
            public string name { get; set; }
            public string id { get; set; }
            public int? num_ratings { get; set; }
            public int? percent_positive { get; set; }
            public bool first_party { get; set; }
        }

        public class HandlingDays
        {
            public int? min { get; set; }
            public int? max { get; set; }
        }

        public class DeliveryDays
        {
            public int? min { get; set; }
            public int? max { get; set; }
        }

        public class ShippingOption
        {
            public DeliveryDays delivery_days { get; set; }
            public string greytext { get; set; }
            public int? price { get; set; }
            public object name { get; set; }
        }

        public class Offer
        {
            public Seller seller { get; set; }
            public object coupon_discount_percent { get; set; }
            public object coupon_discount_fixed { get; set; }
            public string offer_id { get; set; }
            public int? price { get; set; }
            public string currency { get; set; }
            public bool available { get; set; }
            public HandlingDays handling_days { get; set; }
            public bool marketplace_fulfilled { get; set; }
            public bool fba_badge { get; set; }
            public bool prime_badge { get; set; }
            public List<ShippingOption> shipping_options { get; set; }
            public string condition { get; set; }
            public object minimum_quantity { get; set; }
            public bool prime_only { get; set; }
            public bool addon { get; set; }
            public object comments { get; set; }
            public string greytext { get; set; }
            public bool known_greytext { get; set; }
            public bool international { get; set; }
            public bool expired_product_id { get; set; }
            public string asin { get; set; }
            public object buy_box_winner { get; set; }
        }

        public class RootObject
        {
            public int? timestamp { get; set; }
            public string status { get; set; }
            public List<Offer> offers { get; set; }
            public string asin { get; set; }
            public string retailer { get; set; }
            public bool expired_product_id { get; set; }
            public string strategy { get; set; }
        }
    }
}
