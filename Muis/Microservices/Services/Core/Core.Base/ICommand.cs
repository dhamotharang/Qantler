using System;
using System.Threading.Tasks;

namespace Core.Base
{
  public interface ICommand<T>
  {
    Task<T> Invoke();
  }
}
