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
   public class OrderRelationDataAccess
    {
        public string connStr { get; set; }
        public OrderRelationDataAccess(IConnectionString connectionString)
        {
            connStr = connectionString.GetConnectionString();
        }

        public bool InsertOrderRelation(OrderRelationViewModel viewModel)
        {
            bool status = false;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("P_SaveOrderRelation", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("SC_ParentID", viewModel.SC_ParentID);
                    cmd.Parameters.AddWithValue("SC_ChildID", viewModel.SC_ChildID);
                    cmd.Parameters.AddWithValue("BB_OrderID", viewModel.BB_OrderID);
                    cmd.ExecuteNonQuery();
                    status = true;
                }
            }
            catch (Exception ex)
            {
            }
            return status;
        }

        public JobIdReturnViewModel InsertChildOrderForJob(List<OrderRelationToSaveViewModel> viewModel)
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
                    cmd.Parameters.AddWithValue("_JobType", "ImportChildOrderSC");
                    cmd.Parameters.AddWithValue("_Bucket", "");
                    cmd.Parameters.AddWithValue("_File_Name", "");

                    cmd.ExecuteNonQuery();
                    jobIdReturnViewModel.jobid = Convert.ToInt32(cmd.Parameters["Job_Id"].Value);
                    
                    jobIdReturnViewModel.status = true;

                    foreach (var item in viewModel)
                    {
                        MySqlCommand cmd1 = new MySqlCommand("p_SaveOrderforJob", conn);
                        cmd1.CommandType = System.Data.CommandType.StoredProcedure;

                        cmd1.Parameters.AddWithValue("SC_ChildID", item.SC_ChildID);
                        cmd1.Parameters.AddWithValue("JobId", jobIdReturnViewModel.jobid);

                        cmd1.ExecuteNonQuery();
                    }

                   
                }
            }
            catch (Exception ex)
            {
            }
            return jobIdReturnViewModel;
        }


        public List<GetChildORderToimportJobViewModel> GetChildOrderToImport(int JobID)
        {
            List<GetChildORderToimportJobViewModel> listModel = null;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmdd = new MySqlCommand(@"SELECT `ParentChildOrderForJobs`.`ID`,`ParentChildOrderForJobs`.`SC_ChildID` , `ParentChildOrderForJobs`.`ImportStatus` FROM `bestBuyE2`.`ParentChildOrderForJobs` Where  `ParentChildOrderForJobs`.`JobId` =" + "'" + JobID + "'", conn);
                    cmdd.CommandType = System.Data.CommandType.Text;
                    MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter(cmdd);
                    DataTable dt = new DataTable();
                    mySqlDataAdapter.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        listModel = new List<GetChildORderToimportJobViewModel>();
                        foreach (DataRow dr in dt.Rows)
                        {
                            GetChildORderToimportJobViewModel model = new GetChildORderToimportJobViewModel();
                            model.ImportStatus = Convert.ToString(dr["ImportStatus"]);
                            model.SC_ChildID = Convert.ToInt32(dr["SC_ChildID"]);
                            model.ID = Convert.ToInt32(dr["ID"]);
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

        public bool UpdateOrderOrderRelationAsImported(int  jobID)
        {
            bool status = false;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("P_UpdateChildOrdersAsImported", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("JobId", jobID);
                   
                    cmd.ExecuteNonQuery();
                    status = true;
                }
            }
            catch (Exception ex)
            {
            }
            return status;
        }

        public List<RelatedOrderModel> GetChildOrderByParentID(int OrderId)
        {
            List<RelatedOrderModel> relatedOrder = null;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmdd = new MySqlCommand(@"SELECT SC_ChildID FROM bestBuyE2.ParentChildOrderMapping WHERE SC_ParentID=" + OrderId, conn);
                    cmdd.CommandType = System.Data.CommandType.Text;
                    MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter(cmdd);
                    DataTable dt = new DataTable();
                    mySqlDataAdapter.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        relatedOrder = new List<RelatedOrderModel>();
                        foreach (DataRow dr in dt.Rows)
                        {
                            RelatedOrderModel model = new RelatedOrderModel();
                            model.OrderId = Convert.ToInt32(dr["SC_ChildID"]);
                            relatedOrder.Add(model);
                        }
                    }
                }
                return relatedOrder;
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
