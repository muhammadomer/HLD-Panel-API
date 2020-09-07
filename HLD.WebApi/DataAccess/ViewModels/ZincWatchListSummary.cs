using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ViewModels
{
   public class ZincWatchListSummaryViewModal
    {
        public int JobID { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? CompletionTime { get; set; }
        public int Total_ASIN { get; set; }
        public int Available { get; set; }
        public int Prime { get; set; }
        public int NoPrime { get; set; }
        public int Unavailable { get; set; }
    }
}
