using DataAccess.Helper;
using DataAccess.ViewModels;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DataAccess
{
    public class BestBuyOrderDataAccess
    {
        public string connStr { get; set; }
        public BestBuyOrderDataAccess(IConnectionString connectionString)
        {
            connStr = connectionString.GetConnectionString();
        }
        public List<GetOrdersNotFromBBViewModel> GetBestBuyOrderIdsFromSellerCloud()
        {
            List<GetOrdersNotFromBBViewModel> list = null;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_GetOrderFromBestBuyAgainst_SellerCloudBBOrders", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            list = new List<GetOrdersNotFromBBViewModel>();
                            while (reader.Read())
                            {
                                GetOrdersNotFromBBViewModel fromBBViewModel = new GetOrdersNotFromBBViewModel();

                                fromBBViewModel.BBOrderID = Convert.ToString(reader["order_source_order_id"]);
                                fromBBViewModel.SCOrderID = Convert.ToInt32(reader["seller_cloud_order_id"]);
                                list.Add(fromBBViewModel);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return list;
        }
        public bool DeleteDuplicateBestBuyOrdes()
        {
            bool status = false;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("Drop_deuplicateOrdersAndThereDetail", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();
                    status = true;
                }
            }
            catch (Exception ex)
            {
            }
            return status;
        }
        public List<BestBuyDropShipQtyMovementViewModel> GetBestBuyDropShipQtyForUpdateOnBB()
        {
            List<BestBuyDropShipQtyMovementViewModel> list = null;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_GetBestBuyDropShipQtyForUpdateOnBB", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            list = new List<BestBuyDropShipQtyMovementViewModel>();
                            while (reader.Read())
                            {
                                BestBuyDropShipQtyMovementViewModel model = new BestBuyDropShipQtyMovementViewModel();
                                model.ProductSku = Convert.ToString(reader["product_sku"] != DBNull.Value ? reader["product_sku"] : "0");
                                model.DropShipQtyMovementID = Convert.ToInt32(reader["bb_ds_qty_movement_id"] != DBNull.Value ? reader["bb_ds_qty_movement_id"] : 0);
                                model.DropShipQuantity = Convert.ToInt32(reader["ds_qty"] != DBNull.Value ? reader["ds_qty"] : 0);
                                list.Add(model);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return list;
        }
        public bool UpdateBestBuyQtyWhenJobDisabled()
        {
            bool list = false;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("P_UpdateBestBuyQtyWhenJobDisabled", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();
                    list = true;
                }
            }
            catch (Exception ex)
            {
            }
            return list;
        }
        public bool SaveBestBuyOrders(List<BestBuyOrdersImportMainViewModel> mainViewModel)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    foreach (var viewModel in mainViewModel)
                    {
                        MySqlCommand cmd = new MySqlCommand("p_SaveBestBuyOrdersDummy", conn);
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("shipping_id", DBNull.Value);
                        cmd.Parameters.AddWithValue("acceptance_decision_date", viewModel.OrderViewModel.acceptance_decision_date);
                        cmd.Parameters.AddWithValue("inSellerCloud", viewModel.OrderViewModel.acceptance_decision_date);
                        cmd.Parameters.AddWithValue("sellerCloudID", viewModel.OrderViewModel.sellerCloudID);
                        cmd.Parameters.AddWithValue("can_cancel", viewModel.OrderViewModel.can_cancel);
                        cmd.Parameters.AddWithValue("commercial_id", viewModel.OrderViewModel.commercial_id);
                        cmd.Parameters.AddWithValue("created_date", viewModel.OrderViewModel.created_date);
                        cmd.Parameters.AddWithValue("customer_id", viewModel.OrderViewModel.customer_id);
                        cmd.Parameters.AddWithValue("order_id", viewModel.OrderViewModel.order_id);
                        cmd.Parameters.AddWithValue("order_state", viewModel.OrderViewModel.order_state);
                        cmd.Parameters.AddWithValue("total_commission", viewModel.OrderViewModel.total_commission);
                        cmd.Parameters.AddWithValue("total_price", viewModel.OrderViewModel.total_price);
                        cmd.Parameters.AddWithValue("city", viewModel.customerDetailOrderViewModel.city);
                        cmd.Parameters.AddWithValue("country", viewModel.customerDetailOrderViewModel.country);
                        cmd.Parameters.AddWithValue("firstname", viewModel.customerDetailOrderViewModel.firstname);
                        cmd.Parameters.AddWithValue("lastname", viewModel.customerDetailOrderViewModel.lastname);
                        cmd.Parameters.AddWithValue("phone", viewModel.customerDetailOrderViewModel.phone);
                        cmd.Parameters.AddWithValue("phone_secondary", viewModel.customerDetailOrderViewModel.phone_secondary);
                        cmd.Parameters.AddWithValue("state", viewModel.customerDetailOrderViewModel.state);
                        cmd.Parameters.AddWithValue("street_1", viewModel.customerDetailOrderViewModel.street_1);
                        cmd.Parameters.AddWithValue("street_2", viewModel.customerDetailOrderViewModel.street_2);
                        cmd.Parameters.AddWithValue("zip_code", viewModel.customerDetailOrderViewModel.zip_code);
                        cmd.Parameters.AddWithValue("email", viewModel.customerDetailOrderViewModel.email);
                        cmd.Parameters.AddWithValue("_ShippingPrice", viewModel.OrderViewModel.shipping_price);
                        cmd.Parameters.AddWithValue("_IsBox", viewModel.customerDetailOrderViewModel.IsBox);
                        cmd.ExecuteNonQuery();


                        foreach (var item in viewModel.orderDetailViewModel)
                        {
                            using (MySqlCommand command = new MySqlCommand("p_SaveBestBuyOrderDetailDumy", conn))
                            {
                                command.CommandType = System.Data.CommandType.StoredProcedure;
                                command.Parameters.AddWithValue("TaxGST", item.GST ?? 0);
                                command.Parameters.AddWithValue("offer_sku", item.offer_sku);
                                command.Parameters.AddWithValue("order_line_id", item.order_line_id);
                                command.Parameters.AddWithValue("order_line_state", item.order_line_state);
                                command.Parameters.AddWithValue("product_title", item.product_title);
                                command.Parameters.AddWithValue("TaxPST", item.PST ?? 0);
                                command.Parameters.AddWithValue("quantity", item.quantity);
                                command.Parameters.AddWithValue("received_date", item.received_date);
                                command.Parameters.AddWithValue("shipped_date", item.shipped_date);
                                command.Parameters.AddWithValue("total_commission", item.total_commissionOrderLine);
                                command.Parameters.AddWithValue("shipping_fee", item.ShippingFee);
                                command.Parameters.AddWithValue("total_price", item.total_priceOrerLine ?? 0);
                                command.Parameters.AddWithValue("order_id", viewModel.OrderViewModel.order_id);
                                command.Parameters.AddWithValue("SCOrderID", viewModel.OrderViewModel.sellerCloudID);
                                command.ExecuteNonQuery();
                            }
                        }
                    }






                }

            }
            catch (Exception ex)
            {

            }
            return true;
        }

        public bool UpdateBestBuyQtyMovementDropshipStatus(int bestBuyQtyMovementID, string importId)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();

                    MySqlCommand cmd = new MySqlCommand("p_UpdateBestBuyDropShipStatus", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_bb_ds_qty_movement_id", bestBuyQtyMovementID);
                    cmd.Parameters.AddWithValue("importID", importId);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {

            }
            return true;
        }
    }
}
