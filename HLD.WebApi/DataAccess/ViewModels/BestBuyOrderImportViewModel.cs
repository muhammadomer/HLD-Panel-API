using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ViewModels
{
    public class BestBuyOrderImportViewModel
    {
        public string order_id { get; set; }
        public string commercial_id { get; set; }
        public string customer_id { get; set; }
        public bool can_cancel { get; set; }
        public string order_state { get; set; }
        public DateTime acceptance_decision_date { get; set; }
        public DateTime created_date { get; set; }
        public double total_price { get; set; }
        public double total_commission { get; set; }
        public string inSellerCloud { get; set; }
        public int sellerCloudID { get; set; }
        public string shipping_price { get; set; }
    }
}
