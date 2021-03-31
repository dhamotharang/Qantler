using System;
using System.Threading.Tasks;
using Enyim.Caching;

namespace Core.API.Providers
{
  public class CacheProvider : ICacheProvider
  {
    readonly IMemcachedClient _client;

    public CacheProvider(IMemcachedClient client)
    {
      _client = client;
    }

    public T Get<T>(string key)
    {
      return _client.Get<T>(key);
    }

    public async Task<T> GetAsync<T>(string key)
    {
      return (await _client.GetAsync<T>(key)).Value;
    }

    public void Remove(string key)
    {
      _client.Remove(key);
    }

    public async Task RemoveAsync(string key)
    {
      await _client.RemoveAsync(key);
    }

    public void Set<T>(string key, T value)
    {
      _client.Set(key, value, 0);
    }

    public async Task SetAsync<T>(string key, T value)
    {
      await _client.SetAsync(key, value, 0);
    }
  }
}
