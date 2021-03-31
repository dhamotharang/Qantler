using System;
using System.Data;
using System.Threading.Tasks;
using Core.API.Repository;
using Dapper;
using Finance.Model;

namespace Finance.API.Repository
{
  public class AccountRepository : IAccountRepository
  {
    readonly IUnitOfWork _uow;

    public AccountRepository(IUnitOfWork uow)
    {
      _uow = uow;
    }

    public Task InsertOrReplace(Account entity)
    {
      var param = new DynamicParameters();
      param.Add("@ID", entity.ID);
      param.Add("@Name", entity.Name);

      return SqlMapper.ExecuteAsync(_uow.Connection,
        "InsertOrReplaceAccount",
        param,
        commandType: CommandType.StoredProcedure,
        transaction: _uow.Transaction);
    }
  }
}
