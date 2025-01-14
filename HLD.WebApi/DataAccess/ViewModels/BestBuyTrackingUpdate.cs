﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ViewModels
{
    public class BestBuyTrackingUpdate
    {
        public string scOrderID { get; set; }
        public string bbOrderID { get; set; }
        public DateTime shipDate { get; set; }
        public string shippingServiceCode { get; set; }
        public string trackingNumber { get; set; }
        public string BBStatus { get; set; }
        public int inBestbuy { get; set; }
    }
}
