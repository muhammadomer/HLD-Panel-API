using DataAccess.Helper;
using DataAccess.ViewModels;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DataAccess
{
   public class AddOrderToSCDataAccess
    {
        public string connStr { get; set; }
        public AddOrderToSCDataAccess(IConnectionString connectionString)
        {
            connStr = connectionString.GetPhpConnectionString();
        }

        public List<UnCreatedOrderViewModel> GetUncreatedOrder()
        {
            List<UnCreatedOrderViewModel> listModel  = new List<UnCreatedOrderViewModel>(); 
           
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(@"SELECT `order_id`,orders.`bbe2_orders_id` FROM `bestBuyE2`.`orders` 
inner join `customer` on `orders`.`bbe2_orders_id` = `customer`.`bbe2_orders_id`
WHERE orders.`inSellerCloud`= 0 and `customer`.`city` <>'';", conn);
                    cmd.CommandType = System.Data.CommandType.Text;
                    MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    mySqlDataAdapter.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                       
                        foreach (DataRow dr in dt.Rows)
                        {
                            UnCreatedOrderViewModel model = new UnCreatedOrderViewModel();
                            model.Orderid = Convert.ToString(dr["order_id"] != DBNull.Value ? dr["order_id"].ToString() : "");
                            
                            model.bbe2OrdersId = Convert.ToInt32(dr["bbe2_orders_id"] != DBNull.Value ? dr["bbe2_orders_id"].ToString() : "");
                            listModel.Add(model);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return listModel;
        }
        
        public bool CheckCityOrder(int bbe2ordersid)
        {
            bool status = false;

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(@"SELECT `city` FROM `customer` WHERE `bbe2_orders_id` =" + "'" + bbe2ordersid + "'", conn);
                    cmd.CommandType = System.Data.CommandType.Text;
                    MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    mySqlDataAdapter.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                      
                        foreach (DataRow dr in dt.Rows)
                        {
                           
                           string city = Convert.ToString(dr["city"] != DBNull.Value ? dr["city"].ToString() : "");

                            if (!String.IsNullOrEmpty(city))
                                status = true;

                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return status;
        }


        public CreateOrderOnSCViewModel GetSCOrderData(string bbOrderId)
        {
            CreateOrderOnSCViewModel createOrderOnSCViewModel = null;
            try
            {
                System.Data.DataSet ds = new System.Data.DataSet();
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {

                    MySqlCommand cmd = new MySqlCommand("p_GetOrdersToCreateOnSC", conn);

                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_SourceOrderID", bbOrderId);

                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    da.Fill(ds);
                    createOrderOnSCViewModel = new CreateOrderOnSCViewModel();
                    System.Data.DataView dataView = new System.Data.DataView(ds.Tables[0]);
                    System.Data.DataTable distinctValue = dataView.ToTable(true, "order_id");
                    DataTable dt = ds.Tables[0];
                    foreach (System.Data.DataRow reader in distinctValue.Rows)
                    {
                        
                        CustomerDetailsCreateOrder customerDetails = new CustomerDetailsCreateOrder();
                        OrderDetailsCreateOrder orderDetails = new OrderDetailsCreateOrder();
                        GiftDetailsCreateOrder giftDetails = new GiftDetailsCreateOrder();
                        ShippingAddressCreateOrder shippingAddressdetail = new ShippingAddressCreateOrder();
                        BillingAddressCreateOrder billingAddressdetail = new BillingAddressCreateOrder();
                        ShippingMethodDetailsCreateOrder shippingMethodDetails = new ShippingMethodDetailsCreateOrder();
                        PaymentDetailsCreateOrder paymentDetails = new PaymentDetailsCreateOrder();
                        List<ProductCreateOrder> productCreateOrdersList = new List<ProductCreateOrder>();

                        var list = dt.AsEnumerable().Where(e => e.Field<string>("order_id") == reader["order_id"].ToString()).ToList();
                        
                      
                        // order
                        orderDetails.CompanyID = 513;
                        orderDetails.MarketingSource = 0;// confirm
                        orderDetails.SalesRepresentative = 4596685;// confirm
                        orderDetails.TaxExempt = false;
                        orderDetails.GiftOrder = false;
                        orderDetails.Channel = "Website";// confirm
                        orderDetails.OrderSourceOrderID = Convert.ToString(list.Select(e => e.Field<string>("order_id")).FirstOrDefault());
                        // gift 
                        giftDetails.UseGiftWrap = false;
                        giftDetails.GiftMessage = "";
                        giftDetails.GiftWrap = 0;
                        giftDetails.GiftWrapType = "";
                        // shipping 
                        shippingAddressdetail.Business = "";
                        shippingAddressdetail.FirstName = Convert.ToString(list.Select(e => e.Field<string>("firstname")).FirstOrDefault());
                        shippingAddressdetail.MiddleName = "";// Confirm
                        shippingAddressdetail.LastName = Convert.ToString(list.Select(e => e.Field<string>("lastname")).FirstOrDefault());
                        shippingAddressdetail.Country = Convert.ToString(list.Select(e => e.Field<string>("country")).FirstOrDefault());
                        shippingAddressdetail.City = Convert.ToString(list.Select(e => e.Field<string>("city")).FirstOrDefault());
                        shippingAddressdetail.State = Convert.ToString(list.Select(e => e.Field<string>("state")).FirstOrDefault());
                        shippingAddressdetail.Region ="";
                        shippingAddressdetail.ZipCode = Convert.ToString(list.Select(e => e.Field<string>("zip_code")).FirstOrDefault());
                        shippingAddressdetail.Address = Convert.ToString(list.Select(e => e.Field<string>("street_1")).FirstOrDefault());
                        shippingAddressdetail.Address2 = Convert.ToString(list.Select(e => e.Field<string>("street_2")).FirstOrDefault());
                        shippingAddressdetail.Phone = Convert.ToString(list.Select(e => e.Field<string>("phone")).FirstOrDefault());
                        shippingAddressdetail.Fax = Convert.ToString(list.Select(e => e.Field<string>("phone")).FirstOrDefault());
                        // billing 
                        billingAddressdetail.Business = "";
                        billingAddressdetail.FirstName = Convert.ToString(list.Select(e => e.Field<string>("firstname")).FirstOrDefault());
                        billingAddressdetail.MiddleName = "";// Confirm
                        billingAddressdetail.LastName = Convert.ToString(list.Select(e => e.Field<string>("lastname")).FirstOrDefault());
                        billingAddressdetail.Country = Convert.ToString(list.Select(e => e.Field<string>("country")).FirstOrDefault());
                        billingAddressdetail.City = Convert.ToString(list.Select(e => e.Field<string>("city")).FirstOrDefault());
                        billingAddressdetail.State = Convert.ToString(list.Select(e => e.Field<string>("state")).FirstOrDefault());
                        billingAddressdetail.Region = "";
                        billingAddressdetail.ZipCode = Convert.ToString(list.Select(e => e.Field<string>("zip_code")).FirstOrDefault());
                        billingAddressdetail.Address = Convert.ToString(list.Select(e => e.Field<string>("street_1")).FirstOrDefault());
                        billingAddressdetail.Address2 = Convert.ToString(list.Select(e => e.Field<string>("street_2")).FirstOrDefault());
                        billingAddressdetail.Phone = Convert.ToString(list.Select(e => e.Field<string>("phone")).FirstOrDefault());
                        billingAddressdetail.Fax = Convert.ToString(list.Select(e => e.Field<string>("phone")).FirstOrDefault());



                        // customer
                        customerDetails.ID = 0;
                        customerDetails.FirstName = Convert.ToString(list.Select(e => e.Field<string>("firstname")).FirstOrDefault());
                        customerDetails.LastName = Convert.ToString(list.Select(e => e.Field<string>("lastname")).FirstOrDefault());
                        customerDetails.Business = "";// confirm
                        customerDetails.IsWholesale = false;
                        customerDetails.Email = billingAddressdetail.Address.Replace(" ", "") + billingAddressdetail.ZipCode.Replace(" ","")+"@bestbuy.ca";

                        WeightCreateOrder weightCreateOrder = new WeightCreateOrder();
                        DimensionCreateOrder dimensionCreateOrder = new DimensionCreateOrder();
                        // weight
                        weightCreateOrder.Ounces = 0;
                        weightCreateOrder.Pounds = 0;
                        // Dimension
                        dimensionCreateOrder.Width = 0;
                        dimensionCreateOrder.Height = 0;
                        dimensionCreateOrder.Length = 0;
                        // shipping method Detail
                        shippingMethodDetails.Carrier = Convert.ToString(list.Select(e => e.Field<string>("shipping_company")).FirstOrDefault()!=null? list.Select(e => e.Field<string>("shipping_company")).FirstOrDefault() : ""); // confirm
                        shippingMethodDetails.ShippingMethod = "";// confirm
                        
                        shippingMethodDetails.HandlingFee = 0;
                        shippingMethodDetails.ShippingFee = Convert.ToInt32(list.Select(e => e.Field<Int64>("shipping_price")).FirstOrDefault());
                        shippingMethodDetails.InsuranceFee = 0;
                        shippingMethodDetails.LockShippingMethod = false; // Confirm
                        shippingMethodDetails.RushOrder = false; // Confirm
                        shippingMethodDetails.RequirePinToShip = false; // Confirm
                        
                        shippingMethodDetails.Weight = weightCreateOrder;
                        shippingMethodDetails.Dimension = dimensionCreateOrder;
                        // ccinfo
                        CcInfoCreateOrder ccInfoCreate = new CcInfoCreateOrder();
                        ccInfoCreate.CreditCardType = "None";
                        ccInfoCreate.ID = 0;
                        ccInfoCreate.CreditCardNumber = "";
                        ccInfoCreate.CcExpirationMonth = 0;
                        ccInfoCreate.CcExpirationYear = 0;
                        ccInfoCreate.CreditCardNameOnCard = "";
                        ccInfoCreate.CreditCardCVVCode = "";
                        // checkdata
                        CheckDataCreateOrder checkData = new CheckDataCreateOrder();
                        checkData.AccountNumber = "";
                        checkData.CheckNumber = "";
                        checkData.RoutingNumber = "";
                        // payment method
                        Random random = new Random();
                        int transactionid =  random.Next(1111111, 9999999);
                        paymentDetails.Method = "Cash";
                        paymentDetails.PromoCode = "";// confirm
                        paymentDetails.PaymentTerm = 1;// confirm
                        paymentDetails.TransactionID = transactionid.ToString();// confirm
                        paymentDetails.CheckData = checkData;
                      
                        paymentDetails.CcInfo = ccInfoCreate;

                        // notes
                        List<NoteCreateOrder> notes = new List<NoteCreateOrder>();
                        NoteCreateOrder noteCreate = new NoteCreateOrder();
                        noteCreate.NoteID = 0;
                        noteCreate.EntityID = 0;
                        noteCreate.Category = "General";
                        noteCreate.Note = "";
                        var timeUtc = DateTime.UtcNow;
                        TimeZoneInfo easternZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");

                        noteCreate.AuditDate = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, easternZone);
                        noteCreate.CreatedBy = 0;
                        noteCreate.CreatedByName = "";
                        noteCreate.CreatedByEmail = "";

                        notes.Add(noteCreate);
                        
                        foreach (DataRow dataRow in list)
                        {

                            ProductCreateOrder productCreate = new ProductCreateOrder();
                         
                            productCreate.SitePrice = Convert.ToDecimal(dataRow["total_price"] != DBNull.Value ? dataRow["total_price"] : "0");
                            productCreate.ProductID = Convert.ToString(dataRow["offer_sku"] != DBNull.Value ? dataRow["offer_sku"] : string.Empty);

                            productCreate.ProductName = Convert.ToString(dataRow["product_title"] != DBNull.Value ? dataRow["product_title"] : string.Empty);
                            productCreate.Qty = Convert.ToInt32(dataRow["totalQuantity"] != DBNull.Value ? dataRow["totalQuantity"] : "0");

                            productCreate.DiscountType = "FixedAmount";
                            productCreate.DiscountValue = 0;
                            productCreate.LineTaxTotal = Convert.ToDecimal(dataRow["TotalTax"] != DBNull.Value ? dataRow["TotalTax"] : "0");
                           
                            productCreateOrdersList.Add(productCreate);


                        }

                       
                        createOrderOnSCViewModel.ID = transactionid; 
                        createOrderOnSCViewModel.CustomerDetails = customerDetails;
                        createOrderOnSCViewModel.OrderDetails = orderDetails;
                        createOrderOnSCViewModel.GiftDetails = giftDetails;
                        createOrderOnSCViewModel.Products = productCreateOrdersList;
                        createOrderOnSCViewModel.ShippingAddress = shippingAddressdetail;
                        createOrderOnSCViewModel.BillingAddress = billingAddressdetail;
                        createOrderOnSCViewModel.PaymentDetails = paymentDetails;
                        createOrderOnSCViewModel.Notes = notes;
                        createOrderOnSCViewModel.OrderItemDiscountAllowed = false; // confirm
                        createOrderOnSCViewModel.DiscountTotal = 0;
                        createOrderOnSCViewModel.ProceedWithPayment = true; // confirm
                        createOrderOnSCViewModel.ShippingMethodDetails = shippingMethodDetails;



                    }
                }
            }
            catch (Exception ex)
            {
            }

            return createOrderOnSCViewModel;
        }


        public SCOrderCreateNewViewModel GetSCOrderDataForCreation(string bbOrderId)
        {
            SCOrderCreateNewViewModel createOrderOnSCViewModel = null;
            try
            {
                System.Data.DataSet ds = new System.Data.DataSet();
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {

                    MySqlCommand cmd = new MySqlCommand("p_GetOrdersToCreateOnSC", conn);

                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_SourceOrderID", bbOrderId);

                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    da.Fill(ds);
                    createOrderOnSCViewModel = new SCOrderCreateNewViewModel();
                    System.Data.DataView dataView = new System.Data.DataView(ds.Tables[0]);
                    System.Data.DataTable distinctValue = dataView.ToTable(true, "order_id");
                    DataTable dt = ds.Tables[0];
                    foreach (System.Data.DataRow reader in distinctValue.Rows)
                    {
                        PaymentsNew paymentsNew = new PaymentsNew();
                        OrderPaymentDetailsNew orderPaymentDetailsNew = new OrderPaymentDetailsNew();
                        OrderPaymentDetailsNew orderPaymentDetailsNewtax = new OrderPaymentDetailsNew();
                        OrderItemDetailsNew orderItemDetailsNew = new OrderItemDetailsNew();
                        ItemsNew itemsNew = new ItemsNew();
                        OrderPackageDetailsNew orderPackageDetailsNew = new OrderPackageDetailsNew();
                        PackagesNew packagesNew = new PackagesNew();
                        

                        var list = dt.AsEnumerable().Where(e => e.Field<string>("order_id") == reader["order_id"].ToString()).ToList();


                        // order
                        createOrderOnSCViewModel.CompanyID = "513";
                        createOrderOnSCViewModel.IsGiftOrder = 0;// confirm
                        createOrderOnSCViewModel.ShippingLocationID = 0;// confirm
                        createOrderOnSCViewModel.StationID = 0;
                        createOrderOnSCViewModel.HowHeard =0 ;
                        createOrderOnSCViewModel.LockShippingMethod = false ;
                        createOrderOnSCViewModel.OrderCreationSourceApplication = "Default";// confirm
                        createOrderOnSCViewModel.OrderSourceOrderID = Convert.ToString(list.Select(e => e.Field<string>("order_id")).FirstOrDefault());
                        // gift 
                        createOrderOnSCViewModel.Customer_TaxExempt = false;
                        createOrderOnSCViewModel.RushOrder = false;
                        createOrderOnSCViewModel.TaxRate = 0;
                        createOrderOnSCViewModel.ParentOrderID = 0;
                        createOrderOnSCViewModel.SalesRepId = 0;
                        createOrderOnSCViewModel.ShipFromWarehouseId = 0;
                        createOrderOnSCViewModel.DiscountTotal = 0;
                        createOrderOnSCViewModel.PaymentStatus = "Charged";
                        createOrderOnSCViewModel.OrderSource = "Website";
                        createOrderOnSCViewModel.SubTotal = 0;
                        createOrderOnSCViewModel.ShippingTotal = 0;
                        createOrderOnSCViewModel.GiftWrapTotal = 0;
                        createOrderOnSCViewModel.GrandTotal = 0;
                        createOrderOnSCViewModel.AdjustmentTotal = 0;
                        createOrderOnSCViewModel.BillingAddressFirstName = Convert.ToString(list.Select(e => e.Field<string>("firstname")).FirstOrDefault());
                        createOrderOnSCViewModel.BillingAddressLastName = Convert.ToString(list.Select(e => e.Field<string>("lastname")).FirstOrDefault());
                        createOrderOnSCViewModel.BillingAddressStreet1 = Convert.ToString(list.Select(e => e.Field<string>("street_1")).FirstOrDefault());
                        createOrderOnSCViewModel.BillingAddressStreet2 = Convert.ToString(list.Select(e => e.Field<string>("street_2")).FirstOrDefault()); 

                      
                        createOrderOnSCViewModel.CustomerEmail = createOrderOnSCViewModel.BillingAddressStreet2.Replace(" ", "") + createOrderOnSCViewModel.BillingAddressZipCode.Replace(" ", "") + "@bestbuy.ca";
                        // shipping 
                       
                        createOrderOnSCViewModel.CustomerFirstName = Convert.ToString(list.Select(e => e.Field<string>("firstname")).FirstOrDefault());
                        
                        createOrderOnSCViewModel.CustomerLastName = Convert.ToString(list.Select(e => e.Field<string>("lastname")).FirstOrDefault());
                        createOrderOnSCViewModel.OrderDate = Convert.ToString(list.Select(e => e.Field<string>("created_date")).FirstOrDefault());

                        createOrderOnSCViewModel.BillingAddressCountry = Convert.ToString(list.Select(e => e.Field<string>("country")).FirstOrDefault());
                        createOrderOnSCViewModel.BillingAddressCity = Convert.ToString(list.Select(e => e.Field<string>("city")).FirstOrDefault());
                        createOrderOnSCViewModel.BillingAddressState = Convert.ToString(list.Select(e => e.Field<string>("state")).FirstOrDefault());
                       
                        createOrderOnSCViewModel.BillingAddressZipCode = Convert.ToString(list.Select(e => e.Field<string>("zip_code")).FirstOrDefault());

                      
                        createOrderOnSCViewModel.BillingAddressPhone = Convert.ToString(list.Select(e => e.Field<string>("phone")).FirstOrDefault());

                        createOrderOnSCViewModel.ShippingAddressFirstName = Convert.ToString(list.Select(e => e.Field<string>("firstname")).FirstOrDefault());
                        createOrderOnSCViewModel.ShippingAddressLastName = Convert.ToString(list.Select(e => e.Field<string>("lastname")).FirstOrDefault());
                        createOrderOnSCViewModel.ShippingAddressStreet1 = Convert.ToString(list.Select(e => e.Field<string>("street_1")).FirstOrDefault());
                        createOrderOnSCViewModel.ShippingAddressStreet2 = Convert.ToString(list.Select(e => e.Field<string>("street_2")).FirstOrDefault());
                        
                        createOrderOnSCViewModel.ShippingAddressCountry = Convert.ToString(list.Select(e => e.Field<string>("country")).FirstOrDefault());
                        createOrderOnSCViewModel.ShippingAddressCity = Convert.ToString(list.Select(e => e.Field<string>("city")).FirstOrDefault());
                        createOrderOnSCViewModel.ShippingAddressState = Convert.ToString(list.Select(e => e.Field<string>("state")).FirstOrDefault());

                        createOrderOnSCViewModel.ShippingAddressZipCode = Convert.ToString(list.Select(e => e.Field<string>("zip_code")).FirstOrDefault());


                        createOrderOnSCViewModel.ShippingAddressPhone = Convert.ToString(list.Select(e => e.Field<string>("phone")).FirstOrDefault());
                        createOrderOnSCViewModel.ShippingAddressCompany = "";
                        createOrderOnSCViewModel.ShippingMethod = "";
                        createOrderOnSCViewModel.ShippingCarrier = "";
                        createOrderOnSCViewModel.CustomerComments = "";

                        // order payment detail view
                        orderPaymentDetailsNew.Amount = 0;// price
                        orderPaymentDetailsNew.PaymentMethod = "Cash";
                        orderPaymentDetailsNew.PaymentClearanceDate = DateTime.Now;
                        orderPaymentDetailsNew.PaymentTransactionID = "";
                        orderPaymentDetailsNew.PaymentStatus = "Cleared";
                        orderPaymentDetailsNew.StoreCouponOrGiftCertificateID = "";
                        orderPaymentDetailsNew.CreditCardType = "None";
                        orderPaymentDetailsNew.CreditCardCardExpirationMonth = 0;
                        orderPaymentDetailsNew.CreditCardCardExpirationYear = 0;
                        orderPaymentDetailsNew.PaymentFirstName = Convert.ToString(list.Select(e => e.Field<string>("firstname")).FirstOrDefault()); 
                        orderPaymentDetailsNew.PaymentLastName = Convert.ToString(list.Select(e => e.Field<string>("lastname")).FirstOrDefault());

                        orderPaymentDetailsNewtax.Amount = 0;// tax
                        orderPaymentDetailsNewtax.PaymentMethod = "Cash";
                        orderPaymentDetailsNewtax.PaymentClearanceDate = DateTime.Now;
                        orderPaymentDetailsNewtax.PaymentTransactionID = "";
                        orderPaymentDetailsNewtax.PaymentStatus = "Cleared";
                        orderPaymentDetailsNewtax.StoreCouponOrGiftCertificateID = "";
                        orderPaymentDetailsNewtax.CreditCardType = "None";
                        orderPaymentDetailsNewtax.CreditCardCardExpirationMonth = 0;
                        orderPaymentDetailsNewtax.CreditCardCardExpirationYear = 0;
                        orderPaymentDetailsNewtax.PaymentFirstName = Convert.ToString(list.Select(e => e.Field<string>("firstname")).FirstOrDefault());
                        orderPaymentDetailsNewtax.PaymentLastName = Convert.ToString(list.Select(e => e.Field<string>("lastname")).FirstOrDefault());

                        paymentsNew.OrderPaymentDetails.Add( orderPaymentDetailsNew );
                        paymentsNew.OrderPaymentDetails.Add( orderPaymentDetailsNewtax );

                        List < OrderItemDetailsNew > itemlist =new List<OrderItemDetailsNew>();
                        foreach (DataRow dataRow in list)
                        {

                            OrderItemDetailsNew productCreate = new OrderItemDetailsNew();

                            productCreate.UnitPrice = Convert.ToDecimal(dataRow["totalPrice"] != DBNull.Value ? dataRow["totalPrice"] : "0");
                            productCreate.SKU = Convert.ToString(dataRow["offer_sku"] != DBNull.Value ? dataRow["offer_sku"] : string.Empty);

                            productCreate.ItemName = Convert.ToString(dataRow["product_title"] != DBNull.Value ? dataRow["product_title"] : string.Empty);
                            productCreate.Qty = Convert.ToInt32(dataRow["totalQuantity"] != DBNull.Value ? dataRow["totalQuantity"] : "0");
                            productCreate.ShipFromWarehouseID = 358;
                            productCreate.DiscountType = "FixedAmount";
                            productCreate.DiscountAmount = 0;
                            productCreate.DiscountTotal = 0;
                            productCreate.VariantID = 0;
                            productCreate.QtyReturned = 0;
                            productCreate.QtyShipped = 0;
                            productCreate.OrderItemUniqueIDInDB = 0;
                            productCreate.OrderSourceItemID = 0;
                            productCreate.OrderSourceTransactionID = 0;
                            productCreate.ShippingPrice = 0;
                            productCreate.SubTotal = 0;
                            productCreate.Notes = "";



                            itemlist.Add(productCreate);


                        }
                        itemsNew.OrderItemDetails = itemlist;


                    }
                }
            }
            catch (Exception ex)
            {
            }

            return createOrderOnSCViewModel;
        }

        public bool UpdateSellerID(string sourceid, int sellerid)
        {
            bool status = false;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("P_UpdateOrderONlocal", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_SourceOrderID", sourceid);
                    cmd.Parameters.AddWithValue("_SellerID", sellerid);
                   
                    cmd.ExecuteNonQuery();
                    status = true;
                }
            }
            catch (Exception ex)
            {
            }
            return status;
        }




    }
}
