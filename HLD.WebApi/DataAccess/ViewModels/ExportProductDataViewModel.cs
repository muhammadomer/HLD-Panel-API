using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ViewModels
{
   public class ExportProductDataViewModel
    {
        public string ProductSKU { get; set; }
        public int best_buy_product_id { get; set; }

        public bool dropship_status { get; set; }
        public int HLD_CA1 { get; set; }
        public string ImageURL { get; set; }
    }
}
