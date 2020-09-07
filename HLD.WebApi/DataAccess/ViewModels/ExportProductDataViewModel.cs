using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ViewModels
{
   public class ExportProductDataViewModel
    {
        public string ImageURL { get; set; }
        public bool dropship_status { get; set; }
        public int dropship_Qty { get; set; }
        public int best_buy_product_id { get; set; }

        public string ProductSKU { get; set; }
    }
}
