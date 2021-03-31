using Core.Http;
using eHS.Portal.Infrastructure.Providers;
using eHS.Portal.Model;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace eHS.Portal.Client
{
  public class PersonSdk
  {
    readonly string _url;
    readonly IHttpRequestProvider _requestProvider;

    public PersonSdk(string url, IHttpRequestProvider requestProvider)
    {
      _url = url;
      _requestProvider = requestProvider;
    }

    public Task<Person> GetPersonByAltID(string altID)
    {
      return _requestProvider.BuildUpon(_url)
        .Uri("/api/person/find")
        .AddParam("altID", altID)
        .AddInterceptor(new JsonDeserializerInterceptor())
        .Execute<Person>();
    }

    public Task<Person> Submit(Person person)
    {
      return _requestProvider.BuildUpon(_url)
        .Uri("/api/person")
        .Method(HttpMethod.Post)
        .AddInterceptor(new JsonDeserializerInterceptor())
        .Content(new JsonContent(person))
        .Execute<Person>();
    }

    public Task<Person> Update(Person person)
    {
      return _requestProvider.BuildUpon(_url)
        .Uri("/api/person")
        .Method(HttpMethod.Put)
        .AddInterceptor(new JsonDeserializerInterceptor())
        .Content(new JsonContent(person))
        .Execute<Person>();
    }

    public Task<Model.Person> GetByID(Guid id)
    {
      return _requestProvider.BuildUpon(_url)
        .Uri($"/api/person/{id}")
        .AddInterceptor(new JsonDeserializerInterceptor())
        .Execute<Model.Person>();
    }
  }
}

