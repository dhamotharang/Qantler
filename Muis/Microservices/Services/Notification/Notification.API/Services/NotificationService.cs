using System;
using System.Collections.Generic;
using Notification.API.Services;
using Core.EventBus;
using System.Threading.Tasks;
using Notification.Model;
using Notification.API.Services.Commands;
using Core.API;
using Core.API.Provider;
using Notification.API.Repository;

namespace Notification.Service
{
  public class NotificationService : TransactionalService,
                                     INotificationService
  {
    readonly IDbConnectionProvider _connectionProvider;
    readonly IEventBus _eventBus;

    public NotificationService(IDbConnectionProvider connectionProvider, IEventBus eventBus)
         : base(connectionProvider)
    {
      _connectionProvider = connectionProvider;
      _eventBus = eventBus;
    }

    public async Task<Model.Notification> Send(Model.Notification entity,
      IList<Guid> toUserIDs)
    {
      return await Execute(new SendNotificationCommand(entity, toUserIDs, _eventBus));
    }

    public async Task<IList<Model.Notification>> Query(NotificationFilter filter)
    {
      return await Execute(new QueryNotificationCommand(filter));
    }

    public async Task UpdateState(long notificationID, Guid userID, State state)
    {
      await Execute(new UpdateNotificationStateCommand(notificationID, userID, state));
    }

    public async Task Clear(Guid userID)
    {
      await Execute(new ClearNotificationCommand(userID));
    }
  }
}
