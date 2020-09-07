using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using DataAccess.DataAccess;
using DataAccess.Helper;
using DataAccess.ViewModels;
using Quartz;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ImageMagick;
using Amazon.S3.Transfer;

namespace HLD.WebApi.Jobs
{
    [DisallowConcurrentExecution]
    public class CompressImagesJob : IJob
    {
        IConnectionString _connectionString = null;

        CompressImagesFromS3DataAccess UploadFilesToS3 = null;

        // Specify your bucket region (an example region is shown).

        private AmazonS3Client _s3Client = new AmazonS3Client(RegionEndpoint.USEast2);

        public CompressImagesJob(IConnectionString connectionString)
        {
            _connectionString = connectionString;
            _s3Client = new AmazonS3Client(RegionEndpoint.USEast2);

            UploadFilesToS3 = new CompressImagesFromS3DataAccess(_connectionString);

        }

        public async Task Execute(IJobExecutionContext context)
        {
            List<CompressImageViewModel> getUncompressedFile = new List<CompressImageViewModel>();
            List<CompressImageViewModel> updateimages = new List<CompressImageViewModel>();
            getUncompressedFile = UploadFilesToS3.GetImagestoCompress();
            foreach (var item in getUncompressedFile)
            {

                RunjobTocompressImages(item).Wait();
                updateimages.Add(new CompressImageViewModel
                {
                    imageName = item.imageName,
                    sku = item.sku
                

                });


            }
            if (updateimages!=null)
            {
                 UploadFilesToS3.UpdateASCompressedImage(updateimages);
            }



            await Task.CompletedTask;
        }


        public async Task RunjobTocompressImages(CompressImageViewModel uncompressedImages)
        {
            System.Drawing.Image image = null;
            try
            {
                GetObjectRequest request = new GetObjectRequest
                {
                    BucketName = "upload.hld.erp.images",
                    Key = uncompressedImages.imageName
                };
                using (GetObjectResponse response = await _s3Client.GetObjectAsync(request))
                using (Stream responseStream = response.ResponseStream)
             //   using (StreamReader reader = new StreamReader(responseStream))
                {
                    image = System.Drawing.Image.FromStream(responseStream);
                    Stream reduceImageStream = GetStreamOfReducedImage(image);
                    if (reduceImageStream != null)
                    {
                        await uploadASINImagesToS3Compressed(reduceImageStream, uncompressedImages.imageName);

                       
                    }
                }

                

            }
            catch (Exception ex)
            {
               
            }
         }

        public Stream GetStreamOfReducedImage(Image inputPath)
        {
            MemoryStream memoryStream = new MemoryStream();
            try
            {
                var settings = new MagickReadSettings { Format = MagickFormat.Raw };
                byte[] vs = ImageToByteArray(inputPath);
                int size = 100;
                int quality = 75;
                using (var image = new MagickImage(vs))
                {
                    image.Resize(size, size);
                    image.Strip();
                    image.Quality = quality;
                    image.Write(memoryStream, MagickFormat.Jpeg);
                }
            }
            catch (Exception ex)
            {
                memoryStream = null;
            }
            finally
            {

            }
            return memoryStream;
        }

        public byte[] ImageToByteArray(System.Drawing.Image imageIn)
        {
            using (var ms = new MemoryStream())
            {
                imageIn.Save(ms, imageIn.RawFormat);
                return ms.ToArray();
            }
        }
        
        public async Task<bool> uploadASINImagesToS3Compressed(Stream stream, string keyName)
        {
            bool status = false;
            try
            {
                TransferUtility fileTransferUtility = new TransferUtility(new AmazonS3Client(Amazon.RegionEndpoint.USEast2));

                string bucketName = "upload.hld.erp.images.thumbnail";
               
                // 1. Upload a file, file name is used as the object key name.
                await fileTransferUtility.UploadAsync(stream, bucketName, keyName);
                status = true;
                return status;
            }
            catch (AmazonS3Exception s3Exception)
            {
                Console.WriteLine(s3Exception.Message,
                                  s3Exception.InnerException);
            }
            return status;
        }

        //public Stream GetStream(Image img, ImageFormat format)
        //{
        //    var ms = new MemoryStream();
        //    img.Save(ms, format);
        //    return ms;
        //}

        //public System.Drawing.Image DownloadImageFromUrl(string imageUrl)
        //{
        //    System.Drawing.Image image = null;

        //    try
        //    {
        //        //Uri ImageUri = new Uri(imageUrl, UriKind.Absolute);
        //        //System.Net.HttpWebRequest webRequest = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(ImageUri);
        //        //webRequest.AllowWriteStreamBuffering = true;
        //        //webRequest.Timeout = 30000;

        //        //System.Net.WebResponse webResponse = webRequest.GetResponse();

        //        //System.IO.Stream stream = webResponse.GetResponseStream();



        //        image = System.Drawing.Image.FromStream(stream);

        //        webResponse.Close();
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }

        //    return image;
        //}


    }
}
