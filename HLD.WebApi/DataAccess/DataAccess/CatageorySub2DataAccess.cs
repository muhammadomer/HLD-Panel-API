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
    public class CatageorySub2DataAccess
    {
        public string connStr { get; set; }
        public CatageorySub2DataAccess(IConnectionString connectionString)
        {
            connStr = connectionString.GetConnectionString();
        }
        

              public  CatageorySub2ViewModel  GetAllCatageorySub2_ByID(int id)
        {
             CatageorySub2ViewModel catageoryViewModel = null;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_GetCatageory_Sub2_ById", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("cs2id", id);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            catageoryViewModel = new   CatageorySub2ViewModel();
                            while (reader.Read())
                            {
                                 catageoryViewModel = new CatageorySub2ViewModel();
                                catageoryViewModel.CatageorySub1Id = Convert.ToInt32(reader["cs1_id"]);
                                catageoryViewModel.CatageorySub2Name = Convert.ToString(reader["cs2_name"]);
                                catageoryViewModel.CatageorySub2Id = Convert.ToInt32(reader["cs2_id"]);
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

        public List<CatageorySub2ViewModel> GetAllCatageorySub2_BySub1ID(int id)
        {
            List<CatageorySub2ViewModel> listCatageorySub2ViewModel = null;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_GetAllCatageory_Sub2_By_CS1", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("cs1", id);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            listCatageorySub2ViewModel = new List<CatageorySub2ViewModel>();
                            while (reader.Read())
                            {
                                CatageorySub2ViewModel catageoryViewModel = new CatageorySub2ViewModel();
                                catageoryViewModel.CatageorySub1Id = Convert.ToInt32(reader["cs1_id"]);
                                catageoryViewModel.CatageorySub2Name = Convert.ToString(reader["cs2_name"]);
                                catageoryViewModel.CatageorySub2Id = Convert.ToInt32(reader["cs2_id"]);
                                listCatageorySub2ViewModel.Add(catageoryViewModel);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return listCatageorySub2ViewModel;
        }

        public bool SaveCatageoryMain(CatageoryMainViewModel ViewModel)
        {
            bool status = false;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_SaveCatageoryMain", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("CatageoryMainName", ViewModel.CatageoryMainName);
                    cmd.ExecuteNonQuery();
                    status = true;
                }
            }
            catch (Exception ex)
            {
            }
            return status;
        }

        public bool UpdateCatageoryMain(CatageoryMainViewModel ViewModel)
        {
            bool status = false;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_UpdateCatageoryMain", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("CatageoryId", ViewModel.CatageoryMainId);
                    cmd.Parameters.AddWithValue("CatageoryName", ViewModel.CatageoryMainName);
                    cmd.ExecuteNonQuery();
                    status = true;
                }
            }
            catch (Exception ex)
            {
            }
            return status;
        }

        public bool SaveCatageorySub2(CatageorySub2ViewModel ViewModel)
        {
            bool status = false;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_SaveCatageorySub2", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("cs1_id", ViewModel.CatageorySub1Id);
                    cmd.Parameters.AddWithValue("cs2_name", ViewModel.CatageorySub2Name); 

                    cmd.ExecuteNonQuery();
                    status = true;
                }
            }
            catch (Exception ex)
            {
            }
            return status;
        }

        public bool UpdateCatageorySub2(CatageorySub2ViewModel ViewModel)
        {
            bool status = false;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_UpdateCatageorySub2", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("cs1id", ViewModel.CatageorySub1Id);
                    cmd.Parameters.AddWithValue("cs2id", ViewModel.CatageorySub2Id);
                    cmd.Parameters.AddWithValue("cs2name", ViewModel.CatageorySub2Name);
                    cmd.ExecuteNonQuery();
                    status = true;
                }
            }
            catch (Exception ex)
            {
            }
            return status;
        }

        public bool DeleteCatageorySub2(int id)
        {
            bool status = false;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_DeleteCategorySub2", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("cs2Id", id);
                    cmd.ExecuteNonQuery();
                    status = true;
                }
            }
            catch (Exception ex)
            {
            }
            return status;
        }

        public bool CheckCategorySub2Exists(string name)
        {
            bool status = false;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_CheckCategorySub2Exists", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("CategorySub2Name", name.Trim());
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
