using Core.API;
using Core.API.Repository;
using Core.EventBus;
using JobOrder.API.Repository;
using JobOrder.Events;
using JobOrder.Model;
using System.Threading.Tasks;

namespace JobOrder.API.Services.Commands.JobOrders
{
  public class UpdateJobOrderStateCommand : IUnitOfWorkCommand<Model.JobOrder>
  {
    readonly long _id;
    readonly JobOrderStatus _newStatus;

    readonly IEventBus _eventBus;

    public UpdateJobOrderStateCommand(long id, JobOrderStatus newStatus, IEventBus eventBus)
    {
      _id = id;
      _newStatus = newStatus;
      _eventBus = eventBus;
    }

    public async Task<Model.JobOrder> Invoke(IUnitOfWork uow)
    {
      var dbContext = DbContext.From(uow);
      uow.BeginTransaction();

      var result = await dbContext.JobOrder.GetJobOrderByIDBasic(_id);

      var oldStatus = result.Status;

      await dbContext.JobOrder.UpdateJobOrderStatus(_id, _newStatus);

      result = await dbContext.JobOrder.GetJobOrderByIDBasic(_id);

      _eventBus.Publish(new OnJobOrderStatusChangedEvent
      {
        ID = _id,
        RefID = result.RefID,
        Type = result.Type,
        OldStatus = oldStatus,
        NewStatus = _newStatus
      });

      uow.Commit();

      return result;
    }
  }
}