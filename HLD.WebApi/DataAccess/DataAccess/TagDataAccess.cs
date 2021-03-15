using DataAccess.Helper;
using DataAccess.ViewModels;
using Hld.WebApi.ViewModel;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DataAccess
{
    public class TagDataAccess
    {
        public string connStr { get; set; }
        public TagDataAccess(IConnectionString connectionString)
        {
            connStr = connectionString.GetConnectionString();
        }

        public bool SaveTag(TagViewModel tagViewModel)
        {

            bool status = false;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_SaveTags", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_Tag", tagViewModel.TagName);
                    cmd.Parameters.AddWithValue("_Tag_Color", tagViewModel.TagColor);
                    cmd.Parameters.AddWithValue("_Tag_id", tagViewModel.TagId);
                    cmd.ExecuteNonQuery();

                }
                status = true;


            }
            catch (Exception ex)
            {
            }
            return status;
        }

        public List<TagViewModel> GetTag()
        {
            List<TagViewModel> listModel = null;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmdd = new MySqlCommand(@"SELECT * FROM bestBuyE2.Tags;", conn);
                    cmdd.CommandType = System.Data.CommandType.Text;
                    MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter(cmdd);
                    DataTable dt = new DataTable();
                    mySqlDataAdapter.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        listModel = new List<TagViewModel>();
                        foreach (DataRow dr in dt.Rows)
                        {
                            TagViewModel model = new TagViewModel();
                            model.TagColor = Convert.ToString(dr["tag_color"]);
                            model.TagName = Convert.ToString(dr["tag"]);
                            model.TagId = Convert.ToInt32(dr["Tag_id"]);
                            listModel.Add(model);
                        }
                    }


                }
                return listModel;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public TagViewModel GetTagById(int id)
        {
            TagViewModel model = null;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmdd = new MySqlCommand(@"SELECT * FROM bestBuyE2.Tags where Tag_id=" + id, conn);
                    cmdd.CommandType = System.Data.CommandType.Text;
                    using (var reader = cmdd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                model = new TagViewModel();
                                model.TagColor = Convert.ToString(reader["tag_color"]);
                                model.TagName = Convert.ToString(reader["tag"]);
                                model.TagId = Convert.ToInt32(reader["Tag_id"]);

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

        public bool RemoveTag(AssignTagViewModel tagViewModel)
        {

            bool status = false;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();

                    foreach (var listitem in tagViewModel.tags)
                    {
                        MySqlCommand cmd = new MySqlCommand("P_RemoveTagFromSku", conn);
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("_Sku", tagViewModel.SKu);
                        cmd.Parameters.AddWithValue("_Tag", listitem.TagId);
                        cmd.ExecuteNonQuery();
                    }


                }
                status = true;


            }
            catch (Exception ex)
            {
            }
            return status;
        }

        public bool AssignTag(List<AssignTagViewModel> tagViewModel)
        {

            bool status = false;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    foreach (var item in tagViewModel)
                    {
                        foreach (var listitem in item.tags)
                        {
                            MySqlCommand cmd = new MySqlCommand("p_SaveSkuTagsMapping", conn);
                            cmd.CommandType = System.Data.CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("_sku", item.SKu);
                            cmd.Parameters.AddWithValue("_Tag_id", listitem.TagId);
                            cmd.ExecuteNonQuery();
                        }
                    }
                  
                   

                }
                status = true;


            }
            catch (Exception ex)
            {
            }
            return status;
        }


        public List<SkuTagOrderViewModel> GetTagforSku(string sku)
        {
            List<SkuTagOrderViewModel> listModel = null;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmdd = new MySqlCommand(@"SELECT Sku_Tag.sku,Sku_Tag.tag_id,Tags.tag_color,Tags.tag FROM bestBuyE2.Sku_Tag inner join Tags on Sku_Tag.tag_id = Tags.tag_id where sku=" + "'" + sku + "'" , conn);
                    cmdd.CommandType = System.Data.CommandType.Text;
                    MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter(cmdd);
                    DataTable dt = new DataTable();
                    mySqlDataAdapter.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        listModel = new List<SkuTagOrderViewModel>();
                        foreach (DataRow dr in dt.Rows)
                        {
                            SkuTagOrderViewModel model = new SkuTagOrderViewModel();
                            model.TagColor = Convert.ToString(dr["tag_color"]);
                            model.TagName = Convert.ToString(dr["tag"]);
                            model.TagId = Convert.ToInt32(dr["tag_id"]);
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

        public List<SkuTagOrderViewModel> GetTagforSkubulk(string sku, MySqlConnection conn)
        {
            List<SkuTagOrderViewModel> listModel = null;
            try
            {
               
                    MySqlCommand cmdd = new MySqlCommand(@"SELECT Sku_Tag.sku,Sku_Tag.tag_id,Tags.tag_color,Tags.tag FROM bestBuyE2.Sku_Tag inner join Tags on Sku_Tag.tag_id = Tags.tag_id where sku=" + "'" + sku + "'", conn);
                    cmdd.CommandType = System.Data.CommandType.Text;
                    MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter(cmdd);
                    DataTable dt = new DataTable();
                    mySqlDataAdapter.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        listModel = new List<SkuTagOrderViewModel>();
                        foreach (DataRow dr in dt.Rows)
                        {
                            SkuTagOrderViewModel model = new SkuTagOrderViewModel();
                            model.TagColor = Convert.ToString(dr["tag_color"]);
                            model.TagName = Convert.ToString(dr["tag"]);
                            model.TagId = Convert.ToInt32(dr["tag_id"]);
                            listModel.Add(model);
                        }
                    }


                
                return listModel;
            }
            catch (Exception)
            {

                throw;
            }

        }

        public List<SkuTagOrderViewModel> GetTags(string sku)
        {
            List<SkuTagOrderViewModel> listModel = null;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmdd = new MySqlCommand(@"SELECT Sku_Tag.sku,Sku_Tag.tag_id,Tags.tag_color,Tags.tag FROM bestBuyE2.Sku_Tag inner join Tags on Sku_Tag.tag_id = Tags.tag_id where sku=" + "'" + sku + "'", conn);
                    cmdd.CommandType = System.Data.CommandType.Text;
                    MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter(cmdd);
                    DataTable dt = new DataTable();
                    mySqlDataAdapter.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        listModel = new List<SkuTagOrderViewModel>();
                        foreach (DataRow dr in dt.Rows)
                        {
                            SkuTagOrderViewModel model = new SkuTagOrderViewModel();
                            model.TagColor = Convert.ToString(dr["tag_color"]);
                            model.TagName = Convert.ToString(dr["tag"]);
                            model.TagId = Convert.ToInt32(dr["tag_id"]);
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
