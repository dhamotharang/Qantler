using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.API;
using Core.API.Provider;
using Request.API.Repository;
using Request.API.Services.Commands.Menu;
using Request.Model;

namespace Request.API.Services
{
  public class MenuService : TransactionalService,
                             IMenuService
  {
    public MenuService(IDbConnectionProvider connectionProvider)
         : base(connectionProvider)
    {
    }

    public async Task BulkUpdate(IList<Menu> menus, Guid userID, string userName)
    {
      await Execute(new BulkUpdateMenuCommand(menus, userID, userName));
    }

    public async Task BulkReview(IList<Menu> menus, Guid userID, string userName)
    {
      await Execute(new BulkReviewMenuCommand(menus, userID, userName));
    }

    public async Task AddMenus(IList<Menu> menus, Guid userID, string userName)
    {
      await Execute(new AddMenusCommand(menus, userID, userName));
    }
  }
}
