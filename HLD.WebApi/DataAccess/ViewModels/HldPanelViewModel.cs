using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ViewModels
{
    public class HldPanelViewModel
    {
        //customer
        public string Customer_firstName { get; set; }
        public string Customer_lastName { get; set; }
        public string Customer_city { get; set; }
        public string Customer_country { get; set; }

        //order
        public string Order_orderId { get; set; }
        public DateTime? Order_acceptanceDecisionDate { get; set; }
        public DateTime? Order_createdDate { get; set; }
        public string Order_sellerCloudID { get; set; }

        //order lines
        public string OrderLine_offer_sku { get; set; }
        public string OrderLine_product_title { get; set; }
        public string OrderLine_shipped_date { get; set; }
        public string OrderLine_received_date { get; set; }
        public string OrderLine_total_commission { get; set; }
        public string OrderLine_total_price { get; set; }
        public string OrderLine_order_line_state { get; set; }
       
        //shipping
        public string Shipping_shippingCompany { get; set; }
        public string Shipping_shippingTracking { get; set; }
      

    }
}
