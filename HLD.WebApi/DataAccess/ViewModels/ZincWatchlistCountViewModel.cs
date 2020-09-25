using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ViewModels
{
    public class ZincWatchlistCountViewModel
    {
        public int Available { get; set; }
        public int UnAvailable { get; set; }
        public int ListingRemoved { get; set; }
        public int TotalCount { get; set; }
        public int Total { get; set; }
    }
}
