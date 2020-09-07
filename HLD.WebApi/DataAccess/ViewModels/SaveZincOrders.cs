using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ViewModels
{
    public class SaveZincOrders
    {
        public class ClientNotes
        {
            public int our_internal_order_id { get; set; }
        }

        public class Product
        {
            public string product_id { get; set; }
            public int quantity { get; set; }
        }

        public class ShippingAddress
        {
            public string first_name { get; set; }
            public string last_name { get; set; }
            public string address_line1 { get; set; }
            public string address_line2 { get; set; }
            public string zip_code { get; set; }
            public string city { get; set; }
            public string state { get; set; }
            public string country { get; set; }
            public string phone_number { get; set; }
        }

        public class PaymentMethod
        {
            public bool use_gift { get; set; }
        }

        public class RetailerCredentials
        {
            public string email { get; set; }
            public string password { get; set; }
            public string totp_2fa_key { get; set; }
        }

        public class BillingAddress
        {
            public string first_name { get; set; }
            public string last_name { get; set; }
            public string address_line1 { get; set; }
            public string address_line2 { get; set; }
            public string zip_code { get; set; }
            public string city { get; set; }
            public string state { get; set; }
            public string country { get; set; }
            public string phone_number { get; set; }
        }

        public class Shipping
        {
            public string order_by { get; set; }
            public int max_days { get; set; }
            public int max_price { get; set; }
        }

        public class RootObject
        {
            public string retailer { get; set; }
            public ClientNotes client_notes { get; set; }
            public List<Product> products { get; set; }
            public int max_price { get; set; }
            public ShippingAddress shipping_address { get; set; }
            public bool is_gift { get; set; }
            public string gift_message { get; set; }
            public PaymentMethod payment_method { get; set; }
            public RetailerCredentials retailer_credentials { get; set; }
            public BillingAddress billing_address { get; set; }
            public Shipping shipping { get; set; }
            public List<CreditCardDetailViewModel> CreditCardDetail { get; set; }
            public List<ZincAccountsViewModel> ZincAccounts { get; set; }
        }
    }
}
