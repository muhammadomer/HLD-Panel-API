using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ViewModels
{
   public class ZincWatchLogsSearchViewModel
    {
        public string ASIN { get; set; }
        public string JobID { get; set; }
        public string Available { get; set; }

        public int Offset { get; set; }
    }
}
