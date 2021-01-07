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
    public class ZincProxyDataAccess
    {
        public string connStr { get; set; }

        public ZincProxyDataAccess(IConnectionString connectionString)
        {
            connStr = connectionString.GetConnectionString();

        }

        public bool SaveProxy(ZincProxyViewModel viewModel)
        {
            bool status = false;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("P_SaveZincProxy", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_ZincProxyUser", viewModel.ProxyUser);
                    cmd.Parameters.AddWithValue("_ZincProxyPass", viewModel.ProxyPassword);
                    cmd.Parameters.AddWithValue("_ZincProxyEmail", viewModel.ProxyEmail);
                    cmd.Parameters.AddWithValue("_ZincProxyPort", viewModel.ProxyPort);
                    cmd.Parameters.AddWithValue("_ProxyIP", viewModel.ProxyIP);
                    cmd.Parameters.AddWithValue("_ProxyPassShort", viewModel.ProxyPasswordShort);
                    cmd.ExecuteNonQuery();
                    status = true;
                    conn.Close();
                }
            }
            catch (Exception exp)
            {
            }
            return status;
        }


        public List<GetZincProxyViewModel> GetProxyList()
        {
            List<GetZincProxyViewModel> listviewModel = new List<GetZincProxyViewModel>();
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("P_GetProxyList", conn);

                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {

                            while (reader.Read())
                            {
                                GetZincProxyViewModel ViewModel = new GetZincProxyViewModel();
                                ViewModel.idZincProxy = Convert.ToInt32(reader["idZincProxy"]);
                                ViewModel.Status = Convert.ToInt32(reader["Status"]);
                                ViewModel.ProxyUser = Convert.ToString(reader["ZincProxyUser"]);
                                ViewModel.ProxyPort = Convert.ToString(reader["ZincProxyPort"]);
                                ViewModel.ProxyIP = Convert.ToString(reader["ZincProxyIP"]);
                                ViewModel.Isactive = Convert.ToBoolean(reader["IsActive"]);
                                ViewModel.ProxyPasswordShort = Convert.ToString(reader["ProxyPassShort"]);
                                ViewModel.ProxyEmail = Convert.ToString(reader["ZincProxyEmail"]);
                                ViewModel.LatUpdated = Convert.ToDateTime(reader["LastUpdatedDate"]);
                                ViewModel.IsDefault = Convert.ToBoolean(reader["IsDefault"]);

                                listviewModel.Add(ViewModel);
                            }
                        }
                    }
                }
            }
            catch (Exception exp)
            {
            }
            return listviewModel;
        }

        public int UpdateIsActive(ProxySettingViewModal Obj)
        {
            int Id = 0;
            try
            {

                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("P_UpdateZincProxyActive", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_Id", Obj.idZincProxy);
                    cmd.Parameters.AddWithValue("_IsActive", Obj.Isactive);

                    cmd.ExecuteNonQuery();
                    conn.Close();
                    Id = 1;
                }
            }
            catch (Exception exp)
            {
            }
            return Id;
        }
        public int UpdateIsDefault(ProxySettingViewModal Obj)
        {
            int Id = 0;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("P_UpdateProxyAsDefault", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_Id", Obj.idZincProxy);
                    cmd.Parameters.AddWithValue("_IsDefault", Obj.IsDefault);

                    cmd.ExecuteNonQuery();
                    conn.Close();
                    Id = 1;
                }
            }
            catch (Exception exp)
            {

            }
            return Id;
        }

        public bool DeleteProxy(int id)
        {
            bool status = false;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_DeleteProxyById", conn);
                    cmd.Parameters.AddWithValue("_id", id);
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

        public GetZincProxyViewModel GetProxyDefaultForZinc(string email )
        {
            GetZincProxyViewModel ViewModel = new GetZincProxyViewModel();
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("P_GetProxyDefaultForZinc", conn);
                    cmd.Parameters.AddWithValue("_email", email);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {

                            while (reader.Read())
                            {

                                ViewModel.idZincProxy = Convert.ToInt32(reader["idZincProxy"]);
                                ViewModel.ProxyUser = Convert.ToString(reader["ZincProxyUser"]);
                                ViewModel.ProxyPort = Convert.ToString(reader["ZincProxyPort"]);
                                ViewModel.ProxyIP = Convert.ToString(reader["ZincProxyIP"]);
                                ViewModel.Isactive = Convert.ToBoolean(reader["IsActive"]);
                                ViewModel.ProxyPassword = Convert.ToString(reader["ZincProxyPass"]);
                                ViewModel.ProxyEmail = Convert.ToString(reader["ZincProxyEmail"]);
                                ViewModel.IsDefault = Convert.ToBoolean(reader["IsDefault"]);


                            }
                        }
                    }
                }
            }
            catch (Exception exp)
            {
            }
            return ViewModel;
        }

        public bool SaveProxyEmail(SaveZincProxyEmailViewModel viewModel)
        {
            bool status = false;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_SaveProxyEmail", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_email", viewModel.ProxyEmail);

                    cmd.ExecuteNonQuery();
                    status = true;
                    conn.Close();
                }
            }
            catch (Exception exp)
            {
            }
            return status;
        }
        public List<SaveZincProxyEmailViewModel> GetZincProxyEmail()
        {
            List<SaveZincProxyEmailViewModel> listviewModel = new List<SaveZincProxyEmailViewModel>();
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_GetZincProxyEmail", conn);

                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {

                            while (reader.Read())
                            {
                                SaveZincProxyEmailViewModel ViewModel = new SaveZincProxyEmailViewModel();
                                ViewModel.idGetProxyEmail = Convert.ToInt32(reader["idGetProxyEmail"]);
                                ViewModel.ProxyEmail = Convert.ToString(reader["ProxyEmail"]);

                                listviewModel.Add(ViewModel);
                            }
                        }
                    }
                }
            }
            catch (Exception exp)
            {
            }
            return listviewModel;
        }

        public bool DeleteProxyEmail(int id)
        {
            bool status = false;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_DeleteProxyEmailById", conn);
                    cmd.Parameters.AddWithValue("_id", id);
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
        public int UpdateZincProxyEmail(int statusCode, string email)
        {
            int Id = 0;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_UpdateProxyStatus", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_status", statusCode);
                    cmd.Parameters.AddWithValue("_ZincProxyEmail",email);

                    cmd.ExecuteNonQuery();
                    conn.Close();
                    Id = 1;
                }
            }
            catch (Exception exp)
            {

            }
            return Id;
        }

    }
}
