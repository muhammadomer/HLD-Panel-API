using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ViewModels
{
   public class SaveWatchlistViewModel
    {
        public int frequency { get; set; }
        public int CheckafterDays { get; set; }
        public string ASIN { get; set; }
        public string ProductSKU { get; set; }
        public DateTime? LastUpdateDate { get; set; }
        public DateTime? NextUpdateDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
    }

    public class SaveWatchlistForjobsViewModel
    {
        public int frequency { get; set; }
        public int CheckafterDays { get; set; }
        public int Consumed_call { get; set; }
        public int ValidStatus { get; set; }
        public string ASIN { get; set; }
        public string ProductSKU { get; set; }
    
    }

    public class SaveWatchlistForViewModel
    {
     
        public string ASIN { get; set; }
        public string ProductSKU { get; set; }

    }
}
