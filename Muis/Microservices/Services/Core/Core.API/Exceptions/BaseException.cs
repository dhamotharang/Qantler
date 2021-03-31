using System;

namespace Core.API.Exceptions
{
  public class BaseException : Exception
  {
    public int Code { get; set; }

    public BaseException(int code, string message) : base(message)
    {
      Code = code;
    }
  }
}
