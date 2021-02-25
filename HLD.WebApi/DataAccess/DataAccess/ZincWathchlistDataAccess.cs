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
    public class ZincWathchlistDataAccess
    {
        public string connStr { get; set; }
        public ZincWathchlistDataAccess(IConnectionString connectionString)
        {
            connStr = connectionString.GetConnectionString();
        }
        public bool SaveWatchlist(SaveWatchlistViewModel ViewModel)
        {
            bool status = false;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("P_saveWatchlist", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_frequency", ViewModel.frequency);
                    cmd.Parameters.AddWithValue("_ProductSKU", ViewModel.ProductSKU);
                    cmd.Parameters.AddWithValue("_asin", ViewModel.ASIN);
                    cmd.Parameters.AddWithValue("_CheckAfterDays", ViewModel.CheckafterDays);
                    cmd.ExecuteNonQuery();
                    status = true;
                }
            }
            catch (Exception ex)
            {
            }
            return status;
        }
        public bool SaveWatchlistNew(SaveWatchlistViewModel ViewModel)
        {
            bool status = false;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("P_saveWatchlistV1", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_frequency", ViewModel.frequency);
                    cmd.Parameters.AddWithValue("_ProductSKU", ViewModel.ProductSKU);
                    cmd.Parameters.AddWithValue("_asin", ViewModel.ASIN);
                    cmd.Parameters.AddWithValue("_CheckAfterDays", ViewModel.CheckafterDays);
                    cmd.ExecuteNonQuery();
                    status = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return status;
        }

        public SaveWatchlistViewModel GetWatchlist(string ASIN)
        {
            //SaveWatchlistViewModel model = null;
            SaveWatchlistViewModel model = new SaveWatchlistViewModel();
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmdd = new MySqlCommand(@"select ASIN,Frequency,ProductSKU,CheckAfterDays,LastUpdatedDate,NextUpdateDate,ExpiryDate from ZincWatchlistJob Where ASIN='" + ASIN + "'", conn);
                    cmdd.CommandType = System.Data.CommandType.Text;
                    using (var reader = cmdd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                model = new SaveWatchlistViewModel();
                                model.ASIN = Convert.ToString(reader["ASIN"]);
                                model.ProductSKU = Convert.ToString(reader["ProductSKU"]);
                                model.frequency = Convert.ToInt32(reader["Frequency"] != DBNull.Value ? reader["Frequency"] : 0);
                                model.CheckafterDays = Convert.ToInt32(reader["CheckAfterDays"] != DBNull.Value ? reader["CheckAfterDays"] : 0);
                                model.LastUpdateDate = reader["LastUpdatedDate"] != DBNull.Value ? Convert.ToDateTime(reader["LastUpdatedDate"]) : default(DateTime);
                                model.NextUpdateDate = reader["NextUpdateDate"] != DBNull.Value ? Convert.ToDateTime(reader["NextUpdateDate"]) : default(DateTime);
                                model.ExpiryDate = reader["ExpiryDate"] != DBNull.Value ? Convert.ToDateTime(reader["ExpiryDate"]) : default(DateTime);
                            }
                        }
                    }
                }
                return model;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public List<SaveWatchlistForViewModel> GetWatchlistForJob(int JobId)
        {

            List<SaveWatchlistForViewModel> listViewModel = null;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("P_GetASINforWatchlistJobCopy", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_JobId", JobId);
                    
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            listViewModel = new List<SaveWatchlistForViewModel>();
                            while (reader.Read())
                            {
                                SaveWatchlistForViewModel ViewModel = new SaveWatchlistForViewModel();
                                ViewModel.ASIN = Convert.ToString(reader["ASIN"]!= DBNull.Value? reader["ASIN"]:"");
                                ViewModel.ProductSKU = Convert.ToString(reader["SKU"] !=DBNull.Value? reader["SKU"] :"");
                           
                                listViewModel.Add(ViewModel);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return listViewModel;

        }

        public List<SaveWatchlistForjobsViewModel> GetWatchlistForJobNew()
        {

            List<SaveWatchlistForjobsViewModel> listViewModel = new List<SaveWatchlistForjobsViewModel>();
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("P_GetASINforWatchlistJobCopyV1", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                          
                            while (reader.Read())
                            {
                                SaveWatchlistForjobsViewModel ViewModel = new SaveWatchlistForjobsViewModel();
                                ViewModel.ASIN = Convert.ToString(reader["ASIN"] != DBNull.Value ? reader["ASIN"] : "");
                                ViewModel.ProductSKU = Convert.ToString(reader["ProductSKU"] != DBNull.Value ? reader["ProductSKU"] : "");
                                ViewModel.frequency = Convert.ToInt32(reader["Frequency"] != DBNull.Value ? reader["Frequency"] : 0);
                                ViewModel.ValidStatus = Convert.ToInt32(reader["ValidStatus"] != DBNull.Value ? reader["ValidStatus"] : 0);
                                ViewModel.Consumed_call = Convert.ToInt32(reader["Consumed_call"] != DBNull.Value ? reader["Consumed_call"] : 0);
                                ViewModel.CheckafterDays = Convert.ToInt32(reader["CheckAfterDays"] != DBNull.Value ? reader["CheckAfterDays"] : 0);
                                listViewModel.Add(ViewModel);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return listViewModel;

        }


        public bool SaveWatchlistLogs(ZincWatchlistLogsViewModel ViewModel)
        {
            bool status = false;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_saveZincWatchlistlogs", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_JobID", ViewModel.jobID);
                    cmd.Parameters.AddWithValue("_ASIN", ViewModel.ASIN);
                    cmd.Parameters.AddWithValue("_SKU", ViewModel.ProductSKU);
                    cmd.Parameters.AddWithValue("_ZincResponse", ViewModel.ZincResponse);
                    cmd.Parameters.AddWithValue("_SellerName", ViewModel.SellerName);
                    cmd.Parameters.AddWithValue("_AMZPrice", ViewModel.Amz_Price);
                    cmd.Parameters.AddWithValue("_Prime", ViewModel.IsPrime);
                    cmd.Parameters.AddWithValue("_FulfilledBy", ViewModel.FulfilledBY);
                    cmd.ExecuteNonQuery();
                    status = true;
                }
            }
            catch (Exception ex)
            {
            }
            return status;
        }

        public bool SaveWatchlistLogsNew(ZincWatchlistLogsViewModel ViewModel)
        {
            bool status = false;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_saveZincWatchlistlogsV1", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_JobID", ViewModel.jobID);
                    cmd.Parameters.AddWithValue("_ASIN", ViewModel.ASIN);
                    cmd.Parameters.AddWithValue("_SKU", ViewModel.ProductSKU);
                    cmd.Parameters.AddWithValue("_ZincResponse", ViewModel.ZincResponse);
                    cmd.Parameters.AddWithValue("_SellerName", ViewModel.SellerName);
                    cmd.Parameters.AddWithValue("_AMZPrice", ViewModel.Amz_Price);
                    cmd.Parameters.AddWithValue("_Prime", ViewModel.IsPrime);
                    cmd.Parameters.AddWithValue("_FulfilledBy", ViewModel.FulfilledBY);
                    cmd.Parameters.AddWithValue("_UpdateOnHLD", ViewModel.UpdateOnHLD);
                    cmd.Parameters.AddWithValue("_Remarks", ViewModel.Remarks);
                    cmd.ExecuteNonQuery();
                    status = true;
                }
            }
            catch (Exception ex)
            {
            }
            return status;
        }

        public bool UpdateWatchlistForJob(SaveWatchlistForjobsViewModel ViewModel)
        {
            bool status = false;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("P_UpdateWatchlistAccordingToResponse", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("_ASIN", ViewModel.ASIN);
                    cmd.Parameters.AddWithValue("_ValidStatus", ViewModel.ProductSKU);
                    cmd.Parameters.AddWithValue("_Consumed_call", ViewModel.Consumed_call);

                    cmd.ExecuteNonQuery();
                    status = true;
                }
            }
            catch (Exception ex)
            {
            }
            return status;
        }
        public bool UpdateWatchlistForJobNew(SaveWatchlistForjobsViewModel ViewModel)
        {
            bool status = false;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("P_UpdateWatchlistAccordingToResponseV1", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("_ASIN", ViewModel.ASIN);
                    cmd.Parameters.AddWithValue("_ValidStatus", ViewModel.ValidStatus);
                    cmd.Parameters.AddWithValue("_Consumed_call", ViewModel.Consumed_call);

                    cmd.ExecuteNonQuery();
                    status = true;
                }
            }
            catch (Exception ex)
            {
            }
            return status;
        }

        public int SaveWatchlistSummary()
        {
            int jobID = 0;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("P_SaveZincWatchlistsummaryCopy", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add("Job_Id", MySqlDbType.Int32, 10);
                    cmd.Parameters["Job_Id"].Direction = System.Data.ParameterDirection.Output;
                    cmd.ExecuteNonQuery();
                    jobID = Convert.ToInt32(cmd.Parameters["Job_Id"].Value !=DBNull.Value? cmd.Parameters["Job_Id"].Value :0);
                }
            }
            catch (Exception ex)
            {
            }
            return jobID;
        }

        public int StartwatchlistJobWatchlistSummary(int jobid)
        {
            int jobID = 0;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("P_SaveZincStartwatchlistJobWatchlistSummary", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("Job_Id", jobid);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
            }
            return jobID;
        }

        public int SaveWatchlistSummaryNew()
        {
            int jobID = 0;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("P_SaveZincWatchlistsummaryCopyV1", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add("Job_Id", MySqlDbType.Int32, 10);
                    cmd.Parameters["Job_Id"].Direction = System.Data.ParameterDirection.Output;
                    cmd.ExecuteNonQuery();
                    jobID = Convert.ToInt32(cmd.Parameters["Job_Id"].Value != DBNull.Value ? cmd.Parameters["Job_Id"].Value : 0);
                }
            }
            catch (Exception ex)
            {
            }
            return jobID;
        }

        public bool UpdateWatchlistSummary(ZincWatchListSummaryViewModal ViewModel)
        {
            bool status = false;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("P_UpdateZincWatchlistsummary", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_JobID", ViewModel.JobID);
                    cmd.Parameters.AddWithValue("_Total_ASIN", ViewModel.Total_ASIN);
                    cmd.Parameters.AddWithValue("_Available", ViewModel.Available);
                    cmd.Parameters.AddWithValue("_Prime", ViewModel.Prime);
                    cmd.Parameters.AddWithValue("_Unavailable", ViewModel.Unavailable);
                    cmd.Parameters.AddWithValue("_NoPrime", ViewModel.NoPrime);

                    cmd.ExecuteNonQuery();
                    status = true;
                }
            }
            catch (Exception ex)
            {
            }
            return status;
        }
        public bool UpdateWatchlistSummaryNew(ZincWatchListSummaryViewModal ViewModel)
        {
            bool status = false;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("P_UpdateZincWatchlistsummaryV1", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_JobID", ViewModel.JobID);
                    cmd.Parameters.AddWithValue("_Total_ASIN", ViewModel.Total_ASIN);
                    cmd.Parameters.AddWithValue("_Available", ViewModel.Available);
                    cmd.Parameters.AddWithValue("_Prime", ViewModel.Prime);
                    cmd.Parameters.AddWithValue("_Unavailable", ViewModel.Unavailable);
                    cmd.Parameters.AddWithValue("_NoPrime", ViewModel.NoPrime);

                    cmd.ExecuteNonQuery();
                    status = true;
                }
            }
            catch (Exception ex)
            {
            }
            return status;
        }

        public int GetWatchlistSummaryCount()
        {
            int count = 0;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(@"SELECT count(JobID) AS Records FROM bestBuyE2.ZincASINWatchlistSummary ;", conn);
                    cmd.CommandType = System.Data.CommandType.Text;
                    MySqlDataAdapter mySqlDataAdap = new MySqlDataAdapter(cmd);
                    DataTable data = new DataTable();
                    mySqlDataAdap.Fill(data);
                    if (data.Rows.Count > 0)
                    {
                        foreach (DataRow datarow in data.Rows)
                        {
                            ZincWatchListSummaryViewModal model = new ZincWatchListSummaryViewModal();
                            count = Convert.ToInt32(datarow["Records"] != DBNull.Value ? datarow["Records"] : "0");


                        }

                    }
                }
                return count;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public List<ZincWatchListSummaryViewModal> GetWatchlistSummary(int offset)
        {
            List<ZincWatchListSummaryViewModal> listModel = null;
            // List<ZincWatchListSummaryViewModal> listModel = new List<ZincWatchListSummaryViewModal>();
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmdd = new MySqlCommand(@"SELECT * FROM bestBuyE2.ZincASINWatchlistSummary ORDER BY JobID desc limit 25 offset " + offset, conn);
                    cmdd.CommandType = System.Data.CommandType.Text;
                    MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter(cmdd);
                    DataTable dt = new DataTable();
                    mySqlDataAdapter.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        listModel = new List<ZincWatchListSummaryViewModal>();
                        foreach (DataRow dr in dt.Rows)
                        {
                            ZincWatchListSummaryViewModal model = new ZincWatchListSummaryViewModal();
                            model.JobID = Convert.ToInt32(dr["JobID"] != DBNull.Value ? dr["JobID"] : "0");
                            model.StartTime = Convert.ToDateTime(dr["StartTime"] != DBNull.Value ? dr["StartTime"] : DateTime.MinValue);
                            model.CompletionTime = Convert.ToDateTime(dr["CompletionTime"] != DBNull.Value ? dr["CompletionTime"] : DateTime.MinValue);
                            model.Total_ASIN = Convert.ToInt32(dr["Total_ASIN"] != DBNull.Value ? dr["Total_ASIN"] : "0");
                            model.Available = Convert.ToInt32(dr["Available"] != DBNull.Value ? dr["Available"] : "0");
                            model.Prime = Convert.ToInt32(dr["Prime"] != DBNull.Value ? dr["Prime"] : "0");
                            model.NoPrime = Convert.ToInt32(dr["NoPrime"] != DBNull.Value ? dr["NoPrime"] : "0");
                            model.Unavailable = Convert.ToInt32(dr["Unavailable"] != DBNull.Value ? dr["Unavailable"] : "0");
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


        public List<ZincWatchlistLogsViewModel> GetWatchlistLogs(ZincWatchLogsSearchViewModel searchViewModel)
        {
            if (searchViewModel.Available == "Available")
            {
                searchViewModel.IsPrime = 1;
            }
            if (searchViewModel.Available == "Error")
            {
                searchViewModel.IsPrime = 0;
            }

            if (searchViewModel.Available == "Currently Unavailable" || searchViewModel.Available == "Listing Removed")
            {
                searchViewModel.IsPrime = 0;
            }

            if (searchViewModel.Available == null)
            {
                searchViewModel.Available = "";
            }

            List<ZincWatchlistLogsViewModel> modelLog = new List<ZincWatchlistLogsViewModel>();
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    //MySqlCommand cmd = new MySqlCommand("P_GetZincWatchlistLogsCopyPrime", conn);//my comment adeel
                    MySqlCommand cmd = new MySqlCommand("P_GetZincWatchlistLogsCopyPrimeOne", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_ASIN", searchViewModel.ASIN);
                    cmd.Parameters.AddWithValue("_JobID", searchViewModel.JobID);
                    cmd.Parameters.AddWithValue("_Available", searchViewModel.Available);
                    cmd.Parameters.AddWithValue("_IsPrime", searchViewModel.IsPrime);
                    cmd.Parameters.AddWithValue("_Offset", searchViewModel.Offset);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {

                            while (reader.Read())
                            {
                                ZincWatchlistLogsViewModel ViewModel = new ZincWatchlistLogsViewModel();
                                ViewModel.ASIN = Convert.ToString(reader["ASIN"]);
                                ViewModel.ProductSKU = Convert.ToString(reader["SKU"]);
                                ViewModel.ZincResponse = Convert.ToString(reader["ZincResponse"]);
                                ViewModel.SellerName = Convert.ToString(reader["SellerName"]);
                                ViewModel.Amz_Price = Convert.ToInt32(reader["AMZPrice"] != DBNull.Value ? reader["AMZPrice"] : "0");
                                ViewModel.jobID = Convert.ToInt32(reader["JobID"] != DBNull.Value ? reader["JobID"] : "0");
                                ViewModel.IsPrime = Convert.ToInt32(reader["Prime"] != DBNull.Value ? reader["Prime"] : "0");
                                ViewModel.FulfilledBY = Convert.ToString(reader["FulfilledBy"]);
                                ViewModel.Compress_image = Convert.ToString(reader["Compress_image"]);
                                ViewModel.image_name = Convert.ToString(reader["image_name"]);
                                ViewModel.BBProductId = Convert.ToString(reader["BBProductId"]);
                                ViewModel.BBSellingPrice = Convert.ToDecimal(reader["BBSellingPrice"] != DBNull.Value ? reader["BBSellingPrice"] : "0");
                                ViewModel.UnitOriginPrice_MSRP = Convert.ToDouble(reader["unit_origin_price_MSRP"] != DBNull.Value ? reader["unit_origin_price_MSRP"] : "0");
                                ViewModel.UnitOriginPrice_Max = Convert.ToDouble(reader["max_price"] != DBNull.Value ? reader["max_price"] : "0");
                                ViewModel.HLD_CA1 = Convert.ToInt32(reader["HLD_CA1"] != DBNull.Value ? reader["HLD_CA1"] : "0");

                                ViewModel.BBJobID = Convert.ToInt32(reader["BBJobID"] != DBNull.Value ? reader["BBJobID"] : "0");
                                ViewModel.ImportId = Convert.ToInt32(reader["ImportID"] != DBNull.Value ? reader["ImportID"] : "0");
                                ViewModel.UpdatedPriceBB = Convert.ToDecimal(reader["UpdatedPriceBB"] != DBNull.Value ? reader["UpdatedPriceBB"] : "0");
                                modelLog.Add(ViewModel);
                            }
                        }
                    }
                }
                return modelLog;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public List<ZincWatchlistLogsViewModel> GetWatchlistLogsForAllPages(ZincWatchLogsSearchViewModel searchViewModel)
        {
            List<ZincWatchlistLogsViewModel> modelLog = new List<ZincWatchlistLogsViewModel>();
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("P_GetZincWatchlistLogsForSCUpdate", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_ASIN", searchViewModel.ASIN);
                    cmd.Parameters.AddWithValue("_JobID", searchViewModel.JobID);
                    cmd.Parameters.AddWithValue("_Available", searchViewModel.Available);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {

                            while (reader.Read())
                            {
                                ZincWatchlistLogsViewModel ViewModel = new ZincWatchlistLogsViewModel();
                                ViewModel.ASIN = Convert.ToString(reader["ASIN"]);
                                ViewModel.ProductSKU = Convert.ToString(reader["SKU"]);
                                ViewModel.ZincResponse = Convert.ToString(reader["ZincResponse"]);
                                ViewModel.SellerName = Convert.ToString(reader["SellerName"]);
                                ViewModel.Amz_Price = Convert.ToInt32(reader["AMZPrice"] != DBNull.Value ? reader["AMZPrice"] : "0");
                                ViewModel.jobID = Convert.ToInt32(reader["JobID"] != DBNull.Value ? reader["JobID"] : "0");
                                ViewModel.IsPrime = Convert.ToInt32(reader["Prime"] != DBNull.Value ? reader["Prime"] : "0");
                                ViewModel.FulfilledBY = Convert.ToString(reader["FulfilledBy"]);
                                ViewModel.Compress_image = Convert.ToString(reader["Compress_image"]);
                                ViewModel.image_name = Convert.ToString(reader["image_name"]);
                                ViewModel.BBProductId = Convert.ToString(reader["BBProductId"]);
                                ViewModel.BBSellingPrice = Convert.ToDecimal(reader["BBSellingPrice"] != DBNull.Value ? reader["BBSellingPrice"] : "0");
                                ViewModel.UnitOriginPrice_MSRP = Convert.ToDouble(reader["unit_origin_price_MSRP"] != DBNull.Value ? reader["unit_origin_price_MSRP"] : "0");
                                ViewModel.UnitOriginPrice_Max = Convert.ToDouble(reader["max_price"] != DBNull.Value ? reader["max_price"] : "0");
                                modelLog.Add(ViewModel);
                            }
                        }
                    }
                }
                return modelLog;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int GetWatchlistLogsCount(ZincWatchLogsSearchViewModel searchViewModel)
        {
            int Records = 0;

            if (searchViewModel.Available == "Available")
            {
                searchViewModel.IsPrime = 1;
            }
            if (searchViewModel.Available == "Error")
            {
                searchViewModel.IsPrime = 0;
            }
            if (searchViewModel.Available == "Currently Unavailable" || searchViewModel.Available == "Listing Removed")
            {
                searchViewModel.IsPrime = 0;
            }

            if (searchViewModel.Available == null)
            {
                searchViewModel.Available = "";
            }

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    //MySqlCommand cmd = new MySqlCommand("P_GetZincWatchlistLogsCountPrime", conn);
                    MySqlCommand cmd = new MySqlCommand("P_GetZincWatchlistLogsCountPrimeCopy", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_ASIN", searchViewModel.ASIN);
                    cmd.Parameters.AddWithValue("_JobID", searchViewModel.JobID);
                    cmd.Parameters.AddWithValue("_Available", searchViewModel.Available);
                    cmd.Parameters.AddWithValue("_IsPrime", searchViewModel.IsPrime);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {

                            while (reader.Read())
                            {

                                Records = Convert.ToInt32(reader["Records"]);

                            }
                        }
                    }
                    return Records;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        public int GetWatchlistLogsSelectAllCountDA(ZincWatchLogsSearchViewModel searchViewModel)
        {
            int Records = 0;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("P_GetWatchlistLogsSelectAllCount", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_JobID", searchViewModel.JobID);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                Records = Convert.ToInt32(reader["Records"]);
                            }
                        }
                    }
                    return Records;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public ZincWatchListSummaryViewModal GetWatchlistSummaryByJobID(int JobID)
        {
            ZincWatchListSummaryViewModal model = null;
            // List<ZincWatchListSummaryViewModal> listModel = new List<ZincWatchListSummaryViewModal>();
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmdd = new MySqlCommand(@"SELECT * FROM bestBuyE2.ZincASINWatchlistSummary WHERE JobID=" + JobID, conn);
                    cmdd.CommandType = System.Data.CommandType.Text;
                    MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter(cmdd);
                    DataTable dt = new DataTable();
                    mySqlDataAdapter.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        model = new ZincWatchListSummaryViewModal();
                        foreach (DataRow dr in dt.Rows)
                        {

                            model.JobID = Convert.ToInt32(dr["JobID"] != DBNull.Value ? dr["JobID"] : "0");
                            model.StartTime = Convert.ToDateTime(dr["StartTime"] != DBNull.Value ? dr["StartTime"] : DateTime.Now);
                            model.CompletionTime = Convert.ToDateTime(dr["CompletionTime"] != DBNull.Value ? dr["CompletionTime"] : DateTime.Now);
                            model.Total_ASIN = Convert.ToInt32(dr["Total_ASIN"] != DBNull.Value ? dr["Total_ASIN"] : "0");
                            model.Available = Convert.ToInt32(dr["Available"] != DBNull.Value ? dr["Available"] : "0");
                            model.Prime = Convert.ToInt32(dr["Prime"] != DBNull.Value ? dr["Prime"] : "0");
                            model.NoPrime = Convert.ToInt32(dr["NoPrime"] != DBNull.Value ? dr["NoPrime"] : "0");
                            model.Unavailable = Convert.ToInt32(dr["Unavailable"] != DBNull.Value ? dr["Unavailable"] : "0");

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
        public bool UpdateWatchlistLogs(ZincWatchlistLogsViewModel ViewModel)
        {
            bool status = false;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_updateZincWatchlistlogsV1", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_JobID", ViewModel.jobID);
                    cmd.Parameters.AddWithValue("_ASIN", ViewModel.ASIN);
                    cmd.Parameters.AddWithValue("_SKU", ViewModel.ProductSKU);
                    cmd.Parameters.AddWithValue("_ZincResponse", ViewModel.ZincResponse);
                    cmd.Parameters.AddWithValue("_SellerName", ViewModel.SellerName);
                    cmd.Parameters.AddWithValue("_AMZPrice", ViewModel.Amz_Price);
                    cmd.Parameters.AddWithValue("_Prime", ViewModel.IsPrime);
                    cmd.Parameters.AddWithValue("_FulfilledBy", ViewModel.FulfilledBY);
                    cmd.ExecuteNonQuery();
                    status = true;
                }
            }
            catch (Exception ex)
            {
            }
            return status;
        }
        public bool GetStatusResponce(ASINActiveStatusViewModel ViewModel)
        {
            bool status = false;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("P_UpdateValidStatusWatchList", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_asin", ViewModel.ASIN);
                    cmd.Parameters.AddWithValue("_active", ViewModel.Active);
                    cmd.ExecuteNonQuery();
                    status = true;
                }
            }
            catch (Exception ex)
            {
            }
            return status;
        }

        public int UpdateZincJobSwitch(string Value)
        {
            int Id = 0;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_UpdateZincJobSwitch", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_value", Value);
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception exp)
            {
            }
            return Id;
        }
        public int UpdateZincJobStatus(string Value)
        {
            int Id = 0;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_UpdateZincJobStatus", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_value", Value);
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception exp)
            {
            }
            return Id;
        }

        public ZincWatchListJobViewModel GetZincWatchListStatus()
        {
            ZincWatchListJobViewModel item = new ZincWatchListJobViewModel();
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("P_GetZincJobInfo", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    DataTable dt = new DataTable();
                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    da.Fill(dt);
                    foreach (DataRow reader in dt.Rows)
                    {
                        ZincWatchListJobViewModel viewModel = new ZincWatchListJobViewModel
                        {
                            status = reader["status"] != DBNull.Value ? Convert.ToInt32(reader["status"]) : 0,
                            Switch = reader["Switch"] != DBNull.Value ? Convert.ToInt32(reader["Switch"]) : 0,
                        };
                        item = viewModel;
                    }
                    conn.Close();
                }
            }
            catch (Exception ex)
            {

            }
            return item;
        }

        public List<ZincWatchlistLogsViewModel> UpdateBestBuyForAllPages(ZincWatchLogsSearchViewModel searchViewModel)
        {
            List<ZincWatchlistLogsViewModel> modelLog = new List<ZincWatchlistLogsViewModel>();
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    if (searchViewModel.Available == null)
                    {
                        searchViewModel.Available = "";
                    }
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("P_GetZincWatchlistLogsForSCUpdate", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_ASIN", searchViewModel.ASIN);
                    cmd.Parameters.AddWithValue("_JobID", searchViewModel.JobID);
                    cmd.Parameters.AddWithValue("_Available", searchViewModel.Available);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {

                            while (reader.Read())
                            {
                                ZincWatchlistLogsViewModel ViewModel = new ZincWatchlistLogsViewModel();
                                ViewModel.ProductSKU = Convert.ToString(reader["SKU"]);
                                ViewModel.ASIN = Convert.ToString(reader["ASIN"]);
                                ViewModel.jobID = Convert.ToInt32(reader["JobID"] != DBNull.Value ? reader["JobID"] : "0");
                                ViewModel.BBProductId = Convert.ToString(reader["BBProductId"]);
                                ViewModel.UnitOriginPrice_MSRP = Convert.ToDouble(reader["unit_origin_price_MSRP"] != DBNull.Value ? reader["unit_origin_price_MSRP"] : "0");
                                ViewModel.UnitOriginPrice_Max = Convert.ToDouble(reader["max_price"] != DBNull.Value ? reader["max_price"] : "0");
                                modelLog.Add(ViewModel);
                            }
                        }
                    }
                }
                return modelLog;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public int SaveBestBuyUpdateList(List<ZincWatchlistLogsViewModel> ListViewModel)
        {
            int jobId = 0;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("P_SaveBestBuyUpdateJob", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    jobId = Convert.ToInt32(cmd.ExecuteScalar());
                    conn.Close();

                }
                foreach (var item in ListViewModel)
                {
                    using (MySqlConnection conn = new MySqlConnection(connStr))
                    {
                        conn.Open();
                        MySqlCommand cmd = new MySqlCommand("P_SaveBestBuyUpdateListV1", conn);
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("_JobId", jobId);
                        cmd.Parameters.AddWithValue("_ZincJobId",item.ZincJobId);
                        cmd.Parameters.AddWithValue("_Sku", item.ProductSKU);
                        cmd.Parameters.AddWithValue("_ASIN", item.ASIN);
                        cmd.Parameters.AddWithValue("_ProductId", item.BBProductId);
                        cmd.Parameters.AddWithValue("_MSRP", item.UnitOriginPrice_MSRP);
                        cmd.Parameters.AddWithValue("_UpdateSelllingPrice", item.UnitOriginPrice_Max/100);
                        cmd.ExecuteNonQuery();
                        conn.Close();

                    }
                }
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("P_UpdateBestBuyUpdateJobTotalProduct", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_JobId", jobId);
                    cmd.ExecuteNonQuery();
                    conn.Close();

                }
            }
            catch (Exception ex)
            {

            }
            return jobId;
        }



        public bool UpdateMaxPrice(string ASIN, double MaxPrice)
        {
            bool status = false;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_UpdateMaxPrice", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_ASIN", ASIN);
                    cmd.Parameters.AddWithValue("_MaxPrice", MaxPrice * 100);

                    cmd.ExecuteNonQuery();
                    conn.Close();
                    status = true;
                }
            }
            catch (Exception exp)
            {
            }
            return status;
        }

        public int GetBestBuyUpdateJobId()
        {
            int jobID = 0;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("P_GetBestBuyUpdateJobId", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add("Job_Id", MySqlDbType.Int32, 10);
                    cmd.Parameters["Job_Id"].Direction = System.Data.ParameterDirection.Output;
                    cmd.ExecuteNonQuery();
                    jobID = Convert.ToInt32(cmd.Parameters["Job_Id"].Value != DBNull.Value ? cmd.Parameters["Job_Id"].Value : 0);
                }
            }
            catch (Exception ex)
            {
            }
            return jobID;
        }


        public List<BestBuyUpdatePriceJobViewModel> P_GetBestBuyUpdateListForJob(int JobId)
        {

            List<BestBuyUpdatePriceJobViewModel> listViewModel = new List<BestBuyUpdatePriceJobViewModel>();
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("P_GetBestBuyUpdateListByJobIdV1", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_JobId", JobId);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                BestBuyUpdatePriceJobViewModel ViewModel = new BestBuyUpdatePriceJobViewModel();
                                ViewModel.SKU= Convert.ToString(reader["SKU"] != DBNull.Value ? reader["SKU"] : "");
                                ViewModel.ASIN= Convert.ToString(reader["ASIN"] != DBNull.Value ? reader["ASIN"] : "");
                                ViewModel.ProductId = Convert.ToInt32(reader["ProductId"] != DBNull.Value ? reader["ProductId"] : "");
                                ViewModel.MSRP= Convert.ToDecimal(reader["MSRP"] != DBNull.Value ? reader["MSRP"] : 0);
                                ViewModel.UpdateSelllingPrice = Convert.ToDecimal(reader["UpdateSelllingPrice"] != DBNull.Value ? reader["UpdateSelllingPrice"] : 0);

                                ViewModel.ZincJobID = Convert.ToInt32(reader["ZincJobId"] != DBNull.Value ? reader["ZincJobId"] : 0);
                                listViewModel.Add(ViewModel);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return listViewModel;

        }

        public int BestBuyUpdateJobUpdateEndTime(int jobId)
        {
            
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("P_UpdateBestBuyUpdateJobEndTime", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_JobId", jobId);
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception ex)
            {

            }
            return jobId;
        }

        public int UpdateImportIdInZincLog(UpdateImportIdInZincLogViewModel model)
        {

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("P_UpdateImportIdInZincLog", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_Sku", model.SKU);
                    cmd.Parameters.AddWithValue("_ZincJobId", model.ZincJobID);
                    cmd.Parameters.AddWithValue("_ImportId", model.ImportId);
                    cmd.Parameters.AddWithValue("_UpdatedBB", model.price);
                    cmd.Parameters.AddWithValue("_BBJobID", model.JobID);
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception ex)
            {

            }
            return 0;
        }

        public int SaveBestBuyUpdateLogs(BestBuyUpdatePriceJobViewModel ViewModel,int jobId,string ImportId)
        {
            try
            {
                    using (MySqlConnection conn = new MySqlConnection(connStr))
                    {
                        conn.Open();
                        MySqlCommand cmd = new MySqlCommand("P_SaveBestBuyUpdatelogs", conn);
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("_JobId", jobId);
                        cmd.Parameters.AddWithValue("_Sku", ViewModel.SKU);
                        cmd.Parameters.AddWithValue("_ProductId", ViewModel.ProductId);
                        cmd.Parameters.AddWithValue("_MSRP", ViewModel.MSRP);
                        cmd.Parameters.AddWithValue("_ImportId", ImportId);
                        cmd.Parameters.AddWithValue("_UpdateSellingPrice", ViewModel.UpdateSelllingPrice);
                        cmd.ExecuteNonQuery();
                        conn.Close();

                    }
            }
            catch (Exception ex)
            {

            }
            return 0;
        }
        public ZincWatchlistCountViewModel GetCount(ZincWatchLogsSearchViewModel searchViewModel)
        {
            if (string.IsNullOrEmpty(searchViewModel.JobID) || searchViewModel.JobID == "undefined")
                searchViewModel.JobID = "";
            ZincWatchlistCountViewModel modelview = new ZincWatchlistCountViewModel();
            if (searchViewModel.Available == "Available")
            {
                searchViewModel.IsPrime = 1;
            }
            if (searchViewModel.Available == "Error")
            {
                searchViewModel.IsPrime = 0;
            }
            if (searchViewModel.Available == "Currently Unavailable" || searchViewModel.Available == "Listing Removed")
            {
                searchViewModel.IsPrime = 0;
            }

            if (searchViewModel.Available == null)
            {
                searchViewModel.Available = "";
            }
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    //MySqlCommand cmd = new MySqlCommand("P_GetZincWatchlistLogsCountPrime1", conn);comment by adeel
                    MySqlCommand cmd = new MySqlCommand("P_GetZincWatchlistLogsCountPrime1Copy", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_ASIN", searchViewModel.ASIN);
                    cmd.Parameters.AddWithValue("_JobID", searchViewModel.JobID);
                    cmd.Parameters.AddWithValue("_Available", searchViewModel.Available);
                    cmd.Parameters.AddWithValue("_IsPrime", searchViewModel.IsPrime);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {

                            while (reader.Read())
                            {

                                ZincWatchlistCountViewModel model = new ZincWatchlistCountViewModel();

                                model.Available = Convert.ToInt32(reader["AvailableCount"] != DBNull.Value ? reader["AvailableCount"] : "0");
                                model.UnAvailable = Convert.ToInt32(reader["UnavailableCount"] != DBNull.Value ? reader["UnavailableCount"] : "0");
                                model.TotalCount = Convert.ToInt32(reader["Records"] != DBNull.Value ? reader["Records"] : "0");
                                model.Total = Convert.ToInt32(reader["Total"] != DBNull.Value ? reader["Total"] : "0");
                                model.ListingRemoved = Convert.ToInt32(reader["ListingRemovedCount"] != DBNull.Value ? reader["ListingRemovedCount"] : "0");
                                model.ErrorCount = Convert.ToInt32(reader["ErrorCount"] != DBNull.Value ? reader["ErrorCount"] : "0");
                                modelview = model;

                            }
                        }
                    }
                    return modelview;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public int GetCounterLog(string SC_Order_ID, string Amazon_AcName, string Zinc_Status, string CurrentDate, string PreviousDate)
        {
            int counter = 0;
            try
            {
                if (string.IsNullOrEmpty(SC_Order_ID) || SC_Order_ID == "undefined")
                    SC_Order_ID = "";
                if (string.IsNullOrEmpty(CurrentDate) || SC_Order_ID == "undefined")
                    CurrentDate = "";

                if (string.IsNullOrEmpty(PreviousDate) || SC_Order_ID == "undefined")
                    PreviousDate = "";
                if (string.IsNullOrEmpty(Amazon_AcName) || Amazon_AcName == "undefined")
                    Amazon_AcName = "";
                if (string.IsNullOrEmpty(Zinc_Status) || Zinc_Status == "undefined")
                    Zinc_Status = "";
                

                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("P_GetZincLogCount", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_fromDate", PreviousDate);
                    cmd.Parameters.AddWithValue("_toDate", CurrentDate);
                    cmd.Parameters.AddWithValue("_SC_Order_ID", SC_Order_ID);
                    cmd.Parameters.AddWithValue("_Zinc_Status", Zinc_Status);
                    cmd.Parameters.AddWithValue("_Amazon_AcName", Amazon_AcName);                    
                    counter = Convert.ToInt32(cmd.ExecuteScalar());
                    conn.Close();

                }
            }
            catch (Exception exp)
            {

            }
            return counter;
        }
        public List<ZincOrdersLogViewModel> ZincOrdersLogList(string DateTo, string DateFrom, int limit, int offset, string SC_Order_ID, string Amazon_AcName, string Zinc_Status)
        {
            List<ZincOrdersLogViewModel> listModel = new List<ZincOrdersLogViewModel>();
            try
            {
                if (string.IsNullOrEmpty(SC_Order_ID) || SC_Order_ID == "undefined")
                    SC_Order_ID = "";
                if (string.IsNullOrEmpty(DateFrom) || SC_Order_ID == "undefined")
                    DateFrom = "";

                if (string.IsNullOrEmpty(DateTo) || SC_Order_ID == "undefined")
                    DateTo = "";
                if (string.IsNullOrEmpty(Amazon_AcName) || Amazon_AcName == "undefined")
                    Amazon_AcName = "";
                if (string.IsNullOrEmpty(Zinc_Status) || Zinc_Status == "undefined")
                    Zinc_Status = "";
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("P_GetZincLogCountList", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_fromDate", DateFrom);
                    cmd.Parameters.AddWithValue("_toDate", DateTo);
                    cmd.Parameters.AddWithValue("_SC_Order_ID", SC_Order_ID);
                    cmd.Parameters.AddWithValue("_Zinc_Status", Zinc_Status);
                    cmd.Parameters.AddWithValue("_Amazon_AcName", Amazon_AcName);
                    cmd.Parameters.AddWithValue("_limit", limit);
                    cmd.Parameters.AddWithValue("_offset", offset);
                    
                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    DataTable dataTable = new DataTable();
                    da.Fill(dataTable);
                    if (dataTable.Rows.Count > 0)
                    {
                        listModel = new List<ZincOrdersLogViewModel>();
                        foreach (DataRow dataRow in dataTable.Rows)
                        {
                            ZincOrdersLogViewModel ViewModel = new ZincOrdersLogViewModel();
                            ViewModel.SC_Order_ID = Convert.ToString(dataRow["sc_order_id"] != DBNull.Value ? dataRow["sc_order_id"] : "");
                            ViewModel.BB_Order_ID = Convert.ToString(dataRow["order_id"] != DBNull.Value ? dataRow["order_id"] : "");
                            ViewModel.Zinc_Status = Convert.ToString(dataRow["zinc_order_status_internal"] != DBNull.Value ? dataRow["zinc_order_status_internal"] : "");                           
                            ViewModel.order_datetime = Convert.ToDateTime(dataRow["order_datetime"] != DBNull.Value ? dataRow["order_datetime"] : (DateTime?)null);
                            ViewModel.Zinc_Order_ID = Convert.ToString(dataRow["merchant_order_id"] != DBNull.Value ? dataRow["merchant_order_id"] : "");        
                            ViewModel.Amazon_AcName = Convert.ToString(dataRow["AmzAccountName"] != DBNull.Value ? dataRow["AmzAccountName"] : "");
                            ViewModel.order_message = Convert.ToString(dataRow["order_message"] != DBNull.Value ? dataRow["order_message"] : "");
                            listModel.Add(ViewModel);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return listModel;
        }

        public bool UpdateWatchlistResponse(ZincWatchlistLogsViewModel ViewModel)
        {
            bool status = false;
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("P_ZincWatchlistLogsViewModel", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("_ASIN", ViewModel.ASIN);
                cmd.Parameters.AddWithValue("_ProductSku", ViewModel.ProductSKU);
                cmd.Parameters.AddWithValue("_Response", ViewModel.ZincResponse);

                cmd.ExecuteNonQuery();
                status = true;
            }
            return status;
        }
            public int GetWatchlistLogsCountForJob(string ASIN, string SKU, string available, string jobID, string CurrentDate, string PreviousDate)
            {
                ZincWatchlistLogsForJobViewModel searchViewModel = new ZincWatchlistLogsForJobViewModel();
                int Records = 0;

                if (available == "Available")
                {
                    searchViewModel.IsPrime = 1;
                }

                if (available == "Currently Unavailable" || available == "Listing Removed")
                {
                    searchViewModel.IsPrime = 0;
                }

                if (available == null)
                {
                    available = "";
                }
                if (SKU == null)
                {
                    SKU = "";
                }
                if (ASIN == null)
                {
                    ASIN = "";
                }
                if (string.IsNullOrEmpty(CurrentDate) || ASIN == "undefined")
                    CurrentDate = "";

                if (string.IsNullOrEmpty(PreviousDate) || ASIN == "undefined")
                    PreviousDate = "";
                try
                {
                    using (MySqlConnection conn = new MySqlConnection(connStr))
                    {
                        conn.Open();

                        MySqlCommand cmd = new MySqlCommand("P_GetZincWatchlistLogsCountPrimeForJob", conn);
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("_ASIN", ASIN);
                        cmd.Parameters.AddWithValue("_JobID", jobID);
                        cmd.Parameters.AddWithValue("_Available", available);
                        cmd.Parameters.AddWithValue("_sku", SKU);
                        cmd.Parameters.AddWithValue("_fromDate", PreviousDate);
                        cmd.Parameters.AddWithValue("_toDate", CurrentDate);
                        cmd.Parameters.AddWithValue("_IsPrime", searchViewModel.IsPrime);
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {

                                while (reader.Read())
                                {

                                    Records = Convert.ToInt32(reader["Records"]);

                                }
                            }
                        }
                        return Records;
                    }
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            public List<ZincWatchlistLogsForJobViewModel> ZincOrdersLogListForJob(string DateTo, string DateFrom, int limit, int offset, string ASIN, string SKU, string available, string jobID)
            {
                List<ZincWatchlistLogsForJobViewModel> listModel = new List<ZincWatchlistLogsForJobViewModel>();
                try
                {

                    if (available == "Available")
                    {
                        listModel.FirstOrDefault().IsPrime = 1;
                    }

                    if (available == "Currently Unavailable" || available == "Listing Removed")
                    {
                        listModel.FirstOrDefault().IsPrime = 0;
                    }
                    if (ASIN == null)
                    {
                        ASIN = "";
                    }
                    if (string.IsNullOrEmpty(ASIN) || ASIN == "undefined")
                        ASIN = "";
                    if (available == null)
                    {
                        available = "";
                    }
                    if (SKU == null)
                    {
                        SKU = "";
                    }
                    if (string.IsNullOrEmpty(DateFrom) || ASIN == "undefined")
                        DateFrom = "";

                    if (string.IsNullOrEmpty(DateTo) || ASIN == "undefined")
                        DateTo = "";


                    using (MySqlConnection conn = new MySqlConnection(connStr))
                    {
                        conn.Open();
                        MySqlCommand cmd = new MySqlCommand("P_GetZincWatchlistLogsCountPrimeForJobList", conn);
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("_fromDate", DateFrom);
                        cmd.Parameters.AddWithValue("_toDate", DateTo);
                        cmd.Parameters.AddWithValue("_ASIN", ASIN);
                        cmd.Parameters.AddWithValue("_JobID", jobID);
                        cmd.Parameters.AddWithValue("_Available", available);
                        cmd.Parameters.AddWithValue("_sku", SKU);
                        cmd.Parameters.AddWithValue("_limit", limit);
                        cmd.Parameters.AddWithValue("_offset", offset);

                        MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                        DataTable dataTable = new DataTable();
                        da.Fill(dataTable);
                        if (dataTable.Rows.Count > 0)
                        {
                            listModel = new List<ZincWatchlistLogsForJobViewModel>();
                            foreach (DataRow reader in dataTable.Rows)
                            {
                                ZincWatchlistLogsForJobViewModel ViewModel = new ZincWatchlistLogsForJobViewModel();
                                ViewModel.ASIN = Convert.ToString(reader["ASIN"] != DBNull.Value ? reader["ASIN"] : "");
                                ViewModel.ProductSKU = Convert.ToString(reader["SKU"]);
                                ViewModel.ZincResponse = Convert.ToString(reader["ZincResponse"]);
                                ViewModel.SellerName = Convert.ToString(reader["SellerName"]);
                                ViewModel.Amz_Price = Convert.ToInt32(reader["AMZPrice"] != DBNull.Value ? reader["AMZPrice"] : "0");
                                ViewModel.jobID = Convert.ToInt32(reader["JobID"] != DBNull.Value ? reader["JobID"] : "0");
                                ViewModel.IsPrime = Convert.ToInt32(reader["Prime"] != DBNull.Value ? reader["Prime"] : "0");
                                ViewModel.LastUpdatedDate = Convert.ToDateTime(reader["LastUpdatedDate"] != DBNull.Value ? Convert.ToDateTime(reader["LastUpdatedDate"]) : default(DateTime));
                                ViewModel.FulfilledBY = Convert.ToString(reader["FulfilledBy"]);
                                ViewModel.Compress_image = Convert.ToString(reader["Compress_image"]);
                                ViewModel.image_name = Convert.ToString(reader["image_name"]);

                                listModel.Add(ViewModel);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return listModel;
            }

        public int GetZincWatchlistCount(string ProductSKU, string ASIN, string Active_Inactive,string CurrentDate, string PreviousDate, string Enabled_Disabled)
        {
            int counter = 0;
            
                if (string.IsNullOrEmpty(ProductSKU) || ProductSKU == "undefined")
                    ProductSKU = "";
                if (string.IsNullOrEmpty(CurrentDate) || ASIN == "undefined")
                    CurrentDate = "";

                if (string.IsNullOrEmpty(PreviousDate) || ASIN == "undefined")
                    PreviousDate = "";
                if (string.IsNullOrEmpty(Active_Inactive) || Active_Inactive == "undefined")
                    Active_Inactive = "";
                if (string.IsNullOrEmpty(Enabled_Disabled) || Enabled_Disabled == "undefined")
                    Enabled_Disabled = "";
                if (string.IsNullOrEmpty(ASIN) || ASIN == "undefined")
                    ASIN = "";
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("P_GetWatchlistCount", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_fromDate", PreviousDate);
                    cmd.Parameters.AddWithValue("_toDate", CurrentDate);
                    cmd.Parameters.AddWithValue("_Sku", ProductSKU);
                    cmd.Parameters.AddWithValue("_Asin", ASIN);
                    cmd.Parameters.AddWithValue("_IsActive", Active_Inactive);
                    cmd.Parameters.AddWithValue("_IsEnabled", Enabled_Disabled);
                    counter = Convert.ToInt32(cmd.ExecuteScalar());
                    conn.Close();

                }
            }
            catch (Exception exp)
            {

            }
            return counter;
        }

        public List<ZincWatclistViewModel> ZincWatchListDetail(string DateTo, string DateFrom, int limit, int offset, string ProductSKU, string ASIN, string Active_Inactive, string Enabled_Disabled)
        {
            List<ZincWatclistViewModel> listModel = new List<ZincWatclistViewModel>();
            try
            {
                if (string.IsNullOrEmpty(ProductSKU) || ProductSKU == "undefined")
                    ProductSKU = "";
                if (string.IsNullOrEmpty(DateFrom) || ASIN == "undefined")
                    DateFrom = "";
                if (string.IsNullOrEmpty(DateTo) || ASIN == "undefined")
                    DateTo = "";
                if (string.IsNullOrEmpty(Active_Inactive) || Active_Inactive == "undefined")
                    Active_Inactive = "";
                if (string.IsNullOrEmpty(Enabled_Disabled) || Enabled_Disabled == "undefined")
                    Enabled_Disabled = "";
                if (string.IsNullOrEmpty(ASIN) || ASIN == "undefined")
                    ASIN = "";

               
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("P_GetWatchList", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_fromDate", DateFrom);
                    cmd.Parameters.AddWithValue("_toDate", DateTo);
                    cmd.Parameters.AddWithValue("_Sku", ProductSKU);
                    cmd.Parameters.AddWithValue("_Asin", ASIN);
                    cmd.Parameters.AddWithValue("_IsActive", Active_Inactive);
                    cmd.Parameters.AddWithValue("_IsEnabled", Enabled_Disabled);
                    cmd.Parameters.AddWithValue("_limit", limit);
                    cmd.Parameters.AddWithValue("_offset", offset);
                   
                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    DataTable dataTable = new DataTable();
                    da.Fill(dataTable);
                    if (dataTable.Rows.Count > 0)
                    {
                        listModel = new List<ZincWatclistViewModel>();
                        foreach (DataRow dataRow in dataTable.Rows)
                        {
                            ZincWatclistViewModel ViewModel = new ZincWatclistViewModel();

                            ViewModel.ASIN = Convert.ToString(dataRow["ASIN"] != DBNull.Value ? dataRow["ASIN"] : "");
                            ViewModel.ProductSKU = Convert.ToString(dataRow["ProductSKU"] != DBNull.Value ? dataRow["ProductSKU"] : "");
                            ViewModel.Active_Inactive = Convert.ToString(dataRow["ValidStatus"] != DBNull.Value ? dataRow["ValidStatus"] : "");
                            ViewModel.NextUpdateDate = dataRow["NextUpdateDate"] != DBNull.Value ? Convert.ToDateTime(dataRow["NextUpdateDate"]) : default(DateTime);
                            ViewModel.Compress_image = Convert.ToString(dataRow["Compress_image"]);
                            ViewModel.image_name = Convert.ToString(dataRow["image_name"]);

                            listModel.Add(ViewModel);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return listModel;
        }
        public ZincWatclistLogsViewModel logHistory(string ProductSKU, string ASIN)
        {
            ZincWatclistLogsViewModel ViewModel = new ZincWatclistLogsViewModel();
            List<WatclistLogsHistoryViewModel> ListhistoryViewModel = new List<WatclistLogsHistoryViewModel>();
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("P_LogList", conn);
                    cmd.Parameters.AddWithValue("_Asin", ASIN);
                 
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                           
                            while (reader.Read())
                            {
                                ViewModel.ASIN = Convert.ToString(reader["ASIN"] != DBNull.Value ? reader["ASIN"] : "");
                                ViewModel.ProductSKU = Convert.ToString(reader["ProductSKU"] != DBNull.Value ? reader["ProductSKU"] : "");
                                ViewModel.ValidStatus = Convert.ToInt32(reader["ValidStatus"] != DBNull.Value ? reader["ValidStatus"] : 0);
                                ViewModel.Frequency = Convert.ToInt32(reader["Frequency"] != DBNull.Value ? reader["Frequency"] : 0);
                                ViewModel.Consumed_call = Convert.ToInt32(reader["Consumed_call"] != DBNull.Value ? reader["Consumed_call"] : 0);
                                ViewModel.CheckAfterDays = Convert.ToInt32(reader["CheckAfterDays"] != DBNull.Value ? reader["CheckAfterDays"] : 0);
                                ViewModel.NextUpdateDate = reader["NextUpdateDate"] != DBNull.Value ? Convert.ToDateTime(reader["NextUpdateDate"]) : default(DateTime);
                                ViewModel.ExpiryDate = reader["ExpiryDate"] != DBNull.Value ? Convert.ToDateTime(reader["ExpiryDate"]) : default(DateTime);
                                ViewModel.Compress_image = Convert.ToString(reader["Compress_image"]);
                                ViewModel.image_name = Convert.ToString(reader["image_name"]);
                               
                            }
                           
                            if (ViewModel.ValidStatus == 1)
                            {
                                int totaldays = (ViewModel.Frequency - ViewModel.Consumed_call) * ViewModel.CheckAfterDays;
                                while (totaldays > 0)
                                {
                                    WatclistLogsHistoryViewModel historyViewModel = new WatclistLogsHistoryViewModel();

                                    historyViewModel.ASIN = ViewModel.ASIN;
                                    historyViewModel.ZincResponse = "";
                                    historyViewModel.JobID = 0;
                                    historyViewModel.ProductSKU = ViewModel.ProductSKU;
                                    historyViewModel.NextUpdateDate = ViewModel.ExpiryDate;
                                    historyViewModel.StartTime =  default(DateTime);
                                    historyViewModel.CompletionTime = default(DateTime);
                                    historyViewModel.RunTime = historyViewModel.CompletionTime - historyViewModel.StartTime;

                                    ListhistoryViewModel.Add(historyViewModel);
                                    ViewModel.ExpiryDate = ViewModel.ExpiryDate.AddDays(-ViewModel.CheckAfterDays);
                                    totaldays = totaldays - ViewModel.CheckAfterDays;
                                }
                            }
                          
                        }
                    }
                }
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("P_GetConsumedCallsLogs", conn);
                    cmd.Parameters.AddWithValue("_Asin", ASIN);

                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                WatclistLogsHistoryViewModel historyViewModel = new WatclistLogsHistoryViewModel();

                                historyViewModel.ASIN = Convert.ToString(reader["ASIN"] != DBNull.Value ? reader["ASIN"] : "");
                                historyViewModel.ZincResponse = Convert.ToString(reader["ZincResponse"]);
                                historyViewModel.Remarks = Convert.ToString(reader["Remarks"]);
                                historyViewModel.UpdateOnHLD = Convert.ToString(reader["UpdateOnHLD"]);
                                historyViewModel.JobID = Convert.ToInt32(reader["JobID"] != DBNull.Value ? reader["JobID"] : 0);
                                historyViewModel.ProductSKU = Convert.ToString(reader["SKU"] != DBNull.Value ? reader["SKU"] : "");
                                historyViewModel.NextUpdateDate = reader["StartTime"] != DBNull.Value ? Convert.ToDateTime(reader["StartTime"]) : default(DateTime);
                                historyViewModel.StartTime = reader["StartTime"] != DBNull.Value ? Convert.ToDateTime(reader["StartTime"]) : default(DateTime);
                                historyViewModel.CompletionTime = reader["CompletionTime"] != DBNull.Value ? Convert.ToDateTime(reader["CompletionTime"]) : default(DateTime);
                                historyViewModel.RunTime = historyViewModel.CompletionTime - historyViewModel.StartTime;

                                ListhistoryViewModel.Add(historyViewModel);
                            }
                        }
                    }
                }
                ViewModel.Schedule = ListhistoryViewModel;
            }
            catch (Exception ex)
            {
            }
            return ViewModel;
        }
    }
}
