using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ViewModels
{
    public class ShipmentMultiBoxViewModel
    {
        public int MultiBoxId { get; set; }
        public string ShipmentId { get; set; }
        public int VendorId { get; set; }
        public string Vendor { get; set; }
        public decimal Width { get; set; }
        public decimal Height { get; set; }
        public decimal Length { get; set; }
        public decimal Weight { get; set; }
        public string SKU { get; set; }
        public int POId { get; set; }
        public int Boxes { get; set; }
        //public int OpenQty { get; set; }
        public int QtyPerBox { get; set; }
    }
}
