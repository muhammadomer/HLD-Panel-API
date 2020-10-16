using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ViewModels
{
   public class SaveChildSkuVM
    {
        public int product_id { get; set; }
        public int ColorId { get; set; }
        public string Sku { get; set; }
        public string ProductTitle { get; set; }
        public string Upc { get; set; }
        public bool ProductStatus { get; set; }
    }
}
