using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ViewModels
{
   public class SaveSkuShadowViewModel
    {
        public int ParentId { get; set; }
        public string Sku { get; set; }
        public int ChildId { get; set; }
    }

}
