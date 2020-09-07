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
    public class CatageorySub3DataAccess
    {
        public string connStr { get; set; }
        public CatageorySub3DataAccess(IConnectionString connectionString)
        {
            connStr = connectionString.GetConnectionString();
        }

        public List<CatageorySub3ViewModel> GetAllCatageorySub3_BySub2ID(int id)
        {
            List<CatageorySub3ViewModel> listCatageorySub3ViewModel = null;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_GetAllCatageory_Sub3_By_CS2", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("cs2", id);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            listCatageorySub3ViewModel = new List<CatageorySub3ViewModel>();
                            while (reader.Read())
                            {
                                CatageorySub3ViewModel catageoryViewModel = new CatageorySub3ViewModel();
                                catageoryViewModel.CatageorySub2Id = Convert.ToInt32(reader["cs2_id"]);
                                catageoryViewModel.CatageorySub3Name = Convert.ToString(reader["cs3_name"]);
                                catageoryViewModel.CatageorySub3Id = Convert.ToInt32(reader["cs3_id"]);
                                listCatageorySub3ViewModel.Add(catageoryViewModel);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return listCatageorySub3ViewModel;
        }

        public  CatageorySub3ViewModel  GetAllCatageorySub3_ByID(int id)
        {
              CatageorySub3ViewModel catageoryViewModel = null;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_GetCatageory_Sub3_ById", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("cs3Id", id);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            catageoryViewModel =new CatageorySub3ViewModel();
                            while (reader.Read())
                            {
                                 
                                catageoryViewModel.CatageorySub2Id = Convert.ToInt32(reader["cs2_id"]);
                                catageoryViewModel.CatageorySub3Name = Convert.ToString(reader["cs3_name"]);
                                catageoryViewModel.CatageorySub3Id = Convert.ToInt32(reader["cs3_id"]);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return catageoryViewModel;
        }
        public bool SaveCatageorySub3(CatageorySub3ViewModel ViewModel)
        {
            bool status = false;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_SaveCatageorySub3", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("cs2_id", ViewModel.CatageorySub2Id);
                    cmd.Parameters.AddWithValue("cs3_Name", ViewModel.CatageorySub3Name);
                    
                    cmd.ExecuteNonQuery();
                    status = true;
                }
            }
            catch (Exception ex)
            {
            }
            return status;
        }

        public bool UpdateCatageorySub3(CatageorySub3ViewModel ViewModel)
        {
            bool status = false;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_UpdateCatageorySub3", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("cs3id", ViewModel.CatageorySub3Id);
                    cmd.Parameters.AddWithValue("cs3name", ViewModel.CatageorySub3Name);
                    cmd.Parameters.AddWithValue("cs2id", ViewModel.CatageorySub2Id);
                    cmd.ExecuteNonQuery();
                    status = true;
                }
            }
            catch (Exception ex)
            {
            }
            return status;
        }

        public bool DeleteCatageorySub3(int id)
        {
            bool status = false;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_DeleteCategorySub3", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("cs3id", id);
                    cmd.ExecuteNonQuery();
                    status = true;
                }
            }
            catch (Exception ex)
            {
            }
            return status;
        }

        public bool CheckCategorySub3Exists(string name)
        {
            bool status = false;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_CheckCategorySub3Exists", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("CategorySub3Name", name.Trim());
                    cmd.Parameters.Add("Statues", MySqlDbType.Bit, 10);
                    cmd.Parameters["Statues"].Direction = System.Data.ParameterDirection.Output;

                    cmd.ExecuteNonQuery();
                    status = Convert.ToBoolean(cmd.Parameters["Statues"].Value);
                }
            }
            catch (Exception ex)
            {
            }
            return status;
        }
    }
}
