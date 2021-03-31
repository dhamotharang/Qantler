using Core.API.Providers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.API.Provider
{
  public class DefaultCacheProvider : ICacheProvider
  {
    T ICacheProvider.Get<T>(string key)
    {
      return default(T);
    }

    Task<T> ICacheProvider.GetAsync<T>(string key)
    {
      return Task.Run(() => default(T));
    }

    void ICacheProvider.Remove(string key)
    {
      
    }

    Task ICacheProvider.RemoveAsync(string key)
    {
      return Task.CompletedTask;
    }

    void ICacheProvider.Set<T>(string key, T value)
    {
      
    }

    Task ICacheProvider.SetAsync<T>(string key, T value)
    {
      return Task.CompletedTask;
    }
  }
}
