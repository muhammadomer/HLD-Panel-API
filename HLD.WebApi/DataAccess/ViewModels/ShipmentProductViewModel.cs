using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ViewModels
{
    public class ShipmentProductViewModel
    {
        public string ShipmentId { get; set; }
        public string BoxId { get; set; }
        public int VendorId { get; set; }
        public string Vendor { get; set; }
        public int idShipmentProducts { get; set; }
        public string SKU { get; set; }
        public int POId { get; set; }
        public int OpenQty { get; set; }
        public int ShipedQty { get; set; }
        public int NoOfBoxes { get; set; }
        public int CasePackId { get; set; }
    }
}
