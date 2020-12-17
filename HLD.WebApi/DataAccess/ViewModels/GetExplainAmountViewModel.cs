using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ViewModels
{
   public class GetExplainAmountViewModel
    {
        public int quantity { get; set; }
        public decimal total_price { get; set; }
        public decimal unitprice { get; set; }
        public decimal ShippingFee { get; set; }
        public decimal avg_cost { get; set; }
        public decimal total_commission { get; set; }
    }
}
