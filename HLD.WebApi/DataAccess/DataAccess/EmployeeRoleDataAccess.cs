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
    public class EmployeeRoleDataAccess
    {
        public string connStr { get; set; }
        public EmployeeRoleDataAccess(IConnectionString connectionString)
        {
            connStr = connectionString.GetConnectionString();
        }
        public bool SaveEmployeeRole(EmployeeRoleViewModel employeeRoleViewModel)
        {
            bool status = false;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_SaveEmployeeRole", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_RollId", employeeRoleViewModel.RollId);
                    cmd.Parameters.AddWithValue("_EmployeeRole", employeeRoleViewModel.EmployeeRole);
                    cmd.Parameters.AddWithValue("_Permissions", employeeRoleViewModel.Permissions);
                    cmd.Parameters.AddWithValue("_CreatedOn", employeeRoleViewModel.CreatedOn);
                    cmd.ExecuteNonQuery();
                    status = true;
                }
            }
            catch (Exception ex)
            {
            }
            return status;
        }

        public List<EmployeeRoleViewModel> GetAllEmployeeRoles()
        {
            List<EmployeeRoleViewModel> listEmployeeRoleViewModel = null;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_GetAllEmployeeRoles", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            listEmployeeRoleViewModel = new List<EmployeeRoleViewModel>();
                            while (reader.Read())
                            {
                                EmployeeRoleViewModel employeeRoleViewModel = new EmployeeRoleViewModel();
                                employeeRoleViewModel.RollId = Convert.ToInt32(reader["RollId"]);
                                employeeRoleViewModel.EmployeeRole = Convert.ToString(reader["EmployeeRole"]);
                                employeeRoleViewModel.Permissions = Convert.ToString(reader["Permissions"]);
                                //employeeRoleViewModel.CreatedOn = Convert.ToDateTime(reader["CreatedOn"]);
                                employeeRoleViewModel.CreatedOn = Convert.ToDateTime(reader["CreatedOn"]);
                                listEmployeeRoleViewModel.Add(employeeRoleViewModel);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return listEmployeeRoleViewModel;
        }

        public EmployeeRoleViewModel GetEmployeeRollByRollId(int rollId)
        {
            EmployeeRoleViewModel model = null;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmdd = new MySqlCommand(@"SELECT * FROM bestBuyE2.EmployeeRole where RollId=" + rollId, conn);
                    cmdd.CommandType = System.Data.CommandType.Text;
                    using (var reader = cmdd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())

                            {
                                model = new EmployeeRoleViewModel();
                                model.RollId = Convert.ToInt32(reader["RollId"]);
                                model.EmployeeRole = Convert.ToString(reader["EmployeeRole"]);
                                model.Permissions = Convert.ToString(reader["Permissions"]);
                            }

                        }
                    }
                }
                return model;
            }
            catch (Exception)
            {

                throw;
            }

        }

        public bool UpdateEmployeeRollByRollId(EmployeeRoleViewModel model)
        {
            bool status = false;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_UpdateEmployeeRoleByRoleId", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_RollId", model.RollId);
                    cmd.Parameters.AddWithValue("_EmployeeRole", model.EmployeeRole);
                    cmd.Parameters.AddWithValue("_Permissions", model.Permissions);
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
