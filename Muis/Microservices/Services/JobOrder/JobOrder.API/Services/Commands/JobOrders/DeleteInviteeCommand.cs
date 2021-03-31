using Core.API;
using Core.API.Repository;
using Core.EventBus;
using Core.Model;
using JobOrder.API.Repository;
using JobOrder.Events;
using System;
using System.Threading.Tasks;

namespace JobOrder.API.Services.Commands.JobOrders
{
  public class DeleteInviteeCommand : IUnitOfWorkCommand<Unit>
  {
    readonly long _jobID;
    readonly Guid _officerID;
    readonly Officer _user;

    readonly IEventBus _eventBus;

    public DeleteInviteeCommand(long jobID, Guid officerID, Officer user, IEventBus eventBus)
    {
      _jobID = jobID;
      _officerID = officerID;
      _user = user;

      _eventBus = eventBus;
    }

    public async Task<Unit> Invoke(IUnitOfWork uow)
    {
      var dbContext = DbContext.From(uow);
      uow.BeginTransaction();

      var officer = await dbContext.User.GetUserByID(_officerID);

      await dbContext.JobOrder.RemoveInvitee(_jobID, _officerID);

      var logID = await dbContext.Log.InsertLog(new Log
      {
        Action = string.Format(
          await dbContext.Translation.GetTranslation(Locale.EN, "InviteRemoveLog"),
          officer.Name),
        UserID = _user.ID,
        UserName = _user.Name
      });

      await dbContext.JobOrder.MapLog(_jobID, logID);

      _eventBus.Publish(new SendNotificationEvent
      {
        Title = await dbContext.Translation.GetTranslation(Locale.EN, "InviteRemoveTitle"),
        Body = string.Format(
          await dbContext.Translation.GetTranslation(Locale.EN, "InviteRemoveBody"),
          $"#{_jobID}"),
        Module = "JobOrder",
        RefID = $"{_jobID}",
        To = new string[] { _officerID.ToString() }
      });

      uow.Commit();

      return Unit.Default;
    }
  }
}