using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ViewModels
{
    public class PredictionPOViewModel
    {
        public int idPurchaseOrdersItems { get; set; }
        public int VendorId { get; set; }
        public string Vendor { get; set; }
        public string Currency { get; set; }
        public string Notes { get; set; }
        public int POQty { get; set; }
        public string SKU { get; set; }
        public int CasePackQty { get; set; }
        public string LargImage { get; set; }
        public string SmallImage { get; set; }
        public decimal ApprovedUnitPrice { get; set; }
        public int InternalPOID { get; set; }
        public int POstatus { get; set; }
        public DateTime OrderedOn { get; set; }
        public List<PredictionSKUs> list { get; set; }
    }
    public class PredictionSKUs
    {
        public int idPurchaseOrdersItems { get; set; }
        public int POQty { get; set; }
        public int CasePackQty { get; set; }
        public string SKU { get; set; }
        public string LargImage { get; set; }
        public string SmallImage { get; set; }
        public decimal ApprovedUnitPrice { get; set; }
    }

    public class PredictionInternalPOList
    {
        public string Vendor { get; set; }
        public int InternalPOID { get; set; }
    }
}
