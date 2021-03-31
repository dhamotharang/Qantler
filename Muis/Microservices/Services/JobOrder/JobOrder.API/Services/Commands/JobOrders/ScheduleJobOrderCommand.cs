using Core.API;
using Core.API.Repository;
using Core.EventBus;
using Core.Model;
using JobOrder.API.Models;
using JobOrder.API.Repository;
using JobOrder.Events;
using System.Threading.Tasks;
using JobOrder.Model;

namespace JobOrder.API.Services.Commands.JobOrders
{
  public class ScheduleJobOrderCommand : IUnitOfWorkCommand<Unit>
  {
    readonly long _id;
    readonly ScheduleParam _param;

    readonly Officer _user;

    readonly IEventBus _eventBus;

    public ScheduleJobOrderCommand(long id, ScheduleParam param, Officer user,
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

      jobOrder.ScheduledOn = _param.ScheduledOn;
      jobOrder.ScheduledOnTo = _param.ScheduledOnTo;
      jobOrder.Officer = new Officer { ID = _user.ID, Name = _user.Name };
      jobOrder.Status = JobOrderStatus.Pending;

      await dbContext.JobOrder.UpdateJobOrder(jobOrder);

      // TODO Reconsider timezone. As of now, assumes timezone in SGT (+8:00)
      var scheduledOnText = _param.ScheduledOn.Value.AddHours(8).ToString("dd MMM yyyy");
      var scheduledOnToText = _param.ScheduledOnTo.Value.AddHours(8).ToString("dd MMM yyyy");

      var logText = string.Format(
        await dbContext.Translation.GetTranslation(Locale.EN, "ScheduledPIInspection"),
        $"{scheduledOnText} to {scheduledOnToText}");

      var logID = await dbContext.Log.InsertLog(new Log
      {
        Type = LogType.JobOrder,
        Notes = $"{_param.Notes}",
        Action = logText,
        UserID = _user.ID,
        UserName = _user.Name
      });

      await dbContext.JobOrder.MapLog(_id, logID);     

      uow.Commit();

      return Unit.Default;
    }
  }
}
