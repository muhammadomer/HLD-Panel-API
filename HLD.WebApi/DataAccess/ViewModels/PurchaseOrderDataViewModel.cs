using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ViewModels
{
    public class PurchaseOrderDataViewModel
    {
        public int POId { get; set; }
        public int CompanyId { get; set; }
        public int VendorId { get; set; }
        public DateTime OrderedOn { get; set; }
        public int DefaultWarehouseID { get; set; }
        public int CurrencyCode { get; set; }
        public int POStatus { get; set; }
        public int InternalPOId { get; set; }
        public string Notes { get; set; }

        public List<PurchaseOrderItemsDataViewModel> items { get; set; }
    }
    public class PurchaseOrderItemsDataViewModel
    {
        public int ID { get; set; }
        public int PurchaseID { get; set; }
        public string ProductID { get; set; }
        public int QtyOrdered { get; set; }
        public int QtyReceived { get; set; }
        public int QtyOnHand { get; set; }
        public int SkuStatus { get; set; }
        public string ProductTitle { get; set; }
        public double UnitPrice { get; set; }
    }
}
