using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.API;
using Core.API.Repository;
using Core.EventBus;
using Notification.API.Repository;
using Notification.Events;

namespace Notification.API.Services.Commands
{
  public class SendNotificationCommand : IUnitOfWorkCommand<Model.Notification>
  {
    readonly Model.Notification _notification;
    readonly IList<Guid> _toUserIDs;

    readonly IEventBus _eventBus;

    public SendNotificationCommand(Model.Notification notification, IList<Guid> toUserIDs,
      IEventBus eventBus)
    {
      _notification = notification;
      _toUserIDs = toUserIDs;

      _eventBus = eventBus;
    }

    public async Task<Model.Notification> Invoke(IUnitOfWork uow)
    {
      var dbContext = new DbContext(uow);
      uow.BeginTransaction();

      var id = await dbContext.Notification.InsertNotification(_notification, _toUserIDs);

      var result = await dbContext.Notification.GetByID(id);

      _eventBus.Publish(new OnNotificationSentEvent
      {
        ID = id,
        Title = result.Title,
        Preview = string.IsNullOrEmpty(result.Preview) ? result.Body : result.Preview,
        RefID = result.RefID,
        Module = result.Module,
        Level = result.Level,
        Category = result.Category,
        ContentType = result.ContentType,
        To = _toUserIDs.Select(e => e.ToString()).ToList()
      });

      uow.Commit();

      return result;
    }
  }
}
