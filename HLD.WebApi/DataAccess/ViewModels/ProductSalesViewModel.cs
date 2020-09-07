using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ViewModels
{
   public class ProductSalesViewModel
    {
        public int No_of_Orders { get; set; }
        public int Qty { get; set; }
        public string offer_sku { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal total_Sales { get; set; }
        public decimal Total_Amount { get; set; }
        public decimal P_L { get; set; }
    }
}
