using System;
using System.Threading.Tasks;

namespace Core.API
{
  public interface ICommand<T>
  {
    Task<T> Invoke();
  }
}
