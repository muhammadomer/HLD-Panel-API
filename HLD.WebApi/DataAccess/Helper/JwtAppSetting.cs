using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Helper
{
    public class JwtAppSetting
    {
        public string SigningKey { get; set; }
        public string Site { get; set; }
        public int ExpiryInMinutes { get; set; }
    }
}
