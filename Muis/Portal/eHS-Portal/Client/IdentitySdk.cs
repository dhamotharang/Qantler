using Core.Http;
using eHS.Portal.Infrastructure.Providers;
using eHS.Portal.Model;
using eHS.Portal.Services;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace eHS.Portal.Client
{
  public class IdentitySdk
  {
    readonly string _url;
    readonly IHttpRequestProvider _requestProvider;

    public IdentitySdk(string url, IHttpRequestProvider requestProvider)
    {
      _url = url;
      _requestProvider = requestProvider;
    }

    public async Task<Identity> GetIdentityByID(Guid id)
    {
      return await _requestProvider.BuildUpon(_url)
        .Uri($"/api/identity/{id}")
        .AddInterceptor(new JsonDeserializerInterceptor())
        .Execute<Identity>();
    }

    public Task<IList<Identity>> List(IdentityFilter filter)
    {
      var builder = _requestProvider.BuildUpon(_url)
        .Uri("/api/identity/list")
        .AddInterceptor(new JsonDeserializerInterceptor());

      if (!string.IsNullOrEmpty(filter.Name))
      {
        builder.AddParam("name", filter.Name.Trim());
      }

      if (!string.IsNullOrEmpty(filter.Email))
      {
        builder.AddParam("email", filter.Email.Trim());
      }

      if ((filter.RequestTypes?.Length ?? 0) > 0)
      {
        for (int i = 0; i < filter.RequestTypes.Length; i++)
        {
          builder.AddParam($"requestTypes[{i}]", $"{(int)filter.RequestTypes[i]}");
        }
      }

      if ((filter.Permissions?.Length ?? 0) > 0)
      {
        for (int i = 0; i < filter.Permissions.Length; i++)
        {
          builder.AddParam($"permissions[{i}]", $"{(int)filter.Permissions[i]}");
        }
      }

      if ((filter.Clusters?.Length ?? 0) > 0)
      {
        for (int i = 0; i < filter.Clusters.Length; i++)
        {
          builder.AddParam($"clusters[{i}]", $"{filter.Clusters[i]}");
        }
      }

      if ((filter.Nodes?.Length ?? 0) > 0)
      {
        for (int i = 0; i < filter.Nodes.Length; i++)
        {
          builder.AddParam($"nodes[{i}]", $"{filter.Nodes[i]}");
        }
      }

      builder.AddParam($"status", $"{(int)filter.Status}");

      return builder.Execute<IList<Identity>>();
    }

    public Task<Identity> CreateIdentity(Identity identity)
    {
      return _requestProvider.BuildUpon(_url)
        .Uri("/api/identity")
        .Method(HttpMethod.Post)
        .Content(new JsonContent(identity))
        .AddInterceptor(new JsonDeserializerInterceptor())
        .Execute<Identity>();
    }

    public Task<Identity> UpdateIdentity(Identity identity)
    {
      return _requestProvider.BuildUpon(_url)
        .Uri($"/api/identity/{identity.ID}")
        .Method(HttpMethod.Put)
        .Content(new JsonContent(identity))
        .AddInterceptor(new JsonDeserializerInterceptor())
        .Execute<Identity>();
    }

    public Task<string> ResetPassword(Guid id)
    {
      return _requestProvider.BuildUpon(_url)
        .Uri($"/api/identity/{id}/password")
        .Method(HttpMethod.Delete)
        .AddInterceptor(new JsonDeserializerInterceptor())
        .Execute<string>();
    }

    public Task<string> ForgotPassword(string email)
    {
      return _requestProvider.BuildUpon(_url)
        .Uri("/api/identity/forgot-password")
         .AddParam("email", email)
        .Method(HttpMethod.Post)
        .AddInterceptor(new JsonDeserializerInterceptor())
        .Execute<string>();
    }

    public Task<Identity> GetCertAuditor(string ClusterNode, IdentityFilter filter)
    {
      var builder = new HttpRequest.Builder().BaseURL(_url)
        .Uri("/api/identity/CertAuditor")
         .AddParam("ClusterNode", ClusterNode)
        .AddInterceptor(new JsonDeserializerInterceptor());

      if (!string.IsNullOrEmpty(filter.Name))
      {
        builder.AddParam("name", filter.Name.Trim());
      }

      if (!string.IsNullOrEmpty(filter.Email))
      {
        builder.AddParam("email", filter.Email.Trim());
      }

      if ((filter.RequestTypes?.Length ?? 0) > 0)
      {
        for (int i = 0; i < filter.RequestTypes.Length; i++)
        {
          builder.AddParam($"requestTypes[{i}]", $"{(int)filter.RequestTypes[i]}");
        }
      }

      if ((filter.Permissions?.Length ?? 0) > 0)
      {
        for (int i = 0; i < filter.Permissions.Length; i++)
        {
          builder.AddParam($"permissions[{i}]", $"{(int)filter.Permissions[i]}");
        }
      }

      if ((filter.Clusters?.Length ?? 0) > 0)
      {
        for (int i = 0; i < filter.Clusters.Length; i++)
        {
          builder.AddParam($"clusters[{i}]", $"{filter.Clusters[i]}");
        }
      }

      return builder.Execute<Identity>();
    }

  }
}
