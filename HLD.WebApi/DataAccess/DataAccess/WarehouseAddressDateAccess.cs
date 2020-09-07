using DataAccess.Helper;
using DataAccess.ViewModels;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DataAccess
{
    public class WarehouseAddressDateAccess
    {
        public string connStr { get; set; }
        public WarehouseAddressDateAccess(IConnectionString connectionString)
        {
            connStr = connectionString.GetConnectionString();
        }

       
        
        public bool SaveWarehouse(WarehouseAddressViewModel ViewModel)
        {
           bool status = false;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_saveWarehouseAddress", conn);

                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_warehousename", ViewModel.whname);
                    cmd.Parameters.AddWithValue("_companyname", ViewModel.companyname);
                    cmd.Parameters.AddWithValue("_wh_id", ViewModel.whid);
                    cmd.Parameters.AddWithValue("_street1", ViewModel.street1);
                    cmd.Parameters.AddWithValue("_street2", ViewModel.street2);
                    cmd.Parameters.AddWithValue("_unit",ViewModel.unit );
                    cmd.Parameters.AddWithValue("_postalcode", ViewModel.postelcode);
                    cmd.Parameters.AddWithValue("_city", ViewModel.city);
                    cmd.Parameters.AddWithValue("_state", ViewModel.state);
                    cmd.Parameters.AddWithValue("_country", ViewModel.country);
                    cmd.Parameters.AddWithValue("_phone", ViewModel.phone);
         
                    cmd.ExecuteNonQuery();
                   
                }
                status = true;
            }
            catch (Exception ex)
            {
               
            }
            return status;
        }
        public List<WarehouseAddressViewModel> GetWarehouseAddresslist()
        {
            List<WarehouseAddressViewModel> listModel = null;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmdd = new MySqlCommand(@"SELECT * FROM bestBuyE2.WarehouseAddress;", conn);
                    cmdd.CommandType = System.Data.CommandType.Text;
                    MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter(cmdd);
                    DataTable dt = new DataTable();
                    mySqlDataAdapter.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        listModel = new List<WarehouseAddressViewModel>();
                        foreach (DataRow dr in dt.Rows)
                        {
                            WarehouseAddressViewModel model = new WarehouseAddressViewModel();
                            model.ID = Convert.ToInt32(dr["ID"]);
                            model.whname = Convert.ToString(dr["WH_Name"]);
                            model.companyname = Convert.ToString(dr["Company_Name"]);
                            model.whid = Convert.ToString(dr["WH_ID"]);
                            model.street1 = Convert.ToString(dr["Address_Line_1"]);
                            model.street2 = Convert.ToString(dr["Address_Line_2"]);
                            model.unit = Convert.ToString(dr["Unit"]);
                            model.country = Convert.ToString(dr["Country"]);
                            model.city = Convert.ToString(dr["City"]);
                            model.phone = Convert.ToString(dr["Phone"]);
                            model.state = Convert.ToString(dr["State"]);
                            model.postelcode = Convert.ToString(dr["Postal_Code"]);
                            listModel.Add(model);
                        }
                    }


                }
                return listModel;
            }
            catch (Exception)
            {

                throw;
            }

        }
        
      public WarehouseAddressViewModel GetWHAddressById(int id)
        {
            WarehouseAddressViewModel model =null;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmdd = new MySqlCommand(@"SELECT * FROM bestBuyE2.WarehouseAddress where ID="+id, conn);
                    cmdd.CommandType = System.Data.CommandType.Text;
                    using (var reader = cmdd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())

                            {
                                model = new WarehouseAddressViewModel();
                                model.whname = Convert.ToString(reader["WH_Name"]);
                                model.companyname = Convert.ToString(reader["Company_Name"]);
                                model.whid = Convert.ToString(reader["WH_ID"]);
                                model.street1 = Convert.ToString(reader["Address_Line_1"]);
                                model.street2 = Convert.ToString(reader["Address_Line_2"]);
                                model.unit = Convert.ToString(reader["Unit"]);
                                model.country = Convert.ToString(reader["Country"]);
                                model.city = Convert.ToString(reader["City"]);
                                model.phone = Convert.ToString(reader["Phone"]);
                                model.state = Convert.ToString(reader["State"]);
                                model.postelcode = Convert.ToString(reader["Postal_Code"]);
                              

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
        public bool UpdateWHAddress(WarehouseAddressViewModel ViewModel)
        {
            bool status = false;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_UpdateWarehouseAddress", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_warehousename",ViewModel.whname);
                    cmd.Parameters.AddWithValue("_companyname", ViewModel.companyname);
                    cmd.Parameters.AddWithValue("_street1", ViewModel.street1);
                    cmd.Parameters.AddWithValue("_street2", ViewModel.street2);
                    cmd.Parameters.AddWithValue("_wh_id", ViewModel.whid);
                    cmd.Parameters.AddWithValue("_state", ViewModel.state);
                    cmd.Parameters.AddWithValue("_country", ViewModel.country);
                    cmd.Parameters.AddWithValue("_phone", ViewModel.phone);
                    cmd.Parameters.AddWithValue("_city", ViewModel.city);
                    cmd.Parameters.AddWithValue("_postalcode", ViewModel.postelcode);
                    cmd.Parameters.AddWithValue("_unit", ViewModel.unit);
                    cmd.Parameters.AddWithValue("ID", ViewModel.ID);

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
