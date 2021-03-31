using System;
using System.Threading.Tasks;

namespace eHS.Portal.Infrastructure.Providers
{
  public interface ICacheProvider
  {
    T Get<T>(string key);

    void Set<T>(string key, T value);

    Task<T> GetAsync<T>(string key);

    Task SetAsync<T>(string key, T value);
  }
}
