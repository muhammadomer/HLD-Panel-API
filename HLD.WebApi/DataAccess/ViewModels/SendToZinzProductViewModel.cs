using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ViewModels
{
    public class SendToZincProductViewModel
    {
        public int OrderId { get; set; }
        public string Asin { get; set; }
        public string Sku { get; set; }
        public string Shipdays { get; set; }
        public string Address1 { get; set; }
        public string AccountDetail { get; set; }
        public string Address2 { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Phone { get; set; }
        public string Country { get; set; }
        public int Qty { get; set; }
        public string ReqId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string getaddressname { get; set; }
        public int CreditCardId { get; set; }
        public int ZincAccountId { get; set; }
        public decimal max_price { get; set; }
        public List<GetAddressViewModel> getaddress { get; set; }
        public List<CreditCardDetailViewModel> CreditCardDetail { get; set; }
        public List<ZincAccountsViewModel> ZincAccounts { get; set; }

        public int ZincOrderLogDetailID { get; set; }
        public int ZincOrderLogID { get; set; }
        public string Type { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public string Data { get; set; }
        public string ShppingDate { get; set; }
        public string TrackingNumber { get; set; }
        public string Carrier { get; set; }
        public string AmazonTracking { get; set; }
        public string _17Tracking { get; set; }
        public DateTime OrderDatetime { get; set; }
        public string ZincOrderStatusInternal { get; set; }
        public string MerchantOrderId { get; set; }
        public string OurInternalOrderId { get; set; }
        public decimal SubTotal { get; set; }
        public decimal Tax { get; set; }
        public decimal Shipping { get; set; }
        public decimal InternalId { get; set; }
    }
}
