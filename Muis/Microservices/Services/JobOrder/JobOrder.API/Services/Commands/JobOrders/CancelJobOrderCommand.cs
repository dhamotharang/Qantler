using Core.API;
using Core.API.Repository;
using Core.EventBus;
using Core.Model;
using JobOrder.API.Models;
using JobOrder.API.Repository;
using JobOrder.Events;
using JobOrder.Model;
using System.Threading.Tasks;

namespace JobOrder.API.Services.Commands.JobOrders
{
  public class CancelJobOrderCommand : IUnitOfWorkCommand<Unit>
  {
    readonly long _id;
    readonly CancelParam _param;

    readonly Officer _user;

    readonly IEventBus _eventBus;

    public CancelJobOrderCommand(long id, CancelParam param, Officer user,
      IEventBus eventBus)
    {
      _id = id;
      _param = param;

      _user = user;

      _eventBus = eventBus;
    }

    public async Task<Unit> Invoke(IUnitOfWork uow)
    {
      var dbContext = DbContext.From(uow);
      uow.BeginTransaction();

      var jobOrder = await dbContext.JobOrder.GetJobOrderByID(_id);

      jobOrder.ScheduledOn = null;
      jobOrder.Status = JobOrderStatus.Cancelled;

      await dbContext.JobOrder.UpdateJobOrder(jobOrder);

      // TODO Reconsider timezone. As of now, assumes timezone in SGT (+8:00)
      var logText = await dbContext.Translation.GetTranslation(Locale.EN, "JobOrderCancel");

      var logID = await dbContext.Log.InsertLog(new Log
      {
        Type = LogType.JobOrder,
        Notes = $"{_param.Reason.Value}\n{_param.Notes}",
        Action = logText,
        UserID = _user.ID,
        UserName = _user.Name
      });

      await dbContext.JobOrder.MapLog(_id, logID);

      _eventBus.Publish(new OnJobOrderStatusChangedEvent
      {
        ID = _id,
        RefID = jobOrder.RefID,
        Type = jobOrder.Type,
        NewStatus = JobOrderStatus.Cancelled,
        OldStatus = JobOrderStatus.Pending
      });

      uow.Commit();

      return Unit.Default;
    }
  }
}
