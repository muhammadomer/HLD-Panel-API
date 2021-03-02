using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ViewModels
{
    public class ApprovedPriceForInventoryViewModel
    {
        public int idApprovedPrice { get; set; }
        public int? VendorId { get; set; }
        public string SKU { get; set; }
        public string VendorAlias { get; set; }
        public decimal ApprovedUnitPrice { get; set; }
        public decimal CAD { get; set; }
        public decimal USD { get; set; }
        public decimal YEN { get; set; }
        public string Currency { get; set; }
    }
}
