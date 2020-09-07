using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ViewModels
{
  public  class BestBuyOrdersImportMainViewModel
    {
        public BestBuyOrderImportViewModel OrderViewModel { get; set; }
        public List<BestBuyOrderDetailImportViewModel> orderDetailViewModel { get; set; }
        public BestBuyCustomerDetailImportViewModel customerDetailOrderViewModel { get; set; }
    }
}
