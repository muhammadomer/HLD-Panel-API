using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HLD.WebApi.Enum
{
    public enum SellerCloudOrderStatusCategory
    {
        InProcess_or_Completed = 1,
        InProcess_or_Hold = 2,
        ShoppingCart = 4,
        InProcess = 5,
        Completed = 6,
        ProblemOrder = 7,
        OnHold = 8,
        Quote = 9,
        Void = 10,
        Canceled = 11
    }
}
