using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ViewModels
{
    public class ShipmentViewProducListViewModel
    {
        public int idShipmentProducts { get; set; }
        public int SCID { get; set; }
        public string BoxId { get; set; }
        public string SKU { get; set; }
        public int BalanceQty { get; set; }
        public int POId { get; set; }
        public int OpenQty { get; set; }
        public int ShipedQty { get; set; }
        public string Title { get; set; }
        public string CompressedImage { get; set; }
        public string ImageName { get; set; }
        public string Description { get; set; }
        public int ReceivedQty { get; set; }
        public int OrderedQty { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal UnitPriceUSD { get; set; }
        public int CurrencyCode { get; set; }
        public int BoxNo { get; set; }
        public int NoOfBoxes { get; set; }

        public string LocationNotes { get; set; }
        public string ShadowOf { get; set; }
        public string PhysicalInventory { get; set; }
        public int QtyPerCase { get; set; }

        public int QtyPerBox { get; set; }
        public decimal Width { get; set; }
        public decimal Height { get; set; }
        public decimal Length { get; set; }
        public decimal Weight { get; set; }

    }
}
