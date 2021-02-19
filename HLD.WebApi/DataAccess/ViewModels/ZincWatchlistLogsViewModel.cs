using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ViewModels
{
    public class ZincWatchlistLogsViewModel
    {
        public int jobID { get; set; }
        public int ZincJobId { get; set; }
        public int ImportId { get; set; }
        public int BBJobID { get; set; }
        public decimal UpdatedPriceBB { get; set; }
        public string ASIN { get; set; }
        public string ProductSKU { get; set; }
        public string ZincResponse { get; set; }
        public string SellerName { get; set; }
        public decimal Amz_Price { get; set; }
        public int IsPrime { get; set; }
        public string FulfilledBY { get; set; }
        public string Compress_image { get; set; }
        public string image_name { get; set; }
        public string BBProductId { get; set; }
        public decimal BBSellingPrice { get; set; }
        public double UnitOriginPrice_MSRP { get; set; }
        public double UnitOriginPrice_Max { get; set; }
        public int HLD_CA1 { get; set; }
        public string Remarks { get; set; }
        public string UpdateOnHLD { get; set; }
    }
}
