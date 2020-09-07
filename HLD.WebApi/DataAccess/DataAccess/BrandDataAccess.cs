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
    public class BrandDataAccess
    {
        public string connStr { get; set; }
        public BrandDataAccess(IConnectionString connectionString)
        {
            connStr = connectionString.GetConnectionString();
        }
        public bool SaveBrand(BrandViewModel brandViewModel)
        {
            bool status = false;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_SaveBrand", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("BrandName", brandViewModel.BrandName);
                    cmd.ExecuteNonQuery();
                    status = true;
                }
            }
            catch (Exception ex)
            {
            }
            return status;
        }

        public bool UpdateBrand(BrandViewModel brandViewModel)
        {
            bool status = false;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_UpdateBrand", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("BrandId", brandViewModel.BrandId);
                    cmd.Parameters.AddWithValue("BrandName", brandViewModel.BrandName);
                    cmd.ExecuteNonQuery();
                    status = true;
                }
            }
            catch (Exception ex)
            {
            }
            return status;
        }

        public List<BrandViewModel> GetAllBrand()
        {
            List<BrandViewModel> listBrandViewModel = null;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_GetAllBrand", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            listBrandViewModel = new List<BrandViewModel>();
                            while (reader.Read())
                            {
                                BrandViewModel brandViewModel = new BrandViewModel();
                                brandViewModel.BrandId = Convert.ToInt32(reader["brand_id"]);
                                brandViewModel.BrandName = Convert.ToString(reader["brand_name"]);
                                listBrandViewModel.Add(brandViewModel);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return listBrandViewModel;
        }

        public List<BrandViewModel> GetAllBrandByName(string name)
        {
            List<BrandViewModel> listBrandViewModel = null;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_GetAllBrandByName", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("bname", name.Trim());
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            listBrandViewModel = new List<BrandViewModel>();
                            while (reader.Read())
                            {
                                BrandViewModel brandViewModel = new BrandViewModel();
                                brandViewModel.BrandId = Convert.ToInt32(reader["brand_id"]);
                                brandViewModel.BrandName = Convert.ToString(reader["brand_name"]);
                                listBrandViewModel.Add(brandViewModel);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return listBrandViewModel;
        }

        public BrandViewModel GetBrandById(int id)
        {
            BrandViewModel brandViewModel = null;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_GetBrandById", conn);
                    cmd.Parameters.AddWithValue("BrandId", id);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            brandViewModel = new BrandViewModel();
                            while (reader.Read())
                            {
                                brandViewModel.BrandId = Convert.ToInt32(reader["brand_id"]);
                                brandViewModel.BrandName = Convert.ToString(reader["brand_name"]);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return brandViewModel;
        }

        public bool DeleteBrand(int id)
        {
            bool status = false;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_DeleteBrandById", conn);
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


        public bool CheckBrandExists(string name)
        {
            bool status = false;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_CheckBrandExists", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("brandName", name.Trim());
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
