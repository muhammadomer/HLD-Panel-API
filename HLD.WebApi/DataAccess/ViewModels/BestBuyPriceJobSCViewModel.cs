using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ViewModels
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    //public class Ranx
    //{
    // public double price { get; set; }
    // public int quantity_threshold { get; set; }
    //}

    //public class Discounts
    //{
    // public DateTime end_date { get; set; }
    // public double price { get; set; }
    // public List<Ranx> ranges { get; set; }
    // public DateTime start_date { get; set; }
    //}

    //public class OfferAdditionalFields
    //{
    // public string code { get; set; }
    // public string value { get; set; }
    //}

    //public class Offers
    //{
    // public DateTime available_ended { get; set; }
    // public DateTime available_started { get; set; }
    // public string description { get; set; }
    // public Discounts discount { get; set; }
    // public string internal_description { get; set; }
    // public string logistic_class { get; set; }
    // public int min_quantity_alert { get; set; }
    // public List<OfferAdditionalFields> offer_additional_fields { get; set; }
    // public int price { get; set; }
    // public string price_additional_info { get; set; }
    // public int product_id { get; set; }
    // public string product_id_type { get; set; }
    // public int quantity { get; set; }
    // public string shop_sku { get; set; }
    // public string state_code { get; set; }
    // public string update_delete { get; set; }
    //}

    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Ranxs
    {
        public decimal price { get; set; }
        public int quantity_threshold { get; set; }
    }

    public class Discounts
    {
        public String end_date { get; set; }
        public decimal price { get; set; }
        public List<Ranxs> ranges { get; set; }
        public String start_date { get; set; }
    }

    public class OfferAdditionalFields
    {
        public string code { get; set; }
        public string value { get; set; }
    }

    public class Offers
    {
        public String available_ended { get; set; }
        public String available_started { get; set; }
        public string description { get; set; }
        public Discounts discount { get; set; }
        public string internal_description { get; set; }
        public string logistic_class { get; set; }
        public int min_quantity_alert { get; set; }
        public List<OfferAdditionalFields> offer_additional_fields { get; set; }
        public decimal price { get; set; }
        public string price_additional_info { get; set; }
        public int product_id { get; set; }
        public string product_id_type { get; set; }
        public int quantity { get; set; }
        public string shop_sku { get; set; }
        public string state_code { get; set; }
        public string update_delete { get; set; }
    }

    public class BestBuyPriceJobSCViewModel
    {
        public List<Offers> offers { get; set; }
    }



}