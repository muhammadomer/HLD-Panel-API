using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ViewModels
{
  public  class MissingOrderReturnViewModel
    {
        public int MissingOrderCount { get; set; }
        public int ExistingOrderCount { get; set; }
        public List<int> ExistingOrder { get; set; }
        public List<int> MissingOrder { get; set; }
    }
}
