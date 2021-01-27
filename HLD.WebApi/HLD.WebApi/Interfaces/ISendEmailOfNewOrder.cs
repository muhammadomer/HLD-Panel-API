using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HLD.WebApi.Interfaces
{
    public interface ISendEmailOfNewOrder
    {
        Task SendNewEmail(int SCOrderId);

    }
}
