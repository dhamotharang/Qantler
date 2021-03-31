using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.API;
using Core.API.Repository;
using Notification.API.Repository;

namespace Notification.API.Services.Commands
{
  public class QueryNotificationCommand : IUnitOfWorkCommand<IList<Model.Notification>>
  {
    readonly NotificationFilter _filter;

    public QueryNotificationCommand(NotificationFilter filter)
    {
      _filter = filter;
    }

    public async Task<IList<Model.Notification>> Invoke(IUnitOfWork unitOfWork)
    {
      var dbContext = new DbContext(unitOfWork);
      return await dbContext.Notification.Query(_filter);
    }
  }
}
