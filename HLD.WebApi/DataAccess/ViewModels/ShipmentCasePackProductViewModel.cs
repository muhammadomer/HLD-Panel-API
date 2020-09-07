using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ViewModels
{
    public class ShipmentCasePackProductViewModel
    {
        public int idShipmentProducts { get; set; }
        public string ShipmentId { get; set; }
        public int VendorId { get; set; }
        public int CasePackId { get; set; }
        public string Vendor { get; set; }
        public string SKU { get; set; }
        public int POId { get; set; }
        public int OpenQty { get; set; }
        public int ShipedQty { get; set; }
        public int RecivedQty { get; set; }
        public int NoOfBoxes { get; set; }

        public decimal Width { get; set; }
        public decimal Height { get; set; }
        public decimal Length { get; set; }
        public decimal Weight { get; set; }
        public int QtyPerBox { get; set; }

        public string CompressedImage { get; set; }
        public string ImageName { get; set; }
        public string Title { get; set; }
        public int BalanceQty { get; set; }

        public string LocationNotes { get; set; }
        public string ShadowOf { get; set; }
        public string PhysicalInventory { get; set; }
        public int QtyPerCase { get; set; }

    }
}
