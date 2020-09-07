using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ViewModels
{
   public class ZincWebhooksSuccessResponse
    {
        public List<CheckoutItem> _checkout_items { get; set; }
        public DateTime? _created_at { get; set; }
        public string _driver { get; set; }
        public string _type { get; set; }
        public AccountStatusSuccess account_status { get; set; }
        public List<DeliveryDate2> delivery_dates { get; set; }
        public int? gift_balance { get; set; }
        public double? gift_balance_time { get; set; }
        public List<MerchantOrderId> merchant_order_ids { get; set; }
        public List<string> offers_urls { get; set; }
        public PriceComponents price_components { get; set; }
        public int? prior_gift_balance { get; set; }
        public double? prior_gift_balance_time { get; set; }
        public RequestSuccess request { get; set; }
        public List<string> screenshot_urls { get; set; }
        public string request_id { get; set; }
        public List<StatusUpdate> status_updates { get; set; }
        public Stats stats { get; set; }
    }

    public class DeliveryDate
    {
        public string month { get; set; }
        public string promise { get; set; }
        public string human { get; set; }
        public string year { get; set; }
        public object promise_quality { get; set; }
        public string day { get; set; }
    }

    public class CheckoutItem
    {
        public string seller_id { get; set; }
        public string product_id { get; set; }
        public bool has_prime_icon { get; set; }
        public string title { get; set; }
        public string line_item_id { get; set; }
        public DeliveryDate delivery_date { get; set; }
        public int quantity { get; set; }
    }

    public class AccountStatusSuccess
    {
        public bool prime { get; set; }
        public bool fresh { get; set; }
        public DateTime prime_expiration_time { get; set; }
        public bool business { get; set; }
        public object charity { get; set; }
    }

    public class ProductSuccess
    {
        public string product_id { get; set; }
        public int quantity { get; set; }
    }

    public class DeliveryDate2
    {
        public List<ProductSuccess> products { get; set; }
        public string delivery_date { get; set; }
    }

    public class Product2
    {
        public string seller_id { get; set; }
        public string product_id { get; set; }
    }

    public class MerchantOrderId
    {
        public string merchant { get; set; }
        public string account { get; set; }
        public DateTime placed_at { get; set; }
        public List<Product2> products { get; set; }
        public List<string> product_ids { get; set; }
        public string merchant_order_id { get; set; }
        public string shipping_address { get; set; }
        public string tracking_url { get; set; }
        public string delivery_date { get; set; }
    }

    public class ProductSubtotals
    {
     //   public int B07DN6V52X { get; set; }
    }

    public class PriceComponents
    {
        public int shipping { get; set; }
        public int tax { get; set; }
        public ProductSubtotals product_subtotals { get; set; }
        public int gift_certificate { get; set; }
        public int total { get; set; }
        public int subtotal { get; set; }
    }

    public class ClientNotesSuccess
    {
        public int our_internal_order_id { get; set; }
    }

    public class Product3
    {
        public string product_id { get; set; }
        public int quantity { get; set; }
        public List<object> variants { get; set; }
    }

    public class ShippingAddressSuccess
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

    public class PaymentMethodSuccess
    {
        public bool use_gift { get; set; }
    }

    public class RetailerCredentialsSuccess
    {
        public string email { get; set; }
    }

    public class BillingAddressSuccess
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

    public class ShippingSuccess
    {
        public string order_by { get; set; }
        public int max_days { get; set; }
        public int max_price { get; set; }
    }

    public class WebhooksSuccess
    {
        public string request_succeeded { get; set; }
        public string request_failed { get; set; }
        public string tracking_obtained { get; set; }
    }

    public class AutoRetrySettingsSuccess
    {
        public List<int> retry_delays { get; set; }
    }

    public class RequestSuccess
    {
        public bool is_gift { get; set; }
        public List<object> retry_request_ids { get; set; }
        public List<object> bundled_order_ids { get; set; }
        public string retailer { get; set; }
        public ClientNotesSuccess client_notes { get; set; }
        public List<Product3> products { get; set; }
        public int max_price { get; set; }
        public ShippingAddressSuccess shipping_address { get; set; }
        public string gift_message { get; set; }
        public PaymentMethodSuccess payment_method { get; set; }
        public RetailerCredentialsSuccess retailer_credentials { get; set; }
        public BillingAddressSuccess billing_address { get; set; }
        public ShippingSuccess shipping { get; set; }
        public WebhooksSuccess webhooks { get; set; }
        public string client_token { get; set; }
        public AutoRetrySettingsSuccess auto_retry_settings { get; set; }
        public DateTime _created_at { get; set; }
        public List<object> promo_codes { get; set; }
        public List<object> free_gifts { get; set; }
        public List<object> scheduled_delivery_windows { get; set; }
        public bool _placed_order { get; set; }
        public string environment { get; set; }
        public string git_version { get; set; }
        public string server_name { get; set; }
        public DateTime _finalized_at { get; set; }
    }

    public class DataSuccess
    {
    }

    public class StatusUpdate
    {
        public DateTime _created_at { get; set; }
        public string type { get; set; }
        public string message { get; set; }
        public DataSuccess data { get; set; }
    }

    public class Stats
    {
        public DateTime start_time { get; set; }
        public DateTime end_time { get; set; }
        public DateTime launch_time { get; set; }
        public double run_duration { get; set; }
        public double full_duration { get; set; }
        public double queue_duration { get; set; }
    }

}
