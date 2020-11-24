using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ViewModels
{
   public class GetParentSkuById
    {
        public int Parentproduct_id { get; set; }
        public int Childproduct_id { get; set; }
        public int productstatus { get; set; }
        public int ColorIds { get; set; }
        public string Sku { get; set; }
        public string ProductTitle { get; set; }
        public string ManufactureName { get; set; }
        public string ManufactureModel { get; set; }
        public string DeviceModel { get; set; }
        public string Style { get; set; }
        public string Feature { get; set; }
        public String Color { get; set; }
        public String Brand { get; set; }
        public string Condition { get; set; }
    }
}
