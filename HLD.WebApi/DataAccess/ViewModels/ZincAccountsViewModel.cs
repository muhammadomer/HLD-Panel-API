using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ViewModels
{
    public class ZincAccountsViewModel
    {
        public int ZincAccountsId { get; set; }
        public string UserName { get; set; }
        public string AmzAccountName { get; set; }
        public string Password { get; set; }
        public string Key { get; set; }
        public bool IsActive { get; set; }
        public bool IsDefault { get; set; }
        public string UserNameShort { get; set; }
        public string AmzAccountNameShort { get; set; }
        public string PasswordShort { get; set; }
        public string KeyShort { get; set; }
        public string User { get; set; }
        public DateTime LastUpdatedDate { get; set; }
    }
}
