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
   public class AddOrderToSCDataAccessNew
    {
        public string connStr { get; set; }
        public AddOrderToSCDataAccessNew(IConnectionString connectionString)
        {
            connStr = connectionString.GetConnectionString();
           // connStr = connectionString.GetPhpConnectionString();
        }
        public List<UnCreatedOrderViewModel> GetUncreatedOrder()
        {
            List<UnCreatedOrderViewModel> listModel = new List<UnCreatedOrderViewModel>();

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
//                    MySqlCommand cmd = new MySqlCommand(@"SELECT `order_id`,orders.`bbe2_orders_id` FROM `bestBuyE2`.`orders` 
//inner join `customer` on `orders`.`bbe2_orders_id` = `customer`.`bbe2_orders_id`
//WHERE orders.`inSellerCloud`= 0 and `customer`.`city` <>'';", conn);
                    MySqlCommand cmd = new MySqlCommand(@"SELECT `order_id`,`SCOrdersId` FROM `bestBuyE2`.`SCOrders` WHERE `inSellerCloud`= 0 and `city` <>'';", conn);
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

                            model.bbe2OrdersId = Convert.ToInt32(dr["SCOrdersId"] != DBNull.Value ? dr["SCOrdersId"].ToString() : "");
                            listModel.Add(model);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
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
                    MySqlCommand cmd = new MySqlCommand(@"SELECT `city` FROM `SCOrders` WHERE `SCOrdersId` =" + "'" + bbe2ordersid + "'", conn);
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
                        shippingAddressdetail.Region = "";
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
                        customerDetails.Email = billingAddressdetail.Address.Replace(" ", "") + billingAddressdetail.ZipCode.Replace(" ", "") + "@bestbuy.ca";

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
                        shippingMethodDetails.Carrier = Convert.ToString(list.Select(e => e.Field<string>("shipping_company")).FirstOrDefault() != null ? list.Select(e => e.Field<string>("shipping_company")).FirstOrDefault() : ""); // confirm
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
                        int transactionid = random.Next(1111111, 9999999);
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
