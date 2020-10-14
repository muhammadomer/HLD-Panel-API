using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ViewModels
{
   public class SaveParentSkuVM
    {
        public int product_id { get; set; }
        public string Sku { get; set; }
        public string ProductTitle { get; set; }
        public int ConditionId { get; set; }
        public string CatagoryName { get; set; }
        public decimal ShipWt { get; set; }
        public decimal ShipLt { get; set; }
        public decimal ShipHt { get; set; }
        public string Menufacture { get; set; }
        public string MenufactureModel { get; set; }
        public string DeviceModel { get; set; }
        public string Style { get; set; }
        public bool IsCreatedOnSC { get; set; }
        public string Feature { get; set; }
        public string Description { get; set; }
    }
}
