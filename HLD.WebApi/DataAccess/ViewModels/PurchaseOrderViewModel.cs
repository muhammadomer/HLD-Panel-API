using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ViewModels
{
    public class PurchaseOrderViewModel
    {
        public class Purchase
        {
            public int POId { get; set; }
            public int CompanyId { get; set; }
            public int VendorId { get; set; }
            public DateTime OrderedOn { get; set; }
            public object RequestedOn { get; set; }
            public int OrderType { get; set; }
            public bool OrderTypeEnable { get; set; }
            public int POType { get; set; }
            public string Description { get; set; }
            public int DefaultWarehouseID { get; set; }
            public object ExpectedDelivery { get; set; }
            public bool RequireExpectedDeliveryDate { get; set; }
            public object CancelByDate { get; set; }
            public int CreditMemoID { get; set; }
            public int CurrencyCode { get; set; }
            public int ReceivingStatus { get; set; }
            public object RelatedFbaId { get; set; }
            public string Instructions { get; set; }
            public bool IsModified { get; set; }
        }

        public class VendorInvoice
        {
            public string FileName { get; set; }
            public string NewFileToken { get; set; }
            public bool DeleteFile { get; set; }
        }

        public class VendorAndInvoice
        {
            public int PaymentTermID { get; set; }
            public string InvoiceNumber { get; set; }
            public object InvoiceDate { get; set; }
            public VendorInvoice VendorInvoice { get; set; }
            public string Memo { get; set; }
            public string InvoiceLineCount { get; set; }
            public string InvoiceLink { get; set; }
            public bool EnableVendorInvoicesWorkflow { get; set; }
            public bool EnableMultipleinvoicesforpurchaseorders { get; set; }
            public int PaymentStatus { get; set; }
            public double Balance { get; set; }
            public double TotalPaid { get; set; }
            public double TotalRefunded { get; set; }
            public double OverRecived { get; set; }
            public bool IsModified { get; set; }
        }

        public class Changes
        {
            public string CreatedBy { get; set; }
            public int CreatedByEmployeeId { get; set; }
            public DateTime CreatedOn { get; set; }
            public string UpdatedBy { get; set; }
            public int UpdatedByEmployeeId { get; set; }
            public DateTime UpdatedOn { get; set; }
            public string ApprovedBy { get; set; }
            public int ApprovedByEmployeeId { get; set; }
            public DateTime ApprovedOn { get; set; }
            public object ReceivedBy { get; set; }
            public object ReceivedByEmployeeId { get; set; }
            public object ReceivedOn { get; set; }
        }

        public class Item
        {
            public int ID { get; set; }
            public int PurchaseID { get; set; }
            public string ProductID { get; set; }
            public bool IsKit { get; set; }
            public string ImageURL { get; set; }
            public string VendorSKU { get; set; }
            public bool IsVendorSKUEditable { get; set; }
            public string ProductName { get; set; }
            public int QtyOrdered { get; set; }
            public int QtyReceived { get; set; }
            public int QtyOnHand { get; set; }
            public double UnitPrice { get; set; }
            public double AdjustedPrice { get; set; }
            public double ExtraCostPerUnit { get; set; }
            public double ExtraCostPOBasedPerUnit { get; set; }
            public int DefaultWarehouseID { get; set; }
            public double Total { get; set; }
            public bool FinalSubTotalVisible { get; set; }
            public double FinalSubTotal { get; set; }
            public double MultiDiscountTotal { get; set; }
            public double DiscountValue { get; set; }
            public int DiscountType { get; set; }
            public double PricePerCase { get; set; }
            public int TotalCases { get; set; }
            public int QtyPerCase { get; set; }
            public bool PreSellQuantityEnabled { get; set; }
            public int PreSellQuantity { get; set; }
            public object LineDiscountDataContext { get; set; }
            public int RowStatus { get; set; }
            public bool IsModified { get; set; }
        }

        public class TotalInfo
        {
            public double DiscountTotal { get; set; }
            public bool DiscountTotalFixed { get; set; }
            public double MultiDiscountTotal { get; set; }
            public bool EnableMultiDiscountForPurchaseOrder { get; set; }
            public double SubTotal { get; set; }
            public bool FreeShipping { get; set; }
            public double ShippingTotal { get; set; }
            public bool EnableShippingTotal { get; set; }
            public double DropShipFeeTotal { get; set; }
            public double ShippingTotalThirdParty { get; set; }
            public bool EnableShippingTotalThirdParty { get; set; }
            public double CustomsTotal { get; set; }
            public double TaxTotal { get; set; }
            public double OtherTotal { get; set; }
            public double SmallOrderFee { get; set; }
            public double CreditCardFee { get; set; }
            public double GrandTotal { get; set; }
            public double UnreceivedTotal { get; set; }
            public bool EnableFreeShippingForPurchaseOrders { get; set; }
            public bool EnableOrderDropShipFee { get; set; }
            public bool PurchaseOrdersEnableCustomsTotalField { get; set; }
            public bool SmallOrderFeeVisible { get; set; }
            public bool EnableCreditCardFeeOnPurchaseOrders { get; set; }
            public bool HidePrices { get; set; }
            public bool DoNotIncludeThirdPartyShippingInPOGrandTotal { get; set; }
            public bool UseFixedDiscountTotal { get; set; }
            public bool UseDiscountTotal { get; set; }
            public bool EnableVendorInvoicesWorkflow { get; set; }
            public bool IsFullyRecivedandshipped { get; set; }
            public bool IsModified { get; set; }
        }

        public class Metadata
        {
            public bool EnableCaseQty { get; set; }
            public bool EnabledMultipleDiscounts { get; set; }
            public bool FixedTotalDiscount { get; set; }
            public bool EnablePerCaseQtyModeSetting { get; set; }
            public bool EnableVendorInvoicesWorkflow { get; set; }
            public bool AllowToIgnoreInventoryInPurchaseOrder { get; set; }
        }

        public class Statuses
        {
            public int Status { get; set; }
            public bool IsApproved { get; set; }
            public int Priority { get; set; }
            public int ShippingStatus { get; set; }
            public int ReceivingStatus { get; set; }
            public int PaymentStatus { get; set; }
            public string InvoicedStatus { get; set; }
            public string InventoryCount { get; set; }
            public string EmailSent { get; set; }
        }

        public class PurchaseOrderData
        {
            public Purchase Purchase { get; set; }
            public VendorAndInvoice VendorAndInvoice { get; set; }
            public Changes Changes { get; set; }
            public IList<Item> Items { get; set; }
            public IList<object> RelatedItems { get; set; }
            public TotalInfo TotalInfo { get; set; }
            public IList<object> CustomColumns { get; set; }
            public Metadata Metadata { get; set; }
            public Statuses Statuses { get; set; }
            public bool ShowProductImageForItems { get; set; }
            public bool EnablePoPrSellQuantity { get; set; }
            public int NotesCount { get; set; }
            public bool IsModified { get; set; }
        }


    }
}
