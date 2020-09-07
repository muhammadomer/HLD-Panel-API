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
    public class ZincAccountsDataAccess
    {
        public string ConStr { get; set; }
        public ZincAccountsDataAccess(IConnectionString connectionString)
        {

            ConStr = connectionString.GetConnectionString();
        }
        public bool Save(ZincAccountsViewModel ViewModel)
        {
            bool status = false;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(ConStr))
                {
                    conn.Open();
                    MySqlCommand cmdd = new MySqlCommand("p_SaveZincAccounts", conn);
                    cmdd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmdd.Parameters.AddWithValue("_UserName", ViewModel.UserName);
                    cmdd.Parameters.AddWithValue("_Password", ViewModel.Password);
                    cmdd.Parameters.AddWithValue("_Key", ViewModel.Key);
                    cmdd.Parameters.AddWithValue("_UserNameShort", ViewModel.UserNameShort);
                    cmdd.Parameters.AddWithValue("_PasswordShort", ViewModel.PasswordShort);
                    cmdd.Parameters.AddWithValue("_KeyShort", ViewModel.KeyShort);
                    cmdd.Parameters.AddWithValue("_User", ViewModel.User);
                    cmdd.Parameters.AddWithValue("_LastUpdatedDate", ViewModel.LastUpdatedDate);

                    cmdd.ExecuteNonQuery();
                    status = true;
                }
            }
            catch (Exception ex)
            {

            }
            return status;
        }

        public List<ZincAccountsViewModel> GetCreditCardsList()
        {
            List<ZincAccountsViewModel> list = new List<ZincAccountsViewModel>();
            try
            {
                using (MySqlConnection conn = new MySqlConnection(ConStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_GetZincAccounts", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    DataTable dt = new DataTable();
                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    da.Fill(dt);
                    foreach (DataRow reader in dt.Rows)
                    {
                        ZincAccountsViewModel viewModel = new ZincAccountsViewModel
                        {
                            ZincAccountsId = Convert.ToInt32(reader["ZincAccountsId"]),
                            UserNameShort = reader["UserName"] != DBNull.Value ? (string)reader["UserName"] : "",
                            KeyShort = reader["Key"] != DBNull.Value ? (string)reader["Key"] : "",
                            PasswordShort = reader["Password"] != DBNull.Value ? (string)reader["Password"] : "",
                            LastUpdatedDate = reader["LastUpdatedDate"] != DBNull.Value ? Convert.ToDateTime(reader["LastUpdatedDate"]) : DateTime.MinValue,
                            IsActive = reader["IsActive"] != DBNull.Value ? Convert.ToBoolean(reader["IsActive"]) : false,
                            IsDefault = reader["IsDefault"] != DBNull.Value ? Convert.ToBoolean(reader["IsDefault"]) : false,
                        };
                        list.Add(viewModel);
                    }
                    conn.Close();
                }
            }
            catch (Exception ex)
            {

            }
            return list;
        }

        public ZincAccountsViewModel GetAccountDetailById(int Id)
        {
            ZincAccountsViewModel model = new ZincAccountsViewModel();
            try
            {
                using (MySqlConnection conn = new MySqlConnection(ConStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_GetAccountDeatilById", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_Id", Id);
                    DataTable dt = new DataTable();
                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    da.Fill(dt);
                    foreach (DataRow reader in dt.Rows)
                    {
                        ZincAccountsViewModel viewModel = new ZincAccountsViewModel
                        {
                            ZincAccountsId = Convert.ToInt32(reader["idZincAccounts"]),
                            UserName = reader["UserName"] != DBNull.Value ? (string)reader["UserName"] : "",
                            Key = reader["Key"] != DBNull.Value ? (string)reader["Key"] : "",
                            Password = reader["Password"] != DBNull.Value ? (string)reader["Password"] : "",
                            //LastUpdatedDate = reader["LastUpdatedDate"] != DBNull.Value ? Convert.ToDateTime(reader["LastUpdatedDate"]) : DateTime.MinValue,
                            IsActive = reader["IsActive"] != DBNull.Value ? Convert.ToBoolean(reader["IsActive"]) : false,
                            IsDefault = reader["IsDefault"] != DBNull.Value ? Convert.ToBoolean(reader["IsDefault"]) : false,
                        };
                        model = viewModel;
                    }
                    conn.Close();
                }
            }
            catch (Exception ex)
            {

            }
            return model;
        }

        public int UpdateIsActive(ZincAccountsViewModel Obj)
        {
            int Id = 0;
            try
            {
                if (Obj.IsDefault == true)
                {
                    Obj.IsActive = true;
                }
                using (MySqlConnection conn = new MySqlConnection(ConStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_UpdateZAActiveState", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_Id", Obj.ZincAccountsId);
                    cmd.Parameters.AddWithValue("_IsActive", Obj.IsActive);
                    cmd.Parameters.AddWithValue("_Date", Obj.LastUpdatedDate);
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception exp)
            {
            }
            return Id;
        }
        public int UpdateIsDefault(ZincAccountsViewModel Obj)
        {
            int Id = 0;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(ConStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_UpdateZADefaultState", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_Id", Obj.ZincAccountsId);
                    cmd.Parameters.AddWithValue("_IsDefault", Obj.IsDefault);
                    cmd.Parameters.AddWithValue("_Date", Obj.LastUpdatedDate);
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception exp)
            {

            }
            return Id;
        }
    }
}
