using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ViewModels
{
    public class CatalogViewModel
    {
        public string SKU { get; set; }
        public string LocationNotes { get; set; }
        public string ShadowOf { get; set; }
        public int QtySold15 { get; set; }
        public int QtySold30 { get; set; }
        public int QtySold60 { get; set; }
        public int QtySold90 { get; set; }
        public int QtySoldYTD { get; set; }
        public int AggregatePhysicalQty { get; set; }
        public int PhysicalQty { get; set; }
        public int AggregateQty { get; set; }
        public int ReservedQty { get; set; }
        public int OnOrder { get; set; }
        public bool IsEndOfLife { get; set; }
        public int AggregateNonSellableQty { get; set; }
        public string ASINInActiveListing { get; set; }
        public string AmazonFBASKU { get; set; }
        public string PhysicalInventory { get; set; }

    }

    public class CatalogListViewModel
    {
        public List<CatalogViewModel> Items { get; set; }
        public int TotalResults { get; set; }
    }
}
