using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Http.Exceptions
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
