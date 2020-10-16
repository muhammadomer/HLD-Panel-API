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
    public class ColorDataAccess
    {
        public string connStr { get; set; }
        public ColorDataAccess(IConnectionString connectionString)
        {
            connStr = connectionString.GetConnectionString();
        }
        public bool SaveColor(ColorViewModel colorViewModel)
        {
            bool status = false;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_SaveColor", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("ColorName", colorViewModel.ColorName);
                    cmd.Parameters.AddWithValue("ColorCode", colorViewModel.ColorCode);
                    cmd.Parameters.AddWithValue("ColorAlias", colorViewModel.ColorAlias);
                    cmd.ExecuteNonQuery();
                    status = true;
                }
            }
            catch (Exception ex)
            {
            }
            return status;
        }

        public bool UpdateColor(ColorViewModel colorViewModel)
        {
            bool status = false;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_UpdateColor", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("ColorId", colorViewModel.ColorId);
                    cmd.Parameters.AddWithValue("ColorName", colorViewModel.ColorName);
                    cmd.Parameters.AddWithValue("ColorCode", colorViewModel.ColorCode);
                    cmd.Parameters.AddWithValue("ColorAlias", colorViewModel.ColorAlias);
                    cmd.ExecuteNonQuery();
                    status = true;
                }
            }
            catch (Exception ex)
            {
            }
            return status;
        }

        public List<ColorViewModel> GetAllColor()
        {
            List<ColorViewModel> listcolorViewModel = null;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_GetAllColor", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            listcolorViewModel = new List<ColorViewModel>();
                            while (reader.Read())
                            {
                                ColorViewModel colorViewModel = new ColorViewModel();
                                colorViewModel.ColorId = Convert.ToInt32(reader["color_id"]);
                                colorViewModel.ColorName = Convert.ToString(reader["color_name"]);
                                colorViewModel.ColorCode = Convert.ToString(reader["color_code"]);
                                colorViewModel.ColorAlias = Convert.ToString(reader["color_alias"]);
                                listcolorViewModel.Add(colorViewModel);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return listcolorViewModel;
        }


        public List<ColorAutoCompleteViewModel> GetAllColorForAutoComplete(string name)
        {
            List<ColorAutoCompleteViewModel> listcolorViewModel = null;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_GetAllColorForAutoComplete", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("cname", name.Trim());
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            listcolorViewModel = new List<ColorAutoCompleteViewModel>();
                            while (reader.Read())
                            {
                                ColorAutoCompleteViewModel colorViewModel = new ColorAutoCompleteViewModel();
                                colorViewModel.ID = Convert.ToInt32(reader["color_id"]);
                                colorViewModel.Name = Convert.ToString(reader["color_name"]);
                                colorViewModel.ColorAlias = Convert.ToString(reader["color_alias"]);
                                listcolorViewModel.Add(colorViewModel);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return listcolorViewModel;
        }

        public bool CheckColorExists(string name)
        {
            bool status = false;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_CheckColorExists", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("ColorName", name.Trim());
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

        public bool CheckColorAliasExists(string name)
        {
            bool status = false;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_CheckColorAlisExists", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("ColorAlis", name.Trim());
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
        
        public ColorViewModel GetColorById(int id)
        {
            ColorViewModel colorViewModel = null;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_GetColorById", conn);
                    cmd.Parameters.AddWithValue("ColorId", id);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            colorViewModel = new ColorViewModel();
                            while (reader.Read())
                            {
                                colorViewModel.ColorId = Convert.ToInt32(reader["color_id"]);
                                colorViewModel.ColorName = Convert.ToString(reader["color_name"]);
                                colorViewModel.ColorCode = Convert.ToString(reader["color_code"]);
                                colorViewModel.ColorAlias = Convert.ToString(reader["color_alias"]);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return colorViewModel;
        }

        public bool DeleteColor(int id)
        {
            bool status = false;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_DeleteColorById", conn);
                    cmd.Parameters.AddWithValue("ColorId", id);
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
