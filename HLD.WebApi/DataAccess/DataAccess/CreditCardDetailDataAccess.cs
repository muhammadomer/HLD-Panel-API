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
    public class CreditCardDetailDataAccess
    {
        public string ConStr { get; set; }
        public CreditCardDetailDataAccess(IConnectionString connectionString)
        {

            ConStr = connectionString.GetConnectionString();
        }
        public bool Save(CreditCardDetailViewModel ViewModel)
        {
            bool status = false;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(ConStr))
                {
                    conn.Open();
                    MySqlCommand cmdd = new MySqlCommand("p_SaveCreditCardDetail", conn);
                    cmdd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmdd.Parameters.AddWithValue("_name_on_card_hash", ViewModel.name_on_card);
                    cmdd.Parameters.AddWithValue("_security_code_hash", ViewModel.security_code);
                    cmdd.Parameters.AddWithValue("_number_hash", ViewModel.number);
                    cmdd.Parameters.AddWithValue("_expiration_month", ViewModel.expiration_month);
                    cmdd.Parameters.AddWithValue("_expiration_year", ViewModel.expiration_year);
                    cmdd.Parameters.AddWithValue("_first_name", ViewModel.first_name);
                    cmdd.Parameters.AddWithValue("_last_name", ViewModel.last_name);
                    cmdd.Parameters.AddWithValue("_address_line1", ViewModel.address_line1);
                    cmdd.Parameters.AddWithValue("_address_line2", ViewModel.address_line2);
                    cmdd.Parameters.AddWithValue("_zip_code", ViewModel.zip_code);
                    cmdd.Parameters.AddWithValue("_city", ViewModel.city);
                    cmdd.Parameters.AddWithValue("_state", ViewModel.state);
                    cmdd.Parameters.AddWithValue("_country", ViewModel.country);
                    cmdd.Parameters.AddWithValue("_IsActive", ViewModel.IsActive);
                    cmdd.Parameters.AddWithValue("_IsDefault", ViewModel.IsDefault);
                    cmdd.Parameters.AddWithValue("_name_on_card", ViewModel.name_on_cardShort);
                    cmdd.Parameters.AddWithValue("_security_code", ViewModel.security_codeShort);
                    cmdd.Parameters.AddWithValue("_number", ViewModel.numberShort);
                    cmdd.Parameters.AddWithValue("_PhoneNo", ViewModel.PhoneNo);

                    cmdd.ExecuteNonQuery();
                    status = true;
                }
            }
            catch (Exception ex)
            {

            }
            return status;
        }

        public List<CreditCardDetailViewModel> GetCreditCardsList()
        {
            List<CreditCardDetailViewModel> list = new List<CreditCardDetailViewModel>();
            try
            {
                using (MySqlConnection conn = new MySqlConnection(ConStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_GetCreditCardDetail", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    DataSet ds = new DataSet();
                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    da.Fill(ds);
                    DataTable dt = ds.Tables[0];
                    foreach (DataRow reader in dt.Rows)
                    {
                        CreditCardDetailViewModel viewModel = new CreditCardDetailViewModel
                        {
                            CreditCardDetailId = Convert.ToInt32(reader["CreditCardDetailId"]),
                            name_on_cardShort = reader["name_on_card"] != DBNull.Value ? (string)reader["name_on_card"] : "",
                            numberShort = reader["number"] != DBNull.Value ? (string)reader["number"] : "",
                            security_codeShort = reader["security_code"] != DBNull.Value ? (string)reader["security_code"] : "",
                            expiration_month = reader["expiration_month"] != DBNull.Value ? (string)reader["expiration_month"] : "",
                            expiration_year = reader["expiration_year"] != DBNull.Value ? (string)reader["expiration_year"] : "",
                            first_name = reader["first_name"] != DBNull.Value ? (string)reader["first_name"] : "",
                            last_name = reader["last_name"] != DBNull.Value ? (string)reader["last_name"] : "",
                            address_line1 = reader["address_line1"] != DBNull.Value ? (string)reader["address_line1"] : "",
                            address_line2 = reader["address_line2"] != DBNull.Value ? (string)reader["address_line2"] : "",
                            zip_code = reader["zip_code"] != DBNull.Value ? (string)reader["zip_code"] : "",
                            city = reader["city"] != DBNull.Value ? (string)reader["city"] : "",
                            state = reader["state"] != DBNull.Value ? (string)reader["state"] : "",
                            country = reader["country"] != DBNull.Value ? (string)reader["country"] : "",
                            IsActive = reader["IsActive"] != DBNull.Value ? Convert.ToBoolean(reader["IsActive"]) : false,
                            IsDefault = reader["IsDefault"] != DBNull.Value ? Convert.ToBoolean(reader["IsDefault"]) : false,
                            PhoneNo = reader["PhoneNo"] != DBNull.Value ? (string)reader["PhoneNo"] : "",
                        };
                        list.Add(viewModel);
                    }
                    conn.Close();
                }
            }
            catch (Exception ex)
            {

            }
            return list;
        }

        public CreditCardDetailViewModel GetCreditCardDetailById(int Id)
        {
            CreditCardDetailViewModel model = new CreditCardDetailViewModel();
            try
            {
                using (MySqlConnection conn = new MySqlConnection(ConStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_GetCreditCardById", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_Id", Id);
                    DataSet ds = new DataSet();
                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    da.Fill(ds);
                    DataTable dt = ds.Tables[0];
                    foreach (DataRow reader in dt.Rows)
                    {
                        CreditCardDetailViewModel viewModel = new CreditCardDetailViewModel
                        {
                            CreditCardDetailId = Convert.ToInt32(reader["CreditCardDetailId"]),
                            name_on_card = reader["name_on_card"] != DBNull.Value ? (string)reader["name_on_card"] : "",
                            number = reader["number"] != DBNull.Value ? (string)reader["number"] : "",
                            security_code = reader["security_code"] != DBNull.Value ? (string)reader["security_code"] : "",
                            expiration_month = reader["expiration_month"] != DBNull.Value ? (string)reader["expiration_month"] : "",
                            expiration_year = reader["expiration_year"] != DBNull.Value ? (string)reader["expiration_year"] : "",
                            first_name = reader["first_name"] != DBNull.Value ? (string)reader["first_name"] : "",
                            last_name = reader["last_name"] != DBNull.Value ? (string)reader["last_name"] : "",
                            address_line1 = reader["address_line1"] != DBNull.Value ? (string)reader["address_line1"] : "",
                            address_line2 = reader["address_line2"] != DBNull.Value ? (string)reader["address_line2"] : "",
                            zip_code = reader["zip_code"] != DBNull.Value ? (string)reader["zip_code"] : "",
                            city = reader["city"] != DBNull.Value ? (string)reader["city"] : "",
                            state = reader["state"] != DBNull.Value ? (string)reader["state"] : "",
                            country = reader["country"] != DBNull.Value ? (string)reader["country"] : "",
                            IsActive = reader["IsActive"] != DBNull.Value ? Convert.ToBoolean(reader["IsActive"]) : false,
                            IsDefault = reader["IsDefault"] != DBNull.Value ? Convert.ToBoolean(reader["IsDefault"]) : false,
                            PhoneNo = reader["PhoneNo"] != DBNull.Value ? (string)reader["PhoneNo"] : "",
                        };
                        model = viewModel;
                    }
                    conn.Close();
                }
            }
            catch (Exception ex)
            {

            }
            return model;
        }

        public int UpdateIsActive(CreditCardDetailViewModel Obj)
        {
            int Id = 0;
            try
            {
                if (Obj.IsDefault == true)
                {
                    Obj.IsActive = true;
                }
                using (MySqlConnection conn = new MySqlConnection(ConStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_UpdateCreditCardActiveState", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_Id", Obj.CreditCardDetailId);
                    cmd.Parameters.AddWithValue("_IsActive", Obj.IsActive);
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception exp)
            {
            }
            return Id;
        }
        public int UpdateIsDefault(CreditCardDetailViewModel Obj)
        {
            int Id = 0;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(ConStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_UpdateCreditCardDefaultState", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_Id", Obj.CreditCardDetailId);
                    cmd.Parameters.AddWithValue("_IsDefault", Obj.IsDefault);
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception exp)
            {

            }
            return Id;
        }
    }
}
