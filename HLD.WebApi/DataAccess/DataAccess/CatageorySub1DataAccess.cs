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
    public class CatageorySub1DataAccess
    {
        public string connStr { get; set; }
        public CatageorySub1DataAccess(IConnectionString connectionString)
        {
            connStr = connectionString.GetConnectionString();
        }

        public List<CatageorySub1ViewModel> GetAllCatageorySub1_ByMainID(int id)
        {
            List<CatageorySub1ViewModel> listCatageorySub1ViewModel = null;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_GetAllCatageory_Sub1_By_CMain", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("CatageoryMainId", id);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            listCatageorySub1ViewModel = new List<CatageorySub1ViewModel>();
                            while (reader.Read())
                            {
                                CatageorySub1ViewModel catageoryViewModel = new CatageorySub1ViewModel();
                                catageoryViewModel.CatageoryMainId = Convert.ToInt32(reader["c1_id"]);
                                catageoryViewModel.CatageorySub1Name = Convert.ToString(reader["cs1_name"]);
                                catageoryViewModel.CatageorySubId = Convert.ToInt32(reader["cs1_id"]);
                                listCatageorySub1ViewModel.Add(catageoryViewModel);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return listCatageorySub1ViewModel;
        }


        public  CatageorySub1ViewModel  GetAllCatageorySub1_ByID(int id)
        {
            CatageorySub1ViewModel  CatageorySub1ViewModel = null;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_GetCatageory_Sub1_ByID", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("CatageorySubId", id);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                             CatageorySub1ViewModel = new  CatageorySub1ViewModel();
                            while (reader.Read())
                            {
                                CatageorySub1ViewModel.CatageoryMainId = Convert.ToInt32(reader["c1_id"]);
                                CatageorySub1ViewModel.CatageorySub1Name = Convert.ToString(reader["cs1_name"]);
                                CatageorySub1ViewModel.CatageorySubId = Convert.ToInt32(reader["cs1_id"]);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return CatageorySub1ViewModel;
        }


        public bool SaveCatageorySub1(CatageorySub1ViewModel ViewModel)
        {
            bool status = false;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_SaveCatageorySub1", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("CatageorySub1_Name", ViewModel.CatageorySub1Name);
                    cmd.Parameters.AddWithValue("CatageoryMainId", ViewModel.CatageoryMainId);                    
                    cmd.ExecuteNonQuery();
                    status = true;
                }
            }
            catch (Exception ex)
            {
            }
            return status;
        }

        public bool UpdateCatageorySub1(CatageorySub1ViewModel ViewModel)
        {
            bool status = false;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_UpdateCatageorySub1", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("CatageoryMainId", ViewModel.CatageoryMainId);
                    cmd.Parameters.AddWithValue("CatageorySub1Name", ViewModel.CatageorySub1Name);
                    cmd.Parameters.AddWithValue("CatageorySub1_Id", ViewModel.CatageorySubId);
                    cmd.ExecuteNonQuery();
                    status = true;
                }
            }
            catch (Exception ex)
            {
            }
            return status;
        }
        public bool DeleteCatageorySub1(int id)
        {
            bool status = false;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_DeleteCategorySub1", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("cs1Id", id);
                    cmd.ExecuteNonQuery();
                    status = true;
                }
            }
            catch (Exception ex)
            {
            }
            return status;
        }


        public bool CheckCategorySub1Exists(string name)
        {
            bool status = false;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_CheckCategorySub1Exists", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("CategorySub1Name", name.Trim());
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
