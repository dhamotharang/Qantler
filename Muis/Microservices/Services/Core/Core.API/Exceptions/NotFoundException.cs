namespace Core.API.Exceptions
{
  public class NotFoundException : BaseException
  {
    public NotFoundException(string message = "Not found") : base(404, message)
    {
    }
  }
}
