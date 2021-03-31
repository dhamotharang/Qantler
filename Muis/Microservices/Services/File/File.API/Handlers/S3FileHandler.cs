using System;
using System.IO;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;
using Core.API.Exceptions;
using File.API.Config;

namespace File.API.Handlers
{
  public class S3FileHandler : IFileHandler
  {
    readonly AmazonS3Client _client;
    readonly FileConfig _fileConfig;

    public S3FileHandler(FileConfig config)
    {
      _fileConfig = config;

      _client = new AmazonS3Client(_fileConfig.Key,
        _fileConfig.Secret,
        Amazon.RegionEndpoint.GetBySystemName(_fileConfig.Region));
    }

    public async Task Delete(string directory, string fileName)
    {
      await _client.DeleteObjectAsync(new DeleteObjectRequest
      {
        BucketName = _fileConfig.Root,
        Key = fileName
      });
    }

    public async Task<Stream> OpenRead(string directory, string fileName)
    {
      var response = await _client.GetObjectAsync(new GetObjectRequest
      {
        BucketName = _fileConfig.Root,
        Key = _fileConfig.Key
      });

      return response.ResponseStream;
    }

    public async Task Write(string source, string directory, string fileName)
    {
      PutObjectResponse response = null;

      using (var stream = System.IO.File.OpenRead(source))
      {
        var request = new PutObjectRequest
        {
          BucketName = _fileConfig.Root,
          Key = fileName,
          InputStream = stream,
          ContentType = "application/octet-stream",
          CannedACL = S3CannedACL.PublicRead
        };

        response = await _client.PutObjectAsync(request);
      }

      if (response.HttpStatusCode != System.Net.HttpStatusCode.OK)
      {
        throw new BadRequestException(fileName);
      }
    }
  }
}
