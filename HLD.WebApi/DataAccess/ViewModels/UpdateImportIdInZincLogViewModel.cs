using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ViewModels
{
   public class UpdateImportIdInZincLogViewModel
    {
        public string SKU { get; set; }
        public int ZincJobID { get; set; }
        public int ImportId { get; set; }
        public int JobID { get; set; }
        public decimal price { get; set; }
    }
}
