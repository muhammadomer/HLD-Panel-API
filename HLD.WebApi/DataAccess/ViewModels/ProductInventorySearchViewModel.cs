using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ViewModels
{
    public class ProductInventorySearchViewModel
    {
        public string dropshipstatus { get; set; }
        public string dropshipstatusSearch { get; set; }
        public string DSTag { get; set; }
        public string Sku { get; set; }
        public string SearchFromSkuList { get; set; }
        public string asin { get; set; }
        public string Producttitle { get; set; }
        public string TypeSearch { get; set; }
        public string WHQStatus { get; set; }
        public string BBProductID { get; set; }
        public string ASINS { get; set; }
    }
}
