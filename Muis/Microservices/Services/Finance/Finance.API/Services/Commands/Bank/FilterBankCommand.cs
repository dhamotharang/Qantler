using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.API;
using Core.API.Repository;
using Finance.API.Repository;

namespace Finance.API.Services.Commands.Bank
{
  public class FilterBankCommand : IUnitOfWorkCommand<IList<Model.Bank>>
  {
    readonly BankFilter _options;

    public FilterBankCommand(BankFilter options)
    {
      _options = options;
    }

    public async Task<IList<Model.Bank>> Invoke(IUnitOfWork uow)
    {
      return await new DbContext(uow).Bank.Query(_options);
    }
  }
}
