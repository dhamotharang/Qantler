using System;

namespace Core.Http.Exceptions
{
  public class NotFoundException : BaseException
  {
    public NotFoundException(string message = "Not found") : base(404, message)
    {
    }
  }
}
