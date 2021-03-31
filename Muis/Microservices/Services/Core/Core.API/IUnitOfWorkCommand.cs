using System;
using System.Threading.Tasks;
using Core.API.Repository;

namespace Core.API
{
  public interface IUnitOfWorkCommand<T>
  {
    Task<T> Invoke(IUnitOfWork uow);
  }
}
