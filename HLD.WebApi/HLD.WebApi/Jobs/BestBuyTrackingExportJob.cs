using DataAccess.DataAccess;
using DataAccess.Helper;
using Microsoft.Extensions.Configuration;
using Quartz;
using ServiceReference1;
using SCOrderTrackingService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccess.ViewModels;
using Newtonsoft.Json;
using System.Net;
using Newtonsoft.Json.Linq;
using System.IO;

namespace HLD.WebApi.Jobs
{
    [DisallowConcurrentExecution]
    public class BestBuyTrackingExportJob : IJob
    {
        IConnectionString _connectionString = null;
        ServiceReference1.AuthHeader authHeader = null;
        SCOrderTrackingService.AuthHeader sCAuthHeader = null;
        BestBuyTrackingExportDataAccess bestBuyTrackingExportDataAccess = null;
        ShipmentDataAccess _DataAccess = null;
        string bbbaseapiurl = "https://marketplace.bestbuy.ca/api/";
       
        private readonly IConfiguration configuration;
        EncDecChannel _EncDecChannel = null;
        GetChannelCredViewModel _getChannelCredViewModel = null;
        public BestBuyTrackingExportJob(IConnectionString connectionString, IConfiguration configuration)
        {
            _connectionString = connectionString;
            this.configuration = configuration;

            _EncDecChannel = new EncDecChannel(_connectionString);
            bestBuyTrackingExportDataAccess = new BestBuyTrackingExportDataAccess(_connectionString);
            _DataAccess = new ShipmentDataAccess(_connectionString);
        }

        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                _getChannelCredViewModel = new GetChannelCredViewModel();
                _getChannelCredViewModel = _EncDecChannel.DecryptedData("sellercloud");
                sCAuthHeader = new SCOrderTrackingService.AuthHeader();
                sCAuthHeader.ValidateDeviceID = false;
                sCAuthHeader.UserName = _getChannelCredViewModel.UserName;
                sCAuthHeader.Password = _getChannelCredViewModel.Key;

                SCOrderTrackingService.OrderCreationServiceSoapClient sCServiceSoap =
                           new SCOrderTrackingService.OrderCreationServiceSoapClient(SCOrderTrackingService.OrderCreationServiceSoapClient.EndpointConfiguration.OrderCreationServiceSoap12);
                var response = await sCServiceSoap.ListOrdersForTrackingExportAsync(sCAuthHeader, 513);


                string scOrderIdCommaSeprate = string.Join(",", response.ListOrdersForTrackingExportResult.OrderByDescending(e => e.ShipDate).Take(3000).Select(e => e.OrderID));

