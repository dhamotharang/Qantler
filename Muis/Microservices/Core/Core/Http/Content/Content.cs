using System;
using System.Net.Http;

namespace Core.Http
{
  public abstract class Content
  {
    internal abstract void WriteContent(HttpRequestMessage request);
  }
}
