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
        public int OnOrder { get; set; }
        public string Currency { get; set; }
        public decimal LowStock90 { get; set; }

        public int CoverPhy { get; set; }
        public string Notes { get; set; }
        public int POQty { get; set; }
        public string SKU { get; set; }
        public int CasePackQty { get; set; }
        public int ReservedQty { get; set; }
        public string LargImage { get; set; }
        public string SmallImage { get; set; }
        public decimal ApprovedUnitPrice { get; set; }
        public int InternalPOID { get; set; }
        public int QtySold60 { get; set; }
        public int QtySold90 { get; set; }
        public int CoverDays { get; set; }
        public decimal Velocity { get; set; }
        public int PhysicalQty { get; set; }
        public int POstatus { get; set; }
        public DateTime OrderedOn { get; set; }
        public string VendorAlias { get; set; }
        public List<PredictionSKUs> list { get; set; }
    }
    public class PredictionSKUs
    {
        public int idPurchaseOrdersItems { get; set; }
        public int VendorId { get; set; }
        public decimal LowStock90 { get; set; }

        public int CoverPhy { get; set; }
        public int ReservedQty { get; set; }
        public int POQty { get; set; }
        public int CasePackQty { get; set; }
        public int OnOrder { get; set; }
        public string SKU { get; set; }
        public string VendorAlias { get; set; }
        public string LargImage { get; set; }
        public string SmallImage { get; set; }
        public decimal ApprovedUnitPrice { get; set; }

        public string Currency { get; set; }
        public int QtySold60 { get; set; }
        public int QtySold90 { get; set; }
        public int CoverDays { get; set; }
        public decimal Velocity { get; set; }
        public int PhysicalQty { get; set; }

    }

    public class PredictionInternalPOList
    {
        public string Vendor { get; set; }
        public int InternalPOID { get; set; }
    }
    public class PredictionInternalSKUList
    {
        public int VendorId { get; set; }
        public string SKU { get; set; }
    }
}