using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ViewModels
{
   public class GetChildSkuVM
    {
        public int Childproduct_id { get; set; }
        public int Parentproduct_id { get; set; }
        public int ColorIds { get; set; }
        public string Colorname { get; set; }
        public string Sku { get; set; }
        public string title { get; set; }
        public string upc { get; set; }
        public int productstatus { get; set; }
        public string ImageName { get; set; }
        public string CompressedImage { get; set; }
        public string ShadowOff { get; set; }
    }
}
