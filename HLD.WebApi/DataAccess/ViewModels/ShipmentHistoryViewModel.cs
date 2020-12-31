using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ViewModels
{
    public class ShipmentHistoryViewModel
    {
        public string ShipmentId { get; set; }
        public int VendorId { get; set; }
        public string Vendor { get; set; }
        public string CompressedImage { get; set; }
        public string ImageName { get; set; }
        public string SKU { get; set; }
        public int ShipedQty { get; set; }
        public string Title { get; set; }
        public int ReceivedQty { get; set; }
        public string Type { get; set; }
        public int Status { get; set; }
        public string TrakingNumber { get; set; }
        public string TrakingURL { get; set; }
        public string CourierCode { get; set; }
        public int POId { get; set; }
        //public DateTime CreatedOn { get; set; }
        public DateTime ReceivedDate { get; set; }
        public DateTime ShippedDate { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ExpectedDelivery { get; set; }
    }
}
