using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Helper;
using MySql.Data.MySqlClient;
using DataAccess.ViewModels;

namespace DataAccess.DataAccess
{
    public class EmployeeDataAccess
    {
        EmployeeRoleDataAccess EmployeeRoleDataAccess=null;
        public string connStr { get; set; }
        public EmployeeDataAccess(IConnectionString connectionString)
        {
            connStr = connectionString.GetConnectionString();
           //EmployeeRoleDataAccess=new EmployeeRoleDataAccess(connectionString);
        }

        public bool SaveEmployee(EmployeeViewModel employeeViewModel)
        {
            bool status = false;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_SaveEmployeeRecord", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_Id", employeeViewModel.Id);
                    cmd.Parameters.AddWithValue("_EmployeeId", employeeViewModel.EmployeeId);
                    cmd.Parameters.AddWithValue("_EmployeeRole", employeeViewModel.EmployeeRole);
                    cmd.Parameters.AddWithValue("_Active", employeeViewModel.Active);
                    cmd.Parameters.AddWithValue("_CreatedOn", employeeViewModel.CreatedOn);
                    cmd.Parameters.AddWithValue("_EmployeeName", employeeViewModel.EmployeeName);
                    cmd.ExecuteNonQuery();
                    status = true;
                }
            }
            catch (Exception ex)
            {
            }
            return status;
        }

        public List<EmployeeViewModel> GetAllEmployees()
        {
            List<EmployeeViewModel> listEmployeeViewModel = null;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_GetAllEmployees", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            listEmployeeViewModel = new List<EmployeeViewModel>();
                            while (reader.Read())
                            {
                                EmployeeViewModel employeeViewModel = new EmployeeViewModel();
                                employeeViewModel.Id = Convert.ToInt32(reader["Id"]);
                                employeeViewModel.EmployeeId = Convert.ToInt32(reader["EmployeeId"]);
                                employeeViewModel.EmployeeRole = Convert.ToInt32(reader["EmployeeRole"]);
                                employeeViewModel.Active = Convert.ToBoolean(reader["Active"]);
                                employeeViewModel.CreatedOn = Convert.ToDateTime(reader["CreatedOn"]);
                                employeeViewModel.EmployeeName = Convert.ToString(reader["EmployeeName"]);
                                //List<EmployeeRoleViewModel> employeeRoles=EmployeeRoleDataAccess.GetAllEmployeeRoles();
                                //employeeViewModel.employeeRoles = employeeRoles;
                                listEmployeeViewModel.Add(employeeViewModel);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return listEmployeeViewModel;
        }


        public EmployeeViewModel GetEmployeeById(int id)
        {
            EmployeeViewModel model = null;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmdd = new MySqlCommand(@"SELECT * FROM bestBuyE2.Employee where Id=" + id, conn);
                    cmdd.CommandType = System.Data.CommandType.Text;
                    using (var reader = cmdd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())

                            {
                                model = new EmployeeViewModel();
                                model.Id = Convert.ToInt32(reader["Id"]);
                                model.EmployeeId = Convert.ToInt32(reader["EmployeeId"]);
                                model.EmployeeRole = Convert.ToInt32(reader["EmployeeRole"]);
                                model.Active = Convert.ToBoolean(reader["Active"]);
                                model.EmployeeName = Convert.ToString(reader["EmployeeName"]);
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

        public EmployeeViewModel GetEmployeeByEmployeeId(int id)
        {
            EmployeeViewModel model = null;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmdd = new MySqlCommand(@"SELECT * FROM bestBuyE2.Employee where EmployeeId=" + id, conn);
                    cmdd.CommandType = System.Data.CommandType.Text;
                    using (var reader = cmdd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())

                            {
                                model = new EmployeeViewModel();
                                model.Id = Convert.ToInt32(reader["Id"]);
                                model.EmployeeId = Convert.ToInt32(reader["EmployeeId"]);
                                model.EmployeeRole = Convert.ToInt32(reader["EmployeeRole"]);
                                model.Active = Convert.ToBoolean(reader["Active"]);
                                model.EmployeeName = Convert.ToString(reader["EmployeeName"]);
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

        public bool UpdateEmployeeById(EmployeeViewModel model)

        {
            bool status = false;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_UpdateEmployeeById", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_Id", model.Id);
                    cmd.Parameters.AddWithValue("_EmployeeName", model.EmployeeName);
                    cmd.Parameters.AddWithValue("_EmployeeRole", model.EmployeeRole);
                    cmd.Parameters.AddWithValue("_Active", model.Active);
                    cmd.Parameters.AddWithValue("_EmployeeId", model.EmployeeId);
                    cmd.ExecuteNonQuery();
                    status = true;
                }
            }
            catch (Exception ex)
            {
            }
            return status;
        }

        public bool UpdateEmpActiveStatusById(EmployeeViewModel model)
        {
            bool status = false;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_UpdateEmpActiveStatusById", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_Id", model.Id);
                    //cmd.Parameters.AddWithValue("_EmployeeName", model.EmployeeName);
                    //cmd.Parameters.AddWithValue("_EmployeeRole", model.EmployeeRole);
                    cmd.Parameters.AddWithValue("_Active", model.Active);
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
