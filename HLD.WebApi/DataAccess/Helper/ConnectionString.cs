using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace DataAccess.Helper
{
    public class ConnectionString : IConnectionString
    {
        private IConfiguration _config;
        public ConnectionString(IConfiguration config)
        {
            this._config = config;
        }
        public string GetConnectionString()
        {
            return _config.GetValue<string>("ConnectionString:bbe2");
        }

        public string GetPhpConnectionString()
        {
            return _config.GetValue<string>("ConnectionString:bbe2HldPHP");
        }
    }
}
