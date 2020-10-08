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
   public class ShipmentCourierDataAccess
    {
        public string ConStr { get; set; }
        public ShipmentCourierDataAccess(IConnectionString connectionString)
        {

            ConStr = connectionString.GetConnectionString();
        }
        public bool SaveAndEditShipmentCourier(SaveAndEditShipmentCourierVM ViewModel)
        {
            bool status = false;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(ConStr))
                {
                    conn.Open();
                    MySqlCommand cmdd = new MySqlCommand("P_SaveAndEditShipmentCourier", conn);
                    cmdd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmdd.Parameters.AddWithValue("_CourierId", ViewModel.ShipmentCourier_ID);
                    cmdd.Parameters.AddWithValue("_CourierCode", ViewModel.CourierCode);
                    cmdd.Parameters.AddWithValue("_CourierURL", ViewModel.CourierURL);
                    cmdd.ExecuteNonQuery();
                    status = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return status;
        }
        

        public List<SaveAndEditShipmentCourierVM> GetShipmentCourierList()
        {
            List<SaveAndEditShipmentCourierVM> list = new List<SaveAndEditShipmentCourierVM>();
            try
            {
                using (MySqlConnection conn = new MySqlConnection(ConStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("P_GetAllShipmentCourier", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    DataSet ds = new DataSet();
                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    da.Fill(ds);
                    DataTable dt = ds.Tables[0];
                    foreach (DataRow reader in dt.Rows)
                    {
                        SaveAndEditShipmentCourierVM viewModel = new SaveAndEditShipmentCourierVM
                        {
                            ShipmentCourier_ID = reader["ShipmentCourier_ID"] != DBNull.Value ? (int)reader["ShipmentCourier_ID"] : 0,
                            CourierCode = reader["CourierCode"] != DBNull.Value ? (string)reader["CourierCode"] : "",
                            CourierURL = reader["CourierURL"] != DBNull.Value ? (string)reader["CourierURL"] : "",
                        };
                        list.Add(viewModel);
                    }
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return list;
        }

        public SaveAndEditShipmentCourierVM GetShipmentCourierById(int id)
        {
            SaveAndEditShipmentCourierVM model = new SaveAndEditShipmentCourierVM();
            try
            {
                using (MySqlConnection conn = new MySqlConnection(ConStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("P_GetShipmentCourierById", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_CourierId", id);
                    MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter(cmd);
                    cmd.ExecuteNonQuery();
                    DataTable dt = new DataTable();
                    mySqlDataAdapter.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {

                        foreach (DataRow dr in dt.Rows)
                        {
                            SaveAndEditShipmentCourierVM modelview = new SaveAndEditShipmentCourierVM();
                            modelview.ShipmentCourier_ID = dr["ShipmentCourier_ID"] != DBNull.Value ? (int)dr["ShipmentCourier_ID"] : 0;

                            modelview.CourierCode = dr["CourierCode"] != DBNull.Value ? (string)dr["CourierCode"] : "";
                            modelview.CourierURL = dr["CourierURL"] != DBNull.Value ? (string)dr["CourierURL"] : "";
                            model = modelview;
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return model;
        }
    }
}
