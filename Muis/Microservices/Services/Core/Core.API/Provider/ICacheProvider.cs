using System;
using System.Threading.Tasks;

namespace Core.API.Providers
{
  public interface ICacheProvider
  {
    T Get<T>(string key);

    void Set<T>(string key, T value);

    void Remove(string key);

    Task<T> GetAsync<T>(string key);

    Task SetAsync<T>(string key, T value);

    Task RemoveAsync(string key);
  }
}
