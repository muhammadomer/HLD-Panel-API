using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ViewModels
{
    public class BBProductViewModel
    {
        public int BBProductID { get; set; }
        public DateTime? DiscountEndDate { get; set; }
        public DateTime? DiscountStartDate { get; set; }
        public double? UnitDiscountPrice_SellingPrice { get; set; }
        public double? UnitOriginPrice_MSRP { get; set; }
        public string CategoryCode { get; set; }
        public string CategoryName { get; set; }
        public string LogisticClass_Code { get; set; }
        public string Reference_UPC { get; set; }
        public string Product_Sku { get; set; }
        public string Product_Title { get; set; }
        public int? Quantity_BBQ { get; set; }
        public string ShopSKU_OfferSKU { get; set; }
        public int ProductId { get; set; }
        public string BBCategory { get; set; }
        public bool dropship_status { get; set; }
        public int dropship_Qty { get; set; }
        public string DropshipComments { get; set; }
    }
    public class VolumePrice
    {
        public double? price { get; set; }
        public int quantity_threshold { get; set; }
        public double? unit_discount_price { get; set; }
        public double? unit_origin_price { get; set; }
    }

    public class AllPrice
    {
        public object channel_code { get; set; }
        public DateTime? discount_end_date { get; set; }
        public DateTime? discount_start_date { get; set; }
        public double price { get; set; }
        public double? unit_discount_price { get; set; }
        public double? unit_origin_price { get; set; }
        public List<VolumePrice> volume_prices { get; set; }
    }

    public class VolumePrice2
    {
        public double? price { get; set; }
        public int? quantity_threshold { get; set; }
        public double? unit_discount_price { get; set; }
        public double? unit_origin_price { get; set; }
    }

    public class ApplicablePricing
    {
        public object channel_code { get; set; }
        public DateTime? discount_end_date { get; set; }
        public DateTime? discount_start_date { get; set; }
        public double? price { get; set; }
        public double? unit_discount_price { get; set; }
        public double? unit_origin_price { get; set; }
        public List<VolumePrice2> volume_prices { get; set; }
    }

    public class Range
    {
        public double? price { get; set; }
        public int? quantity_threshold { get; set; }
    }

    public class Discount
    {
        public double? discount_price { get; set; }
        public DateTime? end_date { get; set; }
        public double? origin_price { get; set; }
        public List<Range> ranges { get; set; }
        public DateTime? start_date { get; set; }
    }

    public class LogisticClass
    {
        public string code { get; set; }
        public string label { get; set; }
    }

    public class OfferAdditionalField
    {
        public string code { get; set; }
        public string type { get; set; }
        public string value { get; set; }
    }

    public class ProductReference
    {
        public string reference { get; set; }
        public string reference_type { get; set; }
    }

    public class Offer
    {
        public bool active { get; set; }
        public List<AllPrice> all_prices { get; set; }
        public bool allow_quote_requests { get; set; }
        public ApplicablePricing applicable_pricing { get; set; }
        public string category_code { get; set; }
        public string category_label { get; set; }
        public List<string> channels { get; set; }
        public string currency_iso_code { get; set; }
        public object description { get; set; }
        public Discount discount { get; set; }
        public LogisticClass logistic_class { get; set; }
        public decimal? min_shipping_price { get; set; }
        public decimal? min_shipping_price_additional { get; set; }
        public string min_shipping_type { get; set; }
        public string min_shipping_zone { get; set; }
        public List<OfferAdditionalField> offer_additional_fields { get; set; }
        public int offer_id { get; set; }
        public double? price { get; set; }
        public object price_additional_info { get; set; }
        public List<ProductReference> product_references { get; set; }
        public string product_sku { get; set; }
        public string product_title { get; set; }
        public int quantity { get; set; }
        public string shop_sku { get; set; }
        public string state_code { get; set; }
        public double? total_price { get; set; }
    }

    public class RootObject
    {
        public List<Offer> offers { get; set; }
        public int total_count { get; set; }
    }
}
