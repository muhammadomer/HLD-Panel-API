using DataAccess.Helper;
using DataAccess.ViewModels;

using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace DataAccess.DataAccess
{
    public class ProductDataAccess
    {
        public string connStr { get; set; }
        ProductWarehouseQtyDataAccess ProductWHQtyDataAccess = null;
        TagDataAccess _tagDataAccess = null;
        EncDecChannel _EncDecChannel = null;
        public ProductDataAccess(IConnectionString connectionString)
        {
            connStr = connectionString.GetConnectionString();
            _EncDecChannel = new EncDecChannel(connectionString);
            ProductWHQtyDataAccess = new ProductWarehouseQtyDataAccess(connectionString);
            _tagDataAccess = new TagDataAccess(connectionString);
        }

        public ProductDetailForAPViewModel GetProductDetailsForAPBySKU(string SKU)
        {
            ProductDetailForAPViewModel Item = new ProductDetailForAPViewModel();
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_GetProductDetailForAPBySKU", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_SKU", SKU);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                Item.SKU = Convert.ToString(reader["sku"]);

                                Item.Compress_image = reader["Compress_image"] != DBNull.Value ? (string)reader["Compress_image"] : "";
                                Item.image_name = reader["image_name"] != DBNull.Value ? (string)reader["image_name"] : "";

                                Item.Title = Convert.ToString(reader["title"]);
                            }
                        }
                    }
                }
            }
            catch (Exception exp)
            { }
            return Item;
        }
        public bool DeleteProductImage(int id)
        {
            bool status = false;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("P_DeleteProductImages", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("ProductImageId", id);
                    cmd.ExecuteNonQuery();
                    status = true;
                }
            }
            catch (Exception)
            {
            }
            return status;
        }
        /// <summary>
        /// Delete product from product and bestBuy if it's not exists in sellerCloudOrderDetail table
        /// </summary>
        /// <param name="ViewModel"></param>
        /// <returns>status 1 means can'nt be delete ,0 means deleted successfully</returns>
        public int DeleteProduct(DeleteProductViewModel ViewModel)
        {
            int status = 0;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_DeleteProductBySKU", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_product_sku", ViewModel.PorductSku);
                    cmd.Parameters.Add("_status", MySqlDbType.Int32);
                    cmd.Parameters["_status"].Direction = ParameterDirection.Output;
                    cmd.ExecuteNonQuery();
                    status = Convert.ToInt32(cmd.Parameters["_status"].Value);
                }
            }
            catch (Exception ex)
            {
            }
            return status;
        }

        public int SaveProductnew(ProductSaveViewModel viewModel)
        {
            viewModel.IsEndOfLife = viewModel.IsEndOfLife == true ? false : true;
            var LowStock60 = Math.Round(((Convert.ToDecimal(viewModel.QtySold60)) / Convert.ToDecimal(60)) * Convert.ToDecimal(60) - Convert.ToDecimal(viewModel.PhysicalQty) - Convert.ToDecimal(viewModel.OnOrder), 2);
            decimal Velocity = Math.Round(Convert.ToDecimal(viewModel.QtySold60) / Convert.ToDecimal(60), 2);
            int CoverDays = 0;
            if (Velocity != 0)
            {
                CoverDays = Convert.ToInt32(Math.Round((Convert.ToDecimal(viewModel.AggregatePhysicalQty) + Convert.ToDecimal(viewModel.OnOrder)) / Velocity));
            }

            int ProductId = 0;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_SaveProductFromSC", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_sku", viewModel.SKU);
                    cmd.Parameters.AddWithValue("_title", viewModel.ProductName);
                    cmd.Parameters.AddWithValue("_upc", viewModel.UPC);
                    //cmd.Parameters.AddWithValue("_product_location", string.Empty);
                    cmd.Parameters.AddWithValue("_LocationNotes", viewModel.LocationNotes);
                    cmd.Parameters.AddWithValue("_ShadowOf", viewModel.ShadowOf);
                    cmd.Parameters.AddWithValue("_QtySold15", viewModel.QtySold15);
                    cmd.Parameters.AddWithValue("_QtySold30", viewModel.QtySold30);
                    cmd.Parameters.AddWithValue("_QtySold60", viewModel.QtySold60);
                    cmd.Parameters.AddWithValue("_QtySold90", viewModel.QtySold90);
                    cmd.Parameters.AddWithValue("_QtySold180", viewModel.QtySold180);
                    cmd.Parameters.AddWithValue("_QtySold365", viewModel.QtySold365);
                    cmd.Parameters.AddWithValue("_QtySoldYTD", viewModel.QtySoldYTD);
                    cmd.Parameters.AddWithValue("_AggregatePhysicalQtyFBA", viewModel.AggregatePhysicalQty);
                    cmd.Parameters.AddWithValue("_AggregatedQty", viewModel.AggregateQty);
                    cmd.Parameters.AddWithValue("_PhysicalQty", viewModel.PhysicalQty);
                    cmd.Parameters.AddWithValue("_ReservedQty", viewModel.ReservedQty);
                    cmd.Parameters.AddWithValue("_ProductType", viewModel.ProductType);
                    cmd.Parameters.AddWithValue("_OnOrder", viewModel.OnOrder);
                    cmd.Parameters.AddWithValue("_Continue", viewModel.IsEndOfLife);
                    cmd.Parameters.AddWithValue("_LowStock60", LowStock60);
                    cmd.Parameters.AddWithValue("_CoverDays", CoverDays);
                    cmd.Parameters.AddWithValue("_AggregateNonSellableQty", viewModel.AggregateNonSellableQty);
                    cmd.Parameters.AddWithValue("_ASINInActiveListing", viewModel.ASINInActiveListing);
                    cmd.Parameters.AddWithValue("_AmazonFBASKU", viewModel.AmazonFBASKU);
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
            }
            return ProductId;
        }

        public int SaveProduct(ProductInsertUpdateViewModel ViewModel)
        {

            int ProductId = 0;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_SaveProduct", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("sku", ViewModel.ProductSKU);
                    cmd.Parameters.AddWithValue("title", ViewModel.ProductTitle);
                    cmd.Parameters.AddWithValue("upc", ViewModel.Upc);
                    cmd.Parameters.AddWithValue("description", ViewModel.Description);
                    cmd.Parameters.AddWithValue("avg_cost", ViewModel.AvgCost);
                    cmd.Parameters.AddWithValue("product_location", string.Empty);
                    cmd.Parameters.AddWithValue("ship_weight_oz", ViewModel.ShipmentWeight);
                    cmd.Parameters.AddWithValue("condition_id", ViewModel.ConditionId);
                    cmd.Parameters.AddWithValue("ship_weight_lbs", string.Empty);
                    cmd.Parameters.AddWithValue("color_id", ViewModel.ColorId);
                    cmd.Parameters.AddWithValue("brand_id", ViewModel.BrandId);
                    cmd.Parameters.AddWithValue("ship_length", ViewModel.shipmentLenght);
                    cmd.Parameters.AddWithValue("ship_width", ViewModel.shipmentWidth);
                    cmd.Parameters.AddWithValue("ship_height", ViewModel.shipmentHeight);
                    cmd.Parameters.AddWithValue("category_main", ViewModel.CategoryMain);
                    cmd.Parameters.AddWithValue("category_sub1", ViewModel.CategorySub1);
                    cmd.Parameters.AddWithValue("category_sub2", ViewModel.CategorySub2);
                    cmd.Parameters.AddWithValue("category_sub3", ViewModel.CategorySub3);
                    cmd.Parameters.AddWithValue("category_sub4", ViewModel.CategorySub4);
                    cmd.Parameters.AddWithValue("category_name", ViewModel.Category);


                    cmd.Parameters.Add("product_id", MySqlDbType.Int16, 500);
                    cmd.Parameters["product_id"].Direction = ParameterDirection.Output;
                    cmd.ExecuteNonQuery();
                    ProductId = Convert.ToInt32(cmd.Parameters["product_id"].Value);
                }
            }
            catch (Exception ex)
            {
            }
            return ProductId;
        }

        public int UpdateProduct(ProductInsertUpdateViewModel ViewModel)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_UpdateProduct", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("sku", ViewModel.ProductSKU);
                    cmd.Parameters.AddWithValue("title", ViewModel.ProductTitle);
                    cmd.Parameters.AddWithValue("upc", ViewModel.Upc);
                    cmd.Parameters.AddWithValue("description", ViewModel.Description);
                    cmd.Parameters.AddWithValue("avg_cost", ViewModel.AvgCost);
                    cmd.Parameters.AddWithValue("product_location", string.Empty);
                    cmd.Parameters.AddWithValue("ship_weight_oz", ViewModel.ShipmentWeight);
                    cmd.Parameters.AddWithValue("condition_id", ViewModel.ConditionId);
                    cmd.Parameters.AddWithValue("ship_weight_lbs", string.Empty);
                    cmd.Parameters.AddWithValue("color_id", ViewModel.ColorId);
                    cmd.Parameters.AddWithValue("brand_id", ViewModel.BrandId);
                    cmd.Parameters.AddWithValue("ship_length", ViewModel.shipmentLenght);
                    cmd.Parameters.AddWithValue("ship_width", ViewModel.shipmentWidth);
                    cmd.Parameters.AddWithValue("ship_height", ViewModel.shipmentHeight);
                    cmd.Parameters.AddWithValue("category_main", ViewModel.CategoryMain);
                    cmd.Parameters.AddWithValue("category_sub1", ViewModel.CategorySub1);
                    cmd.Parameters.AddWithValue("category_sub2", ViewModel.CategorySub2);
                    cmd.Parameters.AddWithValue("category_sub3", ViewModel.CategorySub3);
                    cmd.Parameters.AddWithValue("category_sub4", ViewModel.CategorySub4);
                    cmd.Parameters.AddWithValue("category_name", ViewModel.Category);


                    cmd.Parameters.AddWithValue("product_id", ViewModel.ProductId);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
            }
            return ViewModel.ProductId;
        }

        public List<ProductImagesViewModel> GetAllProductsImagesByProductId(string productId)
        {
            List<ProductImagesViewModel> _ViewModels = null;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_GetAllProductImageBy_ProductId", conn);
                    cmd.Parameters.AddWithValue("productId", productId);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            _ViewModels = new List<ProductImagesViewModel>();
                            while (reader.Read())
                            {
                                ProductImagesViewModel ViewModel = new ProductImagesViewModel();
                                ViewModel.ProductImageId = Convert.ToInt32(reader["product_image_id"]);
                                ViewModel.ProductId = Convert.ToInt32(reader["product_id"]);
                                ViewModel.ImageURL = Convert.ToString(reader["image_name"]);
                                _ViewModels.Add(ViewModel);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return _ViewModels;
        }

        public int CheckSKUImageExists(string productSku, string imageURL)
        {
            int count = 0;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_CheckSKUImageExists", conn);
                    cmd.Parameters.AddWithValue("_imageName", productSku);
                    cmd.Parameters.AddWithValue("_ProductSku", imageURL);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {

                            while (reader.Read())
                            {
                                count = Convert.ToInt32(reader["ImageStatusCount"] != DBNull.Value ? reader["ImageStatusCount"] : 0);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return count;
        }

        public bool updateProductStatus(String Sku, string productStatusId)
        {
            bool _status = false;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_UpdateProductStatus", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_sku", Sku);
                    cmd.Parameters.AddWithValue("productStatusId", productStatusId);

                    cmd.ExecuteNonQuery();
                    _status = true;
                }
            }
            catch (Exception ex)
            {

            }
            return _status;
        }

        public string updateProductAverageCost(String Sku, string averageCost)
        {
            string _status = "";
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_UpdateProductAverageCost", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_sku", Sku);
                    cmd.Parameters.AddWithValue("avgCost", averageCost);

                    cmd.ExecuteNonQuery();

                }
            }
            catch (Exception ex)
            {
                _status = ex.Message;
            }
            return _status;
        }

        public string UpdateProductDetailFromExcelFile(ProductDisplayViewModel model)
        {
            string _status = "";
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_UpdateProductDetailFromExcelFile", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_sku", model.ProductSKU);
                    cmd.Parameters.AddWithValue("_title", model.ProductTitle);
                    cmd.Parameters.AddWithValue("_upc", model.Upc);
                    cmd.Parameters.AddWithValue("_avg_cost", model.AvgCost);
                    cmd.Parameters.AddWithValue("_image_url", model.ImageURL);
                    cmd.ExecuteNonQuery();

                }
            }
            catch (Exception ex)
            {
                _status = ex.Message;
            }
            return _status;
        }

        public string SaveProductDetailFromExcelFile(ProductDisplayViewModel model)
        {
            string _status = "";
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_SaveProductDetailFromExcelFile", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_sku", model.ProductSKU);
                    cmd.Parameters.AddWithValue("_title", model.ProductTitle);
                    cmd.Parameters.AddWithValue("_upc", model.Upc);
                    cmd.Parameters.AddWithValue("_avg_cost", model.AvgCost);
                    cmd.Parameters.AddWithValue("_image_url", model.ImageURL);
                    cmd.ExecuteNonQuery();

                }
            }
            catch (Exception ex)
            {
                _status = ex.Message;
            }
            return _status;
        }

        public bool updateSCImageStatusInProductTable(String Sku, bool status)
        {
            bool _status = false;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_updateSCImageStatusInProductTable", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_product_sku", Sku);
                    cmd.Parameters.AddWithValue("scStatus", status);

                    cmd.ExecuteNonQuery();
                    _status = true;

                }
            }
            catch (Exception ex)
            {

            }
            return _status;
        }

        public int GetProductIdBySKU(String Sku)
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
        public int GetProductIdByBBSKU(String Sku)
        {
            int productId = 0;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_GetProductIDFromProductID", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("BBSKU", Convert.ToInt32(Sku));
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

        public int SaveProductImages(ProductImagesViewModel ViewModel)
        {
            int id = 0;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_SaveProductImages", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("product_id", ViewModel.ProductId);
                    cmd.Parameters.Add("ProductImageId", MySqlDbType.Int32, 200);
                    cmd.Parameters["ProductImageId"].Direction = ParameterDirection.Output;
                    cmd.Parameters.AddWithValue("Image", ViewModel.Image);
                    cmd.ExecuteNonQuery();
                    id = Convert.ToInt32(cmd.Parameters["ProductImageId"].Value);
                }
            }
            catch (Exception ex)
            {
            }
            return id;
        }

        public List<string> GetProductDetailForWarehouseQtyUpdate()
        {
            List<string> list = new List<string>();
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_GetProductSku_WarehouseProductQty_Enable", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                list.Add(Convert.ToString(reader["sku"] != DBNull.Value ? reader["sku"] : ""));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return list;
        }

        public List<string> GetProductDetailForWarehouseQtyUpdate_ALLSKU()
        {
            List<string> list = new List<string>();
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_GetProductSku_WarehouseProductQty_Enable_ALLActiveSKU", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                list.Add(Convert.ToString(reader["sku"] != DBNull.Value ? reader["sku"] : ""));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return list;
        }
        //change
        public List<ProductDisplayInventoryViewModel> GetAllProducts(int startLimit, int endLimit, string sort, string dropship, string dropshipsearch, string sku, string asin, string Producttitle, string DStag, string TypeSearch)
        {
            List<ProductDisplayInventoryViewModel> _ViewModels = null;
            // MySqlConnection mySqlConnection = null;
            if (string.IsNullOrEmpty(TypeSearch) || TypeSearch == "undefined")
                TypeSearch = "ALL";
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {

                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_GetAllProductsAsinSkuDumy", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("startLimit", startLimit);
                    cmd.Parameters.AddWithValue("endLimit", endLimit);
                    cmd.Parameters.AddWithValue("sort", sort);
                    cmd.Parameters.AddWithValue("dropship", dropship);
                    cmd.Parameters.AddWithValue("dropshipsearch", dropshipsearch);
                    cmd.Parameters.AddWithValue("sku", sku);
                    cmd.Parameters.AddWithValue("asin", asin);
                    cmd.Parameters.AddWithValue("tag", DStag);
                    cmd.Parameters.AddWithValue("ProductTitle", Producttitle);
                    cmd.Parameters.AddWithValue("_TypeSearch", TypeSearch);
                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        //mySqlConnection = new MySqlConnection(connStr);
                        //mySqlConnection.Open();
                        _ViewModels = new List<ProductDisplayInventoryViewModel>();

                        foreach (DataRow reader in dt.Rows)
                        {
                            ProductDisplayInventoryViewModel ViewModel = new ProductDisplayInventoryViewModel();
                            ViewModel.ProductId = Convert.ToInt32(reader["product_id"]);
                            ViewModel.ProductSKU = Convert.ToString(reader["sku"]);
                            ViewModel.ProductTitle = Convert.ToString(reader["title"]);

                            ViewModel.ShipmentWeight = Convert.ToString(reader["ship_weight_oz"]);

                            ViewModel.AvgCost = Convert.ToString(reader["avg_cost"] != DBNull.Value ? reader["avg_cost"] : "0");
                            //ViewModel.Asin = Convert.ToString(reader["AsinCount"] != DBNull.Value ? reader["AsinCount"] : "0");

                            ViewModel.ColorName = Convert.ToString(reader["color_name"]);
                            ViewModel.Continue = Convert.ToBoolean(reader["Continue"] != DBNull.Value ? reader["Continue"] : "false");
                            ViewModel.BBMSRP = Convert.ToDecimal(reader["unit_origin_price_MSRP"] != DBNull.Value ? reader["unit_origin_price_MSRP"] : "0");
                            ViewModel.BBSellingPrice = Convert.ToDecimal(reader["unit_discount_price_selling_price"] != DBNull.Value ? reader["unit_discount_price_selling_price"] : "0");

                            ViewModel.BestBuyProductSKU = Convert.ToString(reader["product_sku"]);
                            ViewModel.dropship_Qty = Convert.ToInt32(reader["dropship_Qty"] != DBNull.Value ? reader["dropship_Qty"] : 0);
                            ViewModel.dropship_status = Convert.ToBoolean(reader["dropship_status"] != DBNull.Value ? reader["dropship_status"] : "false");
                            if (!Convert.IsDBNull(reader["image_name"]))
                            {
                                ViewModel.ImageURL = Convert.ToString(reader["image_name"]);
                            }
                            else
                            {
                                ViewModel.ImageURL = "";
                            }

                            //List<ProductWarehouseQtyViewModel> warehouseQty = ProductWHQtyDataAccess.GetProductQtyBySKU_ForOrdersPage(ViewModel.ProductSKU.Trim(), conn);
                            List<ProductWarehouseQtyViewModel> warehouseQty = ProductWHQtyDataAccess.GetWareHousesQtyList(ViewModel.ProductSKU);
                            ViewModel.ProductrWarehouseQtyViewModel = warehouseQty;
                            List<SkuTagOrderViewModel> skuTagOrders = _tagDataAccess.GetTagforSkubulk(ViewModel.ProductSKU, conn);
                            ViewModel.skuTags = skuTagOrders;


                            _ViewModels.Add(ViewModel);
                        }
                        // mySqlConnection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return _ViewModels;
        }
        //cahnge
        public List<ExportProductDataViewModel> GetAllProductsForExport(string dropship, string dropshipstatusSearch, string sku)
        {
            List<ExportProductDataViewModel> _ViewModels = null;
            //  MySqlConnection mySqlConnection = null;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {

                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("P_GetProductDataForExportFile", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("dropship", dropship);
                    cmd.Parameters.AddWithValue("dropshipstatusSearch", dropshipstatusSearch);
                    cmd.Parameters.AddWithValue("sku", sku);
                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        //mySqlConnection = new MySqlConnection(connStr);
                        //mySqlConnection.Open();
                        _ViewModels = new List<ExportProductDataViewModel>();

                        foreach (DataRow reader in dt.Rows)
                        {
                            ExportProductDataViewModel ViewModel = new ExportProductDataViewModel();

                            ViewModel.ProductSKU = Convert.ToString(reader["sku"]);

                            ViewModel.dropship_Qty = Convert.ToInt32(reader["HLD_CA1"] != DBNull.Value ? reader["HLD_CA1"] : 0);
                            ViewModel.best_buy_product_id = Convert.ToInt32(reader["best_buy_product_id"] != DBNull.Value ? reader["best_buy_product_id"] : 0);
                            ViewModel.dropship_status = Convert.ToBoolean(reader["dropship_status"] != DBNull.Value ? reader["dropship_status"] : "false");

                            if (!Convert.IsDBNull(reader["prouduct_image_url"]))
                            {
                                ViewModel.ImageURL = Convert.ToString(reader["prouduct_image_url"]);
                            }
                            else
                            {
                                ViewModel.ImageURL = "";
                            }

                            _ViewModels.Add(ViewModel);
                        }
                        //   mySqlConnection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return _ViewModels;
        }
        //change
        public List<ProductDisplayInventoryViewModel> GetAllProductsWithoutPageLimit(string dropship, string dropshipsearch, string sku, string skulist)
        {
            List<ProductDisplayInventoryViewModel> _ViewModels = null;
            MySqlConnection mySqlConnection = null;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {

                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_GetAllProducts_WithoutPage_Limit_testCopy1", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("dropship", dropship);
                    cmd.Parameters.AddWithValue("dropshipsearch", dropshipsearch);
                    cmd.Parameters.AddWithValue("sku", sku);
                    cmd.Parameters.AddWithValue("skuList", skulist);
                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        mySqlConnection = new MySqlConnection(connStr);
                        mySqlConnection.Open();
                        _ViewModels = new List<ProductDisplayInventoryViewModel>();

                        foreach (DataRow reader in dt.Rows)
                        {
                            ProductDisplayInventoryViewModel ViewModel = new ProductDisplayInventoryViewModel();
                            ViewModel.ProductId = Convert.ToInt32(reader["product_id"]);
                            ViewModel.ProductSKU = Convert.ToString(reader["sku"]);

                            ViewModel.ProductTitle = Convert.ToString(reader["title"]);
                            ViewModel.ShipmentLWH = Convert.ToString(reader["shipmentLWH"]);
                            ViewModel.ShipmentWeight = Convert.ToString(reader["ship_weight_oz"]);
                            //ViewModel.Upc = Convert.ToString(reader["upc"]);
                            ViewModel.Continue = Convert.ToBoolean(reader["Continue"] != DBNull.Value ? reader["Continue"] : "false");
                            ViewModel.AvgCost = Convert.ToString(reader["avg_cost"] != DBNull.Value ? reader["avg_cost"] : "0");
                            //ViewModel.Asin = Convert.ToString(reader["AsinCount"] != DBNull.Value ? reader["AsinCount"] : "0");
                            //ViewModel.BrandName = Convert.ToString(reader["brand_name"]);
                            //ViewModel.CategoryName = Convert.ToString(reader["category_name"]);
                            ViewModel.ColorName = Convert.ToString(reader["color_name"]);
                            //ViewModel.ConditionName = Convert.ToString(reader["condition_name"]);
                            //ViewModel.Description = Convert.ToString(reader["description"]);
                            ViewModel.BBMSRP = Convert.ToDecimal(reader["unit_origin_price_MSRP"] != DBNull.Value ? reader["unit_origin_price_MSRP"] : "0");
                            ViewModel.BBSellingPrice = Convert.ToDecimal(reader["unit_discount_price_selling_price"] != DBNull.Value ? reader["unit_discount_price_selling_price"] : "0");
                            //ViewModel.DiscountEndDate = Convert.ToDateTime(reader["discount_end_date"] != DBNull.Value ? reader["discount_end_date"] : (DateTime?)null);
                            //ViewModel.DiscountStartDate = Convert.ToDateTime(reader["discount_start_date"] != DBNull.Value ? reader["discount_start_date"] : (DateTime?)null);
                            ViewModel.BestBuyProductSKU = Convert.ToString(reader["product_sku"]);
                            ViewModel.dropship_Qty = Convert.ToInt32(reader["dropship_Qty"] != DBNull.Value ? reader["dropship_Qty"] : 0);
                            ViewModel.dropship_status = Convert.ToBoolean(reader["dropship_status"] != DBNull.Value ? reader["dropship_status"] : "false");
                            if (!Convert.IsDBNull(reader["image_name"]))
                            {
                                ViewModel.ImageURL = Convert.ToString(reader["image_name"]);
                            }
                            else
                            {
                                ViewModel.ImageURL = "";
                            }

                            //geting product warehouse quantity

                            //List<ProductWarehouseQtyViewModel> warehouseQty = ProductWHQtyDataAccess.GetProductQtyBySKU_ForOrdersPage(ViewModel.ProductSKU.Trim(), conn);
                             List<ProductWarehouseQtyViewModel> warehouseQty = ProductWHQtyDataAccess.GetWareHousesQtyList(ViewModel.ProductSKU);
                            ViewModel.ProductrWarehouseQtyViewModel = warehouseQty;
                            List<SkuTagOrderViewModel> skuTagOrders = _tagDataAccess.GetTagforSkubulk(ViewModel.ProductSKU, conn);
                            ViewModel.skuTags = skuTagOrders;
                            _ViewModels.Add(ViewModel);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return _ViewModels;
        }

        public ProductDisplayViewModel GetProductDetailBySKU(String SKU)
        {
            ProductDisplayViewModel ViewModel = null;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_GetProductDetailBySKU", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_product_sku", SKU);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                ViewModel = new ProductDisplayViewModel();
                                ViewModel.ProductId = Convert.ToInt32(reader["product_id"]);
                                ViewModel.ProductSKU = Convert.ToString(reader["sku"]);
                                ViewModel.ProductTitle = Convert.ToString(reader["title"]);
                                ViewModel.ShipmentLWH = Convert.ToString(reader["shipmentLWH"]);
                                ViewModel.ShipmentWeight = Convert.ToString(reader["ship_weight_oz"]);
                                ViewModel.Upc = Convert.ToString(reader["upc"]);
                                ViewModel.AvgCost = Convert.ToString(reader["avg_cost"] != DBNull.Value ? reader["avg_cost"] : "0");
                                ViewModel.BrandName = Convert.ToString(reader["brand_name"]);
                                ViewModel.CategoryName = Convert.ToString(reader["category_name"]);
                                ViewModel.ColorName = Convert.ToString(reader["color_name"]);
                                ViewModel.ConditionName = Convert.ToString(reader["condition_name"]);
                                ViewModel.Description = Convert.ToString(reader["description"]);
                                ViewModel.BBMSRP = Convert.ToDecimal(reader["unit_origin_price_MSRP"] != DBNull.Value ? reader["unit_origin_price_MSRP"] : "0");
                                ViewModel.BBSellingPrice = Convert.ToDecimal(reader["unit_discount_price_selling_price"] != DBNull.Value ? reader["unit_discount_price_selling_price"] : "0");
                                ViewModel.DiscountEndDate = Convert.ToDateTime(reader["discount_end_date"] != DBNull.Value ? reader["discount_end_date"] : (DateTime?)null);
                                ViewModel.DiscountStartDate = Convert.ToDateTime(reader["discount_start_date"] != DBNull.Value ? reader["discount_start_date"] : (DateTime?)null);
                                ViewModel.BestBuyProductSKU = Convert.ToString(reader["product_sku"]);
                                ViewModel.dropship_Qty = Convert.ToInt32(reader["dropship_Qty"] != DBNull.Value ? reader["dropship_Qty"] : 0);
                                ViewModel.dropship_status = Convert.ToBoolean(reader["dropship_status"] != DBNull.Value ? reader["dropship_status"] : "false");
                                if (!Convert.IsDBNull(reader["image_name"]))
                                {
                                    ViewModel.ImageURL = Convert.ToString(reader["image_name"]);
                                }
                                else
                                {
                                    ViewModel.ImageURL = "";
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return ViewModel;
        }

        public List<AsinAmazonePriceViewModel> GetProductBySKuAmazoneprice(string sku)
        {
            List<AsinAmazonePriceViewModel> listModel = null;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("sp_GetAsainAzazonePrice", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_sku", sku);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            listModel = new List<AsinAmazonePriceViewModel>();
                            while (reader.Read())
                            {
                                AsinAmazonePriceViewModel model = new AsinAmazonePriceViewModel();
                                model.Asin = Convert.ToString(reader["z_asin_ca"] != DBNull.Value ? reader["z_asin_ca"] : "0");
                                model.amzonprice = Convert.ToDecimal(reader["amazon_price"] != DBNull.Value ? reader["amazon_price"] : "0");
                                listModel.Add(model);
                            }
                        }
                    }
                }
            }


            catch (Exception ex)
            {

                throw;
            }
            return listModel;
        }


        //change
        public int GetAllProductsCount(string dropship, string dropshipsearch, string sku, string skuList, string asin, string Producttitle, string DSTag, string TypeSearch)
        {
            int totalCount = 0;
            if (string.IsNullOrEmpty(TypeSearch) || TypeSearch == "undefined")
                TypeSearch = "ALL";
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_countTotalProductsIn_InventoryCountAsinDumyCopy", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("filters", dropship);
                    cmd.Parameters.AddWithValue("dropshipsearch", dropshipsearch);
                    cmd.Parameters.AddWithValue("sku", sku);
                    cmd.Parameters.AddWithValue("asin", asin);
                    cmd.Parameters.AddWithValue("ProductTitle", Producttitle);
                    cmd.Parameters.AddWithValue("tag", DSTag);
                    cmd.Parameters.AddWithValue("skuList", skuList);
                    cmd.Parameters.AddWithValue("_TypeSearch", TypeSearch);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                totalCount = Convert.ToInt32(reader[0]);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return totalCount;
        }


        public bool CheckProductSKUExists(string name)
        {
            bool status = false;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_CheckProductSKUExists", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("ProductSKU", name.Trim());
                    cmd.Parameters.Add("Statues", MySqlDbType.Bit, 10);
                    cmd.Parameters["Statues"].Direction = System.Data.ParameterDirection.Output;

                    cmd.ExecuteNonQuery();
                    status = Convert.ToBoolean(cmd.Parameters["Statues"].Value);


                }
            }
            catch (Exception ex)
            {
            }
            return status;
        }

        public bool CheckProductUPCExists(string name)
        {
            bool status = false;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_CheckProductUPCExists", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("ProductUPC", name.Trim());
                    cmd.Parameters.Add("Statues", MySqlDbType.Bit, 10);
                    cmd.Parameters["Statues"].Direction = System.Data.ParameterDirection.Output;

                    cmd.ExecuteNonQuery();
                    status = Convert.ToBoolean(cmd.Parameters["Statues"].Value);
                }
            }
            catch (Exception ex)
            {
            }
            return status;
        }

        public ProductInsertUpdateViewModel GetProductByProductID(string id)
        {
            ProductInsertUpdateViewModel ViewModel = new ProductInsertUpdateViewModel();
            ViewModel.Condition = null;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_GetProductById", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("product_id", id);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {

                            while (reader.Read())
                            {

                                ViewModel.ProductId = Convert.ToInt32(reader["product_id"] != DBNull.Value ? reader["product_id"] : "0");
                                ViewModel.ProductSKU = Convert.ToString(reader["sku"] != DBNull.Value ? reader["sku"] : "0");
                                ViewModel.ProductTitle = Convert.ToString(reader["title"] != DBNull.Value ? reader["title"] : "0");
                                ViewModel.ShipmentWeight = Convert.ToDecimal(reader["ship_weight_oz"] != DBNull.Value ? reader["ship_weight_oz"] : "0");
                                ViewModel.Upc = Convert.ToString(reader["upc"] != DBNull.Value ? reader["upc"] : "0");
                                ViewModel.AvgCost = Convert.ToDecimal(reader["avg_cost"] != DBNull.Value ? reader["avg_cost"] : "0");
                                ViewModel.Brand = Convert.ToString(reader["brand_name"] != DBNull.Value ? reader["brand_name"] : "");
                                ViewModel.Category = Convert.ToString(reader["category_name"] != DBNull.Value ? reader["category_name"] : "");
                                ViewModel.Color = Convert.ToString(reader["color_name"] != DBNull.Value ? reader["color_name"] : "");
                                ViewModel.ColorAlias = Convert.ToString(reader["color_alias"] != DBNull.Value ? reader["color_alias"] : "");
                                
                                ViewModel.ConditionId = Convert.ToInt32(reader["condition_id"] != DBNull.Value ? reader["condition_id"] : "0");
                                if (!Convert.IsDBNull(reader["description"]))
                                {
                                    ViewModel.Description = Convert.ToString(reader["description"]);
                                }
                                else
                                {
                                    ViewModel.Description = string.Empty;
                                }
                                ViewModel.ColorId = Convert.ToInt32(reader["color_id"] != DBNull.Value ? reader["color_id"] : "0");
                                ViewModel.BrandId = Convert.ToInt32(reader["brand_id"] != DBNull.Value ? reader["brand_id"] : "0");
                                ViewModel.CategoryMain = Convert.ToInt32(reader["category_mainId"] != DBNull.Value ? reader["category_mainId"] : "0");
                                ViewModel.CategorySub1 = Convert.ToInt32(reader["category_sub1Id"] != DBNull.Value ? reader["category_sub1Id"] : "0");
                                ViewModel.CategorySub2 = Convert.ToInt32(reader["category_sub2Id"] != DBNull.Value ? reader["category_sub2Id"] : "0");
                                ViewModel.CategorySub3 = Convert.ToInt32(reader["category_sub3Id"] != DBNull.Value ? reader["category_sub3Id"] : "0");
                                ViewModel.CategorySub4 = Convert.ToInt32(reader["category_sub4Id"] != DBNull.Value ? reader["category_sub4Id"] : "0");
                                ViewModel.shipmentHeight = Convert.ToDecimal(reader["ship_height"] != DBNull.Value ? reader["ship_height"] : "0");
                                ViewModel.shipmentWidth = Convert.ToDecimal(reader["ship_width"] != DBNull.Value ? reader["ship_width"] : "0");
                                ViewModel.shipmentLenght = Convert.ToDecimal(reader["ship_length"] != DBNull.Value ? reader["ship_length"] : "0");
                                ViewModel.CategoryIds = Convert.ToString(reader["ProductCategoryId"] != DBNull.Value ? reader["ProductCategoryId"] : "0");
                                ViewModel.StatusName = Convert.ToString(reader["product_status_name"] != DBNull.Value ? reader["product_status_name"] : "");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return ViewModel;
        }

        public bool UpdateProductDropshipStatusAndQty(BBProductViewModel viewModel)
        {
            bool status = false;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_UpdateSkuDropshipStatusAndQty", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_dropship_status", viewModel.dropship_status);
                    cmd.Parameters.AddWithValue("_dropship_qty", viewModel.dropship_Qty);
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


        public CheckProductDropShipStatusViewModel CheckSKuDropShipStatus(string _sku)
        {
            CheckProductDropShipStatusViewModel model = new CheckProductDropShipStatusViewModel();

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmdd = new MySqlCommand(@"SELECT  `dropship_status`,`dropship_Qty` FROM  `bestBuyE2`.`product` WHERE `sku` = '" + _sku + "'", conn);
                    cmdd.CommandType = System.Data.CommandType.Text;
                    MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter(cmdd);
                    DataTable dt = new DataTable();
                    mySqlDataAdapter.Fill(dt);
                    using (var reader = cmdd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {

                                model.dropship_Qty = Convert.ToInt32(reader["dropship_Qty"] != DBNull.Value ? reader["dropship_Qty"] : 0);
                                model.dropship_status = Convert.ToBoolean(reader["dropship_status"] != DBNull.Value ? reader["dropship_status"] : "false");

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

        public List<SkuTagOrderViewModel> GetTagforSkuEmail(string sku)
        {
            List<SkuTagOrderViewModel> listModel = new List<SkuTagOrderViewModel>(); ;
            try
            {
                MySqlConnection conn = new MySqlConnection(connStr);
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("p_GetTagforSkuEmail", conn);

                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter(cmd);
                cmd.Parameters.AddWithValue("_SKU", sku);
                DataTable dt = new DataTable();
                mySqlDataAdapter.Fill(dt);

                if (dt.Rows.Count > 0)
                {

                    foreach (DataRow dr in dt.Rows)
                    {
                        SkuTagOrderViewModel model = new SkuTagOrderViewModel();
                        model.TagName = Convert.ToString(dr["tag"]);
                        model.TagColor = Convert.ToString(dr["tag_color"]);
                        model.TagId = Convert.ToInt32(dr["tag_id"]);
                        listModel.Add(model);
                    }
                }



                return listModel;
            }
            catch (Exception exp)
            {

                throw;
            }

        }

        public CatalogViewModel GetCatalog(string SKU)
        {

            string ApiURL = "";
            ApiURL = "https://lp.api.sellercloud.com/rest/api";
            GetChannelCredViewModel _getChannelCredViewModel = new GetChannelCredViewModel();
            AuthenticateSCRestViewModel authenticate = new AuthenticateSCRestViewModel();
            _getChannelCredViewModel = _EncDecChannel.DecryptedData("sellercloud");
            authenticate = _EncDecChannel.AuthenticateSCForIMportOrder(_getChannelCredViewModel, ApiURL);

            CatalogViewModel item = new CatalogViewModel();
            CatalogListViewModel responses = new CatalogListViewModel();
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest
                    .Create(ApiURL + "/Catalog?model.sKU=" + SKU);
                request.Method = "GET";
                request.Accept = "application/json;";
                request.ContentType = "application/json";
                request.Headers["Authorization"] = "Bearer " + authenticate.access_token;

                string strResponse = "";
                using (WebResponse webResponse = request.GetResponse())
                {
                    using (StreamReader stream = new StreamReader(webResponse.GetResponseStream()))
                    {
                        strResponse = stream.ReadToEnd();
                    }
                }
                responses = JsonConvert.DeserializeObject<CatalogListViewModel>(strResponse);
                item = responses.Items.FirstOrDefault();
                if (item != null)
                {
                    UpdateProductCatalog(item, SKU);
                }
            }
            catch (Exception exp)
            {
                throw;
            }
            return item;
        }

        public ProductSaveViewModel GetProductInfoFromSellerCloudForMIssingSku(string SKU)
        {

            string ApiURL = "";
            ApiURL = "https://lp.api.sellercloud.com/rest/api";
            GetChannelCredViewModel _getChannelCredViewModel = new GetChannelCredViewModel();
            AuthenticateSCRestViewModel authenticate = new AuthenticateSCRestViewModel();
            _getChannelCredViewModel = _EncDecChannel.DecryptedData("sellercloud");
            authenticate = _EncDecChannel.AuthenticateSCForIMportOrder(_getChannelCredViewModel, ApiURL);

            ProductSaveViewModel item = new ProductSaveViewModel();
            ProductSaveListViewModel responses = new ProductSaveListViewModel();
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest
                    .Create(ApiURL + "/Catalog?model.sKU=" + SKU);
                request.Method = "GET";
                request.Accept = "application/json;";
                request.ContentType = "application/json";
                request.Headers["Authorization"] = "Bearer " + authenticate.access_token;

                string strResponse = "";
                using (WebResponse webResponse = request.GetResponse())
                {
                    using (StreamReader stream = new StreamReader(webResponse.GetResponseStream()))
                    {
                        strResponse = stream.ReadToEnd();
                    }
                }
                responses = JsonConvert.DeserializeObject<ProductSaveListViewModel>(strResponse);
                item = responses.Items.FirstOrDefault();
            }
            catch (Exception exp)
            {
                throw;
            }
            return item;
        }

        public bool UpdateProductCatalog(CatalogViewModel item, string SKU)
        {
            bool status = false;
            item.IsEndOfLife = item.IsEndOfLife == true ? false : true;
            item.PhysicalQty = item.PhysicalQty < 0 ? 0 : item.PhysicalQty;
            var LowStock60 = Math.Round(((Convert.ToDecimal(item.QtySold60)) / Convert.ToDecimal(60)) * Convert.ToDecimal(60) - Convert.ToDecimal(item.PhysicalQty) - Convert.ToDecimal(item.OnOrder), 2);
            decimal Velocity = Math.Round(Convert.ToDecimal(item.QtySold60) / Convert.ToDecimal(60), 2);
            int CoverDays = 0;
            if (Velocity != 0)
            {
                CoverDays = Convert.ToInt32(Math.Round((Convert.ToDecimal(item.AggregatePhysicalQty) + Convert.ToDecimal(item.OnOrder)) / Velocity));
            }
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_UpdateProductCatalogbySku", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_LocationNotes", item.LocationNotes);
                    cmd.Parameters.AddWithValue("_ShadowOf", item.ShadowOf);
                    cmd.Parameters.AddWithValue("_QtySold15", item.QtySold15);
                    cmd.Parameters.AddWithValue("_QtySold30", item.QtySold30);
                    cmd.Parameters.AddWithValue("_QtySold60", item.QtySold60);
                    cmd.Parameters.AddWithValue("_QtySold90", item.QtySold90);
                    cmd.Parameters.AddWithValue("_QtySoldYTD", item.QtySoldYTD);
                    cmd.Parameters.AddWithValue("_AggregatePhysicalQtyFBA", item.AggregatePhysicalQty);
                    cmd.Parameters.AddWithValue("_AggregatedQty", item.AggregateQty);
                    cmd.Parameters.AddWithValue("_PhysicalQty", item.PhysicalQty);
                    cmd.Parameters.AddWithValue("_ReservedQty", item.ReservedQty);
                    cmd.Parameters.AddWithValue("_LowStock60", LowStock60);
                    cmd.Parameters.AddWithValue("_CoverDays", CoverDays);
                    cmd.Parameters.AddWithValue("_OnOrder", item.OnOrder);
                    cmd.Parameters.AddWithValue("_Continue", item.IsEndOfLife);
                    cmd.Parameters.AddWithValue("_AggregateNonSellableQty", item.AggregateNonSellableQty);
                    cmd.Parameters.AddWithValue("_ASINInActiveListing", item.ASINInActiveListing);
                    cmd.Parameters.AddWithValue("_AmazonFBASKU", item.AmazonFBASKU);
                    cmd.Parameters.AddWithValue("_SKU", SKU);
                    cmd.ExecuteNonQuery();
                    status = true;
                }
            }
            catch (Exception ex)
            {
            }
            return status;
        }


        public bool UpdateProductByExcel(string SKU, string AmazonMerchantSKU, string AmazonFBASKU, decimal AmazonPrice, string ASIN)
        {
            bool status = false;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_UpdateProductFromExcel", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_AmazonMerchantSKU", AmazonMerchantSKU);
                    cmd.Parameters.AddWithValue("_AmazonFBASKU", AmazonFBASKU);
                    cmd.Parameters.AddWithValue("_AmazonPrice", AmazonPrice);
                    cmd.Parameters.AddWithValue("_ASIN", ASIN);
                    //cmd.Parameters.AddWithValue("_UPC", UPC);
                    cmd.Parameters.AddWithValue("_SKU", SKU);
                    cmd.ExecuteNonQuery();
                    status = true;
                }
            }
            catch (Exception ex)
            {
            }
            return status;
        }

        public List<string> GetAllSKUsForCatalog()
        {
            List<String> listModel = null;
            try
            {
                MySqlConnection conn = new MySqlConnection(connStr);
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("p_GetAllSKUsForCatalog", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                mySqlDataAdapter.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    listModel = new List<String>();
                    foreach (DataRow dr in dt.Rows)
                    {

                        string sku = Convert.ToString(dr["sku"]);
                        listModel.Add(sku);
                    }
                }
                return listModel;
            }
            catch (Exception)
            {

                throw;
            }

        }

        public bool ContinueDisContinue(List<ProductContinueDisContinueViewModel> ListViewModel)
        {
            bool status = false;
            try
            {
                foreach (var item in ListViewModel)
                {
                    using (MySqlConnection conn = new MySqlConnection(connStr))
                    {
                        conn.Open();
                        MySqlCommand cmd = new MySqlCommand("P_UpdateProductWithSKU", conn);
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("_SKU", item.SKU);
                        cmd.Parameters.AddWithValue("_flag", item.Continue);

                        cmd.ExecuteNonQuery();
                        status = true;
                        conn.Close();

                    }
                }

            }
            catch (Exception ex)
            {

            }
            return status;
        }

        public bool KitOrShadow(List<ProductSetAsKitOrShadowViewModel> ListViewModel)
        {
            bool status = false;
            try
            {
                foreach (var item in ListViewModel)
                {
                    using (MySqlConnection conn = new MySqlConnection(connStr))
                    {
                        conn.Open();
                        MySqlCommand cmd = new MySqlCommand("P_UpdateProductKitOrShadow", conn);
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("_SKU", item.SKU);
                        cmd.Parameters.AddWithValue("_KitOrShadow", item.KitOrShadow);

                        cmd.ExecuteNonQuery();
                        status = true;
                        conn.Close();

                    }
                }

            }
            catch (Exception ex)
            {

            }
            return status;
        }
        public int GetStausFromZinc(List<GetStatusFromZincViewModel> ListViewModel)
        {
            bool status = false;
            int jobId = 0;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("P_SaveZincWatchListJob", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    jobId = Convert.ToInt32(cmd.ExecuteScalar());
                   // status = true;
                    conn.Close();

                }
                foreach (var item in ListViewModel)
                {
                    using (MySqlConnection conn = new MySqlConnection(connStr))
                    {
                        conn.Open();
                        MySqlCommand cmd = new MySqlCommand("P_SaveSkuInZincWatchList", conn);
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("_JobId", jobId);
                        cmd.Parameters.AddWithValue("_Sku",item.SKU);
                        cmd.ExecuteNonQuery();
                       // status = true;
                        conn.Close();

                    }
                }
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("P_UpdateWatchListSummeryAsinCount", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_JobId", jobId);
                    cmd.ExecuteNonQuery();
                   // status = true;
                    conn.Close();

                }
            }
            catch (Exception ex)
            {

            }
            return jobId;
        }

        public bool SaveParentSKU(SaveParentSkuVM model)
        {
            bool status = false;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("P_SaveAndEditParentSku", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_Parentproduct_id", model.Parentproduct_id);
                    cmd.Parameters.AddWithValue("_Sku", model.Sku);
                    cmd.Parameters.AddWithValue("_ProductTitle", model.ProductTitle);
                    cmd.Parameters.AddWithValue("_ConditionId", model.ConditionId);
                    cmd.Parameters.AddWithValue("_CatagoryName", model.CatagoryName);
                    cmd.Parameters.AddWithValue("_ShipWt", model.ShipWt);
                    cmd.Parameters.AddWithValue("_ShipLt", model.ShipLt);
                    cmd.Parameters.AddWithValue("_ShipHt", model.ShipHt);
                    cmd.Parameters.AddWithValue("_Menufacture", model.Menufacture);
                    cmd.Parameters.AddWithValue("_MenufactureModel", model.MenufactureModel);
                    cmd.Parameters.AddWithValue("_Style", model.Style);
                    cmd.Parameters.AddWithValue("_IsCreatedOnSC", model.IsCreatedOnSC);
                    cmd.Parameters.AddWithValue("_Feature", model.Feature);
                    cmd.Parameters.AddWithValue("_Description", model.Description);
                    cmd.Parameters.AddWithValue("_DeviceModel", model.DeviceModel);
                    cmd.Parameters.AddWithValue("_productstatus", model.productstatus=1);
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

        public List<SaveParentSkuVM> GetAllParentSKU()
        {
            List<SaveParentSkuVM> ViewModel = new List<SaveParentSkuVM>();
            
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("P_GetAllParentSku", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                SaveParentSkuVM skuVM = new SaveParentSkuVM();
                                skuVM.Sku = Convert.ToString(reader["sku"] != DBNull.Value ? reader["sku"] : "");
                                skuVM.ProductTitle = Convert.ToString(reader["title"] != DBNull.Value ? reader["title"] : "");
                                skuVM.Color = Convert.ToString(reader["color_name"] != DBNull.Value ? reader["color_name"] : "");
                                skuVM.ColorAlias = Convert.ToString(reader["color_alias"] != DBNull.Value ? reader["color_alias"] : "");
                                skuVM.ColorId = Convert.ToInt32(reader["color_id"] != DBNull.Value ? reader["color_id"] : "0");
                                skuVM.productstatus = Convert.ToInt32(reader["productstatus"] != DBNull.Value ? reader["productstatus"] : "");
                                skuVM.Parentproduct_id = Convert.ToInt32(reader["product_id"] != DBNull.Value ? reader["product_id"] : "");
                                //skuVM.CompressedImage = reader["Compress_image"] != DBNull.Value ? (string)reader["Compress_image"] : "";
                                //skuVM.ImageName = reader["image_name"] != DBNull.Value ? (string)reader["image_name"] : "";
                                skuVM.Upc = Convert.ToString(reader["upc"] != DBNull.Value ? reader["upc"] : "0");
                                ViewModel.Add(skuVM);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ViewModel;
        }

        public int DeleteParentSKU(int Id)
        {

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("P_DeleteParentSku", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_Id", Id);
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

        public GetParentSkuById GetParentSkuWithId(int id)
        {
            GetParentSkuById model = new GetParentSkuById();
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("P_GetParentSkuById", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_product_id", id);
                    MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter(cmd);
                    cmd.ExecuteNonQuery();
                    DataTable dt = new DataTable();
                    mySqlDataAdapter.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {

                        foreach (DataRow reader in dt.Rows)
                        {
                            GetParentSkuById skuVM = new GetParentSkuById();

                            skuVM.Sku = Convert.ToString(reader["sku"] != DBNull.Value ? reader["sku"] : "");
                            skuVM.ProductTitle = Convert.ToString(reader["title"] != DBNull.Value ? reader["title"] : "");
                            skuVM.productstatus = Convert.ToInt32(reader["productstatus"] != DBNull.Value ? reader["productstatus"] : "");
                            skuVM.Parentproduct_id = Convert.ToInt32(reader["product_id"] != DBNull.Value ? reader["product_id"] : 0);
                            skuVM.Childproduct_id = Convert.ToInt32(reader["product_id"] != DBNull.Value ? reader["product_id"] : 0);
                            skuVM.ColorIds = Convert.ToInt32(reader["color_id"] != DBNull.Value ? reader["color_id"] : 0);
                            model = skuVM;

                        }
                    }

                }
            }
            catch (Exception ex)
            {
            }
            return model;
        }

        public bool SaveChildSKU(List<SaveChildSkuVM> ListViewModel)
        {
            bool status = false;
            try
            {
                foreach (var item in ListViewModel)
                {
                    using (MySqlConnection conn = new MySqlConnection(connStr))
                    {
                        conn.Open();
                        MySqlCommand cmd = new MySqlCommand("P_saveChildSku", conn);
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("_childProductId", item.Childproduct_id);
                        cmd.Parameters.AddWithValue("_parentProductId", item.Parentproduct_id);
                        cmd.Parameters.AddWithValue("_sku", item.Sku);
                        cmd.Parameters.AddWithValue("_productTitle", item.title);
                        cmd.Parameters.AddWithValue("_upc", item.upc);
                        cmd.Parameters.AddWithValue("_productStatus", 0);
                        cmd.Parameters.AddWithValue("_colorId", item.ColorIds);
                        cmd.ExecuteNonQuery();
                        status = true;

                        conn.Close();

                    }
                }

            }
            catch (Exception ex)
            {

            }
            return status;
        }

        public List<SaveChildSkuVM> GetAllChildSKU()
        {
            List<SaveChildSkuVM> ViewModel = new List<SaveChildSkuVM>();

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("P_GetAllChildSku", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                SaveChildSkuVM skuVM = new SaveChildSkuVM();
                                skuVM.Sku = Convert.ToString(reader["sku"] != DBNull.Value ? reader["sku"] : "");
                                skuVM.title = Convert.ToString(reader["title"] != DBNull.Value ? reader["title"] : "");
                                skuVM.upc = Convert.ToString(reader["upc"] != DBNull.Value ? reader["upc"] : "");
                                skuVM.productstatus = Convert.ToInt32(reader["productstatus"] != DBNull.Value ? reader["productstatus"] : "");
                                skuVM.ColorIds = Convert.ToInt32(reader["color_id"] != DBNull.Value ? reader["color_id"] : 0);
                                
                                ViewModel.Add(skuVM);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ViewModel;
        }

        public int DeleteChildSku(int child_id)
        {
           
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("P_DeleteChildSku", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_productId", child_id);
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
        public int DeleteChildImage(int ChildImage)
        {

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("P_DeleteChildSkuImage", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_productId", ChildImage);
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
        public List<GetChildSkuVM> GetChildSkuById(int id)
        
        {
            List<GetChildSkuVM> listModel = new List<GetChildSkuVM>();
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("GetChildSkuById", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_productId", id);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                           
                            while (reader.Read())
                            {
                                GetChildSkuVM model = new GetChildSkuVM();
                                model.Sku = Convert.ToString(reader["sku"] != DBNull.Value ? reader["sku"] : "");
                                model.title = Convert.ToString(reader["title"] != DBNull.Value ? reader["title"] : "");

                                model.upc = Convert.ToString(reader["upc"] != DBNull.Value ? reader["upc"] : "");

                                model.productstatus = Convert.ToInt32(reader["productstatus"] != DBNull.Value ? reader["productstatus"] : 0);
                                model.Parentproduct_id = Convert.ToInt32(reader["ParentID"] != DBNull.Value ? reader["ParentID"] : 0);
                                model.Childproduct_id = Convert.ToInt32(reader["product_id"] != DBNull.Value ? reader["product_id"] : 0);
                                model.ColorIds = Convert.ToInt32(reader["color_id"] != DBNull.Value ? reader["color_id"] : 0);
                                model.Colorname = Convert.ToString(reader["color_name"] != DBNull.Value ? reader["color_name"] : "");
                                model.ImageName = Convert.ToString(reader["image_name"] != DBNull.Value ? reader["image_name"] : "");
                                model.CompressedImage = Convert.ToString(reader["Compress_image"] != DBNull.Value ? reader["Compress_image"] : "");
                                model.ShadowOff = Convert.ToString(reader["ShadowOf"] != DBNull.Value ? reader["ShadowOf"] : "");
                                model.IsCreatedOnSC = Convert.ToInt32(reader["IsCreatedOnSC"] != DBNull.Value ? reader["IsCreatedOnSC"] : 0);
                                model.CompanyId = Convert.ToInt32(reader["CompanyId"] != DBNull.Value ? reader["CompanyId"] : 0);
                                listModel.Add(model);
                            }
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

        public bool UpdateChildSKU(SaveChildSkuVM model)
        {
            bool status = false; ;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("P_EditChildSku", conn);

                   
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("_productId", model.Childproduct_id);
                        cmd.Parameters.AddWithValue("_sku", model.Sku);
                        cmd.Parameters.AddWithValue("_productTitle", model.title);
                        cmd.Parameters.AddWithValue("_upc", model.upc);
                        cmd.Parameters.AddWithValue("_productStatus", model.productstatus);
                        cmd.Parameters.AddWithValue("_colorId", model.ColorIds);
                        cmd.ExecuteScalar();
                        status = true;
                        conn.Close();




                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return status;
        }

        public List<MarketPlaceShadowViewModel> GetMarketPlaceShadow()
        {
            List<MarketPlaceShadowViewModel> ViewModel = new List<MarketPlaceShadowViewModel>();

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("P_GetMarketPlaceShadows", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                MarketPlaceShadowViewModel skuVM = new MarketPlaceShadowViewModel();
                                skuVM.CompanyId = Convert.ToInt32(reader["CompanyId"] != DBNull.Value ? reader["CompanyId"] : 0);
                                skuVM.Shadow_Key = Convert.ToString(reader["Shadow_Key"] != DBNull.Value ? reader["Shadow_Key"] : "");
                                skuVM.CompanyName = Convert.ToString(reader["CompanyName"] != DBNull.Value ? reader["CompanyName"] : "");
                                ViewModel.Add(skuVM);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ViewModel;
        }

        public bool SaveAndEditChildShadow(SaveAndEditChildShadowViewModel model)
        {
            bool status = false;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("P_SaveAndEditChildShadow", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_Parentproduct_id", model.Parentproduct_id);
                    cmd.Parameters.AddWithValue("_Sku", model.Sku);
                    cmd.Parameters.AddWithValue("_ProductTitle", model.ProductTitle);
                    cmd.Parameters.AddWithValue("_ConditionId", model.ConditionId);
                    cmd.Parameters.AddWithValue("_CatagoryName", model.CatagoryName);
                    cmd.Parameters.AddWithValue("_ShipWt", model.ShipWt);
                    cmd.Parameters.AddWithValue("_ShipLt", model.ShipLt);
                    cmd.Parameters.AddWithValue("_ShipHt", model.ShipHt);
                    cmd.Parameters.AddWithValue("_Menufacture", model.Menufacture);
                    cmd.Parameters.AddWithValue("_MenufactureModel", model.MenufactureModel);
                    cmd.Parameters.AddWithValue("_Style", model.Style);
                    cmd.Parameters.AddWithValue("_IsCreatedOnSC", model.IsCreatedOnSC);
                    cmd.Parameters.AddWithValue("_Feature", model.Feature);
                    cmd.Parameters.AddWithValue("_Description", model.Description);
                    cmd.Parameters.AddWithValue("_DeviceModel", model.DeviceModel);
                    cmd.Parameters.AddWithValue("_productstatus", model.productstatus = 1);
                    cmd.Parameters.AddWithValue("_ShadowOf", model.ShadowOf);
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

        public bool SaveChildSkuShadow(List<SaveSkuShadowViewModel> model)
        {
            bool status = false;
            try
            {
                GetChildSkuImages getChildSkuImages = new GetChildSkuImages();
                List<MarketPlaceShadowViewModel> marketplaceshadow = new List<MarketPlaceShadowViewModel>();
                marketplaceshadow = GetMarketPlaceShadow();
                
                foreach (var childsku in model)
                {
                        foreach (var item in marketplaceshadow)
                        {
                            using (MySqlConnection conn = new MySqlConnection(connStr))
                            {
                                conn.Open();

                                MySqlCommand cmd = new MySqlCommand("P_SaveChildSkuShadow", conn);
                                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("_Parentproduct_id", childsku.ParentId);
                                cmd.Parameters.AddWithValue("_ShadowSku", childsku.Sku + "-" + item.Shadow_Key);
                                cmd.Parameters.AddWithValue("_ChildSku", childsku.Sku);                               
                                cmd.Parameters.AddWithValue("_CompanyName", item.CompanyName);
                                cmd.Parameters.AddWithValue("_CompanyId", item.CompanyId);
                                cmd.ExecuteNonQuery();
                                status = true;
                             getChildSkuImages = GetChildSkuImages(childsku.ChildId);
                            if (getChildSkuImages!=null)
                            { SaveShadowImages(getChildSkuImages.ImageName, childsku.Sku + "-" + item.Shadow_Key); }
                               
                            }
                        }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return status;
        }

        public bool SaveProductImagesFromSellerCloudOrders(ImagesSaveToDatabaseWithURLViewMOdel Data)
        {
            bool status = false;

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("p_SaveProductImagesOfSellerCloudOrdersCopy", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("imageName", Data.FileName);
                    cmd.Parameters.AddWithValue("_product_sku", Data.product_Sku);
                    cmd.Parameters.AddWithValue("_imageURL", Data.ImageURL);
                    cmd.Parameters.Add("_status", MySqlDbType.Int32, 500);
                    cmd.Parameters["_status"].Direction = ParameterDirection.Output;
                    cmd.ExecuteNonQuery();
                    int ID = Convert.ToInt32(cmd.Parameters["_status"].Value);
                    if (ID == 1)
                    {
                        status = true;
                    }
                    else
                    {
                        status = true;
                    }
                }

            }

            catch (Exception ex)
            {
            }
            return status;
        }
        public GetChildSkuImages GetChildSkuImages(int childSkuProductId)
        {

            GetChildSkuImages childSkuImages = new GetChildSkuImages();

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("P_GetChildSkuImages", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_childProductId", childSkuProductId);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                               
                                childSkuImages.ImageName = Convert.ToString(reader["image_name"] != DBNull.Value ? reader["image_name"] :"");
                                
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return childSkuImages;
        }

        public bool SaveShadowImages(string image , string shadow)
        {
            bool status = false;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("P_SaveShadowImages", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    
                    cmd.Parameters.AddWithValue("_ShadowSku", shadow);
                    cmd.Parameters.AddWithValue("_ImageName", image);
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

        public bool UpdateProductStatusWhenProductCreatedOnSC(string sku)
        {
            bool status = false;
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("P_UpdateProductStatusWhenProductCreatedOnSC", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("_sku", sku);
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


        public CheckChildOrShadowCreatedOnSCViewModel CheckChildOrShadowCreatedOnSC(string sku)
        {
            CheckChildOrShadowCreatedOnSCViewModel model = new CheckChildOrShadowCreatedOnSCViewModel();
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("P_ChildSkuCreateOrNotOnSC", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_checkSku", sku);
                    MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter(cmd);
                    cmd.ExecuteNonQuery();
                    DataTable dt = new DataTable();
                    mySqlDataAdapter.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow reader in dt.Rows)
                        {
                           CheckChildOrShadowCreatedOnSCViewModel skuVM = new CheckChildOrShadowCreatedOnSCViewModel();

                            skuVM.IsCreatedOnSC = Convert.ToInt32(reader["IsCreatedOnSC"] != DBNull.Value ? reader["IsCreatedOnSC"] : 0);
                            model = skuVM;

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return model;
        }

        public List<GetShadowsOfChildViewModel> GetShadowsOfChild(string childSku)
        {
            List<GetShadowsOfChildViewModel> listModel = new List<GetShadowsOfChildViewModel>();
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("P_GetShadowOfChildSku", conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("_childSku", childSku);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {

                            while (reader.Read())
                            {
                                GetShadowsOfChildViewModel model = new GetShadowsOfChildViewModel();
                                model.sku = Convert.ToString(reader["sku"] != DBNull.Value ? reader["sku"] : "");
                                model.title = Convert.ToString(reader["title"] != DBNull.Value ? reader["title"] : "");
                                listModel.Add(model);
                            }
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
    }
}
