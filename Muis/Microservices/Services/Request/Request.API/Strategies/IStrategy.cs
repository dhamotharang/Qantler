using System;
using System.Threading.Tasks;

namespace Request.API.Strategies
{
  public interface IStrategy<T>
  {
    Task<T> Invoke();
  }
}
