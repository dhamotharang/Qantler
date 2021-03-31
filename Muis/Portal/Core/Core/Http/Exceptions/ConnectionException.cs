using System;

namespace Core.Http
{
  public class ConnectionException : Exception
  {
    public ConnectionException(string message = null) : base(message)
    {
    }
  }
}
