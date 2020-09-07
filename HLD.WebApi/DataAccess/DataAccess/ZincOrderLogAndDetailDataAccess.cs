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
    public class ZincOrderLogAndDetailDataAccess
    {
        public string connStr { get; set; }
        public ZincOrderLogAndDetailDataAccess(IConnectionString connectionString)
        {
            connStr = connectionString.GetConnectionString();
        }

        public int SaveZincOrderLog(ZincOrderLogViewModel ViewModel)
        {
            int ZincOrderLogID = 0;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_SaveZincOrderLog", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_sc_order_id", ViewModel.SellerCloudOrderId);
                    cmd.Parameters.AddWithValue("_request_id", ViewModel.RequestIDOfZincOrder);
                    cmd.Parameters.AddWithValue("_order_datetime", ViewModel.OrderDatetime);
                    cmd.Parameters.Add("_zinc_order_log_id", MySqlDbType.Int32, 500);
                    cmd.Parameters["_zinc_order_log_id"].Direction = ParameterDirection.Output;
                    cmd.ExecuteNonQuery();
                    ZincOrderLogID = Convert.ToInt32(cmd.Parameters["_zinc_order_log_id"].Value);

                }
            }
            catch (Exception ex)
            {
            }
            return ZincOrderLogID;
        }


        public int SaveZincOrderLogDetail(ZincOrderLogDetailViewModel ViewModel)
        {
            int zincOrderLogDetailID = 0;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_SaveZincOrderLogDetail", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_order_code", ViewModel.Code);
                    cmd.Parameters.AddWithValue("_order_message", ViewModel.Message);
                    cmd.Parameters.AddWithValue("_order_type", ViewModel.Type);
                    cmd.Parameters.AddWithValue("_zinc_order_log_id", ViewModel.ZincOrderLogID);
                    cmd.Parameters.AddWithValue("_order_data", ViewModel.Data);
                    cmd.Parameters.AddWithValue("_shpping_date", ViewModel.ShppingDate);
                    cmd.Parameters.AddWithValue("_tracking_number", ViewModel.TrackingNumber);
                    cmd.Parameters.AddWithValue("_carrier", ViewModel.Carrier);
                    cmd.Parameters.AddWithValue("_amazon_tracking", ViewModel.AmazonTracking);
                    cmd.Parameters.AddWithValue("_17_tracking", ViewModel._17Tracking);
                    cmd.Parameters.AddWithValue("_order_datetime", ViewModel.OrderDatetime);
                    cmd.Parameters.AddWithValue("_zinc_order_status_internal", ViewModel.ZincOrderStatusInternal);
                    cmd.Parameters.AddWithValue("_merchant_order_id", ViewModel.MerchantOrderId);
                    cmd.Parameters.Add("@_zinc_order_log_detail_id", MySqlDbType.Int32, 500);
                    cmd.Parameters["@_zinc_order_log_detail_id"].Direction = ParameterDirection.Output;
                    cmd.ExecuteNonQuery();
                    zincOrderLogDetailID = Convert.ToInt32(cmd.Parameters["@_zinc_order_log_detail_id"].Value);
                }
            }
            catch (Exception ex)
            {
            }
            return zincOrderLogDetailID;
        }



        public ZincOrderLogDetailViewModel GetZincOrderLogDetailById(string zincLogDetailID)
        {
            ZincOrderLogDetailViewModel viewModel = new ZincOrderLogDetailViewModel();
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_GetZincOrderLogByLogDetailID", conn);

                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("zinc_log_detail_id", zincLogDetailID);

                    MySqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            viewModel.TrackingNumber = Convert.ToString(reader["tracking_number"] != DBNull.Value ? reader["tracking_number"] : "");
                            viewModel.AmazonTracking = Convert.ToString(reader["amazon_tracking"] != DBNull.Value ? reader["amazon_tracking"] : "");
                            viewModel.Carrier = Convert.ToString(reader["carrier"] != DBNull.Value ? reader["carrier"] : "");
                            viewModel._17Tracking = Convert.ToString(reader["17_tracking"] != DBNull.Value ? reader["17_tracking"] : "");
                            viewModel.ShppingDate = Convert.ToString(reader["shpping_date"] != DBNull.Value ? reader["shpping_date"] : "");

                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return viewModel;
        }

        public ZincOrderIDModelforWebhooks GetLogid(string OurInternalID)
        {
            ZincOrderIDModelforWebhooks model = null;

            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();
                MySqlCommand cmdd = new MySqlCommand(@"SELECT zinc_order_log_id, request_id FROM bestBuyE2.ZincOrderLog where sc_order_id =" + OurInternalID, conn);
                cmdd.CommandType = System.Data.CommandType.Text;

                using (var reader = cmdd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            model = new ZincOrderIDModelforWebhooks();
                            model.request_id = Convert.ToString(reader["request_id"]);

                            model.zinc_order_log_id = Convert.ToInt32(reader["zinc_order_log_id"]);

                        }

                    }
                }

            }


            return model;
        }


    }
}
