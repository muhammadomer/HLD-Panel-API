using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ViewModels
{
    public class DraftPOViewModel
    {
        public int InternalPOId { get; set; }
        public int VendorId { get; set; }
        public int SKUs { get; set; }
        public DateTime OrderedOn { get; set; }
        public string Notes { get; set; }
        public string Vendor { get; set; }

    }
}
