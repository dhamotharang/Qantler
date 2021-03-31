using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Request.Model;

namespace Request.API.Services
{
  public interface IMenuService
  {
    /// <summary>
    /// Bulk update list of menus.
    /// </summary>
    /// <param name="menus">the list of menus</param>
    /// <param name="userID">the user ID</param>
    /// <param name="userName">the user name</param>
    Task BulkUpdate(IList<Menu> menus, Guid userID, string userName);

    /// <summary>
    /// Bulk review list of menus.
    /// </summary>
    /// <param name="menus">the list of menus</param>
    /// <param name="userID">the user ID</param>
    /// <param name="userName">the user name</param>
    Task BulkReview(IList<Menu> menus, Guid userID, string userName);

    /// <summary>
    /// Add new list of menus.
    /// </summary>
    /// <param name="menus">the list of menus</param>
    /// <param name="userID">the user ID</param>
    /// <param name="userName">the user name</param>
    Task AddMenus(IList<Menu> menus, Guid userID, string userName);
  }
}
