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
    public class ShipmentBoxDataAccess
    {
        public string ConStr { get; set; }
        public ShipmentBoxDataAccess(IConnectionString connectionString)
        {

            ConStr = connectionString.GetConnectionString();
        }

        public string SaveShipmentBox(ShipmentBoxViewModel ViewModel)
        {
            string Id = "";
            try
            {
                using (MySqlConnection conn = new MySqlConnection(ConStr))
                {
                    conn.Open();
                    MySqlCommand cmdd = new MySqlCommand("p_SaveShipmentBox", conn);
                    cmdd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmdd.Parameters.AddWithValue("_ShipmentId", ViewModel.ShipmentId);
                    cmdd.Parameters.AddWithValue("_Height", ViewModel.Height);
                    cmdd.Parameters.AddWithValue("_Width", ViewModel.Width);
                    cmdd.Parameters.AddWithValue("_Length", ViewModel.Length);
                    cmdd.Parameters.AddWithValue("_Weight", ViewModel.Weight);
                    Id = (string)cmdd.ExecuteScalar();
                }
            }
            catch (Exception ex)
            {

            }
            return Id;
        }

        public long GetShipmentBoxListCount(int VendorId)
        {
            long Counter = 0;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(ConStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_GetShipmentBoxListCount", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_VendorId", VendorId);
                    Counter = (long)cmd.ExecuteScalar();
                    conn.Close();
                }
            }
            catch (Exception exp)
            {
            }
            return Counter;
        }

        public List<ShipmentBoxListViewModel> GetShipmentBoxList(int VendorId, int Limit, int Offset)
        {
            List<ShipmentBoxListViewModel> list = new List<ShipmentBoxListViewModel>();
            try
            {
                using (MySqlConnection conn = new MySqlConnection(ConStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_GetShipmentBoxList", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_VendorId", VendorId);
                    cmd.Parameters.AddWithValue("_Limit", Limit);
                    cmd.Parameters.AddWithValue("_OffSet", Offset);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                ShipmentBoxListViewModel viewModel = new ShipmentBoxListViewModel
                                {
                                    IdShipmentBox = (int)reader["idShipmentBox"],
                                    ShipmentId = (string)reader["ShipmentId"],
                                    BoxId = reader["BoxId"] != DBNull.Value ? (string)reader["BoxId"] : "",
                                    //Vendor = reader["Vendor"] != DBNull.Value ? (string)reader["Vendor"] : "",
                                    //VendorId = (int)reader["VendorId"],
                                    //Status = reader["Status"] != DBNull.Value ? (string)reader["Status"] : "",
                                    //CreatedOn = (DateTime)reader["CreatedOn"],
                                    Width = reader["Width"] != DBNull.Value ? (Decimal)reader["Width"] : 0,
                                    Height = reader["Height"] != DBNull.Value ? (Decimal)reader["Height"] : 0,
                                    Length = reader["Length"] != DBNull.Value ? (Decimal)reader["Length"] : 0,
                                    Weight = reader["Weight"] != DBNull.Value ? (decimal)reader["Weight"] : 0,
                                };
                                list.Add(viewModel);
                            }
                        }
                    }
                    conn.Close();
                }
            }
            catch (Exception ex)
            {

            }
            return list;
        }

        public bool UpdateShipmentBox(ShipmentBoxViewModel ViewModel)
        {
            bool status = false;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(ConStr))
                {
                    conn.Open();
                    MySqlCommand cmdd = new MySqlCommand("p_UpdateShipmentBox", conn);
                    cmdd.CommandType = System.Data.CommandType.StoredProcedure;
                    //cmdd.Parameters.AddWithValue("_ShipmentId", ViewModel.ShipmentId);
                    //cmdd.Parameters.AddWithValue("_BoxId", ViewModel.BoxId);
                    cmdd.Parameters.AddWithValue("_Height", ViewModel.Height);
                    cmdd.Parameters.AddWithValue("_Width", ViewModel.Width);
                    cmdd.Parameters.AddWithValue("_Length", ViewModel.Length);
                    cmdd.Parameters.AddWithValue("_Weight", ViewModel.Weight);
                    cmdd.Parameters.AddWithValue("_Id", ViewModel.BoxId);
                    cmdd.ExecuteNonQuery();
                    status = true;
                }
            }
            catch (Exception ex)
            {

            }
            return status;
        }

        public bool DeleteBox(string ID)
        {
            bool status = false;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(ConStr))
                {
                    conn.Open();
                    MySqlCommand cmdd = new MySqlCommand("p_DeleteShipmentBox", conn);
                    cmdd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmdd.Parameters.AddWithValue("_Id", ID);
                    cmdd.ExecuteNonQuery();
                    status = true;
                    conn.Close();
                }
            }
            catch (Exception ex)
            {

            }
            return status;
        }
        public ShipmentBoxDetailViewModel GetBoxDetailById(string BoxId)
        {
            var Item = new ShipmentBoxDetailViewModel();
            try
            {
                using (MySqlConnection conn = new MySqlConnection(ConStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_GetShipmentBoxDetail", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_Id", BoxId);
                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                var Obj = new ShipmentBoxDetailViewModel()
                                {
                                    ShipmentId = (string)reader["ShipmentId"],
                                    BoxId = reader["BoxId"] != DBNull.Value ? (string)reader["BoxId"] : "",
                                    ShipmentName = (string)reader["ShipmentName"],
                                    Notes = reader["Notes"] != DBNull.Value ? (string)reader["Notes"] : "",
                                    Width = reader["Width"] != DBNull.Value ? (Decimal)reader["Width"] : 0,
                                    Height = reader["Height"] != DBNull.Value ? (Decimal)reader["Height"] : 0,
                                    Length = reader["Length"] != DBNull.Value ? (Decimal)reader["Length"] : 0,
                                    Weight = reader["Weight"] != DBNull.Value ? (decimal)reader["Weight"] : 0,
                                    TotalShipedQty = reader["TotalShipedQty"] != DBNull.Value ? Convert.ToInt32(reader["TotalShipedQty"]) : 0,
                                    TotalSKUs = reader["TotalSKUs"] != DBNull.Value ? Convert.ToInt32(reader["TotalSKUs"]) : 0,
                                    Status = reader["Status"] != DBNull.Value ? Convert.ToInt32(reader["Status"]) : 0,
                                    POs = reader["POs"] != DBNull.Value ? Convert.ToInt32(reader["POs"]) : 0,

                                };
                                Item = Obj;
                            }
                        }
                    }
                    conn.Close();
                }
            }
            catch (Exception ex)
            {

            }
            return Item;
        }

    }
}
