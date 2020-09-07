using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ViewModels
{
  
    public class CustomerDetailsCreateOrder
    {
        public int ID { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Business { get; set; }
        public bool IsWholesale { get; set; }
    }

    public class OrderDetailsCreateOrder
    {
        public int CompanyID { get; set; }
        public int MarketingSource { get; set; }
        public int SalesRepresentative { get; set; }
        public bool TaxExempt { get; set; }
        public bool GiftOrder { get; set; }
        public string Channel { get; set; }
        public string OrderSourceOrderID { get; set; }
    }

    public class GiftDetailsCreateOrder
    {
        public bool UseGiftWrap { get; set; }
        public string GiftMessage { get; set; }
        public int GiftWrap { get; set; }
        public string GiftWrapType { get; set; }
    }

    public class ProductCreateOrder
    {
        public string ProductID { get; set; }
        public string ProductName { get; set; }
        public decimal SitePrice { get; set; }
        public int DiscountValue { get; set; }
        public string DiscountType { get; set; }
        public int Qty { get; set; }
        public decimal LineTaxTotal { get; set; }
    }

    public class ShippingAddressCreateOrder
    {
        public string Business { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Region { get; set; }
        public string ZipCode { get; set; }
        public string Address { get; set; }
        public string Address2 { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
    }

    public class BillingAddressCreateOrder
    {
        public string Business { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Region { get; set; }
        public string ZipCode { get; set; }
        public string Address { get; set; }
        public string Address2 { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
    }

    public class WeightCreateOrder
    {
        public int Pounds { get; set; }
        public int Ounces { get; set; }
    }

    public class DimensionCreateOrder
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public int Length { get; set; }
    }

    public class ShippingMethodDetailsCreateOrder
    {
        public string Carrier { get; set; }
        public string ShippingMethod { get; set; }
        public WeightCreateOrder Weight { get; set; }
        public DimensionCreateOrder Dimension { get; set; }
        public int HandlingFee { get; set; }
        public int ShippingFee { get; set; }
        public int InsuranceFee { get; set; }
        public bool LockShippingMethod { get; set; }
        public bool RushOrder { get; set; }
        public bool RequirePinToShip { get; set; }
    }

    public class CcInfoCreateOrder
    {
        public int ID { get; set; }
        public string CreditCardType { get; set; }
        public string CreditCardNumber { get; set; }
        public int CcExpirationMonth { get; set; }
        public int CcExpirationYear { get; set; }
        public string CreditCardCVVCode { get; set; }
        public string CreditCardNameOnCard { get; set; }
    }

    public class CheckDataCreateOrder
    {
        public string CheckNumber { get; set; }
        public string AccountNumber { get; set; }
        public string RoutingNumber { get; set; }
    }

    public class PaymentDetailsCreateOrder
    {
        public string Method { get; set; }
        public string PromoCode { get; set; }
        public string TransactionID { get; set; }
        public int PaymentTerm { get; set; }
        public CcInfoCreateOrder CcInfo { get; set; }
        public CheckDataCreateOrder CheckData { get; set; }
    }

    public class NoteCreateOrder
    {
        public int EntityID { get; set; }
        public string Category { get; set; }
        public int NoteID { get; set; }
        public string Note { get; set; }
        public DateTime AuditDate { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedByName { get; set; }
        public string CreatedByEmail { get; set; }
    }

    public class CreateOrderOnSCViewModel
    {
        public int ID { get; set; }
        public CustomerDetailsCreateOrder CustomerDetails { get; set; }
        public OrderDetailsCreateOrder OrderDetails { get; set; }
        public GiftDetailsCreateOrder GiftDetails { get; set; }
        public List<ProductCreateOrder> Products { get; set; }
        public ShippingAddressCreateOrder ShippingAddress { get; set; }
        public BillingAddressCreateOrder BillingAddress { get; set; }
        public ShippingMethodDetailsCreateOrder ShippingMethodDetails { get; set; }
        public PaymentDetailsCreateOrder PaymentDetails { get; set; }
        public List<NoteCreateOrder> Notes { get; set; }
        public bool OrderItemDiscountAllowed { get; set; }
        public int DiscountTotal { get; set; }
        public bool ProceedWithPayment { get; set; }
    }
}
