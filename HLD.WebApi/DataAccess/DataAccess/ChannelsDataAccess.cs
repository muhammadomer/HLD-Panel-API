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
   public class ChannelsDataAccess
    {
        public string connStr { get; set; }
        public ChannelsDataAccess(IConnectionString connectionString)
        {
            connStr = connectionString.GetConnectionString();
        }
        // Save Cred
        public bool UpdateChannelsCred(UpdateChannelsViewModel ViewModel)
        {
            bool status = false;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("P_UpdateChennelCredendials", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_Method", ViewModel.Method);
                    cmd.Parameters.AddWithValue("_Key", ViewModel.Key);
                    cmd.Parameters.AddWithValue("_UserName", ViewModel.UserName);
                    cmd.Parameters.AddWithValue("_KeyShorts", ViewModel.KeyShort);
                    cmd.Parameters.AddWithValue("_UserNameShorts", ViewModel.UserNameShort);
                    cmd.Parameters.AddWithValue("_Password", ViewModel.password);
                    cmd.Parameters.AddWithValue("_PasswordShorts", ViewModel.passwordShort);
                    cmd.ExecuteNonQuery();
                    status = true;
                }
            }
            catch (Exception ex)
            {
            }
            return status;
        }

        // Get LOgs 
        public List<ChannelLogs> GetChannelsLogs(string method)
        {
            List<ChannelLogs> listModel = null;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmdd = new MySqlCommand(@"SELECT * FROM bestBuyE2.ChennelsCredentilalsLogs Where Method="+"'"+method+"'"+ "Order by LastUpdateDate desc;", conn);
                    cmdd.CommandType = System.Data.CommandType.Text;
                    MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter(cmdd);
                    DataTable dt = new DataTable();
                    mySqlDataAdapter.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        listModel = new List<ChannelLogs>();
                        foreach (DataRow dr in dt.Rows)
                        {
                            ChannelLogs model = new ChannelLogs();
                            model.LastUpdate = Convert.ToString(dr["LastUpdateDate"]);
                            model.User = Convert.ToString(dr["User"]);
                            model.Key = Convert.ToString(dr["Key"]);
                            model.UserName = Convert.ToString(dr["UserName"]);
                            model.password = Convert.ToString(dr["Password"]);
                           
                            listModel.Add(model);
                        }
                    }


                }
                return listModel;
            }
            catch (Exception)
            {

                throw;
            }

        }
    }
}
