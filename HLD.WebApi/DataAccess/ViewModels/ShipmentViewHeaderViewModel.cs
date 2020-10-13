using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ViewModels
{
    public class ShipmentViewHeaderViewModel
    {
        public string ShipingCompany { get; set; }
        public string ShipmentId { get; set; }
        public string ShipmentName { get; set; }
        public int VendorId { get; set; }
        //public string VendorId { get; set; }
        public string Vendor { get; set; }
        public string Notes { get; set; }
        public DateTime CreatedOn { get; set; }
        public int Boxes { get; set; }
        public int SKUs { get; set; }
        public int POs { get; set; }
        public int Status { get; set; }
        public decimal ShipedAmountUSD { get; set; }
        public decimal ReceivedAmountUSD { get; set; }
        public decimal GrossWt { get; set; }
        public decimal CBM { get; set; }
        public decimal ShipedAmountCNY { get; set; }
        public decimal ReceivedAmountCNY { get; set; }
        public int TotalShipedQty { get; set; }
        public int TotalOrderedQty { get; set; }
        public int TotalOpenQty { get; set; }
        public int TotalReceivedQty { get; set; }
        public string TrakingNumber { get; set; }
        public string TrakingURL { get; set; }
        public string CourierCode { get; set; }
        public DateTime ReceivedDate { get; set; }
        public DateTime ShippedDate { get; set; }
    }
}
