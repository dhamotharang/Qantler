using Core.Models;
using System;


namespace Core.Helpers
{
	public class AmazonS3
    {
        protected AmazonS3(Awss3Config s3Config)
        {
            accessKey = s3Config.AWSAccessKey;
            accessSecret = s3Config.AWSSecretKey;
            bucket = s3Config.AWSBucketName;
        }

        private static String accessKey { get; set; }
        private static String accessSecret { get; set; }
        private static String bucket { get; set; }
 
    }
}
