using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ViewModels
{
    public class UpdateSCDropshipStatusViewModel
    {
        public int SCOrderID { get; set; }
        public string StatusName { get; set; }
        public DateTime LogDate { get; set; }
        public bool IsTrackingUpdate { get; set; } = false;
    }
}