                List<int> SCDatabaseId = bestBuyTrackingExportDataAccess.GetSCIdNotupdated(scOrderIdCommaSeprate);
                if (SCDatabaseId != null)
                {
                    var SCIdsNotExistInDatabase = response.ListOrdersForTrackingExportResult.OrderByDescending(e => e.ShipDate).Take(3000).Select(e => e.OrderID).Except(SCDatabaseId);
                    List<UpdateTrackingBestbuyViewModel> bbmlist = new List<UpdateTrackingBestbuyViewModel>();
                    foreach (var item in SCIdsNotExistInDatabase)
                    {
                        var result = response.ListOrdersForTrackingExportResult.Where(e => e.OrderID == item).FirstOrDefault();

                        UpdateTrackingBestbuyViewModel bestBuyModel = new UpdateTrackingBestbuyViewModel();
                        bestBuyModel.bbOrderID = result.OrderSourceOrderID;
                        bestBuyModel.scOrderID = result.OrderID.ToString();
                        bestBuyModel.trackingNumber = result.TrackingNumber;
                        bestBuyModel.shippingServiceCode = result.ShippingServiceCode;
                        bestBuyModel.shippingMethodName = result.ShippingMethodName;
                        bestBuyModel.shippingCost = result.ShippingCost.ToString();
                        bestBuyModel.declaredValue = result.DeclaredValue;
                        bestBuyModel.shipDate = result.ShipDate;
                        bestBuyModel.estimatedDeliveryDate = result.EstimatedDeliveryDate;
                        bestBuyModel.packageWeight = result.PackageWeight.ToString();
                        bestBuyModel.packageID = result.PackageID.ToString();


                        bbmlist.Add(bestBuyModel);
                    }
                    if (bbmlist.Count > 0)
                    {
                        bool status = bestBuyTrackingExportDataAccess.SaveDataUpdateTracking(bbmlist);
                    }
                }
                List<UpdateTrackingBestbuyViewModel> bestbuytracklist = new List<UpdateTrackingBestbuyViewModel>();
                bestbuytracklist = bestBuyTrackingExportDataAccess.GetDataUpdateTracking();
                 List<BBtrackingCodesViewModel> listViewModel = new List<BBtrackingCodesViewModel>();
                  listViewModel= _DataAccess.GetBBtrackingCodesList();
                if (bestbuytracklist != null && bestbuytracklist.Count > 0)
                {
                    UpdateTrackingShippingJsonViewModel updateTrackingShippingJsonViewModel = null;
                    BBtrackingCodesViewModel updateTrackingcodes = null;
                    GetChannelCredViewModel _getChannelCred = new GetChannelCredViewModel();
                    _getChannelCred = _EncDecChannel.DecryptedData("bestbuy");
                    foreach (var list in bestbuytracklist)
                    {
                        try
                        {
                            string is2digit = list.trackingNumber.Substring(0, 2);
                            string is3digit = list.trackingNumber.Substring(0, 3);
                            string is4digit = list.trackingNumber.Substring(0, 4);
                            string is5digit = list.trackingNumber.Substring(0, 5);
                            string is6digit = list.trackingNumber.Substring(0, 6);
                            //   string is7digit = list.trackingNumber.Substring(0, 7);
                            if (list.shippingServiceCode == "generic")
                            {
                                updateTrackingShippingJsonViewModel = new UpdateTrackingShippingJsonViewModel();
                                updateTrackingShippingJsonViewModel.carrier_code = "CPCL";
                                updateTrackingShippingJsonViewModel.carrier_name = "";
                                updateTrackingShippingJsonViewModel.carrier_url = "";
                                updateTrackingShippingJsonViewModel.tracking_number = "Letter Mail";
                                string res_track = BestBuy_PutShippingTracking(list.bbOrderID, _getChannelCred.Key, updateTrackingShippingJsonViewModel);
                                if (res_track == "204")
                                {
                                    int res_ship = BestBuy_PutShippingShippedState(list.bbOrderID, _getChannelCred.Key);
                                    if (res_ship == 204 || res_ship == 400)
                                    {
                                        updateTrackingOne(list.bbe2TrackingId);
                                    }

                                }
                                else if (res_track == "400")
                                {
                                    updateTrackingOne(list.bbe2TrackingId);
                                }


                            }
                            //
                            else
                            {
                                var item = listViewModel.Find(s => s.TrackingNumberCode == list.trackingNumber.Substring(0, s.TrackingNumberCode.Length));


                                if (item != null)
                                {

                                
                                updateTrackingShippingJsonViewModel = new UpdateTrackingShippingJsonViewModel();
                                updateTrackingShippingJsonViewModel.carrier_code = item.CarrierCode;
                                updateTrackingShippingJsonViewModel.carrier_name = item.CarrierName;

                                    if (item.CarrierUrl !=null || item.CarrierUrl !="") {

                                        updateTrackingShippingJsonViewModel.carrier_url = item.CarrierUrl;
                                    } 
                                    else
                                    {
                                        updateTrackingShippingJsonViewModel.carrier_url =item.CarrierUrl.Replace("{TrackingNumber}",item.TrackingNumberCode);

                                    }
                                       
                                   
                                updateTrackingShippingJsonViewModel.tracking_number = item.TrackingNumberCode;

                                string res_track = BestBuy_PutShippingTracking(list.bbOrderID, _getChannelCred.Key, updateTrackingShippingJsonViewModel);
                                if (res_track == "204")
                                {
                                    int res_ship = BestBuy_PutShippingShippedState(list.bbOrderID, _getChannelCred.Key);
                                    if (res_ship == 204 || res_ship == 400)
                                    {
                                        updateTrackingOne(list.bbe2TrackingId);
                                    }
                                }
                                else if (res_track == "400")
                                {
                                    updateTrackingOne(list.bbe2TrackingId);
                                }
                                }
                                else
                                {
                                    updateTrackingShippingJsonViewModel = new UpdateTrackingShippingJsonViewModel();
                                    updateTrackingShippingJsonViewModel.carrier_code = "";
                                    updateTrackingShippingJsonViewModel.carrier_name = "Standard";
                                    updateTrackingShippingJsonViewModel.carrier_url = "";
                                    updateTrackingShippingJsonViewModel.tracking_number = list.trackingNumber;
                                    string res_track = BestBuy_PutShippingTracking(list.bbOrderID, _getChannelCred.Key, updateTrackingShippingJsonViewModel);
                                    if (res_track == "204")
                                    {
                                        int res_ship = BestBuy_PutShippingShippedState(list.bbOrderID, _getChannelCred.Key);
                                        if (res_ship == 204 || res_ship == 400)
                                        {
                                            updateTrackingOne(list.bbe2TrackingId);
                                        }
                                    }
                                    else if (res_track == "400")
                                    {
                                        updateTrackingOne(list.bbe2TrackingId);
                                    }
                                }
                            }

                            //

                                //else if (is4digit == "4001" || is3digit == "201" || is3digit == "102" || is3digit == "401" || is3digit == "400" || is4digit == "1024" || is4digit == "2001" || is4digit == "7316")
                                //{
                                //    updateTrackingShippingJsonViewModel = new UpdateTrackingShippingJsonViewModel();
                                //    updateTrackingShippingJsonViewModel.carrier_code = "CPCL";
                                //    updateTrackingShippingJsonViewModel.carrier_name = "";
                                //    updateTrackingShippingJsonViewModel.carrier_url = "";
                                //    updateTrackingShippingJsonViewModel.tracking_number = list.trackingNumber;
                                //    string res_track = BestBuy_PutShippingTracking(list.bbOrderID, _getChannelCred.Key, updateTrackingShippingJsonViewModel);
                                //    if (res_track == "204")
                                //    {
                                //        int res_ship = BestBuy_PutShippingShippedState(list.bbOrderID, _getChannelCred.Key);
                                //        if (res_ship == 204 || res_ship == 400)
                                //        {
                                //            updateTrackingOne(list.bbe2TrackingId);
                                //        }
                                //    }
                                //    else if (res_track == "400")
                                //    {
                                //        updateTrackingOne(list.bbe2TrackingId);
                                //    }
                                //}
                                //else if (is4digit == "MWG0" || is5digit == "CGK00" || is3digit == "DCM" || is3digit == "TBL" || is3digit == "RJM" || is3digit == "BVG" || is3digit == "VRE" || is3digit == "TBK" || is3digit == "VDM" || is3digit == "CGG")
                                //{
                                //    updateTrackingShippingJsonViewModel = new UpdateTrackingShippingJsonViewModel();
                                //    updateTrackingShippingJsonViewModel.carrier_code = "PRLA";
                                //    updateTrackingShippingJsonViewModel.carrier_name = "";
                                //    updateTrackingShippingJsonViewModel.carrier_url = "";
                                //    updateTrackingShippingJsonViewModel.tracking_number = list.trackingNumber;
                                //    string res_track = BestBuy_PutShippingTracking(list.bbOrderID, _getChannelCred.Key, updateTrackingShippingJsonViewModel);
                                //    if (res_track == "204")
                                //    {
                                //        int res_ship = BestBuy_PutShippingShippedState(list.bbOrderID, _getChannelCred.Key);
                                //        if (res_ship == 204 || res_ship == 400)
                                //        {
                                //            updateTrackingOne(list.bbe2TrackingId);
                                //        }

                                //    }
                                //    else if (res_track == "400")
                                //    {
                                //        updateTrackingOne(list.bbe2TrackingId);
                                //    }
                                //}
                                //else if (is4digit == "4737")
                                //{
                                //    updateTrackingShippingJsonViewModel = new UpdateTrackingShippingJsonViewModel();
                                //    updateTrackingShippingJsonViewModel.carrier_code = "FEDX";
                                //    updateTrackingShippingJsonViewModel.carrier_name = "";
                                //    updateTrackingShippingJsonViewModel.carrier_url = "";
                                //    updateTrackingShippingJsonViewModel.tracking_number = list.trackingNumber;
                                //    string res_track = BestBuy_PutShippingTracking(list.bbOrderID, _getChannelCred.Key, updateTrackingShippingJsonViewModel);
                                //    if (res_track == "204")
                                //    {
                                //        int res_ship = BestBuy_PutShippingShippedState(list.bbOrderID, _getChannelCred.Key);
                                //        if (res_ship == 204 || res_ship == 400)
                                //        {
                                //            updateTrackingOne(list.bbe2TrackingId);
                                //        }

                                //    }
                                //    else if (res_track == "400")
                                //    {
                                //        updateTrackingOne(list.bbe2TrackingId);
                                //    }
                                //}
                                //else if (is3digit == "ZPY")
                                //{
                                //    updateTrackingShippingJsonViewModel = new UpdateTrackingShippingJsonViewModel();
                                //    updateTrackingShippingJsonViewModel.carrier_code = "";
                                //    updateTrackingShippingJsonViewModel.carrier_name = "Standard";
                                //    updateTrackingShippingJsonViewModel.carrier_url = "https://t.17track.net/en#nums=" + list.trackingNumber;
                                //    updateTrackingShippingJsonViewModel.tracking_number = list.trackingNumber;
                                //    string res_track = BestBuy_PutShippingTracking(list.bbOrderID, _getChannelCred.Key, updateTrackingShippingJsonViewModel);
                                //    if (res_track == "204")
                                //    {
                                //        int res_ship = BestBuy_PutShippingShippedState(list.bbOrderID, _getChannelCred.Key);
                                //        if (res_ship == 204 || res_ship == 400)
                                //        {
                                //            updateTrackingOne(list.bbe2TrackingId);
                                //        }

                                //    }
                                //    else if (res_track == "400")
                                //    {
                                //        updateTrackingOne(list.bbe2TrackingId);
                                //    }
                                //}
                                //else if (is3digit == "ASL")
                                //{
                                //    updateTrackingShippingJsonViewModel = new UpdateTrackingShippingJsonViewModel();
                                //    updateTrackingShippingJsonViewModel.carrier_code = "";
                                //    updateTrackingShippingJsonViewModel.carrier_name = "ASL";
                                //    updateTrackingShippingJsonViewModel.carrier_url = "https://ng-amz.shiptrackapp.com/view.aspx?lng=&tracking=" + list.trackingNumber;
                                //    updateTrackingShippingJsonViewModel.tracking_number = list.trackingNumber;
                                //    string res_track = BestBuy_PutShippingTracking(list.bbOrderID, _getChannelCred.Key, updateTrackingShippingJsonViewModel);
                                //    if (res_track == "204")
                                //    {
                                //        int res_ship = BestBuy_PutShippingShippedState(list.bbOrderID, _getChannelCred.Key);
                                //        if (res_ship == 204 || res_ship == 400)
                                //        {
                                //            updateTrackingOne(list.bbe2TrackingId);
                                //        }

                                //    }
                                //    else if (res_track == "400")
                                //    {
                                //        updateTrackingOne(list.bbe2TrackingId);
                                //    }
                                //}
                                //else if (is3digit == "TBC")
                                //{
                                //    updateTrackingShippingJsonViewModel = new UpdateTrackingShippingJsonViewModel();
                                //    updateTrackingShippingJsonViewModel.carrier_code = "";
                                //    updateTrackingShippingJsonViewModel.carrier_name = "AMZN";
                                //    updateTrackingShippingJsonViewModel.carrier_url = "";
                                //    updateTrackingShippingJsonViewModel.tracking_number = list.trackingNumber;
                                //    string res_track = BestBuy_PutShippingTracking(list.bbOrderID, _getChannelCred.Key, updateTrackingShippingJsonViewModel);
                                //    if (res_track == "204")
                                //    {
                                //        int res_ship = BestBuy_PutShippingShippedState(list.bbOrderID, _getChannelCred.Key);
                                //        if (res_ship == 204 || res_ship == 400)
                                //        {
                                //            updateTrackingOne(list.bbe2TrackingId);
                                //        }

                                //    }
                                //    else if (res_track == "400")
                                //    {
                                //        updateTrackingOne(list.bbe2TrackingId);
                                //    }
                                //}
                                //else if (is4digit == "INTL")
                                //{
                                //    updateTrackingShippingJsonViewModel = new UpdateTrackingShippingJsonViewModel();
                                //    updateTrackingShippingJsonViewModel.carrier_code = "";
                                //    updateTrackingShippingJsonViewModel.carrier_name = "Intelcom";
                                //    updateTrackingShippingJsonViewModel.carrier_url = "https://parcelsapp.com/en/tracking/" + list.trackingNumber;
                                //    updateTrackingShippingJsonViewModel.tracking_number = list.trackingNumber;
                                //    string res_track = BestBuy_PutShippingTracking(list.bbOrderID, _getChannelCred.Key, updateTrackingShippingJsonViewModel);
                                //    if (res_track == "204")
                                //    {
                                //        int res_ship = BestBuy_PutShippingShippedState(list.bbOrderID, _getChannelCred.Key);
                                //        if (res_ship == 204 || res_ship == 400)
                                //        {
                                //            updateTrackingOne(list.bbe2TrackingId);
                                //        }

                                //    }
                                //    else if (res_track == "400")
                                //    {
                                //        updateTrackingOne(list.bbe2TrackingId);
                                //    }
                                //}
                                //else if (is3digit == "BNI")
                                //{
                                //    updateTrackingShippingJsonViewModel = new UpdateTrackingShippingJsonViewModel();
                                //    updateTrackingShippingJsonViewModel.carrier_code = "";
                                //    updateTrackingShippingJsonViewModel.carrier_name = "BNI";
                                //    updateTrackingShippingJsonViewModel.carrier_url = "https://bnitracking.com/" + list.trackingNumber;
                                //    updateTrackingShippingJsonViewModel.tracking_number = list.trackingNumber;
                                //    string res_track = BestBuy_PutShippingTracking(list.bbOrderID, _getChannelCred.Key, updateTrackingShippingJsonViewModel);
                                //    if (res_track == "204")
                                //    {
                                //        int res_ship = BestBuy_PutShippingShippedState(list.bbOrderID, _getChannelCred.Key);
                                //        if (res_ship == 204 || res_ship == 400)
                                //        {
                                //            updateTrackingOne(list.bbe2TrackingId);
                                //        }

                                //    }
                                //    else if (res_track == "400")
                                //    {
                                //        updateTrackingOne(list.bbe2TrackingId);
                                //    }
                                //}
                                //else if (is3digit == "DXA")
                                //{
                                //    updateTrackingShippingJsonViewModel = new UpdateTrackingShippingJsonViewModel();
                                //    updateTrackingShippingJsonViewModel.carrier_code = "";
                                //    updateTrackingShippingJsonViewModel.carrier_name = "DYNAMEX";
                                //    updateTrackingShippingJsonViewModel.carrier_url = "https://parcelsapp.com/en/tracking/" + list.trackingNumber;
                                //    updateTrackingShippingJsonViewModel.tracking_number = list.trackingNumber;
                                //    string res_track = BestBuy_PutShippingTracking(list.bbOrderID, _getChannelCred.Key, updateTrackingShippingJsonViewModel);
                                //    if (res_track == "204")
                                //    {
                                //        int res_ship = BestBuy_PutShippingShippedState(list.bbOrderID, _getChannelCred.Key);
                                //        if (res_ship == 204 || res_ship == 400)
                                //        {
                                //            updateTrackingOne(list.bbe2TrackingId);
                                //        }

                                //    }
                                //    else if (res_track == "400")
                                //    {
                                //        updateTrackingOne(list.bbe2TrackingId);
                                //    }
                                //}
                                //else if (is6digit == "JOEYCO")
                                //{
                                //    updateTrackingShippingJsonViewModel = new UpdateTrackingShippingJsonViewModel();
                                //    updateTrackingShippingJsonViewModel.carrier_code = "";
                                //    updateTrackingShippingJsonViewModel.carrier_name = "JOEYCO";
                                //    updateTrackingShippingJsonViewModel.carrier_url = "https://parcelsapp.com/en/tracking/" + list.trackingNumber;
                                //    updateTrackingShippingJsonViewModel.tracking_number = list.trackingNumber;
                                //    string res_track = BestBuy_PutShippingTracking(list.bbOrderID, _getChannelCred.Key, updateTrackingShippingJsonViewModel);
                                //    if (res_track == "204")
                                //    {
                                //        int res_ship = BestBuy_PutShippingShippedState(list.bbOrderID, _getChannelCred.Key);
                                //        if (res_ship == 204 || res_ship == 400)
                                //        {
                                //            updateTrackingOne(list.bbe2TrackingId);
                                //        }

                                //    }
                                //    else if (res_track == "400")
                                //    {
                                //        updateTrackingOne(list.bbe2TrackingId);
                                //    }
                                //}
                                //else if (is2digit == "1Z")
                                //{
                                //    updateTrackingShippingJsonViewModel = new UpdateTrackingShippingJsonViewModel();
                                //    updateTrackingShippingJsonViewModel.carrier_code = "UPSN";
                                //    updateTrackingShippingJsonViewModel.carrier_name = "";
                                //    updateTrackingShippingJsonViewModel.carrier_url = "";
                                //    updateTrackingShippingJsonViewModel.tracking_number = list.trackingNumber;
                                //    string res_track = BestBuy_PutShippingTracking(list.bbOrderID, _getChannelCred.Key, updateTrackingShippingJsonViewModel);
                                //    if (res_track == "204")
                                //    {
                                //        int res_ship = BestBuy_PutShippingShippedState(list.bbOrderID, _getChannelCred.Key);
                                //        if (res_ship == 204 || res_ship == 400)
                                //        {
                                //            updateTrackingOne(list.bbe2TrackingId);
                                //        }
                                //    }
                                //    else if (res_track == "400")
                                //    {
                                //        updateTrackingOne(list.bbe2TrackingId);
                                //    }
                                //}
                            //else
                            //{
                            //    updateTrackingShippingJsonViewModel = new UpdateTrackingShippingJsonViewModel();
                            //    updateTrackingShippingJsonViewModel.carrier_code = "";
                            //    updateTrackingShippingJsonViewModel.carrier_name = "Standard";
                            //    updateTrackingShippingJsonViewModel.carrier_url = "";
                            //    updateTrackingShippingJsonViewModel.tracking_number = list.trackingNumber;
                            //    string res_track = BestBuy_PutShippingTracking(list.bbOrderID, _getChannelCred.Key, updateTrackingShippingJsonViewModel);
                            //    if (res_track == "204")
                            //    {
                            //        int res_ship = BestBuy_PutShippingShippedState(list.bbOrderID, _getChannelCred.Key);
                            //        if (res_ship == 204 || res_ship == 400)
                            //        {
                            //            updateTrackingOne(list.bbe2TrackingId);
                            //        }
                            //    }
                            //    else if (res_track == "400")
                            //    {
                            //        updateTrackingOne(list.bbe2TrackingId);
                            //    }
                            //}
                        }
                        catch (Exception ex)
                        {
                            continue;
                        }
                    }
                }
               

