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
   public class SellerCloudOrderDataAccessNew
    {
        public string connStr { get; set; }
        public SellerCloudOrderDataAccessNew(IConnectionString connectionString)
        {
            connStr = connectionString.GetConnectionString();
        }
        public List<int> GetSellerCloudOrderForUpdateOrderStatus()
        {
            List<int> ordersList = new List<int>();
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();

                MySqlCommand cmd = new MySqlCommand("p_GetSellerCloudOrderIdForUpdateOrderStatus_JobNew", conn);
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
        public bool UpdateSCOrderDropShipStatus(UpdateSCDropshipStatusViewModel model)
        {
            bool status = false;

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_UpdateSCOrderDropShipStatusNew", conn);
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
    }
}
