using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ViewModels
{
    public class CreditCardDetailViewModel
    {
        public int CreditCardDetailId { get; set; }
        public string name_on_card { get; set; }
        public string number { get; set; }
        public string security_code { get; set; }
        public string expiration_month { get; set; }
        public string expiration_year { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string address_line1 { get; set; }
        public string address_line2 { get; set; }
        public string zip_code { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string country { get; set; }
        public bool IsActive { get; set; }
        public bool IsDefault { get; set; }
        public string PhoneNo { get; set; }
        public string name_on_cardShort { get; set; }
        public string numberShort { get; set; }
        public string security_codeShort { get; set; }
    }
}
