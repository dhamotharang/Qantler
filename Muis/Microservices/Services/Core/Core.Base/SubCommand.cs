using System.Threading.Tasks;

namespace Core.Base
{
  public abstract class SubCommand
  {
    SubCommand _next;
    SubCommand _parent;

    public abstract Task Execute();

    public Task Invoke()
    {
      return GetRoot().InternalInvoke();
    }

    public SubCommand Next(SubCommand next)
    {
      next._parent = this;
      _next = next;
      return next;
    }

    SubCommand GetRoot()
    {
      return _parent?.GetRoot() ?? this;
    }

    async Task InternalInvoke()
    {
      await Execute();
      if (_next != null)
      {
        await _next.InternalInvoke();
      }
    }
  }
}
