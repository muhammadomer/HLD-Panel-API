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
    public class CurrencyExchangeDataAccess
    {
        public string connStr { get; set; }
        public CurrencyExchangeDataAccess(IConnectionString connectionString)
        {
            connStr = connectionString.GetConnectionString();
        }
        public bool SaveCurrencyExchange(CurrencyExchangeViewModel currencyExchangeViewModel)
        {
            bool status = false;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_SaveCurrencyExchange", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_currency_date", currencyExchangeViewModel.dateTime);
                    cmd.Parameters.AddWithValue("_usd_to_cad", currencyExchangeViewModel.USD_To_CAD);
                    cmd.Parameters.AddWithValue("_USD_To_CNY", currencyExchangeViewModel.USD_To_CNY);
                    cmd.Parameters.AddWithValue("_status", currencyExchangeViewModel.IsActive);
                    cmd.ExecuteNonQuery();
                    status = true;
                }
            }
            catch (Exception ex)
            {
            }
            return status;
        }

        public bool UpdateCurrencyExchange(CurrencyExchangeViewModel currencyExchangeViewModel)
        {
            bool status = false;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_UpdateCurrencyExchange", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_currency_date", currencyExchangeViewModel.dateTime);
                    cmd.Parameters.AddWithValue("_usd_to_cad", currencyExchangeViewModel.USD_To_CAD);
                    cmd.Parameters.AddWithValue("_USD_To_CNY", currencyExchangeViewModel.USD_To_CNY);
                    cmd.Parameters.AddWithValue("_status", currencyExchangeViewModel.IsActive);
                    cmd.Parameters.AddWithValue("_currency_exchange_id", currencyExchangeViewModel.CurrencyExchangeID);
                    cmd.ExecuteNonQuery();
                    status = true;
                }
            }
            catch (Exception ex)
            {
            }
            return status;
        }

        public List<CurrencyExchangeViewModel> GetAllCurrencyExchangeList()
        {
            List<CurrencyExchangeViewModel> listCurrencyExchangelist = null;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_GetAllCurrencyExchangeList", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            listCurrencyExchangelist = new List<CurrencyExchangeViewModel>();
                            while (reader.Read())
                            {
                                CurrencyExchangeViewModel CurrencyExchangeViewModel = new CurrencyExchangeViewModel();
                                CurrencyExchangeViewModel.CurrencyExchangeID = Convert.ToInt32(reader["currency_exchange_id"] != DBNull.Value ? reader["currency_exchange_id"] : "0");
                                CurrencyExchangeViewModel.dateTime = Convert.ToDateTime(reader["currency_date"] != DBNull.Value ? reader["currency_date"] : DateTime.Now);
                                CurrencyExchangeViewModel.USD_To_CAD = Convert.ToDecimal(reader["usd_to_cad"] != DBNull.Value ? reader["usd_to_cad"] : "0");
                                CurrencyExchangeViewModel.USD_To_CNY = Convert.ToDecimal(reader["USD_To_CNY"] != DBNull.Value ? reader["USD_To_CNY"] : "0");
                                CurrencyExchangeViewModel.IsActive = Convert.ToBoolean(reader["status"] != DBNull.Value ? reader["status"] : "false");
                                listCurrencyExchangelist.Add(CurrencyExchangeViewModel);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return listCurrencyExchangelist;
        }



        public CurrencyExchangeViewModel GetCurrencyExchangeById(int id)
        {
            CurrencyExchangeViewModel currencyExchangeViewModel = null;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_GetCurrencyExchangeByID", conn);
                    cmd.Parameters.AddWithValue("_currency_exchange_id", id);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            currencyExchangeViewModel = new CurrencyExchangeViewModel();
                            while (reader.Read())
                            {
                                currencyExchangeViewModel.CurrencyExchangeID = Convert.ToInt32(reader["currency_exchange_id"] != DBNull.Value ? reader["currency_exchange_id"] : "0");
                                currencyExchangeViewModel.dateTime = Convert.ToDateTime(reader["currency_date"] != DBNull.Value ? reader["currency_date"] : DateTime.Now);
                                currencyExchangeViewModel.USD_To_CAD = Convert.ToDecimal(reader["usd_to_cad"] != DBNull.Value ? reader["usd_to_cad"] : "0");
                                currencyExchangeViewModel.USD_To_CNY = Convert.ToDecimal(reader["USD_To_CNY"] != DBNull.Value ? reader["USD_To_CNY"] : "0");
                                currencyExchangeViewModel.IsActive = Convert.ToBoolean(reader["status"] != DBNull.Value ? reader["status"] : "false");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return currencyExchangeViewModel;
        }


        public double GetLatestCurrencyRate()
        {
            double currencyRate = 0;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_GetLatestCurrencyRate", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {

                            while (reader.Read())
                            {
                                currencyRate = Convert.ToDouble(reader["usd_to_cad"] != DBNull.Value ? reader["usd_to_cad"] : "0");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return currencyRate;
        }


        public bool DeleteBrand(int id)
        {
            bool status = false;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_DeleteCurrencyExchange", conn);
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
    }
}
