using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.API;
using Core.API.Repository;
using Finance.API.Repository;

namespace Finance.API.Services.Commands.Payment
{
  public class FilterPaymentCommand : IUnitOfWorkCommand<IList<Model.Payment>>
  {
    readonly PaymentFilter _options;

    public FilterPaymentCommand(PaymentFilter options)
    {
      _options = options;
    }

    public async Task<IList<Model.Payment>> Invoke(IUnitOfWork uow)
    {
      return await new DbContext(uow).Payment.Query(_options);
    }
  }
}
