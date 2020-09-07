using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ViewModels
{
  public  class DropShipEnableDisableLogViewModel
    {
        public int ID { get; set; }
        public string ProductSku { get; set; }
        public DateTime? LastUpdate { get; set; }
        public bool EnableDisable { get; set; }
        public int Qty { get; set; }
        public string Comments { get; set; }
        public decimal MSRP { get; set; }
        public decimal SellingFee { get; set; }
        public string BBProductID { get; set; }
        public DateTime? OfferStartDate { get; set; }
        public DateTime? OfferEndDate { get; set; }
    }
}
