using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ViewModels
{
   public class AsinSkuMappingViewModel
    {
        public string ASIN { get; set; }
        public string SKU { get; set; }
        public decimal? MAXPrice { get; set; }
        public decimal? AmzPrice { get; set; }
     
    }
}
