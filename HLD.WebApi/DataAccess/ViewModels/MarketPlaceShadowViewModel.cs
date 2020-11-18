using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ViewModels
{
   public class MarketPlaceShadowViewModel
    {
        public int CompanyId { get; set; }
        public string Shadow_Key { get; set; }
        public string CompanyName { get; set; }
        public bool AmazonEnabled { get; set; }
        public string FulfilledBy { get; set; }
        public bool WebsiteEnabled { get; set; }
    }
}
