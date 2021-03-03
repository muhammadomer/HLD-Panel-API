using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ViewModels
{
    public class ApprovedPriceForInventoryPageViewModel
    {
        public string VendorAlias { get; set; }
        public decimal ApprovedUnitPrice { get; set; }
        public string Currency { get; set; }
        public string SKU { get; set; }
    }
}
