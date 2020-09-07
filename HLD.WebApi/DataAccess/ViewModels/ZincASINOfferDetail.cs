using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ViewModels
{
   public class ZincASINOfferDetail
    {
        public int timestemp { get; set; }
        public string status { get; set; }
        public int bb_product_zinc_id { get; set; }
        public string sellerName { get; set; }
        public int percent_positive { get; set; }
        public int itemprice { get; set; }
        public bool itemavailable { get; set; }
        public int handlingday_min { get; set; }
        public int handlingday_max { get; set; }
        public bool item_prime_badge { get; set; }
        public int delivery_days_max { get; set; }
        public int delivery_days_min { get; set; }
        public string item_condition { get; set; }
        public string ASIN { get; set; }
        public string Product_sku { get; set; }
        public string Priorty { get; set; }
        public string max_price_limit { get; set; }
        public string primeAvailable { get; set; }
        public DateTime updateDate { get; set; }
    }
}
