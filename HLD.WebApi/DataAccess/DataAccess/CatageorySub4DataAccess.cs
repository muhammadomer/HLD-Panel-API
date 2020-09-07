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
    public class CatageorySub4DataAccess
    {
        public string connStr { get; set; }
        public CatageorySub4DataAccess(IConnectionString connectionString)
        {
            connStr = connectionString.GetConnectionString();
        }

        public List<CatageorySub4ViewModel> GetAllCatageorySub4_BySub3ID(int id)
        {
            List<CatageorySub4ViewModel> listCatageorySub4ViewModel = null;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_GetAllCatageory_Sub4_By_CS3", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("cs3", id);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            listCatageorySub4ViewModel = new List<CatageorySub4ViewModel>();
                            while (reader.Read())
                            {
                                CatageorySub4ViewModel catageoryViewModel = new CatageorySub4ViewModel();
                                catageoryViewModel.CatageorySub3Id = Convert.ToInt32(reader["cs3_id"]);
                                catageoryViewModel.CatageorySub4Name = Convert.ToString(reader["cs4_name"]);
                                catageoryViewModel.CatageorySub4Id = Convert.ToInt32(reader["cs4_id"]);
                                listCatageorySub4ViewModel.Add(catageoryViewModel);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return listCatageorySub4ViewModel;
        }

        public  CatageorySub4ViewModel  GetAllCatageorySub4_ByID(int id)
        {
             CatageorySub4ViewModel catageoryViewModel = null;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_GetCatageory_Sub4_ById", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("cs4Id", id);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            catageoryViewModel = new CatageorySub4ViewModel();
                            while (reader.Read())
                            {
                                
                                catageoryViewModel.CatageorySub3Id = Convert.ToInt32(reader["cs3_id"]);
                                catageoryViewModel.CatageorySub4Name = Convert.ToString(reader["cs4_name"]);
                                catageoryViewModel.CatageorySub4Id = Convert.ToInt32(reader["cs4_id"]);
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

        public bool SaveCatageorySub4(CatageorySub4ViewModel ViewModel)
        {
            bool status = false;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_SaveCatageorySub4", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("cs3_id", ViewModel.CatageorySub3Id);
                    cmd.Parameters.AddWithValue("cs4_name", ViewModel.CatageorySub4Name);

                    cmd.ExecuteNonQuery();
                    status = true;
                }
            }
            catch (Exception ex)
            {
            }
            return status;
        }

        public bool UpdateCatageorySub4(CatageorySub4ViewModel ViewModel)
        {
            bool status = false;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_UpdateCatageorySub4", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("cs4id", ViewModel.CatageorySub4Id);
                    cmd.Parameters.AddWithValue("cs4name", ViewModel.CatageorySub4Name);
                    cmd.Parameters.AddWithValue("cs3id", ViewModel.CatageorySub3Id);
                    cmd.ExecuteNonQuery();
                    status = true;
                }
            }
            catch (Exception ex)
            {
            }
            return status;
        }

        public bool DeleteCatageorySub4(int id)
        {
            bool status = false;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_DeleteCategorySub4", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("cs4Id", id);
                    cmd.ExecuteNonQuery();
                    status = true;
                }
            }
            catch (Exception ex)
            {
            }
            return status;
        }

        public bool CheckCategorySub4Exists(string name)
        {
            bool status = false;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_CheckCategorySub4Exists", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("CategorySub4Name", name.Trim());
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
