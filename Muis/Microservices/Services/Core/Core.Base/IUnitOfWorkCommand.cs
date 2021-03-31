using System;
using System.Threading.Tasks;
using Core.Base.Repository;

namespace Core.Base
{
  public interface IUnitOfWorkCommand<T>
  {
    Task<T> Invoke(IUnitOfWork uow);
  }
}
