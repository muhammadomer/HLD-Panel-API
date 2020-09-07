using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ViewModels
{
    public class CasePackViewModel
    {
        public int CasePackId { get; set; }
        public string SKU { get; set; }
        public int VendorId { get; set; }
        public string UserAlias { get; set; }
        public int QtyPerBox { get; set; }
        public decimal Width { get; set; }
        public decimal Height { get; set; }
        public decimal Length { get; set; }
        public decimal Weight { get; set; }
        public string Title { get; set; }
        public string CompressedImage { get; set; }
        public string ImageName { get; set; }
        public int Counter { get; set; }
    }
}
