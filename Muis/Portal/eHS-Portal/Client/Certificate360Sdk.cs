using Core.Http;
using eHS.Portal.Infrastructure.Providers;
using eHS.Portal.Model;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eHS.Portal.Client
{
  public class Certificate360Sdk
  {
    readonly string _url;
    readonly IHttpRequestProvider _requestProvider;

    public Certificate360Sdk(string url, IHttpRequestProvider requestProvider)
    {
      _url = url;
      _requestProvider = requestProvider;
    }

    public Task<Certificate360> GetByID(long id)
    {
      return _requestProvider.BuildUpon(_url)
        .Uri($"/api/certificate360/{id}")
        .AddInterceptor(new JsonDeserializerInterceptor())
        .Execute<Certificate360>();
    }

    public Task<IList<Certificate360History>> GetHistoryByID(long id)
    {
      return _requestProvider.BuildUpon(_url)
        .Uri($"/api/certificate360/{id}/history")
        .AddInterceptor(new JsonDeserializerInterceptor())
        .Execute<IList<Certificate360History>>();
    }

    public Task<IList<Menu>> GetMenuByID(long id)
    {
      return _requestProvider.BuildUpon(_url)
        .Uri($"/api/certificate360/{id}/menu")
        .AddInterceptor(new JsonDeserializerInterceptor())
        .Execute<IList<Menu>>();
    }

    public Task<IList<Ingredient>> GetIngredientByID(long id)
    {
      return _requestProvider.BuildUpon(_url)
        .Uri($"/api/certificate360/{id}/ingredient")
        .AddInterceptor(new JsonDeserializerInterceptor())
        .Execute<IList<Ingredient>>();
    }

    public Task<IList<Certificate360>> GetWithIngredient(Certificate360IngredientOptions options)
    {
      var builder = _requestProvider.BuildUpon(_url)
        .Uri($"/api/certificate360/with-ingredient")
        .AddInterceptor(new JsonDeserializerInterceptor());

      if (!string.IsNullOrEmpty(options.Name?.Trim()))
      {
        builder.AddParam("name", options.Name.Trim());
      }

      if (!string.IsNullOrEmpty(options.BrandName?.Trim()))
      {
        builder.AddParam("brand", options.BrandName.Trim());
      }

      if (!string.IsNullOrEmpty(options.SupplierName?.Trim()))
      {
        builder.AddParam("supplier", options.SupplierName.Trim());
      }

      if (!string.IsNullOrEmpty(options.CertifyingBodyName?.Trim()))
      {
        builder.AddParam("certifyingBody", options.CertifyingBodyName.Trim());
      }

      return builder.Execute<IList<Certificate360>>();
    }

    public Task<IList<Certificate>> Query(Certificate360Options options)
    {
      var builder = _requestProvider.BuildUpon(_url)
        .Uri("/api/certificate360/query")
        .AddInterceptor(new JsonDeserializerInterceptor());

      if (options.IDs?.Any() ?? false)
      {
        for (int i = 0; i < options.IDs.Length; i++)
        {
          builder.AddParam($"ids[{i}]", $"{(long)options.IDs[i]}");
        }
      }

      if (options.PremiseIDs?.Any() ?? false)
      {
        for (int i = 0; i < options.PremiseIDs.Length; i++)
        {
          builder.AddParam($"premiseIDs[{i}]", $"{(long)options.PremiseIDs[i]}");
        }
      }

      return builder.Execute<IList<Certificate>>();
    }
  }
}