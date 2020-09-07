using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ViewModels
{
   public class AuthenticateSCRestViewModel
    {
       
            public string access_token { get; set; }
            public string token_type { get; set; }
            public string username { get; set; }
            public double expires_in { get; set; }
           
        
    }
}
