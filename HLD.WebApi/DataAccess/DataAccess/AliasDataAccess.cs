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
    public class AliasDataAccess
    {
        public string connStr { get; set; }
        public AliasDataAccess(IConnectionString connectionString)
        {
            connStr = connectionString.GetConnectionString();
        }
        public bool SaveAlias(AliasViewModel aliasViewModel)
        {
            bool status = false;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_SaveAlias", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("AliasName", aliasViewModel.AliaName);
                    cmd.ExecuteNonQuery();
                    status = true;
                }
            }
            catch (Exception ex)
            {
            }
            return status;
        }

        public bool UpdateAlias(AliasViewModel aliasViewModel)
        {
            bool status = false;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_UpdateAlias", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("AliasId", aliasViewModel.AliaseID);
                    cmd.Parameters.AddWithValue("AliasName", aliasViewModel.AliaName);
                    cmd.ExecuteNonQuery();
                    status = true;
                }
            }
            catch (Exception ex)
            {
            }
            return status;
        }

        public List<AliasViewModel> GetAllAlias()
        {
            List<AliasViewModel> listAliasViewModel = null;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_GetAllAlias", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            listAliasViewModel = new List<AliasViewModel>();
                            while (reader.Read())
                            {
                                AliasViewModel aliasViewModel = new AliasViewModel();
                                aliasViewModel.AliaseID = Convert.ToInt32(reader["alias_id"]);
                                aliasViewModel.AliaName = Convert.ToString(reader["alias_name"]);
                                listAliasViewModel.Add(aliasViewModel);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return listAliasViewModel;
        }

        public AliasViewModel GetAliasById(int id)
        {
            AliasViewModel aliasViewModel = null;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_GetAliasById", conn);
                    cmd.Parameters.AddWithValue("AliasId", id);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            aliasViewModel = new AliasViewModel();
                            while (reader.Read())
                            {
                                aliasViewModel.AliaseID = Convert.ToInt32(reader["alias_id"]);
                                aliasViewModel.AliaName = Convert.ToString(reader["alias_name"]);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return aliasViewModel;
        }

        public bool DeleteAlias(int id)
        {
            bool status = false;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_DeleteAliasById", conn);
                    cmd.Parameters.AddWithValue("BrandId", id);
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
    }
}
