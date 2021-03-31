using System;
using Core.API.Repository;

namespace Notification.API.Repository
{
  public class DbContext
  {
    readonly IUnitOfWork _unitOfWork;

    public DbContext(IUnitOfWork unitOfWork)
    {
      _unitOfWork = unitOfWork;
    }

    INotificationRepository _notification;
    public INotificationRepository Notification =>
      _notification
      ??= new NotificationRepository(_unitOfWork);
  }
}
