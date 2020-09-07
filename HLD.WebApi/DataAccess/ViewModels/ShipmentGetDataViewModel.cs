using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ViewModels
{
    public class ShipmentGetDataViewModel
    {
        public string ShipmentId { get; set; }
        public int VendorId { get; set; }
        public string Vendor { get; set; }
        public string ShipmentName { get; set; }
        public int NoOfBoxes { get; set; }
        public int NoOfSKUs { get; set; }
        public int NoOfPOs { get; set; }
        public int TotalShipedQty { get; set; }
        public string Notes { get; set; }
        public int Status { get; set; }
        public DateTime CreatedOn { get; set; }
        public int ShipmentAutoID { get; set; }
        public string TrakingNumber { get; set; }
        public string CourierCode { get; set; }
        public string Type { get; set; }
        public int QtyReceived { get; set; }
        public decimal AmountReceived { get; set; }
        public decimal RecivedAmountCNY { get; set; }
        public decimal GrossWt { get; set; }
        public DateTime ReceivedDate { get; set; }

        public DateTime ShippedDate { get; set; }

    }
}