                //foreach (var sku in skuList)
                //{
                //    GetProductInventoryForALLWarehousesResponseType[] result = null;

                //    try
                //    {
                //        var request = await sCServiceSoap.GetProductInventoryForALLWarehousesAsync(authHeader, null, sku);
                //        result = request.GetProductInventoryForALLWarehousesResult;
                //    }
                //    catch (Exception ex)
                //    {
                //        continue;
                //    }
                //}
                //throw new NotImplementedException();
            }
            catch (Exception ex)
            {

            }
            await Task.CompletedTask;
        }

        private void updateTrackingOne(int bbe2_tracking_id)
        {
            bool status = bestBuyTrackingExportDataAccess.UpdateTracking(bbe2_tracking_id);

        }

        private int BestBuy_PutShippingShippedState(string bbOrderID, string token)
        {

            string strResponse = "";
            int StatusCode = 0;
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(bbbaseapiurl + "orders/" + bbOrderID + "/ship");
                request.Method = "PUT";
                request.Accept = "application/json;";
                request.ContentType = "application/json";
                request.Headers["Authorization"] = token;

                using (HttpWebResponse webResponse = (HttpWebResponse)request.GetResponse())
                {
                    using (StreamReader stream = new StreamReader(webResponse.GetResponseStream()))
                    {
                        strResponse = stream.ReadToEnd();
                        StatusCode = (int)webResponse.StatusCode;

                    }
                }
            }
            catch (WebException ex)
            {
                HttpWebResponse response = (HttpWebResponse)ex.Response;
                WebExceptionStatus responsest = ex.Status;
                string responsems = response.StatusDescription;
                StatusCode = (int)response.StatusCode;
            }
            return StatusCode;
        }

        private string BestBuy_PutShippingTracking(string bbOrderID,string token, UpdateTrackingShippingJsonViewModel viewModel)
        {
            var data = JsonConvert.SerializeObject(viewModel);
            string strResponse = "";
            string importID = "";
            int StatusCode = 0;
           

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(bbbaseapiurl + "orders/" + bbOrderID + "/tracking");
                request.Method = "PUT";
                request.Accept = "application/json;";
                request.ContentType = "application/json";
                request.Headers["Authorization"] = token;

                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    streamWriter.Write(data);
                    streamWriter.Flush();
                    streamWriter.Close();
                }

                using (HttpWebResponse webResponse = (HttpWebResponse)request.GetResponse())
                {
                    using (StreamReader stream = new StreamReader(webResponse.GetResponseStream()))
                    {
                        strResponse = stream.ReadToEnd();
                    }
                    StatusCode = (int)webResponse.StatusCode;
                }

            }
            catch (WebException ex)
            {
                HttpWebResponse response = (HttpWebResponse)ex.Response;
                StatusCode = (int)response.StatusCode;

            }
            return StatusCode.ToString();
        }
    }
}
