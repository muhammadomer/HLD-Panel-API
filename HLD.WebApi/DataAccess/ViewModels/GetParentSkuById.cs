using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ViewModels
{
   public class GetParentSkuById
    {
        public int product_id { get; set; }
        public int productstatus { get; set; }
        public string Sku { get; set; }
        public string ProductTitle { get; set; }
    }
}
