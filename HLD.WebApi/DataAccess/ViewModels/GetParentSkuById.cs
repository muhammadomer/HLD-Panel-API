using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ViewModels
{
   public class GetParentSkuById
    {
        public int Parentproduct_id { get; set; }
        public int Childproduct_id { get; set; }
        public int productstatus { get; set; }
        public int ColorIds { get; set; }
        public string Sku { get; set; }
        public string ProductTitle { get; set; }
    }
}
