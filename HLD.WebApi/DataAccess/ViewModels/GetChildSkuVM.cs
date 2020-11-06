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
        public int CompanyId { get; set; }
        public int IsCreatedOnSC { get; set; }
        public string Shadow_Key { get; set; }
        public string CompanyName { get; set; }
        public string IsRelated { get; set; }
        //bulk update colums
        public string AmazonMerchantSKU { get; set; }
        public string AmazonEnabled { get; set; }
        public string ASIN { get; set; }
        public string FulfilledBy { get; set; }
        public string AmazonFBASKU { get; set; }
        public string WebsiteEnabled { get; set; }
    }
}
