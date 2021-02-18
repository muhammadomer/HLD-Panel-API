using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ViewModels
{
   public class ZincWatclistLogHistoryViewModel
    {
        public string ASIN { get; set; }
        public string ProductSKU { get; set; }
        public string Compress_image { get; set; }
        public string image_name { get; set; }
        public DateTime NextUpdateDate { get; set; }
        public string Active_Inactive { get; set; }
        public string Enabled_Disabled { get; set; }
        public int JobID { get; set; }
        public string ZincResponse { get; set; }
        public int ValidStatus { get; set; }
        public string UpdateOnHLD { get; set; }
        public string Remarks { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime CompletionTime { get; set; }
        public TimeSpan RunTime { get; set; }

    }

    public class ZincWatclistLogsViewModel
    {
        public string ASIN { get; set; }
        public string ProductSKU { get; set; }
        public string Compress_image { get; set; }
        public string image_name { get; set; }
        public DateTime NextUpdateDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string Active_Inactive { get; set; }
        public string Enabled_Disabled { get; set; }
        public int ValidStatus { get; set; }
        public int Consumed_call { get; set; }
        public int CheckAfterDays { get; set; }
        public int Frequency { get; set; }
        public List<WatclistLogsHistoryViewModel> Schedule { get; set; }

    }
    public class WatclistLogsHistoryViewModel
    {
        public string ASIN { get; set; }
        public string ProductSKU { get; set; }
        public DateTime NextUpdateDate { get; set; }
        public int JobID { get; set; }
        public string ZincResponse { get; set; }
        public string UpdateOnHLD { get; set; }
        public string Remarks { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime CompletionTime { get; set; }
        public TimeSpan RunTime { get; set; }

    }
}
