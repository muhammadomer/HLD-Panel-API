using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ViewModels
{
    public class SaveApprovedPricesViewModel
    {
        public int VendorID { get; set; }
        public string Vendor { get; set; }
        public string ApprovedPrice { get; set; }
        public string Currency { get; set; }
        public string SKU { get; set; }
        public string RowNumber { get; set; }
    }
}
