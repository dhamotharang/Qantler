using System;
using System.Threading.Tasks;
using Enyim.Caching;

namespace eHS.Portal.Infrastructure.Providers
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

    public void Set<T>(string key, T value)
    {
      _client.Set(key, value, (int)DateTime.UtcNow.AddMonths(3).Ticks);
    }

    public async Task SetAsync<T>(string key, T value)
    {
      await _client.SetAsync(key, value, (int)DateTime.UtcNow.AddMonths(3).Ticks);
    }
  }
}
