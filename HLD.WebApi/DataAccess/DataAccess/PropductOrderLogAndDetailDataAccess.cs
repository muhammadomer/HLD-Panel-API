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
   public class PropductOrderLogAndDetailDataAccess
    {
        public string connStr { get; set; }
        public PropductOrderLogAndDetailDataAccess(IConnectionString connectionString)
        {
            connStr = connectionString.GetConnectionString();
        }
        public int SaveProductOrderLogDetail(SendToZincProductViewModel ViewModel)
        {
            int zincOrderLogDetailID = 0;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("P_SaveSendToZincProduct", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_OrderId", ViewModel.OrderId);
                    cmd.Parameters.AddWithValue("_Asin", ViewModel.Asin);
                    cmd.Parameters.AddWithValue("_Sku", ViewModel.Sku);
                    cmd.Parameters.AddWithValue("_ShipDays", ViewModel.Shipdays);
                    cmd.Parameters.AddWithValue("_AccountDetail", ViewModel.ZincAccountId);
                    cmd.Parameters.AddWithValue("_CreditCardDetail", ViewModel.CreditCardId);
                    cmd.Parameters.AddWithValue("_AddressLine1", ViewModel.Address1);
                    cmd.Parameters.AddWithValue("_AddressLine2", ViewModel.Address2);
                    cmd.Parameters.AddWithValue("_PostalCode", ViewModel.PostalCode);
                    cmd.Parameters.AddWithValue("_City", ViewModel.City);
                    cmd.Parameters.AddWithValue("_State", ViewModel.State);
                    cmd.Parameters.AddWithValue("_Phone", ViewModel.Phone);
                    cmd.Parameters.AddWithValue("_Country", ViewModel.Country);
                    cmd.Parameters.AddWithValue("_FirstName", ViewModel.FirstName);
                    cmd.Parameters.AddWithValue("_LastName", ViewModel.LastName);
                    cmd.Parameters.AddWithValue("_Qty", ViewModel.Qty);
                    cmd.Parameters.AddWithValue("_ReqId", ViewModel.ReqId);
                    cmd.Parameters.AddWithValue("_LastUpdate", DateTime.Now);
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
                    //cmd.Parameters.Add("@_zinc_order_log_detail_id", MySqlDbType.Int32, 500);
                    //cmd.Parameters["@_zinc_order_log_detail_id"].Direction = ParameterDirection.Output;
                    cmd.ExecuteNonQuery();
                    //zincOrderLogDetailID = Convert.ToInt32(cmd.Parameters["@_zinc_order_log_detail_id"].Value);
                }
            }
            catch (Exception ex)
            {
            }
            return zincOrderLogDetailID;
        }
        //public ProductOrderIDModelforWebhooks GetLogid(string OurInternalID)
        //{
        //    ProductOrderIDModelforWebhooks model = null;

        //    using (MySqlConnection conn = new MySqlConnection(connStr))
        //    {
        //        conn.Open();
        //        MySqlCommand cmdd = new MySqlCommand(@"SELECT zinc_order_log_id, request_id FROM bestBuyE2.ZincOrderLog where sc_order_id =" + OurInternalID, conn);
        //        cmdd.CommandType = System.Data.CommandType.Text;

        //        using (var reader = cmdd.ExecuteReader())
        //        {
        //            if (reader.HasRows)
        //            {
        //                while (reader.Read())
        //                {
        //                    model = new ProductOrderIDModelforWebhooks();
        //                    model.request_id = Convert.ToString(reader["request_id"]);
        //                    model.product_order_log_id = Convert.ToInt32(reader["product_order_log_id"]);
        //                }

        //            }
        //        }

        //    }


        //    return model;
        //}

        public SendToZincProductViewModel GetZincProduct()
        {
            SendToZincProductViewModel model = new SendToZincProductViewModel();
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("P_GetAllFromSendToZincProduct", conn);

                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                   // cmd.Parameters.AddWithValue("ID", id);
                    MySqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            model.Asin = Convert.ToString(reader["_Asin"] != DBNull.Value ? reader["_Asin"] : "");
                            model.Sku = Convert.ToString(reader["_Sku"] != DBNull.Value ? reader["_Sku"] : "");
                            model.Shipdays = Convert.ToString(reader["_ShipDays"] != DBNull.Value ? reader["_ShipDays"] : "");
                            model.AccountDetail = Convert.ToString(reader["_AccountDetail"] != DBNull.Value ? reader["_AccountDetail"] : "");
                            
                            model.CreditCardId = Convert.ToInt32(reader["_CreditCardDetail"] != DBNull.Value ? reader["_CreditCardDetail"] : "");

                           
                            
                            model._17Tracking = Convert.ToString(reader["_17_tracking"] != DBNull.Value ? reader["_17_tracking"] : "");

                            model.OrderDatetime = Convert.ToDateTime(reader["_order_datetime"] != DBNull.Value ? reader["_order_datetime"] : "");

                            model.ZincOrderStatusInternal = Convert.ToString(reader["_zinc_order_status_internal"] != DBNull.Value ? reader["_zinc_order_status_internal"] : "");

                            model.MerchantOrderId = Convert.ToString(reader["_merchant_order_id"] != DBNull.Value ? reader["_merchant_order_id"] : "");

                            model.Address1 = Convert.ToString(reader["_AddressLine1"] != DBNull.Value ? reader["_AddressLine1"] : "");

                            model.Address2 = Convert.ToString(reader["_AddressLine2"] != DBNull.Value ? reader["_AddressLine2"] : "");

                            model.PostalCode = Convert.ToString(reader["_PostalCode"] != DBNull.Value ? reader["_PostalCode"] : "");

                            model.City = Convert.ToString(reader["_City"] != DBNull.Value ? reader["_City"] : "");

                            model.State = Convert.ToString(reader["_State"] != DBNull.Value ? reader["_State"] : "");

                            model.Phone = Convert.ToString(reader["_Phone"] != DBNull.Value ? reader["_Phone"] : "");

                            model.Country = Convert.ToString(reader["_Country"] != DBNull.Value ? reader["_Country"] : "");

                            model.FirstName = Convert.ToString(reader["_FirstName"] != DBNull.Value ? reader["_FirstName"] : "");

                            model.LastName = Convert.ToString(reader["_LastName"] != DBNull.Value ? reader["_LastName"] : "");

                            model.Qty = Convert.ToInt32(reader["_Qty"] != DBNull.Value ? reader["_Qty"] : 0);

                            model.ReqId = Convert.ToString(reader["_ReqId"] != DBNull.Value ? reader["_ReqId"] : "");

                            model.Code = Convert.ToString(reader["_order_code"] != DBNull.Value ? reader["_order_code"] : "");

                            model.Message = Convert.ToString(reader["_order_message"] != DBNull.Value ? reader["_order_message"] : "");

                            model.Type = Convert.ToString(reader["_order_type"] != DBNull.Value ? reader["_order_type"] : "");

                            model.ZincOrderLogID = Convert.ToInt32(reader["_zinc_order_log_id"] != DBNull.Value ? reader["_zinc_order_log_id"] : "");

                            model.Data = Convert.ToString(reader["_order_data"] != DBNull.Value ? reader["_order_data"] : "");

                            model.ShppingDate = Convert.ToString(reader["_shpping_date"] != DBNull.Value ? reader["_shpping_date"] : "");

                            model.TrackingNumber = Convert.ToString(reader["_tracking_number"] != DBNull.Value ? reader["_tracking_number"] : "");

                            model.Carrier = Convert.ToString(reader["_carrier"] != DBNull.Value ? reader["_carrier"] : "");

                            model.AmazonTracking = Convert.ToString(reader["_amazon_tracking"] != DBNull.Value ? reader["_amazon_tracking"] : "");
                           
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return model;
        }
    }
}
