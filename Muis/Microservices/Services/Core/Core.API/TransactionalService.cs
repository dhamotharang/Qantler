using System;
using System.Threading.Tasks;
using Core.API.Provider;
using Core.API.Repository;

namespace Core.API
{
  public class TransactionalService
  {
    readonly IDbConnectionProvider _connectionProvider;

    public TransactionalService(IDbConnectionProvider connectionProvider)
    {
      _connectionProvider = connectionProvider;
    }

    protected async Task<T> Execute<T>(IUnitOfWorkCommand<T> cmd)
    {
      T result = default(T);

      using (var unitOfWork = new UnitOfWork(_connectionProvider))
      {
        try
        {
          result = await cmd.Invoke(unitOfWork);
        }
        catch (Exception e)
        {
          unitOfWork.Rollback();
          throw e;
        }
      }

      return result;
    }
  }
}
