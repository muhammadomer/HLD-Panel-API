using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ViewModels
{
    public class WarehouseAddressViewModel
    {
        public int? ID { get; set; }
        public string whname { get; set; }
        public string companyname { get; set; }
        public string whid { get; set; }
       
        public string street1{get;set;}
        public string street2 { get; set; }
        public string unit { get; set; }
        public string postelcode { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string country { get; set; }
        public string phone { get; set; }
    }
}
