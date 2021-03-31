using Core.API;
using Core.API.Provider;
using Finance.API.Models;
using Finance.API.Services.Commands.TransactionCode;
using Finance.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Finance.API.Services
{
  public class TransactionCodeService: TransactionalService,
                                       ITransactionCodeService
  {
    public TransactionCodeService(IDbConnectionProvider connectionProvide)
         : base(connectionProvide)
    {
    }

    public async Task<IEnumerable<TransactionCode>> QueryTransactionCode(
      TransactionCodeOptions options)
    {
      return await Execute(new QueryTransactionCodeCommand(options));
    }

    public async Task<TransactionCode> GetTransactionCodeByID(long id)
    {
      return await Execute(new GetTransactionCodeByIDCommand(id));
    }

    public async Task<bool> UpdatePrice(IList<Price> priceList, Guid userID, string userName)
    {
      return await Execute(new UpdatePriceCommand(priceList, userID, userName));
    }
  }
}
