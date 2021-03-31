using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Model;
using Request.Model;

namespace Request.API.Repository
{
  public interface IMenuRepository
  {
    /// <summary>
    /// Query menu with specified filters.
    /// </summary>
    /// <param name="options">the filters</param>
    Task<IList<Menu>> Query(MenuFilter filter);

    /// <summary>
    /// Bulk update list of menus.
    /// </summary>
    /// <param name="menus">the list of menus</param>
    /// <param name="userID">the user ID</param>
    /// <param name="userName">the user name</param>
    Task BulkUpdate(IList<Menu> menus, Guid userID, string userName);

    /// <summary>
    /// Bulk add review to list of menus.
    /// </summary>
    /// <param name="menus">the list of menus</param>
    /// <param name="userID">the user ID</param>
    /// <param name="userName">the user name</param>
    Task BulkAddReview(IList<Menu> menus, Guid userID, string userName);

    /// <summary>
    /// Add new list of menus.
    /// </summary>
    /// <param name="menus">the list of menus</param>
    /// <param name="userID">the user ID</param>
    /// <param name="userName">the user name</param>
    Task AddMenus(IList<Menu> menus, Guid userID, string userName);
  }

  public class MenuFilter
  {
    public long? RequestID { get; set; }

    public Scheme? Scheme { get; set; }
  }
}
