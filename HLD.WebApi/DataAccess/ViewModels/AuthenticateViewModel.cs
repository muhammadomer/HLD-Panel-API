using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ViewModels
{
  public  class AuthenticateViewModel
    {
        public int Id { get; set; }
        public string Method { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string KeyValue { get; set; }
        public string Token { get; set; }
        public DateTime expiration { get; set; }
        public string Message   { get; set; }
        public bool status { get; set; }
    }
}
