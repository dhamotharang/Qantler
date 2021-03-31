using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Core.Http;
using eHS.Portal.Infrastructure.Providers;
using eHS.Portal.Model;

namespace eHS.Portal.Client
{
  public class CertificateSdk
  {
    readonly string _url;
    readonly IHttpRequestProvider _requestProvider;

    public CertificateSdk(string url, IHttpRequestProvider requestProvider)
    {
      _url = url;
      _requestProvider = requestProvider;
    }

    public Task<IList<CertificateBatch>> GetCertificateBatch(CertificateBatchOptions options, 
      bool full)
    {
      var builder = _requestProvider.BuildUpon(_url)
        .Uri("/api/certificate/batch/list")
        .AddInterceptor(new JsonDeserializerInterceptor())
        .AddParam("includeAll", $"{full}")
        .AddParam("from", options.From.UtcDateTime.ToString());

      if (options.Status?.Any() ?? false)
      {
        for (int i = 0; i < options.Status.Count; i++)
        {
          var status = options.Status[i];
          builder.AddParam($"status[{i}]", $"{(int)status}");
        }
      }

      return builder.Execute<IList<CertificateBatch>>();
    }

    public Task<CertificateBatch> GetCertificateBatchByID(long id, bool full)
    {
      return _requestProvider.BuildUpon(_url)
        .Uri($"/api/certificate/batch/{id}")
        .AddParam("includeAll", $"{full}")
        .AddInterceptor(new JsonDeserializerInterceptor())
        .Execute<CertificateBatch>();
    }

    public Task<IList<Comment>> GetCertificateBatchComments(long id)
    {
      return _requestProvider.BuildUpon(_url)
        .Uri($"/api/certificate/batch/{id}/comments")
        .AddInterceptor(new JsonDeserializerInterceptor())
        .Execute<IList<Comment>>();
    }

    public Task<string> AcknowledgeBatch(long[] ids)
    {
      return _requestProvider.BuildUpon(_url)
        .Method(HttpMethod.Post)
        .Uri($"/api/certificate/batch/acknowledge")
        .Content(new JsonContent(ids))
        .AddInterceptor(new JsonDeserializerInterceptor())
        .Execute<string>();
    }

    public Task<Comment> AddComment(long batchID, string text)
    {
      return _requestProvider.BuildUpon(_url)
        .Uri($"/api/certificate/batch/{batchID}/comment")
        .Method(HttpMethod.Post)
        .AddParam("text", text)
        .AddInterceptor(new JsonDeserializerInterceptor())
        .Execute<Comment>();
    }

    public Task<string> MapFileToCertificateBatch(long batchID, Guid fileID)
    {
      return _requestProvider.BuildUpon(_url)
        .Uri($"/api/certificate/batch/{batchID}/link/file")
        .Method(HttpMethod.Post)
        .AddParam("fileID", fileID.ToString())
        .AddInterceptor(new JsonDeserializerInterceptor())
        .Execute<string>();
    }

    public Task<string> UpdateCertificateBatchStatus(long batchID, CertificateBatchStatus status)
    {
      return _requestProvider.BuildUpon(_url)
        .Uri($"/api/certificate/batch/{batchID}/status")
        .Method(HttpMethod.Put)
        .AddParam("status", $"{(int)status}")
        .AddInterceptor(new JsonDeserializerInterceptor())
        .Execute<string>();
    }

    public Task<IList<Certificate>> GetCertificates(CertificateOptions options)
    {
      var builder = _requestProvider.BuildUpon(_url)
        .Uri("/api/certificate/list")
        .AddInterceptor(new JsonDeserializerInterceptor());

      if (options.IDs?.Any() ?? false)
      {
        for (int i = 0; i < options.IDs.Length; i++)
        {
          var id = options.IDs[i];
          builder.AddParam($"ids[{i}]", $"{id}");
        }
      }

      return builder.Execute<IList<Certificate>>();
    }

    public Task<IList<Certificate>> GetDeliveryCertificates(CertificateDeliveryOptions options)
    {
      var builder = _requestProvider.BuildUpon(_url)
        .Uri("/api/certificate/deliverylist")
        .AddInterceptor(new JsonDeserializerInterceptor())

      .AddParam("customerCode", options.CustomerCode)
      .AddParam("customerName", options.CustomerName)
      .AddParam("postal", options.Postal)
      .AddParam("premise", options.Premise)
      .AddParam("serialNo", options.SerialNo);

      if (options.Status?.Any() ?? false)
      {
        for (int i = 0; i < options.Status.Count; i++)
        {
          var status = options.Status[i];
          builder.AddParam($"status[{i}]", $"{(int)status}");
        }
      }

      if (options.IssuedOnFrom != null)
      {
        builder.AddParam($"issuedOnFrom", $"{options.IssuedOnFrom}");
      }

      if (options.IssuedOnTo != null)
      {
        builder.AddParam($"issuedOnTo", $"{options.IssuedOnTo}");
      }

      return builder.Execute<IList<Certificate>>();
    }

    public Task<string> ExecuteCertDelivery(long[] IDs)
    {
      return _requestProvider.BuildUpon(_url)
        .Uri($"/api/certificate/deliver")
        .Method(HttpMethod.Post)
        .Content(new JsonContent(IDs))
        .AddInterceptor(new JsonDeserializerInterceptor())
        .Execute<string>();
    }
  }
}
