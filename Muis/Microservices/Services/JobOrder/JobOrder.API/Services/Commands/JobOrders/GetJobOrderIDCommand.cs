using Core.API;
using Core.API.Repository;
using JobOrder.API.Repository;
using System.Threading.Tasks;

namespace JobOrder.API.Services.Commands.JobOrders
{
  public class GetJobOrderIDCommand : IUnitOfWorkCommand<Model.JobOrder>
  {
    readonly long _id;

    public GetJobOrderIDCommand(long id)
    {
      _id = id;
    }

    public Task<Model.JobOrder> Invoke(IUnitOfWork uow)
    {
      return DbContext.From(uow).JobOrder.GetJobOrderByID(_id);
    }
  }
}
