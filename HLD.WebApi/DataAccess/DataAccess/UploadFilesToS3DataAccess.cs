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
    public class UploadFilesToS3DataAccess
    {
        public string connStr { get; set; }
        ProductDataAccess _ProductDataAccess = null;
        public UploadFilesToS3DataAccess(IConnectionString connectionString)
        {
            connStr = connectionString.GetConnectionString();
            _ProductDataAccess = new ProductDataAccess(connectionString);
        }

        public JobIdReturnViewModel SaveFileUploadJobDetail(UploadFilesToS3ViewModel uploadFilesToS3ViewModel)
        {
            JobIdReturnViewModel jobIdReturnViewModel = new JobIdReturnViewModel();
            jobIdReturnViewModel.jobid = 0;
            jobIdReturnViewModel.status = false;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("P_SaveS3FileJobsDetail", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add("Job_Id", MySqlDbType.Int32, 10);
                    cmd.Parameters["Job_Id"].Direction = System.Data.ParameterDirection.Output;
                    cmd.Parameters.AddWithValue("_JobType", uploadFilesToS3ViewModel.JobType);
                    cmd.Parameters.AddWithValue("_Bucket", uploadFilesToS3ViewModel.FilePath);
                    cmd.Parameters.AddWithValue("_File_Name", uploadFilesToS3ViewModel.FileName);

                    cmd.ExecuteNonQuery();
                    jobIdReturnViewModel.jobid = Convert.ToInt32(cmd.Parameters["Job_Id"].Value);



                    jobIdReturnViewModel.status = true;
                }
            }
            catch (Exception ex)
            {
            }
            return jobIdReturnViewModel;
        }


        public List<GetFileJobsToRunViewModel> GetFileJobsToRun()
        {
            List<GetFileJobsToRunViewModel> runViewModel = new List<GetFileJobsToRunViewModel>();
            try

            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("P_GetFileJobsToRun", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {

                            while (reader.Read())
                            {
                                GetFileJobsToRunViewModel model = new GetFileJobsToRunViewModel();
                                model.File_Bucket = Convert.ToString(reader["File_Bucket"] != DBNull.Value ? reader["File_Bucket"] : string.Empty);
                                model.Job_Type = Convert.ToString(reader["Job_Type"] != DBNull.Value ? reader["Job_Type"] : string.Empty);
                                model.File_Name = Convert.ToString(reader["File_Name"] != DBNull.Value ? reader["File_Name"] : string.Empty);

                                model.Job_Id = Convert.ToInt32(reader["Job_Id"] != DBNull.Value ? reader["Job_Id"] : 0);

                                runViewModel.Add(model);
                            }
                        }
                    }
                }
            }


            catch (Exception)
            {

                throw;
            }
            return runViewModel;




        }

        // set job as running
        public bool UpdateFileJobsASRunning(int jobId)
        {
            var timeUtc = DateTime.UtcNow;
            TimeZoneInfo easternZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");

            DateTime AuditDate = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, easternZone);
            bool status = false;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("P_UpdateFileJobStatus", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("JobId", jobId);
                    cmd.Parameters.AddWithValue("_dateTime", AuditDate);

                    cmd.ExecuteNonQuery();
                    status = true;
                }
            }
            catch (Exception ex)
            {
            }
            return status;


        }


        public bool UpdateFileJobsASNotRunning(int jobId)
        {
            var timeUtc = DateTime.UtcNow;
            TimeZoneInfo easternZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");

            DateTime AuditDate = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, easternZone);
            bool status = false;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("P_UpdateFileJobStatusNotRunning", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("JobId", jobId);
                    cmd.Parameters.AddWithValue("_dateTime", AuditDate);

                    cmd.ExecuteNonQuery();
                    status = true;
                }
            }
            catch (Exception ex)
            {
            }
            return status;


        }


        // set job as completed
        public bool UpdateFileJobsASCompleted(int jobId)
        {
            var timeUtc = DateTime.UtcNow;
            TimeZoneInfo easternZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");

            DateTime AuditDate = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, easternZone);
            bool status = false;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("P_UpdateFileJobStatusAsCompleted", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("JobId", jobId);
                    cmd.Parameters.AddWithValue("_dateTime", AuditDate);

                    cmd.ExecuteNonQuery();


                    status = true;
                }
            }
            catch (Exception ex)
            {
            }
            return status;


        }


        public bool UpdateProductDropshipStatusAndQtyForCommentsJobs(UpdateQtyCommentsByJobViewModel viewModel, bool dropshipstatus)
        {
            bool status = false;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_UpdateSkuDropshipStatusAndQty", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_dropship_status", dropshipstatus);
                    cmd.Parameters.AddWithValue("_dropship_qty", Convert.ToInt32(viewModel.dropship_Qty));
                    cmd.Parameters.AddWithValue("_sku", viewModel.ShopSKU_OfferSKU);
                    cmd.Parameters.AddWithValue("_comments", viewModel.DropshipComments);

                    cmd.ExecuteNonQuery();
                    status = true;
                }
            }
            catch (Exception ex)
            {
            }
            return status;
        }

        public bool InsertS3BestBuyMIssingSKUFromSellerCloud(int jobid, List<ImportMissingSkuViewModel> updateQtyCommentsByJob)
        {
            bool st = false;
            try
            {
                List<FileJobLogsViewModel> listModel = new List<FileJobLogsViewModel>();

                int Success = 0;
                int Fail = 0;
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    foreach (var item in updateQtyCommentsByJob)
                    {

                        if (!String.IsNullOrEmpty(item.Product_Sku))

                        {

                            int productID = _ProductDataAccess.GetProductIdBySKU(item.Product_Sku);
                            if (productID > 0)
                            {
                                Fail++;
                                InsertJobLog(Success, Fail, item.Product_Sku, Convert.ToInt32(item.RowNumber), "SKU already exist.", jobid);


                            }
                            else
                            {



                                Success++;
                                InsertJobLog(Success, Fail, "", 0, "", jobid);

                            }


                        }


                        else
                        {

                            Fail++;
                            InsertJobLog(Success, Fail, item.Product_Sku, Convert.ToInt32(item.RowNumber), " Contain invalid data in file.", jobid);

                        }


                    }
                    st = true;


                }
            }
            catch (Exception)
            {


            }
            return st;

        }
        public bool InsertS3BestBuySKUQtyComments(int jobid, List<UpdateQtyCommentsByJobViewModel> updateQtyCommentsByJob)
        {
            bool st = false;
            try
            {
                List<FileJobLogsViewModel> listModel = new List<FileJobLogsViewModel>();

                int Success = 0;
                int Fail = 0;
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    foreach (var item in updateQtyCommentsByJob)
                    {
                        bool dropshipstatus = false;
                        if (!String.IsNullOrEmpty(item.ShopSKU_OfferSKU) &&
                            !String.IsNullOrEmpty(item.dropship_status) && !String.IsNullOrEmpty(item.dropship_Qty) &&
                            !String.IsNullOrEmpty(item.DropshipComments)
                            )
                        {
                            if (item.dropship_status.ToLower().Trim().Equals("enabled"))
                            {
                                dropshipstatus = true;
                            }
                            else
                            {
                                dropshipstatus = false;
                            }
                            bool status = UpdateProductDropshipStatusAndQtyForCommentsJobs(item, dropshipstatus);
                            if (status == true)
                            {
                                MySqlCommand mySqlCommand = new MySqlCommand("p_SaveBestBuyQtyMovementForDropshipNone_SKU", conn);
                                mySqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                                mySqlCommand.Parameters.AddWithValue("_product_sku", item.ShopSKU_OfferSKU);
                                mySqlCommand.Parameters.AddWithValue("_ds_status", dropshipstatus);
                                mySqlCommand.Parameters.AddWithValue("_ds_qty", Convert.ToInt32(item.dropship_Qty));
                                mySqlCommand.Parameters.AddWithValue("_order_date", DateTime.Now);
                                mySqlCommand.Parameters.AddWithValue("_dropshipComments", item.DropshipComments);

                                mySqlCommand.ExecuteNonQuery();

                                Success++;
                                InsertJobLog(Success, Fail, "", 0, "", jobid);
                            }
                            else
                            {
                                Fail++;
                                InsertJobLog(Success, Fail, item.ShopSKU_OfferSKU, Convert.ToInt32(item.RowNumber), "invalid Sku.", jobid);

                            }


                        }


                        else
                        {

                            Fail++;
                            InsertJobLog(Success, Fail, item.ShopSKU_OfferSKU, Convert.ToInt32(item.RowNumber), " Contain invalid data in file.", jobid);

                        }


                    }
                    st = true;


                }
            }
            catch (Exception)
            {


            }
            return st;

        }
        public bool InsertS3FileDataOfSkuAsinDropship(int jobid, List<UpdateAsinSkuDropShipDataJobViewModel> SkuAsinmodel)
        {
            bool st = false;
            try
            {
                List<FileJobLogsViewModel> listModel = new List<FileJobLogsViewModel>();

                int Success = 0;
                int Fail = 0;
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    foreach (var item in SkuAsinmodel)
                    {
                        bool dropshipstatus = false;
                        if (!String.IsNullOrEmpty(item.SKU) && !String.IsNullOrEmpty(item.ASIN) &&
                            !String.IsNullOrEmpty(item.AmzPrice) && !String.IsNullOrEmpty(item.MAXPrice) &&
                            !String.IsNullOrEmpty(item.DropShip) && !String.IsNullOrEmpty(item.DropShipQty) &&
                            !String.IsNullOrEmpty(item.DropShipComments) && !String.IsNullOrEmpty(item.AvgCost)
                            )
                        {
                            if (item.DropShip.ToLower().Trim().Equals("enabled"))
                            {
                                dropshipstatus = true;
                            }
                            else
                            {
                                dropshipstatus = false;
                            }
                            bool status = UpdateProductDropshipStatusAndQtyWithWxcellJob(item);
                            if (status == true)
                            {
                                MySqlCommand mySqlCommand = new MySqlCommand("P_SaveSkuAsinDropShipQtyFromS3FileJob", conn);
                                mySqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                                mySqlCommand.Parameters.AddWithValue("_product_sku", item.SKU);
                                mySqlCommand.Parameters.AddWithValue("_ds_status", dropshipstatus);
                                mySqlCommand.Parameters.AddWithValue("_ds_qty", Convert.ToInt32(item.DropShipQty));
                                mySqlCommand.Parameters.AddWithValue("_order_date", DateTimeExtensions.ConvertToEST(DateTime.Now));
                                mySqlCommand.Parameters.AddWithValue("_dropshipComments", item.DropShipComments);

                                mySqlCommand.Parameters.AddWithValue("_z_asin_ca", item.ASIN);
                                mySqlCommand.Parameters.AddWithValue("_amazon_price", Convert.ToDecimal(item.AmzPrice) * 100);
                                mySqlCommand.Parameters.AddWithValue("_max_price", Convert.ToDecimal(item.MAXPrice) * 100);

                                mySqlCommand.Parameters.AddWithValue("_updateDate", DateTimeExtensions.ConvertToEST(DateTime.Now));

                                mySqlCommand.ExecuteNonQuery();

                                Success++;
                                InsertJobLog(Success, Fail, "", 0, "", jobid);
                            }
                            else
                            {
                                Fail++;
                                InsertJobLog(Success, Fail, item.SKU, Convert.ToInt32(item.RowNumber), "invalid Sku.", jobid);

                            }


                        }


                        else
                        {

                            Fail++;
                            InsertJobLog(Success, Fail, item.SKU, Convert.ToInt32(item.RowNumber), " Contain invalid data in file.", jobid);

                        }


                    }
                    st = true;


                }
            }
            catch (Exception)
            {


            }
            return st;


        }


        public bool InsertS3FileDataOfInventory(int jobid, List<ProductContinueDisContinueViewModel> SKUlist)
        {
            bool st = false;
            try
            {
                List<FileJobLogsViewModel> listModel = new List<FileJobLogsViewModel>();

                int Success = 0;
                int Fail = 0;
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    foreach (var item in SKUlist)
                    {
                        if (!String.IsNullOrEmpty(item.SKU))
                        {
                            MySqlCommand mySqlCommand = new MySqlCommand("P_UpdateProductWithSKU", conn);
                            mySqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                            mySqlCommand.Parameters.AddWithValue("_SKU", item.SKU);
                            mySqlCommand.Parameters.AddWithValue("_flag", item.Continue);
                            mySqlCommand.ExecuteNonQuery();
                            Success++;
                            InsertJobLog(Success, Fail, "", 0, "", jobid);
                        }
                        else
                        {
                            Fail++;
                            InsertJobLog(Success, Fail, item.SKU, Convert.ToInt32(item.RowNumber), " Contain invalid data in file.", jobid);
                        }
                    }
                    st = true;
                }
            }
            catch (Exception)
            {


            }
            return st;


        }

        public bool InsertS3FileDataOfPOProduct(int jobid, List<SaveApprovedPricesViewModel> SkuAsinmodel)
        {
            bool st = false;
            try
            {
                List<FileJobLogsViewModel> listModel = new List<FileJobLogsViewModel>();

                int Success = 0;
                int Fail = 0;
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    foreach (var item in SkuAsinmodel)
                    {

                        if (!String.IsNullOrEmpty(item.SKU) && !String.IsNullOrEmpty(item.Currency) &&
                            !String.IsNullOrEmpty(item.Vendor) && !String.IsNullOrEmpty(item.ApprovedPrice)
                            )
                        {

                            int status = GetProductIdBySKUForJobs(item.SKU);
                            if (status > 0)
                            {
                                MySqlCommand mySqlCommand = new MySqlCommand("P_SaveApprovedPriceByJob", conn);
                                mySqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                                mySqlCommand.Parameters.AddWithValue("_SKU", item.SKU);
                                mySqlCommand.Parameters.AddWithValue("_VendorAlias", item.Vendor);
                                mySqlCommand.Parameters.AddWithValue("_ApprovedUnitPrice", Convert.ToDecimal(item.ApprovedPrice));
                                mySqlCommand.Parameters.AddWithValue("_Currency", item.Currency);

                                mySqlCommand.ExecuteNonQuery();

                                Success++;
                                InsertJobLog(Success, Fail, "", 0, "", jobid);
                            }
                            else
                            {
                                Fail++;
                                InsertJobLog(Success, Fail, item.SKU, Convert.ToInt32(item.RowNumber), "Invalid SKU.", jobid);

                            }


                        }


                        else
                        {

                            Fail++;
                            InsertJobLog(Success, Fail, item.SKU, Convert.ToInt32(item.RowNumber), " Contain invalid data in file.", jobid);

                        }


                    }
                    st = true;


                }
            }
            catch (Exception)
            {


            }
            return st;


        }

        public bool InsertJobLog(int success, int fail, string sku, int row, string message, int jobid)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();


                    MySqlCommand mySqlCommand = new MySqlCommand("P_SaveSkuASinJobLog", conn);
                    mySqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    mySqlCommand.Parameters.AddWithValue("_success", success);
                    mySqlCommand.Parameters.AddWithValue("_fail", fail);
                    mySqlCommand.Parameters.AddWithValue("_jobid", jobid);
                    mySqlCommand.Parameters.AddWithValue("_Sku", sku);
                    mySqlCommand.Parameters.AddWithValue("_Row", row);
                    mySqlCommand.Parameters.AddWithValue("_message", message);

                    mySqlCommand.ExecuteNonQuery();



                }


                return true;


            }
            catch (Exception ex)
            {

                throw;
            }



        }

        public List<GetJobDetailViewModel> GetJobsOfS3()
        {
            List<GetJobDetailViewModel> listModel = new List<GetJobDetailViewModel>();

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(@"SELECT * FROM bestBuyE2.S3FileUploadJobsDetails order by job_id desc;", conn);
                    cmd.CommandType = System.Data.CommandType.Text;
                    using (var dr = cmd.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                GetJobDetailViewModel model = new GetJobDetailViewModel();
                                model.Job_Type = Convert.ToString(dr["Job_Type"] != DBNull.Value ? dr["Job_Type"].ToString() : "");
                                model.File_Name = Convert.ToString(dr["File_Name"] != DBNull.Value ? dr["File_Name"].ToString() : "");
                                model.Status = Convert.ToInt32(dr["Status"] != DBNull.Value ? dr["Status"].ToString() : "");
                                model.Running = Convert.ToInt32(dr["Running"] != DBNull.Value ? dr["Running"].ToString() : "");
                                model.Job_Start = Convert.ToString(dr["Job_Start"] != DBNull.Value ? dr["Job_Start"] : "");
                                model.Job_Completed = Convert.ToString(dr["Job_Completed"] != DBNull.Value ? dr["Job_Completed"] : "");
                                model.File_Bucket = Convert.ToString(dr["File_Bucket"] != DBNull.Value ? dr["File_Bucket"].ToString() : "");

                                model.Job_Id = Convert.ToInt32(dr["Job_Id"] != DBNull.Value ? dr["Job_Id"].ToString() : "");

                                listModel.Add(model);
                            }
                        }
                    }



                    //            MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter(cmd);
                    //DataTable dt = new DataTable();
                    //mySqlDataAdapter.Fill(dt);

                    //if (dt.Rows.Count > 0)
                    //{

                    //    foreach (DataRow dr in dt.Rows)
                    //    {
                    //        GetJobDetailViewModel model = new GetJobDetailViewModel();
                    //        model.Job_Type = Convert.ToString(dr["Job_Type"] != DBNull.Value ? dr["Job_Type"].ToString() : "");
                    //        model.File_Name = Convert.ToString(dr["File_Name"] != DBNull.Value ? dr["File_Name"].ToString() : "");
                    //        model.Status = Convert.ToInt32(dr["Status"] != DBNull.Value ? dr["Status"].ToString() : "");
                    //        model.Running = Convert.ToInt32(dr["Running"] != DBNull.Value ? dr["Running"].ToString() : "");
                    //        model.Job_Start = Convert.ToString(dr["Job_Start"] != DBNull.Value ? dr["Job_Start"] : "");
                    //        model.Job_Completed = Convert.ToString(dr["Job_Completed"] != DBNull.Value ? dr["Job_Completed"] : "");
                    //        model.File_Bucket = Convert.ToString(dr["File_Bucket"] != DBNull.Value ? dr["File_Bucket"].ToString() : "");

                    //        model.Job_Id = Convert.ToInt32(dr["Job_Id"] != DBNull.Value ? dr["Job_Id"].ToString() : "");

                    //        listModel.Add(model);
                    //    }
                    //}
                }
            }
            catch (Exception ex)
            {
            }
            return listModel;
        }

        public S3LogViewModel GetS3JobLogsDetail(int jobId)
        {
            S3LogViewModel s3LogView = new S3LogViewModel();
            try
            {
                System.Data.DataSet ds = new System.Data.DataSet();
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {

                    MySqlCommand cmd = new MySqlCommand("P_GetS3JobLogsDetail", conn);

                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("Job_id", jobId);

                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    da.Fill(ds);

                    System.Data.DataView dataView = new System.Data.DataView(ds.Tables[0]);
                    System.Data.DataTable distinctValue = dataView.ToTable(true, "Job_id");
                    DataTable dt = ds.Tables[0];
                    foreach (System.Data.DataRow reader in distinctValue.Rows)
                    {
                        List<Messagelog> MessageList = new List<Messagelog>();

                        var list = dt.AsEnumerable().Where(e => e.Field<int>("Job_id") == Convert.ToInt32(reader["Job_id"])).ToList();

                        s3LogView.job_id = Convert.ToInt32(list.Select(e => e.Field<int>("Job_Id")).FirstOrDefault());
                        s3LogView.Success = Convert.ToInt32(list.Select(e => e.Field<int>("Success")).FirstOrDefault());
                        s3LogView.Fail = Convert.ToInt32(list.Select(e => e.Field<int>("Fail")).FirstOrDefault());

                        foreach (DataRow dataRow in list)
                        {

                            Messagelog messagedetail = new Messagelog();

                            messagedetail.message = Convert.ToString(dataRow["message"] != DBNull.Value ? dataRow["message"] : string.Empty);
                            messagedetail.sku = Convert.ToString(dataRow["SKU"] != DBNull.Value ? dataRow["SKU"] : string.Empty);
                            messagedetail.row = Convert.ToInt32(dataRow["Row"] != DBNull.Value ? dataRow["Row"] : string.Empty);

                            MessageList.Add(messagedetail);


                        }


                        s3LogView.message = MessageList;


                    }
                }
            }
            catch (Exception ex)
            {
            }

            return s3LogView;

        }

        public bool UpdateProductDropshipStatusAndQtyWithWxcellJob(UpdateAsinSkuDropShipDataJobViewModel viewModel)
        {
            bool status = false;
            bool dropshipstatus = false;
            int id = 0;
            try
            {
                id = GetProductIdBySKUForJobs(viewModel.SKU);

                if (id != 0)
                {
                    if (viewModel.DropShip.ToLower().Trim().Equals("enabled"))
                    {
                        dropshipstatus = true;
                    }
                    else
                    {
                        dropshipstatus = false;
                    }


                    using (MySqlConnection conn = new MySqlConnection(connStr))
                    {
                        conn.Open();
                        MySqlCommand cmd = new MySqlCommand("p_UpdateSkuDropshipStatusAndQtyForJobs", conn);
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("_dropship_status", dropshipstatus);
                        cmd.Parameters.AddWithValue("_dropship_qty", viewModel.DropShipQty);
                        cmd.Parameters.AddWithValue("_sku", viewModel.SKU);
                        cmd.Parameters.AddWithValue("_AvgCost", Convert.ToDecimal(viewModel.AvgCost));
                        cmd.Parameters.AddWithValue("_comments", viewModel.DropShipComments);

                        cmd.ExecuteNonQuery();
                        status = true;
                    }

                }


            }
            catch (Exception ex)
            {
            }
            return status;
        }


        public int GetProductIdBySKUForJobs(string Sku)
        {
            int productId = 0;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_GetProductIDFromProductSku", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("ProductSku", Sku);
                    cmd.Parameters.Add("ProductId", MySqlDbType.Int32, 10);
                    cmd.Parameters["ProductId"].Direction = ParameterDirection.Output;
                    cmd.ExecuteNonQuery();
                    productId = Convert.ToInt32(cmd.Parameters["ProductId"].Value != DBNull.Value ? cmd.Parameters["ProductId"].Value : "0");

                }
            }
            catch (Exception ex)
            {

            }
            return productId;
        }


    }
}

