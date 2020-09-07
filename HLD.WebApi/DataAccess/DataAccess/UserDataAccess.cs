using DataAccess.EntityModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MySql.Data.MySqlClient;
using DataAccess.Helper;

using DataAccess.ViewModels;

namespace DataAccess.DataAccess
{
    public class UserDataAccess
    {
        public string connstr { get; set; }
        public UserDataAccess(IConnectionString connString)
        {
            connstr= connString.GetConnectionString();
        }

        public AuthenticateViewModel AuthenticateUser(string userName, string password,string Method= "hldpanel")
        {
            AuthenticateViewModel authenticateViewModel = null;
            using (MySqlConnection conn = new MySqlConnection(connstr))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("p_AuthenticateUser_configuration", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("UserName", userName.Trim());
                cmd.Parameters.AddWithValue("Pass", password.Trim());
                cmd.Parameters.AddWithValue("Method", Method.Trim());
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        authenticateViewModel = new AuthenticateViewModel();
                        while (reader.Read())
                        {
                            authenticateViewModel.Id = Convert.ToInt32(reader["id"]);
                            authenticateViewModel.Method = Convert.ToString(reader["password"]);
                            authenticateViewModel.Username = Convert.ToString(reader["username"]);
                        }
                    }
                }
            }
            return authenticateViewModel;
        }
        public AuthenticateViewModel AuthenticateUser_Managed(string userName, string password)
        {
            AuthenticateViewModel authenticateViewModel = null;
            using (MySqlConnection conn = new MySqlConnection(connstr))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("P_AuthenticateUser", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("_UserName", userName.Trim());
                cmd.Parameters.AddWithValue("_Password", password.Trim());
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        authenticateViewModel = new AuthenticateViewModel();
                        while (reader.Read())
                        {
                            authenticateViewModel.Method = Convert.ToString(reader["Email"]);
                            authenticateViewModel.Username = Convert.ToString(reader["Id"]);
                        }
                    }
                }
            }
            return authenticateViewModel;
        }
        public AuthenticateViewModel GetUserById(int UserId)
        {
            AuthenticateViewModel authenticateViewModel = null;
            using (MySqlConnection conn = new MySqlConnection(connstr))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("p_GetUserById_configuration", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("UserId", UserId);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        authenticateViewModel = new AuthenticateViewModel();
                        while (reader.Read())
                        {
                            authenticateViewModel.Id = Convert.ToInt32(reader["id"]);
                            authenticateViewModel.Method = Convert.ToString(reader["password"]);
                            authenticateViewModel.Username = Convert.ToString(reader["username"]);
                        }
                    }
                }
            }
            return authenticateViewModel;
        }
    }
}
