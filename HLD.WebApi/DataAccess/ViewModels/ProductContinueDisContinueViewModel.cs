using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ViewModels
{
    public class ProductContinueDisContinueViewModel
    {
        public string SKU { get; set; }
        public bool Continue { get; set; }
        public string RowNumber { get; set; }
    }
}
