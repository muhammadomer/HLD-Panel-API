using DataAccess.Helper;
using DataAccess.ViewModels;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace DataAccess.DataAccess
{
    public class ConditionDataAccess
    {

        public string connStr { get; set; }
        public ConditionDataAccess(IConnectionString connectionString)
        {
            connStr = connectionString.GetConnectionString();
        }

        public bool SaveCondition(ConditionViewModel conditionViewModel)
        {
            bool status = false;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_SaveCondition", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("ConditionName", conditionViewModel.ConditionName);
                    cmd.ExecuteNonQuery();
                    status = true;
                }
            }
            catch (Exception ex)
            {
            }
            return status;
        }

        public bool UpdateCondition(ConditionViewModel conditionViewModel)
        {
            bool status = false;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_UpdateCondition", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("ConditionId", conditionViewModel.ConditionId);
                    cmd.Parameters.AddWithValue("ConditionName", conditionViewModel.ConditionName);
                    cmd.ExecuteNonQuery();
                    status = true;
                }
            }
            catch (Exception ex)
            {
            }
            return status;
        }

        public List<ConditionViewModel> GetAllCondition()
        {
            List<ConditionViewModel> _conditionViewModels = null;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_GetAllConditions", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            _conditionViewModels = new List<ConditionViewModel>();
                            while (reader.Read())
                            {
                                ConditionViewModel conditionViewModel = new ConditionViewModel();
                                conditionViewModel.ConditionId = Convert.ToInt32(reader["condition_id"]);
                                conditionViewModel.ConditionName = Convert.ToString(reader["condition_name"]);
                                _conditionViewModels.Add(conditionViewModel);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return _conditionViewModels;
        }

        public ConditionViewModel GetConditionById(int id = 0)
        {
            ConditionViewModel _conditionViewModel = null;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_GetConditionById", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("ConditionId", id);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            _conditionViewModel = new ConditionViewModel();
                            while (reader.Read())
                            {
                                _conditionViewModel .ConditionId= Convert.ToInt32(reader["condition_id"]);
                                _conditionViewModel.ConditionName = reader["condition_name"].ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return _conditionViewModel;
        }

        public bool CheckConditionExists(string name)
        {
            bool status = false;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_CheckConditionExists", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("conditionName", name.Trim());
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
