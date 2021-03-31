using Core.API;
using Core.API.Repository;
using Finance.API.Repository;
using System.Threading.Tasks;

namespace Finance.API.Services.Commands.TransactionCode
{
  public class GetTransactionCodeByIDCommand : IUnitOfWorkCommand<Model.TransactionCode>
  {
    readonly long _id;

    public GetTransactionCodeByIDCommand(long id)
    {
      _id = id;
    }

    public async Task<Model.TransactionCode> Invoke(IUnitOfWork uow)
    {
      return await new DbContext(uow).Transactioncode.GetTransactionCodeByID(_id);
    }
  }
}
