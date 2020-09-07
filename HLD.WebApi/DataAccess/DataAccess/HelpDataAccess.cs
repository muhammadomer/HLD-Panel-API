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
   public class HelpDataAccess
    {
        public string connStr { get; set; }
        public HelpDataAccess(IConnectionString connectionString)
        {
            connStr = connectionString.GetConnectionString();
        }
        public bool SaveEdirData(PostDataViewModel ViewModel)
        {

            bool status = false;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("sp_PostEditorSave", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("PostEditor", ViewModel.postdata);
                    cmd.Parameters.AddWithValue("Post_date", ViewModel.Post_date);
                    cmd.Parameters.AddWithValue("PostTitle", ViewModel.posttitle);
                    cmd.Parameters.AddWithValue("_catagorid", ViewModel.catagoryid);
                    cmd.ExecuteNonQuery();

                }
                status = true;


            }
            catch (Exception ex)
            {
            }
            return status;
        }
        public List<PostDataViewModel> GetEditorData()
        {
            List<PostDataViewModel> listModel = null;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmdd = new MySqlCommand(@"SELECT * FROM bestBuyE2.PostEditor;", conn);
                    cmdd.CommandType = System.Data.CommandType.Text;
                    MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter(cmdd);
                    DataTable dt = new DataTable();
                    mySqlDataAdapter.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        listModel = new List<PostDataViewModel>();
                        foreach (DataRow dr in dt.Rows)
                        {
                            PostDataViewModel model = new PostDataViewModel();
                            model.idPostEditor = Convert.ToInt32(dr["idPostEditor"]);
                           
                            model.postdata = Convert.ToString(dr["PostEditor"]);
                            model.posttitle = Convert.ToString(dr["PostTitle"]);
                            model.Post_date = Convert.ToDateTime(dr["Post_date"] != DBNull.Value ? dr["Post_date"] : (DateTime?)null);

                           
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
        public bool DeleteData(int id)
        {
            bool status = false;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_DeleteEditorDataById", conn);
                    cmd.Parameters.AddWithValue("_idPostEditor", id);
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
        public PostDataViewModel UpdateEditorData(int id)
        {
            PostDataViewModel model = new PostDataViewModel();
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_GetEditorDataById", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_idPostEditor",id);
                    MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter(cmd);
                    cmd.ExecuteNonQuery();
                    DataTable dt = new DataTable();
                    mySqlDataAdapter.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                       
                        foreach (DataRow dr in dt.Rows)
                        {
                            PostDataViewModel modelview = new PostDataViewModel();
                            modelview.idPostEditor = Convert.ToInt32(dr["idPostEditor"]);
                            modelview.postdata = Convert.ToString(dr["PostEditor"]);
                            modelview.posttitle = Convert.ToString(dr["PostTitle"]);
                            modelview.Post_date = Convert.ToDateTime(dr["Post_date"] != DBNull.Value ? dr["Post_date"] : (DateTime?)null);
                            model = modelview;
                        }
                    }

                }
            }
            catch (Exception ex)
            {
            }
            return model;
        }
        public bool UpdateData(PostDataViewModel ViewModel)
        {
            bool status = false;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_UpdateEditerData", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_idPostEditor", ViewModel.idPostEditor);
                    cmd.Parameters.AddWithValue("PostEditor", ViewModel.postdata);
                    cmd.Parameters.AddWithValue("_PostTitle", ViewModel.posttitle);
                    cmd.Parameters.AddWithValue("Post_date", ViewModel.Post_date);
                    cmd.ExecuteNonQuery();
                    status = true;
                }
            }
            catch (Exception ex)
            {
            }
            return status;
        }
        public List<PostData> GetCatagory()
        {
            List<PostData> listviewModel = new List<PostData>();
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("s_GetPostData", conn);

                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {

                            while (reader.Read())
                            {
                                PostData ViewModel = new PostData();
                                ViewModel.catagoryid = Convert.ToInt32(reader["idCatagory"]);
                                ViewModel.catagory = Convert.ToString(reader["Catagory"]);

                                listviewModel.Add(ViewModel);
                            }
                        }
                    }
                }
            }
            catch (Exception exp)
            {
            }
            return listviewModel;
        }
        public List<PostDataViewModel> GetDataByCatagory(int catagoryid)
        {
            List<PostDataViewModel> listModel = null;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmdd = new MySqlCommand("p_GetDataById", conn);
                    cmdd.Parameters.AddWithValue("_idCatagory", catagoryid);
                    cmdd.CommandType = System.Data.CommandType.StoredProcedure;
                    MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter(cmdd);
                    DataTable dt = new DataTable();
                    mySqlDataAdapter.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        listModel = new List<PostDataViewModel>();
                        foreach (DataRow dr in dt.Rows)
                        {
                            PostDataViewModel model = new PostDataViewModel();
                            model.postdata = Convert.ToString(dr["PostEditor"]);
                            model.posttitle = Convert.ToString(dr["PostTitle"]);
                           
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

        public List<PostDataViewModel> GetDataByCatagoryByTitle(int catagoryidByTitle)
        {
            List<PostDataViewModel> listModel = null;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmdd = new MySqlCommand("s_GetPostDatabyTitle", conn);
                    cmdd.Parameters.AddWithValue("_idCatagory", catagoryidByTitle);
                    cmdd.CommandType = System.Data.CommandType.StoredProcedure;
                    MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter(cmdd);
                    DataTable dt = new DataTable();
                    mySqlDataAdapter.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        listModel = new List<PostDataViewModel>();
                        foreach (DataRow dr in dt.Rows)
                        {
                            PostDataViewModel model = new PostDataViewModel();
                            model.idPostEditor = Convert.ToInt32(dr["idPostEditor"]);
                            model.posttitle = Convert.ToString(dr["PostTitle"]);

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
        public List<PostDataViewModel> GetDataByCatagoryByTitleSearch(string title)
        {
            List<PostDataViewModel> listModel = null;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmdd = new MySqlCommand("p_GetDataByIdProductTitle", conn);
                    cmdd.Parameters.AddWithValue("_PostTitle", title);
                    cmdd.CommandType = System.Data.CommandType.StoredProcedure;
                    MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter(cmdd);
                    DataTable dt = new DataTable();
                    mySqlDataAdapter.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        listModel = new List<PostDataViewModel>();
                        foreach (DataRow dr in dt.Rows)
                        {
                            PostDataViewModel model = new PostDataViewModel();
                          
                            model.posttitle = Convert.ToString(dr["PostTitle"]);
                            model.postdata = Convert.ToString(dr["PostEditor"]);

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
    }
}
