using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.API;
using Core.API.Repository;
using Finance.API.Repository;

namespace Finance.API.Services.Commands.Payment
{
  public class GetCustomerRecentPaymentCommand : IUnitOfWorkCommand<IList<Model.Payment>>
  {
    readonly Guid _customerID;

    public GetCustomerRecentPaymentCommand(Guid customerID)
    {
      _customerID = customerID;
    }

    public async Task<IList<Model.Payment>> Invoke(IUnitOfWork uow)
    {
      return await new DbContext(uow).Payment.GetCustomerRecentPayment(_customerID);
    }
  }
}
