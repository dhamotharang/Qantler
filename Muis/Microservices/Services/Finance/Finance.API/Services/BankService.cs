using Core.API;
using Core.API.Provider;
using Finance.API.Services.Commands.Bank;
using Finance.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Finance.API.Services
{
  public class BankService : TransactionalService, 
                              IBankService
  {
    public BankService(IDbConnectionProvider connectionProvider)
         : base(connectionProvider)
    {      
    }

    public async Task<IList<Bank>> Filter(BankFilter filter)
    {
      return await Execute(new FilterBankCommand(filter));
    }
  }
}
