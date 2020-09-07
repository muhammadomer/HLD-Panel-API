using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ViewModels
{
   public class SCOrderCreateNewViewModel
    {
        public int IsGiftOrder { get; set; }
        public int ShippingLocationID { get; set; }
        public int StationID { get; set; }
        public string OrderSourceOrderID2 { get; set; }
        public string CustomerIP { get; set; }
        public int HowHeard { get; set; }
        public bool LockShippingMethod { get; set; }
        public string OrderCreationSourceApplication { get; set; }
        public string Customer_TaxID { get; set; }
        public bool Customer_TaxExempt { get; set; }
        public string CouponCode { get; set; }
        public decimal TaxRate { get; set; }
        public int ParentOrderID { get; set; }
        public int ShipFromWarehouseId { get; set; }
        public int SalesRepId { get; set; }
        public decimal DiscountTotal { get; set; }
        public bool RushOrder { get; set; }
        public PaymentsNew Payments { get; set; }
        public ItemsNew Items { get; set; }
        public PackagesNew Packages { get; set; }
        public string CompanyID { get; set; }
        public string OrderSource { get; set; }
        public string OrderSourceOrderID { get; set; }
        public string OrderDate { get; set; }
        public string CustomerFirstName { get; set; }
        public string CustomerLastName { get; set; }
        public string CustomerEmail { get; set; }
        public string BillingAddressFirstName { get; set; }
        public string BillingAddressLastName { get; set; }
        public string BillingAddressCompany { get; set; }
        public string BillingAddressStreet1 { get; set; }
        public string BillingAddressStreet2 { get; set; }
        public string BillingAddressCity { get; set; }
        public string BillingAddressState { get; set; }
        public string BillingAddressZipCode { get; set; }
        public string BillingAddressCountry { get; set; }
        public string BillingAddressPhone { get; set; }
        public string ShippingAddressFirstName { get; set; }
        public string ShippingAddressLastName { get; set; }
        public string ShippingAddressCompany { get; set; }
        public string ShippingAddressStreet1 { get; set; }
        public string ShippingAddressStreet2 { get; set; }
        public string ShippingAddressCity { get; set; }
        public string ShippingAddressState { get; set; }
        public string ShippingAddressZipCode { get; set; }
        public string ShippingAddressCountry { get; set; }
        public string ShippingAddressPhone { get; set; }
        public string ShippingMethod { get; set; }
        public string ShippingCarrier { get; set; }
        public string CustomerComments { get; set; }
        public string ShippingStatus { get; set; }
        public string PaymentStatus { get; set; }
        public decimal SubTotal { get; set; }
        public decimal TaxTotal { get; set; }
        public decimal ShippingTotal { get; set; }
        public decimal GiftWrapTotal { get; set; }
        public decimal AdjustmentTotal { get; set; }
        public decimal GrandTotal { get; set; }

    }

    public class OrderPaymentDetailsNew
    {
        public string CheckNumber { get; set; }
        public string PaymentTransactionAuthID { get; set; }
        public string StoreCouponOrGiftCertificateID { get; set; }
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; }
        public string CreditCardType { get; set; }
        public string CreditCardNumber { get; set; }
        public string CreditCardSecurityCode { get; set; }
        public string CreditCardCVV2Response { get; set; }
        public int CreditCardCardExpirationMonth { get; set; }
        public int CreditCardCardExpirationYear { get; set; }
        public string PaymentFirstName { get; set; }
        public string PaymentLastName { get; set; }
        public string PaymentTransactionID { get; set; }
        public string PaymentStatus { get; set; }
        public DateTime PaymentClearanceDate { get; set; }
        public string PaymentEmailAddress { get; set; }
    }


   

    public class PaymentsNew
    {
        public List<OrderPaymentDetailsNew> OrderPaymentDetails { get; set; }
    }



    public class OrderItemDetailsNew
    {
        public string ShipType { get; set; }
        public string ReturnReason { get; set; }
        public int ShipFromWarehouseID { get; set; }
        public string ExportedProductID { get; set; }
        public decimal VariantID { get; set; }
        public string SalesOutlet { get; set; }
        //public SerialNumbers SerialNumbers { get; set; }
        public decimal DiscountTotal { get; set; }
        public decimal DiscountAmount { get; set; }
        public string DiscountType { get; set; }
        public int QtyReturned { get; set; }
        public int QtyShipped { get; set; }
        public decimal OrderItemUniqueIDInDB { get; set; }
        public string SKU { get; set; }
        public string ItemName { get; set; }
        public int Qty { get; set; }
        public int OrderSourceItemID { get; set; }
        public int OrderSourceTransactionID { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal ShippingPrice { get; set; }
        public decimal SubTotal { get; set; }
        public string Notes { get; set; }
    }

    public class ItemsNew
    {
        public List<OrderItemDetailsNew> OrderItemDetails { get; set; }
    }

    public class OrderPackageDetailsNew
    {
        public string Carrier { get; set; }
        public string ShipMethod { get; set; }
        public string TrackingNumber { get; set; }
        public string ShipDate { get; set; }
        public string FinalShippingCost { get; set; }
        public string ShippingWeight { get; set; }
        public string ShippingWidth { get; set; }
        public string ShippingLength { get; set; }
        public string ShippingHeight { get; set; }
    }

    public class PackagesNew
    {
        public OrderPackageDetailsNew OrderPackageDetails { get; set; }
    }
}
