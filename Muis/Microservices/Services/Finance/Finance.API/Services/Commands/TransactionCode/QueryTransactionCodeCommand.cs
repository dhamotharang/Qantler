using Core.API;
using Core.API.Repository;
using Finance.API.Models;
using Finance.API.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Finance.API.Services.Commands.TransactionCode
{
  public class QueryTransactionCodeCommand : IUnitOfWorkCommand<IEnumerable<Model.TransactionCode>>
  {
    readonly TransactionCodeOptions _options;

    public QueryTransactionCodeCommand(TransactionCodeOptions options)
    {
      _options = options;
    }

    public async Task<IEnumerable<Model.TransactionCode>> Invoke(IUnitOfWork uow)
    {
      return await new DbContext(uow).Transactioncode.Select(_options);
    }
  }
}
