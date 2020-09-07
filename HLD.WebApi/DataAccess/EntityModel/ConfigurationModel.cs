using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.EntityModel
{
    public class ConfigurationModel
    {
        public long Id { get; set; }
        public string Method { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string KeyValue { get; set; }
    }
}
