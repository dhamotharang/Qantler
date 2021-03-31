using System;

namespace Core.Http
{
  public class ExceptionInterceptor : AbsInterceptor
  {
    public override bool OnError(Exception ex)
    {
      if (   !(ex is ConnectionException)
          && !(ex is HttpException)
          && !(ex is ServerNotFoundException)
          && !(ex is UnauthorizedException))
      {
        throw new ServerNotFoundException();
      }
      return base.OnError(ex);
    }
  }
}
