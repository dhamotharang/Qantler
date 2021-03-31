using System;
using System.Threading.Tasks;
using Core.API;
using Core.API.Repository;
using Finance.API.Repository;

namespace Finance.API.Services.Commands.Payment
{
  public class GetPaymentByIDCommand : IUnitOfWorkCommand<Model.Payment>
  {
    readonly long _id;

    public GetPaymentByIDCommand(long id)
    {
      _id = id;
    }

    public async Task<Model.Payment> Invoke(IUnitOfWork uow)
    {
      return await new DbContext(uow).Payment.GetPaymentByID(_id);
    }
  }
}
