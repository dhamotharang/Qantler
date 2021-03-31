using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Http.Exceptions
{
  public class BadRequestException : BaseException
  {
    public BadRequestException(string message) : base(400, message)
    {
    }

    public BadRequestException(int code, string message) : base(code, message)
    {
    }
  }
}
