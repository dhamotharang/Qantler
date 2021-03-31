using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Core.Http
{
  public interface IInterceptor
  {
    /// <summary>
    /// Called before the request is executed.
    /// </summary>
    /// <param name="request"></param>
    void PreHandle(HttpRequestMessage request);

    /// <summary>
    /// Called after the request has been executed.
    /// </summary>
    /// <typeparam name="T">The T result</typeparam>
    /// <param name="response">the response</param>
    /// <returns>return true if handled the response</returns>
    Task<T> PostHandle<T>(HttpResponseMessage response);

    /// <summary>
    /// Called request execution is completed.
    /// </summary>
    void OnCompleted<T>(T data);

    /// <summary>
    /// Called when a WebException has been thrown.
    /// </summary>
    /// <param name="ex">the exception</param>
    /// <returns>false if break the process flow.</returns>
    bool OnError(Exception ex);
  }

  public abstract class AbsInterceptor : IInterceptor
  {
    public virtual void PreHandle(HttpRequestMessage request)
    {
    }

    public virtual Task<T> PostHandle<T>(HttpResponseMessage response)
    {
      return null;
    }

    public virtual bool OnError(Exception ex)
    {
      return true;
    }

    public virtual void OnCompleted<T>(T data)
    {
    }
  }
}
