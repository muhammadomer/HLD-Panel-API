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
   public class FileUploadDataAccess
    {
        public string connStr { get; set; }
        public FileUploadDataAccess(IConnectionString connectionString)
        {
            connStr = connectionString.GetConnectionString();
        }

        public bool SaveFileUpload(FileUploadViewModel ViewModel)
        {
            bool status = false;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_SaveFileUpload", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_file_name", ViewModel.FileName);
                    cmd.Parameters.AddWithValue("_file_type", ViewModel.FileType);
                    cmd.Parameters.AddWithValue("_upload_date", ViewModel.UploadDate);
                    cmd.Parameters.AddWithValue("_file_extension", ViewModel.FileExtension);
                    cmd.ExecuteNonQuery();
                    status = true;
                }
            }
            catch (Exception ex)
            {
            }
            return status;
        }

        public bool SaveFileUploadStatusLog(FileUploadStatusLogViewModel ViewModel)
        {
            bool status = false;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {                 
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_SaveFileUploadLogs", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;                   
                    cmd.Parameters.AddWithValue("_sku", ViewModel.Sku);
                    cmd.Parameters.AddWithValue("_status", ViewModel.Status);
                    cmd.Parameters.AddWithValue("_error_message", ViewModel.ErrorMessage);
                    cmd.ExecuteNonQuery();
                    status = true;
                }
            }
            catch (Exception ex)
            {
            }
            return status;
        }


        public FileUploadViewModel GetFileUpload(string fileName,string fileType)
        {
            FileUploadViewModel fileUploadViewModelfileUploadViewModel = new FileUploadViewModel();
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_GetFileUploadByFileName", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_file_name", fileName);
                    cmd.Parameters.AddWithValue("_file_type", fileType);

                    MySqlDataReader reader=cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        fileUploadViewModelfileUploadViewModel.FileName = Convert.ToString(reader["file_name"] != DBNull.Value ? reader["file_name"] : "");
                        fileUploadViewModelfileUploadViewModel.FileType = Convert.ToString(reader["file_type"] != DBNull.Value ? reader["file_type"] : "");
                        fileUploadViewModelfileUploadViewModel.FileExtension = Convert.ToString(reader["file_extension"] != DBNull.Value ? reader["file_extension"] : "");
                    }

                    return fileUploadViewModelfileUploadViewModel;
                }
            }
            catch (Exception ex)
            {
            }
            return fileUploadViewModelfileUploadViewModel;
        }
        // sku asin mapping 
        public bool AsinSkuMappingDataAccess(List<AsinSkuMappingViewModel> asinSkuMappingViewModel)
        {
            bool status = false;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    
                    foreach (var skulist in asinSkuMappingViewModel)
                    {
                        MySqlCommand cmd = new MySqlCommand("p_SaveAsinSkuMapping", conn);
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("_z_asin_ca", skulist.ASIN);
                        cmd.Parameters.AddWithValue("_amazon_price", skulist.AmzPrice);
                        cmd.Parameters.AddWithValue("_max_price", skulist.MAXPrice);
                        cmd.Parameters.AddWithValue("_product_sku", skulist.SKU);
                        cmd.Parameters.AddWithValue("_updateDate", DateTimeExtensions.ConvertToEST(DateTime.Now));

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



    }
}
