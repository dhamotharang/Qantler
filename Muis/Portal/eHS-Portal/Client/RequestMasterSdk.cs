using Core.Http;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace eHS.Portal.Client
{
  public class RequestMasterSdk
  {
    readonly string _url;

    public RequestMasterSdk(string url)
    {
      _url = url;
    }

    public async Task<IList<Model.Master>> GetMasterList(Model.MasterType type)
    {
      return await new HttpRequest.Builder()
       .BaseURL(_url)
       .Uri($"/api/Master")
       .AddParam("type", $"{(int)type}")
       .AddInterceptor(new JsonDeserializerInterceptor())
       .Execute<IList<Model.Master>>();
    }

    public async Task<bool> InsertMaster(Model.Master data)
    {
      return await new HttpRequest.Builder()
       .BaseURL(_url)
       .Uri($"/api/Master")
       .Method(HttpMethod.Post)
       .AddInterceptor(new JsonDeserializerInterceptor())
       .Content(new JsonContent(data))
       .Execute<bool>();
    }

    public async Task<bool> UpdateMaster(Model.Master data)
    {
      return await new HttpRequest.Builder()
       .BaseURL(_url)
       .Uri($"/api/Master")
       .Method(HttpMethod.Put)
       .AddInterceptor(new JsonDeserializerInterceptor())
       .Content(new JsonContent(data))
       .Execute<bool>();
    }

    public async Task<bool> DeleteMaster(Guid id)
    {
      return await new HttpRequest.Builder()
       .BaseURL(_url)
       .Uri($"/api/Master")
       .AddParam("id", $"{id}")
       .Method(HttpMethod.Delete)
       .AddInterceptor(new JsonDeserializerInterceptor())
       .Execute<bool>();
    }
  }
}
