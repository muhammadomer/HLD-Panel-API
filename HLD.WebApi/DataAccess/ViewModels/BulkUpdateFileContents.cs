using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ViewModels
{
   public class BulkUpdateFileContents
    {
        public string ProductID { get; set; }
        public string UPC { get; set; }
        public string BrandName { get; set; }
        public string Manufacturer { get; set; }
        public string ManufacturerSKU { get; set; }
        public bool PackageWeightOz { get; set; }
        public bool ShippingWidth { get; set; }
        public bool ShippingHeight { get; set; }
        public bool ShippingLength { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
        public bool AmazonEnabled { get; set; }
        public string ASIN { get; set; }
        public string AmazonMerchantSKU { get; set; }
        public string FulfilledBy { get; set; }
        public string AmazonFBASKU { get; set; }
        public int CompanyID { get; set; }
       // public string ProductSku { get; set; }
        public string brand_name { get; set; }
    }
}
