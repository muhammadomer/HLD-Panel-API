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
    public class BestBuyTrackingExportDataAccess
    {
        public string connStr { get; set; }
        public BestBuyTrackingExportDataAccess(IConnectionString connectionString)
        {
            connStr = connectionString.GetPhpConnectionString();
        }

        public List<UpdateTrackingBestbuyViewModel> GetDataUpdateTracking()
        {
            List<UpdateTrackingBestbuyViewModel> listModel = null;
            bool status = false;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(@"SELECT `trackingExport`.`bbe2_tracking_id`,`trackingExport`.`bbOrderID`,`trackingExport`.`trackingNumber`,`trackingExport`.`shippingServiceCode` FROM `bestBuyE2`.`trackingExport` where inBestbuy = 0; ", conn);
                    cmd.CommandType = System.Data.CommandType.Text;
                    MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    mySqlDataAdapter.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        listModel = new List<UpdateTrackingBestbuyViewModel>();
                        foreach (DataRow dr in dt.Rows)
                        {
                            UpdateTrackingBestbuyViewModel model = new UpdateTrackingBestbuyViewModel();
                            model.bbOrderID = Convert.ToString(dr["bbOrderID"] != DBNull.Value ? dr["bbOrderID"].ToString() : "");
                            model.trackingNumber = Convert.ToString(dr["trackingNumber"] != DBNull.Value ? dr["trackingNumber"].ToString() : "");
                            model.bbe2TrackingId = Convert.ToInt32(dr["bbe2_tracking_id"] != DBNull.Value ? dr["bbe2_tracking_id"].ToString() : "");
                            model.shippingServiceCode = Convert.ToString(dr["shippingServiceCode"] != DBNull.Value ? dr["shippingServiceCode"].ToString() : "");
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

        public bool SaveDataUpdateTracking(List<UpdateTrackingBestbuyViewModel> ViewModel)
        {

            bool status = false;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    foreach (var list in ViewModel)
                    {

                       
                        MySqlCommand cmd = new MySqlCommand("p_SaveBestBuyTrackingExport", conn);
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("_scOrderID", list.scOrderID);
                        cmd.Parameters.AddWithValue("_bbOrderID", list.bbOrderID);
                        cmd.Parameters.AddWithValue("_trackingNumber", list.trackingNumber);
                        cmd.Parameters.AddWithValue("_shippingServiceCode", list.shippingServiceCode);
                        cmd.Parameters.AddWithValue("_shippingMethodName", list.shippingMethodName);
                        cmd.Parameters.AddWithValue("_shippingCost", list.shippingCost);
                        cmd.Parameters.AddWithValue("_declaredValue", list.declaredValue);
                        cmd.Parameters.AddWithValue("_shipDate", list.shipDate);
                        cmd.Parameters.AddWithValue("_estimatedDeliveryDate", list.estimatedDeliveryDate);
                        cmd.Parameters.AddWithValue("_packageWeight", list.packageWeight);
                        cmd.Parameters.AddWithValue("_packageID", list.packageID);
                        cmd.Parameters.AddWithValue("_inBestbuy", 0);

                        cmd.ExecuteNonQuery();
                        

                    }
                    status = true;
                }
            }
            catch (Exception ex)
            {
            }
            return status;
        }

        public bool UpdateTracking(int trakingid)
        {
            bool status = false;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(@"UPDATE `bestBuyE2`.`trackingExport` SET inBestbuy = 1 WHERE `trackingExport`.`bbe2_tracking_id`= " + trakingid, conn);
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.ExecuteNonQuery();
                    status = true;
                }
               
            }
            catch (Exception)
            {

                throw;
            }
            return status;
        }

        public List<int> GetSCIdNotupdated(string scOrderIds)
        {
            List<int> sclist = null;
            bool status = false;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("P_CheckSCIDforTrackingExport", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("SCId", scOrderIds);
                    MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    mySqlDataAdapter.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        sclist = new List<int>();
                        foreach (DataRow dr in dt.Rows)
                        {
                            int scid;
                            scid = Convert.ToInt32(dr["scOrderID"].ToString());
                            sclist.Add(scid);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return sclist;
        }
    }




}
