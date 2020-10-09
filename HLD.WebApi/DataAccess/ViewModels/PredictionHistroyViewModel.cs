using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ViewModels
{
    public class PredictionHistroyViewModel
    {
        public int idApprovedPrice { get; set; }
        public int POStatus { get; set; }
        public string SKU { get; set; }
        public string ProductTitle { get; set; }
        public int? VendorId { get; set; }
        public string VendorAlias { get; set; }
        public decimal ApprovedUnitPrice { get; set; }
        public decimal CAD { get; set; }
        public decimal USD { get; set; }
        public decimal YEN { get; set; }
        public string Currency { get; set; }
        public string ImageName { get; set; }
        public string CompressedImage { get; set; }
        public int PhysicalQty { get; set; }
        public int AggregatedQty { get; set; }
        public int QtySold60 { get; set; }
        public int OnOrder { get; set; }
        public decimal LowStock60 { get; set; }
        public decimal LowStock90 { get; set; }
        public bool Continue { get; set; }
        public int CoverDays { get; set; }
        public int CoverPhy { get; set; }
        public int ReservedQty { get; set; }
        public int QtyPerBox { get; set; }
        public string ShadowOf { get; set; }
        public string LocationNotes { get; set; }
        public bool PredictIncluded { get; set; }
        public bool KitShadowStatus { get; set; }
        public int ProductDependency { get; set; }
        public string ProductType { get; set; }
        public int InternalPOId { get; set; }
        public List<Vendorlist> list { get; set; }
        public List<int> InternalPOs { get; set; }
    }
    public class Vendorlist
    {
        public int? VendorId { get; set; }
        public string VendorAlias { get; set; }
        public string Currency { get; set; }
        public decimal ApprovedUnitPrice { get; set; }
    }

    public class SoldQtyDaysViewModel
    {
        public int QtySold15 { get; set; }
        public int QtySold30 { get; set; }
        public int QtySold60 { get; set; }
        public int QtySold90 { get; set; }
        public int QtySoldYTD { get; set; }
    }

    public class SKUDetailModel
    {
        public int POId { get; set; }
        public string ProductTitle { get; set; }
        public string ShipmentId { get; set; }
        public string Type { get; set; }
        public string Vendor { get; set; }
        public int VendorId { get; set; }
        public int QtyOrdered { get; set; }
        public int ShippedQty { get; set; }

        public DateTime OrderedOn { get; set; }
        public DateTime ShippedOn { get; set; }
        public List<ShipmentList> list { get; set; }
    }

    public class ShipmentList
    {
        public string ShipmentId { get; set; }
        public string Type { get; set; }
        public DateTime ShippedOn { get; set; }
        public int ShippedQty { get; set; }
    }
}
