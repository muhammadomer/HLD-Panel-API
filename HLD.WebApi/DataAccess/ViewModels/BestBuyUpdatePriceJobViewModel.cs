using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ViewModels
{
    public class BestBuyUpdatePriceJobViewModel
    {
        public string SKU { get; set; }
        public string ASIN { get; set; }
        public int ProductId{ get; set; }
        public int ZincJobID{ get; set; }
        public decimal UpdateSelllingPrice { get; set; }
        public decimal MSRP { get; set; }

    }
}
