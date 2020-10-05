using DataAccess.Helper;
using DataAccess.ViewModels;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DataAccess
{
    public class ZincDataAccess
    {
        public string connStr { get; set; }
        public IConnectionString _connectionString { get; set; }
        public ZincDataAccess(IConnectionString connectionString)
        {
            connStr = connectionString.GetConnectionString();
            _connectionString = connectionString;
        }
        public int SaveZincProductASIN(ZincProductSaveViewModel ViewModel)
        {
            int status = 0;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_SaveBestBuyProductZinc", conn);

                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_z_asin_ca", ViewModel.ASIN);
                    cmd.Parameters.AddWithValue("_priority", ViewModel.Priorty);
                    cmd.Parameters.AddWithValue("_available_quantity", 0);
                    cmd.Parameters.AddWithValue("_amazon_price", ViewModel.itemprice);
                    cmd.Parameters.AddWithValue("_max_price", ViewModel.max_price_limit);
                    cmd.Parameters.AddWithValue("_status", ViewModel.status);
                    cmd.Parameters.AddWithValue("_prime", ViewModel.item_prime_badge);
                    cmd.Parameters.AddWithValue("_reviews", "");
                    cmd.Parameters.AddWithValue("_product_sku", ViewModel.Product_sku);
                    cmd.Parameters.AddWithValue("_timestamp", ViewModel.timestemp);
                    cmd.Parameters.AddWithValue("_seller_name", ViewModel.sellerName);
                    cmd.Parameters.AddWithValue("_price", ViewModel.max_price_limit);
                    cmd.Parameters.AddWithValue("_available", ViewModel.itemavailable);
                    cmd.Parameters.AddWithValue("_handling_days_max", ViewModel.handlingday_max);
                    cmd.Parameters.AddWithValue("_handling_days_min", ViewModel.handlingday_min);
                    cmd.Parameters.AddWithValue("_deleivery_days_min", ViewModel.delivery_days_min);
                    cmd.Parameters.AddWithValue("_deleivery_days_max", ViewModel.delivery_days_max);
                    cmd.Parameters.AddWithValue("_condition", ViewModel.item_condition);


                    cmd.Parameters.AddWithValue("_percent_poistive_feedback", ViewModel.percent_positive);
                    cmd.Parameters.AddWithValue("_updateDate", DateTime.Now);

                    cmd.ExecuteNonQuery();
                    status = 1;
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("z_asin_ca_UNIQUE"))
                {
                    status = 2;
                }
            }
            return status;
        }

        public int SaveZincASINOffer(ZincASINOfferDetail ViewModel)
        {
            int status = 0;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_SaveASINOfferDetail", conn);

                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_z_asin_ca", ViewModel.ASIN);
                    cmd.Parameters.AddWithValue("_priority", ViewModel.Priorty);
                    cmd.Parameters.AddWithValue("_available_quantity", 0);
                    cmd.Parameters.AddWithValue("_amazon_price", ViewModel.itemprice);
                    cmd.Parameters.AddWithValue("_max_price", ViewModel.max_price_limit);
                    cmd.Parameters.AddWithValue("_status", ViewModel.status);
                    cmd.Parameters.AddWithValue("_prime", ViewModel.item_prime_badge);
                    cmd.Parameters.AddWithValue("_reviews", "");
                    cmd.Parameters.AddWithValue("_product_sku", ViewModel.Product_sku);
                    cmd.Parameters.AddWithValue("_timestamp", ViewModel.timestemp);
                    cmd.Parameters.AddWithValue("_seller_name", ViewModel.sellerName);
                    cmd.Parameters.AddWithValue("_price", ViewModel.max_price_limit);
                    cmd.Parameters.AddWithValue("_available", ViewModel.itemavailable);
                    cmd.Parameters.AddWithValue("_handling_days_max", ViewModel.handlingday_max);
                    cmd.Parameters.AddWithValue("_handling_days_min", ViewModel.handlingday_min);
                    cmd.Parameters.AddWithValue("_deleivery_days_min", ViewModel.delivery_days_min);
                    cmd.Parameters.AddWithValue("_deleivery_days_max", ViewModel.delivery_days_max);
                    cmd.Parameters.AddWithValue("_condition", ViewModel.item_condition);
                    cmd.Parameters.AddWithValue("_percent_poistive_feedback", ViewModel.percent_positive);
                    cmd.Parameters.AddWithValue("_updateDate", DateTimeExtensions.ConvertToEST(DateTime.Now));

                    cmd.ExecuteNonQuery();
                    status = 1;
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("z_asin_ca_UNIQUE"))
                {
                    status = 2;
                }
            }
            return status;
        }

        public bool SaveASINProductDetail(ASINDetailViewModel ViewModel)
        {
            bool status = false;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_saveASINProductDetail", conn);

                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_title", ViewModel.Title);
                    cmd.Parameters.AddWithValue("_asin", ViewModel.ASIN);
                    cmd.Parameters.AddWithValue("_description", ViewModel.Description);


                    cmd.Parameters.AddWithValue("_amazon_price", ViewModel.amazon_price);


                    cmd.Parameters.AddWithValue("_asin_main_image_bucket_url", ViewModel.S3BucketULR_image1);
                    cmd.Parameters.AddWithValue("_asin_image_1_bucket_url", ViewModel.S3BucketULR_image1);
                    cmd.Parameters.AddWithValue("_asin_image_2_bucket_url", ViewModel.S3BucketURL_image2);
                    cmd.Parameters.AddWithValue("_asin_main_image_origional_url", ViewModel.AsinMainImage_Url);
                    cmd.Parameters.AddWithValue("_asin_image_1_origional_url", ViewModel.AsinImage1_Url);
                    cmd.Parameters.AddWithValue("_asin_image_2_origional_url", ViewModel.AsinImage2_Url);
                    cmd.Parameters.AddWithValue("_asin_images_other_urls", ViewModel.OtherImagesURL);
                    cmd.Parameters.AddWithValue("_feature_bullets", ViewModel.feature_bullets);
                    cmd.Parameters.AddWithValue("_S3CombinedImagesUrl", ViewModel.AmazonImagesListCombined);
                    cmd.Parameters.AddWithValue("_color", ViewModel.Color);
                    cmd.Parameters.AddWithValue("_brand", ViewModel.BrandName);
                    cmd.Parameters.AddWithValue("_main_image_path", ViewModel.MainImage_imgPath);
                    cmd.Parameters.AddWithValue("_image_1_path", ViewModel.Image1_imgPath);
                    cmd.Parameters.AddWithValue("_image_2_path", ViewModel.Image2_imgPath);

                    cmd.Parameters.AddWithValue("_asindate", DateTimeExtensions.ConvertToEST(DateTime.Now));


                    cmd.ExecuteNonQuery();
                    status = true;
                }
            }
            catch (Exception ex)
            {
            }
            return status;
        }

        public bool SaveASINProductImageDetail(ASINProductImageViewModel ViewModel)
        {
            bool status = false;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_SaveAsinProductImages", conn);

                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_asin", ViewModel.ASIN);
                    cmd.Parameters.AddWithValue("_BucketName", ViewModel.BucketName);
                    cmd.Parameters.AddWithValue("_KeyName", ViewModel.KeyName);
                    cmd.ExecuteNonQuery();
                    status = true;
                }
            }
            catch (Exception ex)
            {

            }
            return status;
        }

        public List<string> CheckASINProductImageExists(string zinc_ca)
        {
            List<string> list = new List<string>();
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_checkASINImagesExist", conn);

                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_asin", zinc_ca);

                    MySqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            list.Add(Convert.ToString(reader["KeyName"] != DBNull.Value ? reader["KeyName"] : 0));

                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return list;
        }

        public bool DeleteASINProductImageList(string asin)
        {
            bool status = false;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_DeleteASINProductImageList", conn);

                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_asin", asin);

                    cmd.ExecuteNonQuery();
                    status = true;

                }
            }
            catch (Exception ex)
            {
            }
            return status;
        }

        public bool UpdateZincProductASINDetail(ZincProductSaveViewModel ViewModel)
        {
            bool status = false;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_UpdateBestBuyProductZincASIN", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_z_asin_ca", ViewModel.ASIN);
                    cmd.Parameters.AddWithValue("_available_quantity", 0);
                    cmd.Parameters.AddWithValue("_amazon_price", ViewModel.itemprice);
                    cmd.Parameters.AddWithValue("_status", ViewModel.status);
                    cmd.Parameters.AddWithValue("_prime", ViewModel.item_prime_badge);
                    cmd.Parameters.AddWithValue("_reviews", "");
                    cmd.Parameters.AddWithValue("_product_sku", ViewModel.Product_sku);
                    cmd.Parameters.AddWithValue("_timestamp", ViewModel.timestemp);
                    cmd.Parameters.AddWithValue("_seller_name", ViewModel.sellerName);
                    cmd.Parameters.AddWithValue("_price", ViewModel.max_price_limit);
                    cmd.Parameters.AddWithValue("_available", ViewModel.itemavailable);
                    cmd.Parameters.AddWithValue("_handling_days_max", ViewModel.handlingday_max);
                    cmd.Parameters.AddWithValue("_handling_days_min", ViewModel.handlingday_min);
                    cmd.Parameters.AddWithValue("_deleivery_days_min", ViewModel.delivery_days_min);
                    cmd.Parameters.AddWithValue("_deleivery_days_max", ViewModel.delivery_days_max);
                    cmd.Parameters.AddWithValue("_condition", ViewModel.item_condition);
                    cmd.Parameters.AddWithValue("_percent_poistive_feedback", ViewModel.percent_positive);
                    cmd.Parameters.AddWithValue("_updateDate", DateTime.Now);
                    cmd.Parameters.AddWithValue("_bb_product_zinc_id", ViewModel.bb_product_zinc_id);

                    cmd.ExecuteNonQuery();
                    status = true;
                }
            }
            catch (Exception ex)
            {
            }
            return status;
        }

        public bool UpdateZincProductASINDetailWatchList(ZincProductSaveViewModel ViewModel)
        {
            bool status = false;
            try
            {
                decimal maxPrice = Convert.ToDecimal(Convert.ToDecimal(ViewModel.itemprice) * Convert.ToDecimal(1.15));
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_UpdateBestBuyProductZincASINFromWatchList", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_z_asin_ca", ViewModel.ASIN);
                    cmd.Parameters.AddWithValue("_available_quantity", 0);
                    cmd.Parameters.AddWithValue("_amazon_price", ViewModel.itemprice);
                    cmd.Parameters.AddWithValue("_max_price", maxPrice);
                    cmd.Parameters.AddWithValue("_status", ViewModel.status);
                    cmd.Parameters.AddWithValue("_prime", ViewModel.item_prime_badge);
                    cmd.Parameters.AddWithValue("_reviews", "");
                    cmd.Parameters.AddWithValue("_product_sku", ViewModel.Product_sku);
                    cmd.Parameters.AddWithValue("_timestamp", ViewModel.timestemp);
                    cmd.Parameters.AddWithValue("_seller_name", ViewModel.sellerName);
                    cmd.Parameters.AddWithValue("_price", ViewModel.max_price_limit);
                    cmd.Parameters.AddWithValue("_available", ViewModel.itemavailable);
                    cmd.Parameters.AddWithValue("_handling_days_max", ViewModel.handlingday_max);
                    cmd.Parameters.AddWithValue("_handling_days_min", ViewModel.handlingday_min);
                    cmd.Parameters.AddWithValue("_deleivery_days_min", ViewModel.delivery_days_min);
                    cmd.Parameters.AddWithValue("_deleivery_days_max", ViewModel.delivery_days_max);
                    cmd.Parameters.AddWithValue("_condition", ViewModel.item_condition);
                    cmd.Parameters.AddWithValue("_percent_poistive_feedback", ViewModel.percent_positive);
                    cmd.Parameters.AddWithValue("_updateDate", DateTime.Now);
                    cmd.ExecuteNonQuery();
                    status = true;
                }
            }
            catch (Exception ex)
            {
            }
            return status;
        }
        public bool UpdateZincProductASIN(ZincProductSaveViewModel ViewModel)
        {
            bool status = false;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_UpdateBestBuyProductZinc", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_priority", ViewModel.Priorty);
                    cmd.Parameters.AddWithValue("_max_price", ViewModel.max_price_limit);
                    cmd.Parameters.AddWithValue("_amazon_price", ViewModel.itemprice);
                    cmd.Parameters.AddWithValue("_bb_product_zinc_id", ViewModel.bb_product_zinc_id);
                    cmd.Parameters.AddWithValue("_z_asin_ca", ViewModel.ASIN);

                    cmd.ExecuteNonQuery();
                    status = true;
                }
            }
            catch (Exception ex)
            {
            }
            return status;
        }
        public ZincProductSaveViewModel GetProductASIN_Detail_byID(int id)
        {
            ZincProductSaveViewModel model = new ZincProductSaveViewModel();
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_GetBestBuyProductZincByID", conn);

                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("ID", id);
                    MySqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            model.bb_product_zinc_id = Convert.ToInt32(reader["bb_product_zinc_id"] != DBNull.Value ? reader["bb_product_zinc_id"] : 0);
                            model.Priorty = Convert.ToString(reader["priority"] != DBNull.Value ? reader["priority"] : "0");
                            model.max_price_limit = Convert.ToString(reader["max_price"] != DBNull.Value ? reader["max_price"] : "0");
                            model.itemprice = Convert.ToInt32(reader["amazon_price"] != DBNull.Value ? reader["amazon_price"] : 0);
                            model.Product_sku = Convert.ToString(reader["product_sku"] != DBNull.Value ? reader["product_sku"] : "0");
                            model.ASIN = Convert.ToString(reader["z_asin_ca"] != DBNull.Value ? reader["z_asin_ca"] : "0");
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return model;
        }
        public List<ProductZinAsinDetail> GetProductZincDetailBySKU(string productSKU)
        {
            List<ProductZinAsinDetail> list = new List<ProductZinAsinDetail>();
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_GetProductZincDetailBySKU", conn);

                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_product_sku", productSKU);

                    MySqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            ProductZinAsinDetail viewModel = new ProductZinAsinDetail();
                            viewModel.max_price = Convert.ToString(reader["max_price"] != DBNull.Value ? reader["max_price"] : 0);
                            viewModel.amazon_price = Convert.ToString(reader["amazon_price"] != DBNull.Value ? reader["amazon_price"] : 0);
                            viewModel.bb_product_zinc_id = Convert.ToInt32(reader["bb_product_zinc_id"] != DBNull.Value ? reader["bb_product_zinc_id"] : 0);
                            viewModel.priority = Convert.ToInt32(reader["priority"] != DBNull.Value ? reader["priority"] : 0);
                            viewModel.prime = Convert.ToBoolean(reader["prime"] != DBNull.Value ? reader["prime"] : false);
                            viewModel.ProductSku = Convert.ToString(reader["product_sku"] != DBNull.Value ? reader["product_sku"] : "");
                            viewModel.ASIN = Convert.ToString(reader["z_asin_ca"] != DBNull.Value ? reader["z_asin_ca"] : "");
                            list.Add(viewModel);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return list;
        }
        public SaveZincOrders.RootObject GetCustomerDetailForSendOrderToZinc(string ASIN, string MaxPrice, string orderid, string SellerOrderID, string orderDetailID)
        {
            SaveZincOrders.RootObject rootObject = new SaveZincOrders.RootObject();

            ChannelDecrytionDataAccess channelDecrytionDataAccess = new ChannelDecrytionDataAccess(_connectionString);
            GetChannelCredViewModel _getChannelCredViewModel = new GetChannelCredViewModel();
            _getChannelCredViewModel = channelDecrytionDataAccess.GetChannelDec("ZincDays");
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(" p_GetCustomerDetailForSendOrderToZinc", conn);

                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_Order_id", orderid);
                    cmd.Parameters.AddWithValue("_Order_DetailID", orderDetailID);
                    MySqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            rootObject.shipping_address = new SaveZincOrders.ShippingAddress();

                            rootObject.retailer = "amazon_ca";

                            rootObject.max_price = Convert.ToInt32(Convert.ToDecimal(MaxPrice) * 100) * (Convert.ToInt32(reader["quantity"].ToString()));
                            rootObject.shipping_address.first_name = Convert.ToString(reader["firstname"] != DBNull.Value ? reader["firstname"] : "");
                            rootObject.shipping_address.last_name = Convert.ToString(reader["lastname"] != DBNull.Value ? reader["lastname"] : "");
                            rootObject.shipping_address.phone_number = Convert.ToString(reader["phone"] != DBNull.Value ? reader["phone"] : "");
                            rootObject.shipping_address.state = Convert.ToString(reader["state"] != DBNull.Value ? reader["state"] : "");
                            rootObject.shipping_address.zip_code = Convert.ToString(reader["zip_code"] != DBNull.Value ? reader["zip_code"] : "");
                            rootObject.shipping_address.city = Convert.ToString(reader["city"] != DBNull.Value ? reader["city"] : "");
                            rootObject.shipping_address.address_line1 = Convert.ToString(reader["street_1"] != DBNull.Value ? reader["street_1"] : "");
                            rootObject.shipping_address.address_line2 = Convert.ToString(reader["street_2"] != DBNull.Value ? reader["street_2"] : "");
                            rootObject.shipping_address.country = "CA";
                            rootObject.is_gift = false;
                            rootObject.gift_message = "";
                            rootObject.shipping = new SaveZincOrders.Shipping();
                            rootObject.shipping.max_days = Convert.ToInt32(_getChannelCredViewModel.password);
                            rootObject.shipping.max_price = 0;
                            rootObject.shipping.order_by = "price";
                            rootObject.client_notes = new SaveZincOrders.ClientNotes();
                            rootObject.client_notes.our_internal_order_id = int.Parse(SellerOrderID);
                            rootObject.products = new List<SaveZincOrders.Product>();
                            rootObject.products.Add(new SaveZincOrders.Product()
                            {
                                product_id = ASIN,
                                quantity = Convert.ToInt32(reader["quantity"] != DBNull.Value ? reader["quantity"] : 0)
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return rootObject;
        }

        public SaveZincOrders.RootObject GetCustomerDetailForSendOrderToZincDumy(string ASIN, string MaxPrice, string orderid, string SellerOrderID, string orderDetailID)
        {
            SaveZincOrders.RootObject rootObject = new SaveZincOrders.RootObject();
            List<CreditCardDetailViewModel> CardList = new List<CreditCardDetailViewModel>();
            List<ZincAccountsViewModel> AccountsList = new List<ZincAccountsViewModel>();
            ChannelDecrytionDataAccess channelDecrytionDataAccess = new ChannelDecrytionDataAccess(_connectionString);
            GetChannelCredViewModel _getChannelCredViewModel = new GetChannelCredViewModel();
            _getChannelCredViewModel = channelDecrytionDataAccess.GetChannelDec("ZincDays");
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(" p_GetCustomerDetailForSendOrderToZincDumy", conn);

                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_Order_id", orderid);
                    cmd.Parameters.AddWithValue("_Order_DetailID", orderDetailID);
                    DataSet ds = new DataSet();
                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    da.Fill(ds);
                    DataTable Customerdt = ds.Tables[0];
                    DataTable Carddt = ds.Tables[1];
                    DataTable Accountsdt = ds.Tables[2];



                    foreach (DataRow reader in Customerdt.Rows)
                    {
                        rootObject.shipping_address = new SaveZincOrders.ShippingAddress();

                        rootObject.retailer = "amazon_ca";

                        rootObject.max_price = Convert.ToInt32(Convert.ToDecimal(MaxPrice) * 100) * (Convert.ToInt32(reader["quantity"].ToString()));
                        rootObject.shipping_address.first_name = Convert.ToString(reader["firstname"] != DBNull.Value ? reader["firstname"] : "");
                        rootObject.shipping_address.last_name = Convert.ToString(reader["lastname"] != DBNull.Value ? reader["lastname"] : "");
                        rootObject.shipping_address.phone_number = Convert.ToString(reader["phone"] != DBNull.Value ? reader["phone"] : "");
                        rootObject.shipping_address.state = Convert.ToString(reader["state"] != DBNull.Value ? reader["state"] : "");
                        rootObject.shipping_address.zip_code = Convert.ToString(reader["zip_code"] != DBNull.Value ? reader["zip_code"] : "");
                        rootObject.shipping_address.city = Convert.ToString(reader["city"] != DBNull.Value ? reader["city"] : "");
                        rootObject.shipping_address.address_line1 = Convert.ToString(reader["street_1"] != DBNull.Value ? reader["street_1"] : "");
                        rootObject.shipping_address.address_line2 = Convert.ToString(reader["street_2"] != DBNull.Value ? reader["street_2"] : "");
                        rootObject.shipping_address.country = "CA";
                        rootObject.is_gift = false;
                        rootObject.gift_message = "";
                        rootObject.shipping = new SaveZincOrders.Shipping();
                        rootObject.shipping.max_days = Convert.ToInt32(_getChannelCredViewModel.password);
                        rootObject.shipping.max_price = 0;
                        rootObject.shipping.order_by = "price";
                        rootObject.client_notes = new SaveZincOrders.ClientNotes();
                        rootObject.client_notes.our_internal_order_id = int.Parse(SellerOrderID);
                        rootObject.products = new List<SaveZincOrders.Product>();
                        rootObject.products.Add(new SaveZincOrders.Product()
                        {
                            product_id = ASIN,
                            quantity = Convert.ToInt32(reader["quantity"] != DBNull.Value ? reader["quantity"] : 0)
                        });
                    }
                    foreach (DataRow reader in Carddt.Rows)
                    {
                        CreditCardDetailViewModel viewModel = new CreditCardDetailViewModel
                        {
                            CreditCardDetailId = Convert.ToInt32(reader["CreditCardDetailId"]),
                            name_on_cardShort = reader["name_on_card"] != DBNull.Value ? (string)reader["name_on_card"] : "",
                        };
                        CardList.Add(viewModel);
                    }

                    foreach (DataRow reader in Accountsdt.Rows)
                    {
                        ZincAccountsViewModel viewModel = new ZincAccountsViewModel
                        {
                            ZincAccountsId = Convert.ToInt32(reader["ZincAccountsId"]),
                            UserNameShort = reader["UserName"] != DBNull.Value ? (string)reader["UserName"] : "",
                        };
                        AccountsList.Add(viewModel);
                    }
                    rootObject.CreditCardDetail = CardList;
                    rootObject.ZincAccounts = AccountsList;
                }
            }
            catch (Exception ex)
            {

            }
            return rootObject;
        }

        public SaveZincOrders.RootObject GetActiveZincAccountsList()
        {
            SaveZincOrders.RootObject rootObject = new SaveZincOrders.RootObject();
            List<CreditCardDetailViewModel> CardList = new List<CreditCardDetailViewModel>();
            List<ZincAccountsViewModel> AccountsList = new List<ZincAccountsViewModel>();
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(" p_GetActiveZincAccounts", conn);

                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    DataSet ds = new DataSet();
                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    da.Fill(ds);
                    DataTable Carddt = ds.Tables[0];
                    DataTable Accountsdt = ds.Tables[1];

                    foreach (DataRow reader in Carddt.Rows)
                    {
                        CreditCardDetailViewModel viewModel = new CreditCardDetailViewModel
                        {
                            CreditCardDetailId = Convert.ToInt32(reader["CreditCardDetailId"]),
                            name_on_cardShort = reader["name_on_card"] != DBNull.Value ? (string)reader["name_on_card"] : "",
                        };
                        CardList.Add(viewModel);
                    }

                    foreach (DataRow reader in Accountsdt.Rows)
                    {
                        ZincAccountsViewModel viewModel = new ZincAccountsViewModel
                        {
                            ZincAccountsId = Convert.ToInt32(reader["ZincAccountsId"]),
                            UserNameShort = reader["UserName"] != DBNull.Value ? (string)reader["UserName"] : "",
                        };
                        AccountsList.Add(viewModel);
                    }
                    rootObject.CreditCardDetail = CardList;
                    rootObject.ZincAccounts = AccountsList;
                }
            }
            catch (Exception ex)
            {

            }
            return rootObject;
        }




        public SaveZincOrders.RootObject GetCustomerDetailForSendOrderToZincForOrderView(string ASIN, string orderid, string SellerOrderID)
        {
            SaveZincOrders.RootObject rootObject = new SaveZincOrders.RootObject();
            List<SendDataZincViewModel> list = JsonConvert.DeserializeObject<List<SendDataZincViewModel>>(ASIN);
            ChannelDecrytionDataAccess channelDecrytionDataAccess = new ChannelDecrytionDataAccess(_connectionString);
            GetChannelCredViewModel _getChannelCredViewModel = new GetChannelCredViewModel();
            _getChannelCredViewModel = channelDecrytionDataAccess.GetChannelDec("ZincDays");
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(" p_GetCustomerDetailForSendOrderToZincForOrderView", conn);

                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("_SC_OrderID", SellerOrderID);
                    MySqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            rootObject.shipping_address = new SaveZincOrders.ShippingAddress();

                            rootObject.retailer = "amazon_ca";

                            rootObject.shipping_address.first_name = Convert.ToString(reader["firstname"] != DBNull.Value ? reader["firstname"] : "");
                            rootObject.shipping_address.last_name = Convert.ToString(reader["lastname"] != DBNull.Value ? reader["lastname"] : "");
                            rootObject.shipping_address.phone_number = Convert.ToString(reader["phone"] != DBNull.Value ? reader["phone"] : "");
                            rootObject.shipping_address.state = Convert.ToString(reader["state"] != DBNull.Value ? reader["state"] : "");
                            rootObject.shipping_address.zip_code = Convert.ToString(reader["zip_code"] != DBNull.Value ? reader["zip_code"] : "");
                            rootObject.shipping_address.city = Convert.ToString(reader["city"] != DBNull.Value ? reader["city"] : "");
                            rootObject.shipping_address.address_line1 = Convert.ToString(reader["street_1"] != DBNull.Value ? reader["street_1"] : "");
                            rootObject.shipping_address.address_line2 = Convert.ToString(reader["street_2"] != DBNull.Value ? reader["street_2"] : "");
                            rootObject.shipping_address.country = "CA";
                            rootObject.is_gift = false;
                            rootObject.gift_message = "";
                            rootObject.shipping = new SaveZincOrders.Shipping();
                            rootObject.shipping.max_days = Convert.ToInt32(_getChannelCredViewModel.password);
                            rootObject.shipping.max_price = 0;
                            rootObject.shipping.order_by = "price";
                            rootObject.client_notes = new SaveZincOrders.ClientNotes();
                            rootObject.client_notes.our_internal_order_id = int.Parse(SellerOrderID);
                            rootObject.products = new List<SaveZincOrders.Product>();

                        }
                    }
                }

                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    double maxprice = 0;
                    foreach (var item in list)
                    {
                        MySqlCommand cmdd = new MySqlCommand(@"SELECT quantity FROM bestBuyE2.orderLines where  bbe2_line_id =" + item.OrderlineId, conn);
                        cmdd.CommandType = System.Data.CommandType.Text;
                        MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter(cmdd);
                        DataTable dt = new DataTable();
                        mySqlDataAdapter.Fill(dt);
                        if (dt.Rows.Count > 0)
                        {

                            foreach (DataRow dr in dt.Rows)
                            {
                                SaveZincOrders.Product model = new SaveZincOrders.Product();
                                model.product_id = item.Asin;
                                model.quantity = Convert.ToInt32(dr["quantity"]);
                                maxprice += item.MaxPrice * model.quantity;
                                rootObject.products.Add(model);
                            }
                        }
                    }
                    rootObject.max_price = Convert.ToInt32(maxprice * 100);
                }
            }
            catch (Exception ex)
            {

            }
            return rootObject;
        }
        public int GetProductASINAlreadyExists(string productSKU, string zinc_ca)
        {
            int count = 0;

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_CheckProductASINAlreadyExists", conn);

                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_product_sku", productSKU);
                    cmd.Parameters.AddWithValue("_z_asin_ca", zinc_ca);
                    MySqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            count = Convert.ToInt32(reader["_Count"] != DBNull.Value ? reader["_Count"] : 0);

                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return count;
        }

        public int GetProductASINAlreadyExistsInASINProductDetail(string zinc_ca)
        {
            int count = 0;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_CheckAsinAlreadyExistIn_ASIN_Detail", conn);

                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_asin", zinc_ca);

                    MySqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            count = Convert.ToInt32(reader["_Count"] != DBNull.Value ? reader["_Count"] : 0);

                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return count;
        }

        public int GetAvailablePrimeDetail(string SKU)
        {
            int count = 0;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("P_CheckAvailableASINAgainstSKU", conn);

                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_Sku", SKU);

                    MySqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            count = Convert.ToInt32(reader["Count"] != DBNull.Value ? reader["Count"] : 0);

                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return count;
        }


        public bool DeleteProductASIN_Detail_byID(int id)
        {
            bool status = false;
            ZincProductSaveViewModel model = new ZincProductSaveViewModel();
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_DeleteBestBuyProductZinc", conn);

                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_bb_product_zinc_id", id);
                    cmd.ExecuteNonQuery();
                    status = true;
                }
            }
            catch (Exception ex)
            {

            }
            return status;
        }
        public bool DeleteBestBuyProductZinc_ByZincID(string asin)
        {
            bool status = false;
            ZincProductSaveViewModel model = new ZincProductSaveViewModel();
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_DeletBestBuyProductZincByZincID", conn);

                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_asin", asin);
                    cmd.ExecuteNonQuery();
                    status = true;
                }
            }
            catch (Exception ex)
            {

            }
            return status;
        }


        public List<ZincProductSaveViewModel> GetProductASIN_Detail_bySKU(string sku)
        {
            List<ZincProductSaveViewModel> listViewModel = new List<ZincProductSaveViewModel>();
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_GetBestBuyProductZincByProductSKY", conn);

                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("ProductSku", sku);
                    MySqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            ZincProductSaveViewModel model = new ZincProductSaveViewModel();
                            model.bb_product_zinc_id = Convert.ToInt32(reader["bb_product_zinc_id"] != DBNull.Value ? reader["bb_product_zinc_id"] : 0);
                            model.ASIN = Convert.ToString(reader["z_asin_ca"] != DBNull.Value ? reader["z_asin_ca"] : "");
                            model.Priorty = Convert.ToString(reader["priority"] != DBNull.Value ? reader["priority"] : "0");
                            model.itemprice = Convert.ToInt32(reader["amazon_price"] != DBNull.Value ? reader["amazon_price"] : "0");
                            model.ValidStatus = Convert.ToInt32(reader["ValidStatus"] != DBNull.Value ? reader["ValidStatus"] : "0");
                            model.Frequency = Convert.ToInt32(reader["Frequency"] != DBNull.Value ? reader["Frequency"] : "0");
                            model.status = Convert.ToString(reader["status"] != DBNull.Value ? reader["status"] : "");
                            model.max_price_limit = Convert.ToString(reader["max_price"] != DBNull.Value ? reader["max_price"] : "0");
                            model.primeAvailable = Convert.ToString(reader["PrimeAvailable"] != DBNull.Value ? reader["PrimeAvailable"] : "Not Available");
                            model.item_prime_badge = Convert.ToBoolean(reader["prime"] != DBNull.Value ? reader["prime"] : "false");

                            model.Product_sku = Convert.ToString(reader["product_sku"] != DBNull.Value ? reader["product_sku"] : "");
                            model.sellerName = Convert.ToString(reader["seller_name"] != DBNull.Value ? reader["seller_name"] : "");
                            model.percent_positive = Convert.ToInt32(reader["percent_poistive_feedback"] != DBNull.Value ? reader["percent_poistive_feedback"] : "0");
                            model.item_condition = Convert.ToString(reader["condition"] != DBNull.Value ? reader["condition"] : "");
                            DateTime date = Convert.ToDateTime(reader["updateDate"] != DBNull.Value ? reader["updateDate"] : DateTime.Now);
                            model.listSKU = GetProductskuFromAsin(model.ASIN);
                            model.updateDate = DateTimeExtensions.ConvertToEST(date);

                            listViewModel.Add(model);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return listViewModel;
        }

        public bool UpdateNewAsin(string OldASIN, string NewASIN)
        {
            bool status = false;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_UpdateASINInProductZinc", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_OldASIN", OldASIN);
                    cmd.Parameters.AddWithValue("_NewASIN", NewASIN);
                    cmd.ExecuteNonQuery();
                    status = true;
                }
            }
            catch (Exception ex)
            {
            }
            return status;
        }

        public int GetAsinProductDetailCount(string DateTo, string DateFrom, string ASIN = "", string Title = "")
        {
            int counter = 0;
            try
            {
                if (string.IsNullOrEmpty(ASIN) || ASIN == "undefined")
                    ASIN = "";
                if (string.IsNullOrEmpty(Title) || Title == "undefined")
                    Title = "";
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_GetAsinProductDetailCountDumy", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_ASIN", ASIN);
                    cmd.Parameters.AddWithValue("_Title", Title);
                    cmd.Parameters.AddWithValue("dateFrom", DateFrom);
                    cmd.Parameters.AddWithValue("dateTo", DateTo);
                    counter = Convert.ToInt32(cmd.ExecuteScalar());
                    conn.Close();
                }
            }
            catch (Exception exp)
            {

            }
            return counter;
        }

        public List<ASINDetailViewModel> GetAsinProductDetailList(string DateTo, string DateFrom, string ASIN, string Title, int limit, int offset)
        {
            if (string.IsNullOrEmpty(ASIN) || ASIN == "undefined")
                ASIN = "";
            if (string.IsNullOrEmpty(Title) || Title == "undefined")
                Title = "";

            List<ASINDetailViewModel> listViewModel = new List<ASINDetailViewModel>();
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_GetAsinProductDetailDumy", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_ASIN", ASIN);
                    cmd.Parameters.AddWithValue("_Title", Title);
                    cmd.Parameters.AddWithValue("dateFrom", DateFrom);
                    cmd.Parameters.AddWithValue("dateTo", DateTo);
                    cmd.Parameters.AddWithValue("_Limit", limit);
                    cmd.Parameters.AddWithValue("_OffSet", offset);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                ASINDetailViewModel model = new ASINDetailViewModel();
                                model.ASIN = Convert.ToString(reader["asin"] != DBNull.Value ? reader["asin"] : "0");
                                model.asin_date = Convert.ToDateTime(reader["asin_date"] != DBNull.Value ? reader["asin_date"] : DateTime.Now);
                                model.feature_bullets = Convert.ToString(reader["feature_bullets"] != DBNull.Value ? reader["feature_bullets"] : "");
                                //model.Description = model.Description.Replace("\n", " , ").Replace("["," ").Replace("]"," ");
                                model.Title = Convert.ToString(reader["title"] != DBNull.Value ? reader["title"] : "").Replace("\n", " , ").Replace("[", " ").Replace("]", " ");
                                model.Image1_imgPath = Convert.ToString(reader["image_1_path"] != DBNull.Value ? reader["image_1_path"] : "");
                                model.Image2_imgPath = Convert.ToString(reader["image_2_path"] != DBNull.Value ? reader["image_2_path"] : "");
                                model.MainImage_imgPath = Convert.ToString(reader["main_image_path"] != DBNull.Value ? reader["main_image_path"] : "");
                                model.OtherImagesURL = Convert.ToString(reader["asin_images_other_urls"] != DBNull.Value ? reader["asin_images_other_urls"] : "");
                                model.amazon_price = Convert.ToString(reader["amazon_price"] != DBNull.Value ? reader["amazon_price"].ToString() : "0");
                                model.S3BucketULR_image1 = Convert.ToString(reader["asin_image_1_bucket_url"] != DBNull.Value ? reader["asin_image_1_bucket_url"] : "");
                                model.S3BucketURL_image2 = Convert.ToString(reader["asin_image_2_bucket_url"] != DBNull.Value ? reader["asin_image_2_bucket_url"] : "");
                                model.AmazonImagesListCombined = Convert.ToString(reader["S3BucketImagesList"] != DBNull.Value ? reader["S3BucketImagesList"] : "");
                                model.AsinProductDetailID = Convert.ToInt32(reader["asin_complete_detail_id"] != DBNull.Value ? reader["asin_complete_detail_id"] : "0");
                                model.product_sku = Convert.ToString(reader["product_sku"] != DBNull.Value ? reader["product_sku"] : "");
                                listViewModel.Add(model);
                            }
                        }
                    }
                    conn.Close();
                }
            }
            catch (Exception ex)
            {

            }
            return listViewModel;
        }


        public List<ASINDetailViewModel> GetASINProductDetail()
        {
            List<ASINDetailViewModel> listViewModel = new List<ASINDetailViewModel>();
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_GetAsinProductDetail", conn);

                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    MySqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            ASINDetailViewModel model = new ASINDetailViewModel();
                            model.ASIN = Convert.ToString(reader["asin"] != DBNull.Value ? reader["asin"] : "0");
                            model.asin_date = Convert.ToDateTime(reader["asin_date"] != DBNull.Value ? reader["asin_date"] : DateTime.Now);
                            model.feature_bullets = Convert.ToString(reader["feature_bullets"] != DBNull.Value ? reader["feature_bullets"] : "");
                            //model.Description = model.Description.Replace("\n", " , ").Replace("["," ").Replace("]"," ");
                            model.Title = Convert.ToString(reader["title"] != DBNull.Value ? reader["title"] : "").Replace("\n", " , ").Replace("[", " ").Replace("]", " ");
                            model.Image1_imgPath = Convert.ToString(reader["image_1_path"] != DBNull.Value ? reader["image_1_path"] : "");
                            model.Image2_imgPath = Convert.ToString(reader["image_2_path"] != DBNull.Value ? reader["image_2_path"] : "");
                            model.MainImage_imgPath = Convert.ToString(reader["main_image_path"] != DBNull.Value ? reader["main_image_path"] : "");
                            model.OtherImagesURL = Convert.ToString(reader["asin_images_other_urls"] != DBNull.Value ? reader["asin_images_other_urls"] : "");
                            model.amazon_price = Convert.ToString(reader["amazon_price"] != DBNull.Value ? reader["amazon_price"].ToString() : "0");
                            model.S3BucketULR_image1 = Convert.ToString(reader["asin_image_1_bucket_url"] != DBNull.Value ? reader["asin_image_1_bucket_url"] : "");
                            model.S3BucketURL_image2 = Convert.ToString(reader["asin_image_2_bucket_url"] != DBNull.Value ? reader["asin_image_2_bucket_url"] : "");
                            model.AmazonImagesListCombined = Convert.ToString(reader["S3BucketImagesList"] != DBNull.Value ? reader["S3BucketImagesList"] : "");
                            model.AsinProductDetailID = Convert.ToInt32(reader["asin_complete_detail_id"] != DBNull.Value ? reader["asin_complete_detail_id"] : "0");
                            model.product_sku = Convert.ToString(reader["product_sku"] != DBNull.Value ? reader["product_sku"] : "");
                            listViewModel.Add(model);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return listViewModel;
        }

        //For Quartz Job
        public List<ZincOrderInProcess_OrderRequestSentViewModel> GetAllSCZincOrders_InProcessAndRequestSentState_ForJob()
        {
            List<ZincOrderInProcess_OrderRequestSentViewModel> listViewModel = new List<ZincOrderInProcess_OrderRequestSentViewModel>();
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_GetAllSCZincOrders_in_InProcessAndRequestSentState_ForJob", conn);

                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    MySqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            ZincOrderInProcess_OrderRequestSentViewModel model = new ZincOrderInProcess_OrderRequestSentViewModel();
                            model.RequestID = Convert.ToString(reader["request_id"] != DBNull.Value ? reader["request_id"] : "0");
                            model.ZincOrderLogID = Convert.ToString(reader["zinc_order_log_id"] != DBNull.Value ? reader["zinc_order_log_id"] : "");
                            model.SellerCloudOrderID = Convert.ToString(reader["sc_order_id"] != DBNull.Value ? reader["sc_order_id"] : "");

                            listViewModel.Add(model);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return listViewModel;
        }
        public List<ZincOrderInProgressSuccessViewModel> GetAllSCZincOrders_InProgressSuccess_ForJob()
        {
            List<ZincOrderInProgressSuccessViewModel> listViewModel = new List<ZincOrderInProgressSuccessViewModel>();
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_GetAllSCZincOrders_in_InProgressSuccessState_ForJob", conn);

                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    MySqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            ZincOrderInProgressSuccessViewModel model = new ZincOrderInProgressSuccessViewModel();
                            model.RequestID = Convert.ToString(reader["request_id"] != DBNull.Value ? reader["request_id"] : "0");
                            model.ZincOrderLogID = Convert.ToString(reader["zinc_order_log_id"] != DBNull.Value ? reader["zinc_order_log_id"] : "");
                            model.Qty = Convert.ToString(reader["qty"] != DBNull.Value ? reader["qty"] : "");
                            model.SCOrderId = Convert.ToString(reader["sc_order_id"] != DBNull.Value ? reader["sc_order_id"] : "");
                            model.ZincOrderLogDetailID = Convert.ToString(reader["zinc_order_log_detail_id"] != DBNull.Value ? reader["zinc_order_log_detail_id"] : "");
                            model.ProductSKU = Convert.ToString(reader["product_sku"] != DBNull.Value ? reader["product_sku"] : "");

                            listViewModel.Add(model);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return listViewModel;
        }

        public SKUQTYModelforWebhooks GetQTyAndSKU(string OurInternalID)
        {
            SKUQTYModelforWebhooks model = null;
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();
                MySqlCommand cmdd = new MySqlCommand(@"SELECT product_sku,qty FROM bestBuyE2.sellerCloudOrderDetail where sellerCloudOrderDetail.seller_cloud_order_id=" + OurInternalID, conn);
                cmdd.CommandType = System.Data.CommandType.Text;

                using (var reader = cmdd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            model = new SKUQTYModelforWebhooks();
                            model.SKU = Convert.ToString(reader["product_sku"]);

                            model.Qty = Convert.ToInt32(reader["qty"]);

                        }

                    }
                }

            }


            return model;
        }

        public List<ProductSkuFromAsinViewModel> GetProductskuFromAsin(string asin)
        {
            List<ProductSkuFromAsinViewModel> modellist = new List<ProductSkuFromAsinViewModel>();
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_GetBestBuyProductZincByProductSKYCopy", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("asin", asin);
                    MySqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {

                            ProductSkuFromAsinViewModel model = new ProductSkuFromAsinViewModel();
                            {
                                model.productsku = Convert.ToString(reader["product_sku"]);
                                modellist.Add(model);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return modellist;
        }

        public bool SendToZinzProduct(SendToZincProductViewModel ViewModel)
        {
            bool status = false;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("P_SaveSendToZincProduct", conn);

                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_Asin", ViewModel.Asin);
                    cmd.Parameters.AddWithValue("_Sku", ViewModel.Sku);
                    cmd.Parameters.AddWithValue("_ShipDays", ViewModel.Shipdays);
                    cmd.Parameters.AddWithValue("_AccountDetail", ViewModel.AccountDetail);
                    cmd.Parameters.AddWithValue("_CreditCardDetail", ViewModel.CreditCardDetail);
                    cmd.Parameters.AddWithValue("_AddressLine1", ViewModel.Address1);
                    cmd.Parameters.AddWithValue("_AddressLine2", ViewModel.Address2);
                    cmd.Parameters.AddWithValue("_PostalCode", ViewModel.PostalCode);
                    cmd.Parameters.AddWithValue("_City", ViewModel.City);
                    cmd.Parameters.AddWithValue("_State", ViewModel.State);
                    cmd.Parameters.AddWithValue("_Phone", ViewModel.Phone);
                    cmd.Parameters.AddWithValue("_Country", ViewModel.Country);
                    cmd.Parameters.AddWithValue("_FirstName", ViewModel.FirstName);
                    cmd.Parameters.AddWithValue("_LastName", ViewModel.LastName);
                   // cmd.Parameters.AddWithValue("_TrackingNumber", ViewModel.TrackingNumber);
                  //  cmd.Parameters.AddWithValue("_Date", ViewModel.Date);
                   // cmd.Parameters.AddWithValue("_Response", ViewModel.Response);
                   // cmd.Parameters.AddWithValue("_LastUpdate", ViewModel.LastUpdate);
                    cmd.ExecuteNonQuery();
                    status = true;
                }
            }
            catch (Exception ex)
            {

            }
            return status;
        }

        public List<GetSendToZincOrderViewModel> GetSendToZincOrder(int _offset, string Sku, string Asin, string FromDate = "", string ToDate = "")
        {
            List<GetSendToZincOrderViewModel> list = new List<GetSendToZincOrderViewModel>();
            if (string.IsNullOrEmpty(Sku) || Sku == "undefined")
                Sku = "";
            if (string.IsNullOrEmpty(Asin) || Asin == "undefined")
                Asin = "";
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("P_GetSendToZincProduct", conn);

                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_offset", _offset);
                    cmd.Parameters.AddWithValue("_Sku", Sku);
                    cmd.Parameters.AddWithValue("_Asin", Asin);
                    cmd.Parameters.AddWithValue("dateFrom", FromDate);
                    cmd.Parameters.AddWithValue("dateTo", ToDate);

                    MySqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            GetSendToZincOrderViewModel viewModel = new GetSendToZincOrderViewModel();
                            viewModel.Asin = Convert.ToString(reader["Asin"] != DBNull.Value ? reader["Asin"] : "");
                            viewModel.Sku = Convert.ToString(reader["Sku"] != DBNull.Value ? reader["Sku"] : "");
                            viewModel.AccountDetail = Convert.ToString(reader["AccountDetail"] != DBNull.Value ? reader["AccountDetail"] : "");
                            viewModel.name_on_card = Convert.ToString(reader["name_on_card"] != DBNull.Value ? reader["name_on_card"] : "");

                            viewModel.UserName = Convert.ToString(reader["UserName"] != DBNull.Value ? reader["UserName"] : "");
                            viewModel.Date = Convert.ToDateTime(reader["Date"] != DBNull.Value ? reader["Date"] : DateTime.MinValue);
                            viewModel.TrackingNumber = Convert.ToString(reader["TrackingNumber"] != DBNull.Value ? reader["TrackingNumber"] : "");
                            viewModel.Response = Convert.ToString(reader["Response"] != DBNull.Value ? reader["Response"] : "");
                            viewModel.LastUpdate = Convert.ToDateTime(reader["LastUpdate"] != DBNull.Value ? reader["LastUpdate"] : DateTime.MinValue);
                            viewModel.Qty = Convert.ToInt32(reader["Qty"] != DBNull.Value ? reader["Qty"] : "");
                            viewModel.OrderId = Convert.ToInt32(reader["OrderId"] != DBNull.Value ? reader["OrderId"] : "");
                            viewModel.order_type = Convert.ToString(reader["order_type"] != DBNull.Value ? reader["order_type"] : "");
                            viewModel.amazon_tracking = Convert.ToString(reader["amazon_tracking"] != DBNull.Value ? reader["amazon_tracking"] : "");
                            viewModel._tracking = Convert.ToString(reader["17_tracking"] != DBNull.Value ? reader["17_tracking"] : "");
                            viewModel.carrier = Convert.ToString(reader["carrier"] != DBNull.Value ? reader["carrier"] : "");
                            viewModel.shpping_date = Convert.ToString(reader["shpping_date"] != DBNull.Value ? reader["shpping_date"] : "");
                            viewModel.OrderDate = Convert.ToString(reader["order_datetime"] != DBNull.Value ? reader["order_datetime"] :"");
                            viewModel.merchant_order_id = Convert.ToString(reader["merchant_order_id"] != DBNull.Value ? reader["merchant_order_id"] : "");
                            viewModel.RequestId = Convert.ToString(reader["RequestId"] != DBNull.Value ? reader["RequestId"] : "");

                            viewModel.RecievedOrderQty = Convert.ToInt32(reader["ReceivedQty"] != DBNull.Value ? reader["ReceivedQty"] : 0);
                            viewModel.RecievedOrderDate = Convert.ToDateTime(reader["ReceivedDate"] != DBNull.Value ? reader["ReceivedDate"] : DateTime.MinValue);
                            viewModel.Price = Convert.ToDecimal(reader["Price"] != DBNull.Value ? reader["Price"] : "");
                            if (viewModel.Price !=0) {
                                viewModel.Price = Convert.ToDecimal(reader["Price"] != DBNull.Value ? reader["Price"] : "")/100;
                            }
                            else
                            {
                                viewModel.Price =0;
                            }
                            viewModel.CompressedImage = reader["Compress_image"] != DBNull.Value ? (string)reader["Compress_image"] : "";
                            viewModel.ImageName = reader["image_name"] != DBNull.Value ? (string)reader["image_name"] : "";
                            list.Add(viewModel);
                            
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return list;
        }
        public int GetSendToZincOrderCount(string Sku, string Asin, string FromDate = "", string ToDate = "")
        {
            int count = 0;
            if (string.IsNullOrEmpty(Sku) || Sku == "undefined")
                Sku = "";
            if (string.IsNullOrEmpty(Asin) || Asin == "undefined")
                Asin = "";
            

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("P_GetSendToZincProductCount", conn);

                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_Sku", Sku);
                    cmd.Parameters.AddWithValue("_Asin", Asin);
                    cmd.Parameters.AddWithValue("dateFrom", FromDate);
                    cmd.Parameters.AddWithValue("dateTo", ToDate);
                    MySqlDataAdapter mySqlDataAdap = new MySqlDataAdapter(cmd);
                    DataTable data = new DataTable();
                    mySqlDataAdap.Fill(data);
                    if (data.Rows.Count > 0)
                    {
                        foreach (DataRow datarow in data.Rows)
                        {
                            GetSendToZincOrderViewModel model = new GetSendToZincOrderViewModel();
                            count = Convert.ToInt32(datarow["Records"] != DBNull.Value ? datarow["Records"] : "0");
                        }

                    }
                }
            }
            catch (Exception ex)
            {

            }
            return count;
        }

        public List<GetAddressViewModel> GetAddress()
        {
            List<GetAddressViewModel> model = new List<GetAddressViewModel>();
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("P_GetWHName", conn);

                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    MySqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            GetAddressViewModel getAddress = new GetAddressViewModel();
                            getAddress.WH_Name = Convert.ToString(reader["WH_Name"] != DBNull.Value ? reader["WH_Name"] : "");
                            model.Add(getAddress);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return model;
        }

        public bool SendToZincProduct(SendToZincProductViewModel ViewModel)
        {
            bool status = false;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("P_SaveSendToZincProduct", conn);

                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_OrderId", ViewModel.OrderId);
                    cmd.Parameters.AddWithValue("_Asin", ViewModel.Asin);
                    cmd.Parameters.AddWithValue("_Sku", ViewModel.Sku);
                    cmd.Parameters.AddWithValue("_ShipDays", ViewModel.Shipdays);
                    cmd.Parameters.AddWithValue("_AccountDetail", ViewModel.ZincAccountId);
                    cmd.Parameters.AddWithValue("_CreditCardDetail", ViewModel.CreditCardId);
                    cmd.Parameters.AddWithValue("_AddressLine1", ViewModel.Address1);
                    cmd.Parameters.AddWithValue("_AddressLine2", ViewModel.Address2);
                    cmd.Parameters.AddWithValue("_PostalCode", ViewModel.PostalCode);
                    cmd.Parameters.AddWithValue("_City", ViewModel.City);
                    cmd.Parameters.AddWithValue("_State", ViewModel.State);
                    cmd.Parameters.AddWithValue("_Phone", ViewModel.Phone);
                    cmd.Parameters.AddWithValue("_Country", ViewModel.Country);
                    cmd.Parameters.AddWithValue("_FirstName", ViewModel.FirstName);
                    cmd.Parameters.AddWithValue("_LastName", ViewModel.LastName);
                    cmd.Parameters.AddWithValue("_Qty", ViewModel.Qty);
                    cmd.Parameters.AddWithValue("_ReqId", ViewModel.ReqId);
                    // cmd.Parameters.AddWithValue("_TrackingNumber", ViewModel.TrackingNumber);
                    // cmd.Parameters.AddWithValue("_Date", ViewModel.Date);
                    // cmd.Parameters.AddWithValue("_Response", ViewModel.Response);
                    cmd.Parameters.AddWithValue("_LastUpdate", DateTime.Now);
                    cmd.Parameters.AddWithValue("_order_code", ViewModel.Code);
                    cmd.Parameters.AddWithValue("_order_message", ViewModel.Message);
                    cmd.Parameters.AddWithValue("_order_type", ViewModel.Type);
                    cmd.Parameters.AddWithValue("_zinc_order_log_id", ViewModel.ZincOrderLogID);
                    cmd.Parameters.AddWithValue("_order_data", ViewModel.Data);
                    cmd.Parameters.AddWithValue("_shpping_date", ViewModel.ShppingDate);
                    cmd.Parameters.AddWithValue("_tracking_number", ViewModel.TrackingNumber);
                    cmd.Parameters.AddWithValue("_carrier", ViewModel.Carrier);
                    cmd.Parameters.AddWithValue("_amazon_tracking", ViewModel.AmazonTracking);
                    cmd.Parameters.AddWithValue("_17_tracking", ViewModel._17Tracking);
                    cmd.Parameters.AddWithValue("_order_datetime", ViewModel.OrderDatetime);
                    cmd.Parameters.AddWithValue("_zinc_order_status_internal", ViewModel.ZincOrderStatusInternal);
                    cmd.Parameters.AddWithValue("_merchant_order_id", ViewModel.MerchantOrderId);
                    cmd.ExecuteNonQuery();
                    status = true;
                }
            }
            catch (Exception ex)
            {

            }
            return status;
        }



        public int SaveZincOrderBeforeCreating(SendToZincProductViewModel ViewModel)
        {
            int status = 0;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("P_SaveZincOrderBeforeCreating", conn);

                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                 
                    cmd.Parameters.AddWithValue("_Asin", ViewModel.Asin);
                    cmd.Parameters.AddWithValue("_Sku", ViewModel.Sku);
                    cmd.Parameters.AddWithValue("_ShipDays", ViewModel.Shipdays);
                    cmd.Parameters.AddWithValue("_AccountDetail", ViewModel.ZincAccountId);
                    cmd.Parameters.AddWithValue("_CreditCardDetail", ViewModel.CreditCardId);
                    cmd.Parameters.AddWithValue("_AddressLine1", ViewModel.Address1);
                    cmd.Parameters.AddWithValue("_AddressLine2", ViewModel.Address2);
                    cmd.Parameters.AddWithValue("_PostalCode", ViewModel.PostalCode);
                    cmd.Parameters.AddWithValue("_City", ViewModel.City);
                    cmd.Parameters.AddWithValue("_State", ViewModel.State);
                    cmd.Parameters.AddWithValue("_Phone", ViewModel.Phone);
                    cmd.Parameters.AddWithValue("_Country", ViewModel.Country);
                    cmd.Parameters.AddWithValue("_FirstName", ViewModel.FirstName);
                    cmd.Parameters.AddWithValue("_LastName", ViewModel.LastName);
                    cmd.Parameters.AddWithValue("_Qty", ViewModel.Qty);
                    cmd.Parameters.AddWithValue("_Price", ViewModel.max_price);

                    status = Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
            catch (Exception ex)
            {

            }
            return status;
        } 
        public int UpdateReqIDafterOrderOnZinc(RequestIdUpdateViewModel ViewModel)
        {
            int status = 0;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("P_UpdateReqIDafterOrderOnZinc", conn);

                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_OrderId", ViewModel._OrderId);
                    cmd.Parameters.AddWithValue("_ReqId", ViewModel._ReqId);
                  
                    cmd.Parameters.AddWithValue("_order_message", "Order Request Sent");

                    cmd.ExecuteNonQuery();
                    status = 1;
                }
            }
            catch (Exception ex)
            {

            }
            return status;
        }

        public bool UpdateZincOrder(UpdateZincOrderViewModel ViewModel)
        {
            bool status = false;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("P_UpdateZincOrder", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_OrderId", ViewModel.OrderId);
                    cmd.Parameters.AddWithValue("_ReceivedOrderQty", ViewModel.RecievedOrderQty);
                    cmd.Parameters.AddWithValue("_ReceivedOrderDate", ViewModel.RecievedOrderDate);
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
