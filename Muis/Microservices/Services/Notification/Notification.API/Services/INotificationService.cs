using Notification.API.Repository;
using Notification.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Notification.API.Services
{
  public interface INotificationService
  {
    /// <summary>
    /// Insert notifications.
    /// </summary>
    /// <param name="entity">the notification entity</param>
    /// <param name="toUserIDs">the list of users</param>
    /// <returns>the notification id</returns>
    Task<Model.Notification> Send(Model.Notification entity, IList<Guid> toUserIDs);

    /// <summary>
    /// Query notification.
    /// </summary>
    /// <param name="filter">the filter</param>
    /// <returns>the list of notifications</returns>
    Task<IList<Model.Notification>> Query(NotificationFilter filter);

    /// <summary>
    /// Update notifiation state for specified user.
    /// </summary>
    /// <param name="notificationID">the notification ID</param>
    /// <param name="userID">the user ID</param>
    /// <param name="state">to state</param>
    Task UpdateState(long notificationID, Guid userID, State state);

    /// <summary>
    /// Clear notification for specified user.
    /// </summary>
    /// <param name="userID">the user id</param>
    Task Clear(Guid userID);
  }
}
