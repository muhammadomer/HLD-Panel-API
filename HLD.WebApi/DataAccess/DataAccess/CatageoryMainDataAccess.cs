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
    public class CatageoryMainDataAccess
    {
        public string connStr { get; set; }
        public CatageoryMainDataAccess(IConnectionString connectionString)
        {
            connStr = connectionString.GetConnectionString();
        }
        public List<CategoriesAutoCompleteViewModel> GetAllCatageoryForAutoComplete(string categoryName)
        {
            List<CategoriesAutoCompleteViewModel> list = null;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_GetAllCategoriesForAutoComplete", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("CategoryName", categoryName.Trim());
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            list = new List<CategoriesAutoCompleteViewModel>();
                            while (reader.Read())
                            {
                                CategoriesAutoCompleteViewModel catageoryViewModel = new CategoriesAutoCompleteViewModel();
                                catageoryViewModel.ID = reader["ID"].ToString();
                                catageoryViewModel.Name = Convert.ToString(reader["Name"]);
                                list.Add(catageoryViewModel);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return list;
        }
        public List<CatageoryMainViewModel> GetAllCatageoryMain()
        {
            List<CatageoryMainViewModel> listCatageoryMainViewModel = null;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_GetAllCatageory", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            listCatageoryMainViewModel = new List<CatageoryMainViewModel>();
                            while (reader.Read())
                            {
                                CatageoryMainViewModel catageoryViewModel = new CatageoryMainViewModel();
                                catageoryViewModel.CatageoryMainId = Convert.ToInt32(reader["c1_id"]);
                                catageoryViewModel.CatageoryMainName = Convert.ToString(reader["c1_name"]);
                                listCatageoryMainViewModel.Add(catageoryViewModel);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return listCatageoryMainViewModel;
        }


        public CatageoryMainViewModel GetCatageoryMainById(int id)
        {
            CatageoryMainViewModel  ViewModel = null;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_GetAllCatageoryById", conn);
                    cmd.Parameters.AddWithValue("CatageoryId", id);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            ViewModel = new CatageoryMainViewModel();
                            while (reader.Read())
                            {
                                ViewModel.CatageoryMainId = Convert.ToInt32(reader["c1_id"]);
                                ViewModel.CatageoryMainName = Convert.ToString(reader["c1_name"]);                               
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return ViewModel;
        }
        public bool SaveCatageoryMain(CatageoryMainViewModel  ViewModel)
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

        public bool UpdateCatageoryMain(CatageoryMainViewModel  ViewModel)
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

        public bool DeleteCatageoryMain(int id)
        {
            bool status = false;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_DeleteCategoryMain", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("c1Id", id);                    
                    cmd.ExecuteNonQuery();
                    status = true;
                }
            }
            catch (Exception ex)
            {
            }
            return status;
        }

        public bool CheckCategoryMainExists(string name)
        {
            bool status = false;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_CheckCategoryMainExists", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("CategoryMain", name.Trim());
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
