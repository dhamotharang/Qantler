using System.Threading.Tasks;

namespace Core.Base
{
  public abstract class Validator
  {
    Validator _next;
    Validator _parent;

    public abstract Task Validate();

    public Task Invoke()
    {
      return GetRoot().InternalInvoke();
    }

    public Validator Next(Validator next)
    {
      next._parent = this;
      _next = next;
      return next;
    }

    Validator GetRoot()
    {
      return _parent?.GetRoot() ?? this;
    }

    async Task InternalInvoke()
    {
      await Validate();
      if (_next != null)
      {
        await _next.InternalInvoke();
      }
    }
  }
}
