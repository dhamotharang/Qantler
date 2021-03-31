using Notification.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Notification.API.Repository
{
  public interface INotificationRepository
  {
    /// <summary>
    /// Retrieve notification instance base on specified ID.
    /// </summary>
    /// <param name="id">the notification ID</param>
    /// <returns>the instance, null if does not exists</returns>
    Task<Model.Notification> GetByID(long id);

    /// <summary>
    /// Insert notifications.
    /// </summary>
    /// <param name="entity">the notification entity</param>
    /// <param name="toUserIDs">the list of users</param>
    /// <returns>the notification id</returns>
    Task<long> InsertNotification(Model.Notification entity, IList<Guid> toUserIDs);

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

  public class NotificationFilter
  {
    public Guid? UserID { get; set; }

    public DateTimeOffset? From { get; set; }

    public DateTimeOffset? LastModified { get; set; }
  }
}
