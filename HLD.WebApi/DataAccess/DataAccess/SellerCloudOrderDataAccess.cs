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
    public class SellerCloudOrderDataAccess
    {
        public string connStr { get; set; }
        public SellerCloudOrderDataAccess(IConnectionString connectionString)
        {
            connStr = connectionString.GetConnectionString();
        }




        public List<int> GetSellerCloudOrderIdForImportImages()
        {
            List<int> ordersList = new List<int>();
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();

                MySqlCommand cmd = new MySqlCommand("p_GetAllSellerCloudOrderIdsWhichImagesNotExist", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                MySqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        ordersList.Add(Convert.ToInt32(reader["seller_cloud_order_id"]));
                    }
                }

            }
            return ordersList;
        }

        public List<EmailJobDetailViewModel> GetDetailFromEmailJob(string sellerCloudOrderId)
        {
            List<EmailJobDetailViewModel> ordersList = new List<EmailJobDetailViewModel>();
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();

                MySqlCommand cmd = new MySqlCommand("p_GetSCandZincDetailForSendEmail", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("sc_order_id", sellerCloudOrderId);


                MySqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        EmailJobDetailViewModel emailModel = new EmailJobDetailViewModel();
                        emailModel.SCOrderID = Convert.ToString(reader["seller_cloud_order_id"] != DBNull.Value ? reader["seller_cloud_order_id"] : "0");
                        emailModel.RequestID = Convert.ToString(reader["request_id"] != DBNull.Value ? reader["request_id"] : "0");
                        emailModel.ProductSku = Convert.ToString(reader["product_sku"] != DBNull.Value ? reader["product_sku"] : "0");
                        emailModel.ImageName = Convert.ToString(reader["productImage"] != DBNull.Value ? reader["productImage"] : "0");
                        ordersList.Add(emailModel);
                    }
                }

            }
            return ordersList;
        }






        public List<int> GetSellerCloudOrderWhichAreExists(string sellerCloudOrderList)
        {
            List<int> ordersList = new List<int>();
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();

                MySqlCommand cmd = new MySqlCommand("p_GetAllUnshippedSellerCloudOrdersWhichExists", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("sellerCloudIds", sellerCloudOrderList);

                MySqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        ordersList.Add(Convert.ToInt32(reader["seller_cloud_order_id"]));
                    }
                }

            }
            return ordersList;
        }



        public List<int> GetSellerCloudOrderForUpdateOrderStatus()
        {
            List<int> ordersList = new List<int>();
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();

                MySqlCommand cmd = new MySqlCommand("p_GetSellerCloudOrderIdForUpdateOrderStatus_Job", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;


                MySqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        ordersList.Add(Convert.ToInt32(reader["seller_cloud_order_id"]));
                    }
                }

            }
            return ordersList;
        }



        public bool InsertDataFromSellerCloudTableToBestBuyTable()
        {
            bool status = false;
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("p_InsertProductDataFromSellerCloudToBestBuy", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();
                status = true;
            }
            return status;
        }
        public bool UpdateProductImages()
        {

            bool status = false;
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("p_UpdateProductImageToProductTable", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();
                status = true;
            }
            return status;
        }


        public bool SaveSellerCloudOrderStatus(string sellerCloudOrderId, string statusName, string paymentStatus)
        {
            bool status = false;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_SaveSellerCloudOrderStatus_log", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("order_id", sellerCloudOrderId);
                    cmd.Parameters.AddWithValue("sstatus", statusName);
                    cmd.Parameters.AddWithValue("Order_status_date", DateTimeExtensions.ConvertToEST(DateTime.Now));
                    cmd.Parameters.AddWithValue("paymentStatus", paymentStatus);
                    cmd.ExecuteNonQuery();
                }
                status = true;
            }

            catch (Exception ex)
            {
            }
            return status;
        }




        public List<string> GetSKUWhichImagesNotExists()
        {
            List<string> skuList = new List<string>();
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_GetSKUWhichImagesNotExists", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    MySqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        if (reader.HasRows)
                        {
                            skuList.Add(reader["sku"].ToString());
                        }
                    }
                }

            }

            catch (Exception ex)
            {
            }
            return skuList;
        }


        public List<SKUAndSellerCloudImageURLWhichImagesNotExistsViewModel> GetSKUAndSellerCloudImageURLWhichImagesNotExists()
        {
            List<SKUAndSellerCloudImageURLWhichImagesNotExistsViewModel> skuList = new List<SKUAndSellerCloudImageURLWhichImagesNotExistsViewModel>();
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("P_GetSKUAndSellerCloudImageURLWhichImagesNotExists", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    MySqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        if (reader.HasRows)
                        {
                            SKUAndSellerCloudImageURLWhichImagesNotExistsViewModel model = new SKUAndSellerCloudImageURLWhichImagesNotExistsViewModel();

                            model.sku = reader["sku"].ToString();
                            model.ImageURL = reader["prouduct_image_url"].ToString();

                            skuList.Add(model);
                        }
                    }
                }

            }

            catch (Exception ex)
            {
            }
            return skuList;
        }

        public string SellerCloudOrderStatusLatestUpdate(string sellerCloudOrderId)
        {
            string status = "";
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_GetSellerCloudOrderStatusLatestUpdate", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("order_id", sellerCloudOrderId);

                    MySqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        if (reader.HasRows)
                        {
                            status = reader["orderStatus"].ToString();
                        }
                    }
                }

            }

            catch (Exception ex)
            {
            }
            return status;
        }



        public bool SaveOrderAndCustomerDetail(List<SellerCloudOrder_CustomerViewModel> Data)
        {
            bool status = false;
            try
            {
                if (Data.Count > 0)
                {

                }
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {

                    conn.Open();

                    foreach (var ViewModel in Data)
                    {
                        MySqlCommand cmd = new MySqlCommand("p_SaveSellerCloudOrdersCopy", conn);
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("First_name", ViewModel.Customer.firstName);
                        cmd.Parameters.AddWithValue("Last_name", ViewModel.Customer.lastName);
                        cmd.Parameters.AddWithValue("Phone_number", ViewModel.Customer.phoneNumber);
                        cmd.Parameters.AddWithValue("Postal_code", ViewModel.Customer.postalCode);
                        cmd.Parameters.AddWithValue("State", ViewModel.Customer.stateCode);
                        cmd.Parameters.AddWithValue("Street_line1", ViewModel.Customer.streetLine1);
                        cmd.Parameters.AddWithValue("Street_line2", ViewModel.Customer.streetLine2);
                        cmd.Parameters.AddWithValue("City", ViewModel.Customer.city);
                        cmd.Parameters.AddWithValue("Total_count", ViewModel.Order.totalCount);
                        cmd.Parameters.AddWithValue("Customer_id", ViewModel.Order.customerId);
                        cmd.Parameters.AddWithValue("Drop_ship_status", ViewModel.Order.dropShipStatus);
                        cmd.Parameters.AddWithValue("Last_update", ViewModel.Order.lastUpdate);
                        cmd.Parameters.AddWithValue("Orde_currency_code", ViewModel.Order.orderCurrencyCode);
                        cmd.Parameters.AddWithValue("Order_source_order_id", ViewModel.Order.orderSourceOrderId);
                        cmd.Parameters.AddWithValue("Payment_date", ViewModel.Order.paymentDate);
                        cmd.Parameters.AddWithValue("Seller_cloud_order_id", ViewModel.Order.sellerCloudID);
                        cmd.Parameters.AddWithValue("Shipping_status", ViewModel.Order.shippingStatus);
                        cmd.Parameters.AddWithValue("Shipping_weight_total_Oz", ViewModel.Order.shippingWeightTotalOz);
                        cmd.Parameters.AddWithValue("Total_tax", ViewModel.Order.taxTotal);
                        cmd.Parameters.AddWithValue("Time_of_order", ViewModel.Order.timeOfOrder);
                        cmd.Parameters.AddWithValue("Currency_rate_From_USD", ViewModel.Order.currencyRateFromUSD);
                        cmd.Parameters.AddWithValue("_IsBox", ViewModel.Customer.IsBox);
                        cmd.ExecuteNonQuery();

                        foreach (var item in ViewModel.orderDetail)
                        {
                            MySqlCommand OrderDetailcmd = new MySqlCommand("p_SaveSellerCloudOrderDetail", conn);
                            OrderDetailcmd.CommandType = System.Data.CommandType.StoredProcedure;
                            OrderDetailcmd.Parameters.AddWithValue("Seller_cloud_order_id", ViewModel.Order.sellerCloudID);
                            OrderDetailcmd.Parameters.AddWithValue("Drop_shipped_on", item.DropShippedOn);
                            OrderDetailcmd.Parameters.AddWithValue("Drop_shipped_status", item.DropShippedStatus);
                            OrderDetailcmd.Parameters.AddWithValue("MinQty", item.MinQTY);
                            OrderDetailcmd.Parameters.AddWithValue("Product_sku", item.SKU);
                            OrderDetailcmd.Parameters.AddWithValue("Status_code", item.StatusCode);
                            OrderDetailcmd.Parameters.AddWithValue("Qty", item.Qty);
                            OrderDetailcmd.Parameters.AddWithValue("Adjusted_site_price", item.AdjustedSitePrice);
                            OrderDetailcmd.Parameters.AddWithValue("Avg_cost", item.AverageCost);
                            OrderDetailcmd.Parameters.AddWithValue("Price_per_case", item.PricePerCase);
                            OrderDetailcmd.Parameters.AddWithValue("Unit_price", item.unitPrice);
                            OrderDetailcmd.Parameters.AddWithValue("UPC", item.UPC);
                            OrderDetailcmd.Parameters.AddWithValue("ProductTitle", item.ProductTitle);

                            OrderDetailcmd.ExecuteNonQuery();

                        }

                    }
                    status = true;
                }
            }
            catch (Exception ex)
            {
            }
            return status;
        }


        public bool SaveProductImagesFromSellerCloudOrders(ImagesSaveToDatabaseWithURLViewMOdel Data)
        {
            bool status = false;

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_SaveProductImagesOfSellerCloudOrders", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("imageName", Data.FileName);
                    cmd.Parameters.AddWithValue("_product_sku", Data.product_Sku);
                    cmd.Parameters.AddWithValue("_imageURL", Data.ImageURL);
                    cmd.Parameters.Add("_status", MySqlDbType.Int32, 500);
                    cmd.Parameters["_status"].Direction = ParameterDirection.Output;
                    cmd.ExecuteNonQuery();
                    int ID = Convert.ToInt32(cmd.Parameters["_status"].Value);
                    if (ID == 1)
                    {
                        status = true;
                    }
                    else
                    {
                        status = true;
                    }
                }

            }

            catch (Exception ex)
            {
            }
            return status;
        }



        public bool UpdateSCOrderDropShipStatus(UpdateSCDropshipStatusViewModel model)
        {
            bool status = false;

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_UpdateSCOrderDropShipStatus", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_orderID", model.SCOrderID);
                    cmd.Parameters.AddWithValue("_drop_ship_status", model.StatusName);
                    cmd.Parameters.AddWithValue("_order_datetime", model.LogDate);
                    cmd.Parameters.AddWithValue("_is_tracking_updated", model.IsTrackingUpdate);
                    cmd.ExecuteNonQuery();
                    status = true;
                }

            }

            catch (Exception ex)
            {

            }
            return status;
        }

        public bool UpdatePaymentOrderStatus(string paymentStatus, string OrderID)
        {
            bool status = false;

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_UpdatePaymentOrderStatus", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_order_id", OrderID);
                    cmd.Parameters.AddWithValue("_paymentStatus", paymentStatus);
                    cmd.ExecuteNonQuery();
                    status = true;
                }

            }

            catch (Exception ex)
            {

            }
            return status;
        }

        public string GetproducTtitle(GetProductTitleViewModel saveWatchlistViewModel)
        {
            string status = "";
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmdd = new MySqlCommand(@"select product_title from sellerCloudOrderDetail where seller_cloud_order_id='" + saveWatchlistViewModel.sellercloudid + "'AND product_sku='" + saveWatchlistViewModel.producktsku + "'", conn);
                    cmdd.CommandType = System.Data.CommandType.Text;

                    using (var reader = cmdd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {

                                status = reader["product_title"].ToString();



                            }

                        }
                    }
                }
                return status;
            }

            catch (Exception ex)
            {
            }
            return status;
        }


        public bool UpdateAccounts(int Id, int ZincAccountId, int CreditCardId)
        {
            bool status = false;

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_UpdateSCOAccounts", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_Id", Id);
                    cmd.Parameters.AddWithValue("_ZincAccountId", ZincAccountId);
                    cmd.Parameters.AddWithValue("_CreditCardId", CreditCardId);
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
