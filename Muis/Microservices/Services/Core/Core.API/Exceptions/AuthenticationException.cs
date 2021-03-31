namespace Core.API.Exceptions
{
  public class AuthenticationException : BaseException
  {
    public AuthenticationException() : base(400, null)
    {
    }

    public AuthenticationException(string message) : base(400, message)
    {
    }
  }
}
