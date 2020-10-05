using DataAccess.Helper;
using DataAccess.ViewModels;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace DataAccess.DataAccess
{
    public class QuotationDataAccess
    {
        public string connStr { get; set; }
        public QuotationDataAccess(IConnectionString connectionString)
        {
            connStr = connectionString.GetConnectionString();
        }
        public int SaveMainQoute(SaveQuotationMainVM ViewModel)
        {
            int last_insert_id = 0;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("P_InsertQuotationMain", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_Quotation_main_id", ViewModel.Quotation_main_id);
                    cmd.Parameters.AddWithValue("_Sku", ViewModel.Sku);
                    cmd.Parameters.AddWithValue("_Currency", ViewModel.Currency);
                    cmd.Parameters.AddWithValue("_Notes", ViewModel.Notes);
                    cmd.Parameters.AddWithValue("_Feature", ViewModel.Feature);
                    cmd.Parameters.AddWithValue("_Title", ViewModel.Title);
                    cmd.Parameters.AddWithValue("_CreationDate", DateTime.Now);
                    last_insert_id = Convert.ToInt32(cmd.ExecuteScalar());
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
            }
            return last_insert_id;
        }

        public int SaveSubQoute(SaveQuotationSubVM ViewModel)
        {
            int last_insert_id = 0;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("P_SaveQuotationMain", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_Quotation_main_id", ViewModel.latestqouteId);
                    cmd.Parameters.AddWithValue("_MainSku", ViewModel.MainSku);
                    cmd.Parameters.AddWithValue("_SubSku", ViewModel.SubSku);
                    cmd.Parameters.AddWithValue("_Price", ViewModel.Price);
                    last_insert_id = Convert.ToInt32(cmd.ExecuteScalar());
                    conn.Close();

                }
            }
            catch (Exception ex)
            {
            }
            return last_insert_id;
        }

        public int SaveQouteImage(QuotationImagesVM ViewModel)
        {

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {

                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("P_InsertQuotImages", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_LastSubQouteId", ViewModel.LastSubQouteId);
                    cmd.Parameters.AddWithValue("_Sku", ViewModel.Sku);
                    cmd.Parameters.AddWithValue("_Images", ViewModel.Images);
                    cmd.ExecuteScalar();
                    conn.Close();

                }
            }
            catch (Exception ex)
            {
            }
            return 0;
        }

        public int DeleteMainQoute(int Id)
        {
            
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("P_DeleteQuoteMain", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_Quotation_main_id", Id);
                    cmd.ExecuteScalar();
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return 0;
        }

        public int DeleteSubQoute(int Id)
        {

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("P_DeleteQuoteSub", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_Quotation_Sub_Id", Id);
                    cmd.ExecuteScalar();
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return 0;
        }

        public int DeleteQouteImage(int Id)
        {

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("P_DeleteQuoteImage", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_Quotation_Images_Id", Id);
                    cmd.ExecuteScalar();
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return 0;
        }
        public string GenerateMainSku()
        {
            string SKU = "";
            try
            {
               
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("P_GenerateNewSku", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    SKU = Convert.ToString(cmd.ExecuteScalar());
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
            }
            return SKU;
        }
        public int CreateSubSku(string _sku, int _mainSkuId)
        {
            int last_insert_id = 0;
            try
            {

                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("P_CreateSubSKU", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_Quotation_main_id", _sku);
                    cmd.Parameters.AddWithValue("_Quotation_main_id", _mainSkuId);
                    last_insert_id = Convert.ToInt32(cmd.ExecuteScalar());
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
            }
            return last_insert_id;
        }

        public int DeleteMainSku(int Id)
        {

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("P_DeleteMainSKU", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_Sku", Id);
                    cmd.ExecuteScalar();
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return 0;
        }

        public List<SaveQuotationMainVM> QuotationList()
        {
            List<SaveQuotationMainVM> list = new List<SaveQuotationMainVM>();
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_GetMainQuotationDetail", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    DataSet ds = new DataSet();
                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    da.Fill(ds);
                    DataTable dt = ds.Tables[0];
                    foreach (DataRow reader in dt.Rows)
                    {
                        SaveQuotationMainVM viewModel = new SaveQuotationMainVM
                        {
                           
                            Sku = reader["Sku"] != DBNull.Value ? (string)reader["Sku"] : "",
                            Title = reader["Title"] != DBNull.Value ? (string)reader["Title"] : "",
                            Currency = reader["Currency"] != DBNull.Value ? (string)reader["Currency"] : "",
                            Notes = reader["Notes"] != DBNull.Value ? (string)reader["Notes"] : "",
                            Feature = reader["Feature"] != DBNull.Value ? (string)reader["Feature"] : "",
                            CreationDate = Convert.ToDateTime(reader["CreationDate"] != DBNull.Value ? reader["CreationDate"] : DateTime.MinValue),
                         
                           
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
    }
}
