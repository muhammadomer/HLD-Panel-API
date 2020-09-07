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
    public class ChannelDecrytionDataAccess
    {
        public string connStr { get; set; }
        public ChannelDecrytionDataAccess(IConnectionString connectionString)
        {
            connStr = connectionString.GetConnectionString();
        }
        public GetChannelCredViewModel GetChannelDec(string method)
        {
            GetChannelCredViewModel model = null;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    if (method.Equals("sellercloud"))
                    {
                        MySqlCommand cmdd = new MySqlCommand(@"SELECT username,password FROM bestBuyE2.configuration Where Method=" + "'" + method + "'", conn);
                        cmdd.CommandType = System.Data.CommandType.Text;
                        using (var reader = cmdd.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    model = new GetChannelCredViewModel();
                                    model.UserName = Convert.ToString(reader["username"]);
                                    model.Key = Convert.ToString(reader["password"]);

                                }

                            }
                        }

                    }

                    else if (method.Equals("bestbuy"))
                    {
                        MySqlCommand cmdd = new MySqlCommand(@"SELECT keyValue FROM bestBuyE2.configuration Where Method=" + "'" + method + "'", conn);
                        cmdd.CommandType = System.Data.CommandType.Text;
                        using (var reader = cmdd.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    model = new GetChannelCredViewModel();
                                    model.Key = Convert.ToString(reader["keyValue"]);

                                }

                            }
                        }


                    }
                    else if (method.Equals("ZincDays"))
                    {
                        MySqlCommand cmdd = new MySqlCommand(@"SELECT password FROM bestBuyE2.configuration Where Method=" + "'" + method + "'", conn);
                        cmdd.CommandType = System.Data.CommandType.Text;
                        using (var reader = cmdd.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    model = new GetChannelCredViewModel();
                                    model.password = Convert.ToString(reader["password"]);

                                }

                            }
                        }


                    }

                    else if (method.Equals("Zinc"))
                    {
                        MySqlCommand cmdd = new MySqlCommand(@"SELECT keyValue FROM bestBuyE2.configuration Where Method=" + "'" + method + "'", conn);
                        cmdd.CommandType = System.Data.CommandType.Text;

                        using (var reader = cmdd.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    model = new GetChannelCredViewModel();
                                    model.Key = Convert.ToString(reader["keyValue"]);

                                }

                            }
                        }


                    }
                    else if (method.Equals("AmzZinc"))
                    {
                        MySqlCommand cmdd = new MySqlCommand(@"SELECT username,password,keyValue FROM bestBuyE2.configuration Where Method=" + "'" + method + "'", conn);
                        cmdd.CommandType = System.Data.CommandType.Text;
                        using (var reader = cmdd.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    model = new GetChannelCredViewModel();
                                    model.UserName = Convert.ToString(reader["username"]);
                                    model.password = Convert.ToString(reader["password"]);
                                    model.Key = Convert.ToString(reader["keyValue"]);

                                }

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

        public int CheckZincJobsStatus(string Control)
        {

            try
            {
                int status = 0;
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();

                    MySqlCommand cmdd = new MySqlCommand(@"SELECT * FROM bestBuyE2.AutoContorls Where Contorls=" + "'" + Control + "'", conn);
                    cmdd.CommandType = System.Data.CommandType.Text;
                    using (var reader = cmdd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {


                                status = Convert.ToInt32(reader["status"]);

                            }

                        }
                    }


                }
                return status;
            }
            catch (Exception)
            {

                throw;
            }

        }

        public int CheckZincJobsSwitch(string Control)
        {

            try
            {
                int Switch = 0;
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmdd = new MySqlCommand(@"SELECT * FROM bestBuyE2.AutoContorls Where Contorls=" + "'" + Control + "'", conn);
                    cmdd.CommandType = System.Data.CommandType.Text;
                    using (var reader = cmdd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                Switch = Convert.ToInt32(reader["Switch"]);
                            }
                        }
                    }
                }
                return Switch;
            }
            catch (Exception)
            {

                throw;
            }

        }

    }
}
