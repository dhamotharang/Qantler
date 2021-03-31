using Core.Models;
using Microsoft.Win32.SafeHandles;
using Minio;
using Minio.DataModel;
using Minio.Exceptions;
using OERService.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace OERService.Core.Helpers
{
	public class MinIO :IDisposable
    {
        public MinIO(string AccessKey, string AccessSecret, string Bucket)
        {
            accessKey = AccessKey;
            accessSecret = AccessSecret;
            bucket = Bucket;
        }
		//asd
		bool disposed = false;
		readonly SafeHandle handle = new SafeFileHandle(IntPtr.Zero, true);
		private static string accessKey { get; set; }
        private static string accessSecret { get; set; }
        private string bucket { get; set; }
        public string location { get; set; }
        public string endpoint { get; set; }
        public string BaseUrl { get; set; }
        public async Task<DownloadResponse> DownloadFiles(string objectKey)
        {
            DownloadResponse result = null;
            try
            {
                var minio = new MinioClient(this.endpoint, accessKey, accessSecret);

                await minio.GetObjectAsync(this.bucket, objectKey,
                    (stream) =>
                    {
                        result = new DownloadResponse();
                        result.HasSucceed = false;
                        result.FileObject = ReadStream(stream);

                    });

            }
            catch (MinioException e)
            {
                Console.WriteLine("Error occurred: " + e);
            }
            return result;
        }

        private static byte[] ReadStream(Stream responseStream)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = responseStream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }
        public async Task<UploadResponse> FileUpload(byte[] imageBytes,string fileName)
        {
            var bucketName = this.bucket;
            UploadResponse obj = new UploadResponse();

            try
            {
                var minio = new MinioClient(this.endpoint, accessKey, accessSecret);
                // Make a bucket on the server, if not already present.
                bool found = await minio.BucketExistsAsync(bucketName);
                if (!found)
                {
                    await minio.MakeBucketAsync(bucketName, this.location);
                }
                    using (var stream = new MemoryStream(imageBytes))
                    {
                        // Upload a file to bucket.
                        await minio.PutObjectAsync(bucketName, fileName, stream, stream.Length);
                    }
                    obj.Message = "Uploded Successfully.";
                    obj.HasSucceed = true;
                    obj.FileName = fileName;
                    obj.FileUrl = this.BaseUrl + bucketName + "/" + fileName;

            }
            catch (MinioException e)
            {
                Console.WriteLine("File Upload Error: {0}", e.Message);
                obj.Message = "Uploaded failed with error " + e.message;
                obj.HasSucceed = false;
                obj.FileUrl = this.BaseUrl + bucketName + "/";
            }
            return obj;
        }

        public async Task<UploadResponse> FileUpload(List<ContentMedia> contentMedia)
        {
            var bucketName = this.bucket;
            byte[] imageBytes = null;
            UploadResponse obj = new UploadResponse();

            try
            {
                var minio = new MinioClient(this.endpoint, accessKey, accessSecret);
                // Make a bucket on the server, if not already present.
                bool found = await minio.BucketExistsAsync(bucketName);
                if (!found)
                {
                    await minio.MakeBucketAsync(bucketName, this.location);
                }
                foreach (var item in contentMedia)
                {
                    imageBytes = Convert.FromBase64String(item.FileBase64);
                    using (var stream = new MemoryStream(imageBytes))
                    {
                        // Upload a file to bucket.
                        await minio.PutObjectAsync(bucketName, item.FileName, stream, stream.Length);
                    }
                }
                if (contentMedia.Count > 0)
                {
                    obj.Message = "Uploaded Successfully.";
                    obj.HasSucceed = true;
                    obj.FileName = contentMedia[0].FileName;
                    obj.FileUrl = this.BaseUrl + bucketName + "/" + contentMedia[0].FileName;
                }
                else
                {
                    obj.Message = this.BaseUrl + bucketName + "/";
                }
            }
            catch (MinioException e)
            {
                Console.WriteLine("File Upload Error: {0}", e.Message);
                obj.Message = "Uploaded failed with error " + e.message;
                obj.HasSucceed = false;
                obj.FileUrl = this.BaseUrl + bucketName + "/";
            }
            return obj;
        }

        public async Task<UploadResponse> FileUploadv2(Microsoft.AspNetCore.Http.IFormFile file)
        {
            var bucketName = this.bucket;
            UploadResponse obj = new UploadResponse();

            try
            {
                var minio = new MinioClient(this.endpoint, accessKey, accessSecret);
                // Make a bucket on the server, if not already present.
                bool found = await minio.BucketExistsAsync(bucketName);
                if (!found)
                {
                    await minio.MakeBucketAsync(bucketName, this.location);
                }

                var stream = file.OpenReadStream();
                await minio.PutObjectAsync(bucketName, file.Name, stream, stream.Length);

                if (file.Length > 0)
                {
                    obj.Message = "Uploaded Successfully.";
                    obj.HasSucceed = true;
                    obj.FileName = file.Name;
                    obj.FileUrl = this.BaseUrl + bucketName + "/" + file.Name;
                }
                else
                {
                    obj.Message = this.BaseUrl + bucketName + "/";
                }



            }
            catch (MinioException e)
            {
                Console.WriteLine("File Upload Error: {0}", e.Message);
                obj.Message = "Uploaded failed with error " + e.message;
                obj.HasSucceed = false;
                obj.FileUrl = this.BaseUrl + bucketName + "/";
            }
            return obj;
        }

        public async Task<UploadResponse> FileUploadCopy(List<TempFilesToDestination> content)
        {
            var bucketName = this.bucket;
            UploadResponse obj = new UploadResponse();

            try
            {
                var minio = new MinioClient(this.endpoint, accessKey, accessSecret);
                // Make a bucket on the server, if not already present.
                bool found = await minio.BucketExistsAsync(bucketName);
                if (!found)
                {
                    await minio.MakeBucketAsync(bucketName, this.location);
                }

                foreach (var item in content)
                {
                    try
                    {
                        await minio.StatObjectAsync(bucketName, item.distObjectName);
                    }
                    catch (MinioException e)
                    {
                        await minio.CopyObjectAsync(bucketName, item.tempObjectName, bucketName, item.distObjectName);
                        await minio.RemoveObjectAsync(bucketName, item.tempObjectName);
                    }
                }
                    obj.Message = "Uploaded Successfully.";
                    obj.HasSucceed = true;
              
            }
            catch (MinioException e)
            {
                Console.WriteLine("File Upload Error: {0}", e.Message);
                obj.Message = "Uploaded failed with error " + e.message;
                obj.HasSucceed = false;
                obj.FileUrl = this.BaseUrl + bucketName + "/";
            }
            return obj;
        }

        public async Task<UploadResponse> FilesDelete(List<string> content)
        {
            var bucketName = this.bucket;
            UploadResponse obj = new UploadResponse();

            try
            {
                var minio = new MinioClient(this.endpoint, accessKey, accessSecret);
                // Make a bucket on the server, if not already present.
                bool found = await minio.BucketExistsAsync(bucketName);
                if (!found)
                {
                    await minio.MakeBucketAsync(bucketName, this.location);
                }

                string failedObj = string.Empty;
                foreach (var item in content)
                {
                    try
                    {
                        await minio.StatObjectAsync(bucketName, item);
                        await minio.RemoveObjectAsync(bucketName, item);
                        obj.Message = "Delete Successfully.";
                    }
                    catch (MinioException e)
                    {
                        failedObj = failedObj +","+ item;
                    }
                }
                if (string.IsNullOrEmpty(obj.Message))
                {
                    obj.HasSucceed = false;
                    obj.Message = failedObj + " are not found in bucket" + bucketName;
                }
                else
                {
                    obj.HasSucceed = true;
                    if(!string.IsNullOrEmpty(failedObj))
                    {
                        obj.Message = obj.Message + " and " + failedObj + " are not found in bucket" + bucketName;
                    }
                }

            }
            catch (MinioException e)
            {
                Console.WriteLine("File Delete Error: {0}", e.Message);
                obj.Message = "Delete failed with error " + e.message;
                obj.HasSucceed = false;
                obj.FileUrl = this.BaseUrl + bucketName + "/";
            }
            return obj;
        }

        public void Dispose()
        {
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (disposed)
				return;

			if (disposing)
			{
				handle.Dispose();
				// Free any other managed objects here.
				//
			}

			disposed = true;
		}
	}
}
