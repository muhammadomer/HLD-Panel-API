using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ViewModels
{
    public class ZincProxyViewModel
    {
        public string ProxyIP { get; set; }
        public string ProxyPassword { get; set; }
        public string ProxyPasswordShort { get; set; }
        public string ProxyPort { get; set; }
        public string ProxyUser { get; set; }
        public string ProxyEmail { get; set; }
    }
    public class GetZincProxyViewModel
    {
        public string ProxyIP { get; set; }
        public string ProxyPassword { get; set; }
        public string ProxyPasswordShort { get; set; }
        public string ProxyPort { get; set; }
        public string ProxyUser { get; set; }
        public string ProxyEmail { get; set; }
        public bool IsDefault { get; set; }
        public bool Isactive { get; set; }
        public int idZincProxy { get; set; }
        public int Status { get; set; }
        public DateTime LatUpdated { get; set; }
    }
    public class ProxySettingViewModal
    {
        public bool IsDefault { get; set; }
        public bool Isactive { get; set; }
        public int idZincProxy { get; set; }
    }
}
