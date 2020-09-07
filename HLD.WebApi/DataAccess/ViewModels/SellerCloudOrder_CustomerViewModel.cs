using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ViewModels
{
   public class SellerCloudOrder_CustomerViewModel
    {
        public SellerCloudOrderViewModel Order { get; set; }
        public SellerCloudCustomerDetail Customer { get; set; }
        public List<SellerCloudOrderDetailViewModel> orderDetail { get; set; }
    }
}
