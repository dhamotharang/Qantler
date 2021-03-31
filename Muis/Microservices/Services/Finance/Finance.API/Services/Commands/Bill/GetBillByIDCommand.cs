using System;
using System.Threading.Tasks;
using Core.API;
using Core.API.Repository;
using Finance.API.Repository;

namespace Finance.API.Services.Commands.Bill
{
  public class GetBillByIDCommand : IUnitOfWorkCommand<Model.Bill>
  {
    readonly long _id;

    public GetBillByIDCommand(long id)
    {
      _id = id;
    }

    public async Task<Model.Bill> Invoke(IUnitOfWork uow)
    {
      return await new DbContext(uow).Bill.GetBillByID(_id);
    }
  }
}
