using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ViewModels
{
   public class ImportDataFromBestBuy
    {
        public class BillingAddress
        {
            public string city { get; set; }
            public string civility { get; set; }
            public string company { get; set; }
            public string country { get; set; }
            public object country_iso_code { get; set; }
            public string firstname { get; set; }
            public string lastname { get; set; }
            public string phone { get; set; }
            public string phone_secondary { get; set; }
            public string state { get; set; }
            public string street_1 { get; set; }
            public string street_2 { get; set; }
            public string zip_code { get; set; }
        }

        public class ShippingAddress
        {
            public string additional_info { get; set; }
            public string city { get; set; }
            public string civility { get; set; }
            public string company { get; set; }
            public string country { get; set; }
            public object country_iso_code { get; set; }
            public string firstname { get; set; }
            public string lastname { get; set; }
            public string phone { get; set; }
            public string phone_secondary { get; set; }
            public string state { get; set; }
            public string street_1 { get; set; }
            public string street_2 { get; set; }
            public string zip_code { get; set; }
        }

        public class Customer
        {
            public BillingAddress billing_address { get; set; }
            public string civility { get; set; }
            public string customer_id { get; set; }
            public string firstname { get; set; }
            public string lastname { get; set; }
            public string locale { get; set; }
            public ShippingAddress shipping_address { get; set; }
        }

        public class CommissionTax
        {
            public double amount { get; set; }
            public string code { get; set; }
            public string rate { get; set; }
        }

        public class OrderLineAdditionalField
        {
            public string code { get; set; }
            public string type { get; set; }
            public string value { get; set; }
        }

        public class ShippingTax
        {
            public string amount { get; set; }
            public string code { get; set; }
        }

        public class Tax
        {
            public double amount { get; set; }
            public string code { get; set; }
        }

        public class OrderLine
        {
            public bool can_refund { get; set; }
            public List<object> cancelations { get; set; }
            public string category_code { get; set; }
            public string category_label { get; set; }
            public double commission_fee { get; set; }
            public string commission_rate_vat { get; set; }
            public List<CommissionTax> commission_taxes { get; set; }
            public double commission_vat { get; set; }
            public DateTime? created_date { get; set; }
            public DateTime? debited_date { get; set; }
            public object description { get; set; }
            public DateTime last_updated_date { get; set; }
            public string offer_id { get; set; }
            public string offer_sku { get; set; }
            public string offer_state_code { get; set; }
            public List<OrderLineAdditionalField> order_line_additional_fields { get; set; }
            public string order_line_id { get; set; }
            public string order_line_index { get; set; }
            public string order_line_state { get; set; }
            public object order_line_state_reason_code { get; set; }
            public object order_line_state_reason_label { get; set; }
            public double price { get; set; }
            public object price_additional_info { get; set; }
            public double price_unit { get; set; }
            public List<object> product_medias { get; set; }
            public string product_sku { get; set; }
            public string product_title { get; set; }
            public List<object> promotions { get; set; }
            public string quantity { get; set; }
            public DateTime? received_date { get; set; }
            public List<object> refunds { get; set; }
            public DateTime? shipped_date { get; set; }
            public string shipping_price { get; set; }
            public object shipping_price_additional_unit { get; set; }
            public object shipping_price_unit { get; set; }
            public List<ShippingTax> shipping_taxes { get; set; }
            public List<Tax> taxes { get; set; }
            public double total_commission { get; set; }
            public double total_price { get; set; }
        }

        public class Promotions
        {
            public List<object> applied_promotions { get; set; }
            public string total_deduced_amount { get; set; }
        }

        public class Order
        {
            public DateTime acceptance_decision_date { get; set; }
            public bool can_cancel { get; set; }
            public object channel { get; set; }
            public string commercial_id { get; set; }
            public DateTime created_date { get; set; }
            public string currency_iso_code { get; set; }
            public Customer customer { get; set; }
            public DateTime? customer_debited_date { get; set; }
            public bool has_customer_message { get; set; }
            public bool has_incident { get; set; }
            public bool has_invoice { get; set; }
            public DateTime last_updated_date { get; set; }
            public string leadtime_to_ship { get; set; }
            public List<object> order_additional_fields { get; set; }
            public string order_id { get; set; }
            public List<OrderLine> order_lines { get; set; }
            public string order_state { get; set; }
            public object order_state_reason_code { get; set; }
            public object order_state_reason_label { get; set; }
            public string paymentType { get; set; }
            public string payment_type { get; set; }
            public string payment_workflow { get; set; }
            public double price { get; set; }
            public Promotions promotions { get; set; }
            public object quote_id { get; set; }
            public string shipping_carrier_code { get; set; }
            public string shipping_company { get; set; }
            public string shipping_price { get; set; }
            public string shipping_tracking { get; set; }
            public string shipping_tracking_url { get; set; }
            public string shipping_type_code { get; set; }
            public string shipping_type_label { get; set; }
            public string shipping_zone_code { get; set; }
            public string shipping_zone_label { get; set; }
            public double total_commission { get; set; }
            public double total_price { get; set; }
        }

        public class BestBuyRootObject
        {
            public List<Order> orders { get; set; }
            public string total_count { get; set; }
        }
    }
}
