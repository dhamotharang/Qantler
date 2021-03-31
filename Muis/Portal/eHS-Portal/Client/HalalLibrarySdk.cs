using Core.Http;
using eHS.Portal.Infrastructure.Providers;
using eHS.Portal.Model;
using eHS.Portal.Models.HalalLibrary;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace eHS.Portal.Client
{
  public class HalalLibrarySdk : IHalalLibrarySdk
  {
    readonly string _url;
    readonly IHttpRequestProvider _requestProvider;

    public HalalLibrarySdk(string url, IHttpRequestProvider requestProvider)
    {
      _url = url;
      _requestProvider = requestProvider;
    }

    public Task<IndexModel> Search(HalalLibraryOptions options, int offsetRows, int nextRows)
    {
      var builder = _requestProvider.BuildUpon(_url)
        .Uri("/api/ingredient/query")
        .AddInterceptor(new JsonDeserializerInterceptor());
      builder.AddParam("nextRows", $"{nextRows}");
      builder.AddParam("offsetRows", $"{offsetRows}");

      if (!string.IsNullOrEmpty(options.Name))
      {
        builder.AddParam("Name", options.Name.Trim());
      }

      if (!string.IsNullOrEmpty(options.Brand))
      {
        builder.AddParam("Brand", options.Brand.Trim());
      }

      if (!string.IsNullOrEmpty(options.Supplier))
      {
        builder.AddParam("Supplier", options.Supplier.Trim());
      }

      if (!string.IsNullOrEmpty(options.CertifyingBody))
      {
        builder.AddParam("CertifyingBody", options.CertifyingBody.Trim());
      }

      if (!string.IsNullOrEmpty(options.VerifiedBy))
      {
        builder.AddParam("VerifiedBy", options.VerifiedBy.Trim());
      }

      if (options.Status != null)
      {
        builder.AddParam("Status", $"{(int)options.Status}");
      }

      if (options.RiskCategory != null)
      {
        builder.AddParam("RiskCategory", $"{(int)options.RiskCategory}");
      }

      return builder.Execute<IndexModel>();
    }

    public async Task<long> InsertHalalLibrary(HLIngredient data)
    {
      return await _requestProvider.BuildUpon(_url)
       .Uri($"/api/ingredient")
       .Method(HttpMethod.Post)
       .Content(new JsonContent(data))
       .AddInterceptor(new JsonDeserializerInterceptor())       
       .Execute<long>();
    }

    public async Task<long> UpdateHalalLibrary(HLIngredient data)
    {
      return await _requestProvider.BuildUpon(_url)
       .Uri($"/api/ingredient")
       .Method(HttpMethod.Put)
       .AddInterceptor(new JsonDeserializerInterceptor())
       .Content(new JsonContent(data))
       .Execute<long>();
    }

    public async Task<bool> DeleteHalalLibrary(long id)
    {
      return await _requestProvider.BuildUpon(_url)
       .Uri($"/api/ingredient")
       .AddParam("id", $"{id}")
       .Method(HttpMethod.Delete)
       .AddInterceptor(new JsonDeserializerInterceptor())
       .Execute<bool>();
    }

    public Task<IEnumerable<Supplier>> GetSupplier()
    {
      var builder = _requestProvider.BuildUpon(_url)
        .Uri("/api/supplier/select")
        .AddInterceptor(new JsonDeserializerInterceptor());

      return builder.Execute<IEnumerable<Supplier>>();
    }

    public async Task<long> InsertSupplier(Supplier data)
    {
      return await _requestProvider.BuildUpon(_url)
       .Uri($"/api/supplier")
       .Method(HttpMethod.Post)
       .AddInterceptor(new JsonDeserializerInterceptor())
       .Content(new JsonContent(data))
       .Execute<long>();
    }

    public Task<IEnumerable<CertifyingBody>> GetCertifyingBody()
    {
      var builder = _requestProvider.BuildUpon(_url)
        .Uri("/api/certifyingbody/select")
        .AddInterceptor(new JsonDeserializerInterceptor());

      return builder.Execute<IEnumerable<CertifyingBody>>();
    }

    public async Task<long> InsertCertifyingBody(CertifyingBody data)
    {
      return await _requestProvider.BuildUpon(_url)
       .Uri($"/api/certifyingbody")
       .Method(HttpMethod.Post)
       .AddInterceptor(new JsonDeserializerInterceptor())
       .Content(new JsonContent(data))
       .Execute<long>();
    }
  }
}
