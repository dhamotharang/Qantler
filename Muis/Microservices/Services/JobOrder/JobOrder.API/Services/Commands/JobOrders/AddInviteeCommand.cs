using Core.API;
using Core.API.Repository;
using Core.EventBus;
using Core.Model;
using JobOrder.API.Repository;
using JobOrder.Events;
using System.Threading.Tasks;

namespace JobOrder.API.Services.Commands.JobOrders
{
  public class AddInviteeCommand: IUnitOfWorkCommand<Unit>
  {
    readonly long _jobID;
    readonly Officer _officer;

    readonly Officer _user;

    readonly IEventBus _eventBus;

    public AddInviteeCommand(long jobID, Officer officer, Officer user, IEventBus eventBus)
    {
      _jobID = jobID;
      _officer = officer;
      _user = user;

      _eventBus = eventBus;
    }

    public async Task<Unit> Invoke(IUnitOfWork uow)
    {
      var dbContext = DbContext.From(uow);
      uow.BeginTransaction();

      await dbContext.JobOrder.AddInvitee(_jobID, _officer);

      var logID = await dbContext.Log.InsertLog(new Log
      {
        Action = string.Format(
          await dbContext.Translation.GetTranslation(Locale.EN, "InviteAddLog"),
          _officer.Name),
        UserID = _user.ID,
        UserName = _user.Name
      });

      await dbContext.JobOrder.MapLog(_jobID, logID);

      _eventBus.Publish(new SendNotificationEvent
      {
        Title = await dbContext.Translation.GetTranslation(Locale.EN, "InviteSentTitle"),
        Body = string.Format(
          await dbContext.Translation.GetTranslation(Locale.EN, "InviteSentBody"),
          $"#{_jobID}"),
        Module = "JobOrder",
        RefID = $"{_jobID}",
        To = new string[] { _officer.ID.ToString() }
      });

      uow.Commit();

      return Unit.Default;
    }
  }
}