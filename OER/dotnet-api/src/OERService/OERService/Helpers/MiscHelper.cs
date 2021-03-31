using Core.Models;
using System.Collections.Generic;
using System.Linq;

namespace OERService.Helpers
{
	public class MiscHelper
    {
        public Awss3Config GeAWSConfig(List<Dictionary<string, string>> configDict)
        {
            Awss3Config config = new Awss3Config();            
            config.AWSAccessKey = configDict.Single(x => x["key"] == "AWSAccessKey")["value"];
            config.AWSSecretKey = configDict.Single(x => x["key"] == "AWSSecretKey")["value"];
            config.AWSBucketName = configDict.Single(x => x["key"] == "AWSBucketName")["value"];
            config.AWSUser = configDict.Single(x => x["key"] == "AWSUser")["value"];
            config.AWSEndPoint = configDict.Single(x => x["key"] == "AWSEndPoint")["value"];
            return config;
        }
    }
}
