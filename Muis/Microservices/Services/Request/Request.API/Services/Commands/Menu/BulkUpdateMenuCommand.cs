using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.API;
using Core.API.Repository;
using Core.Model;
using Request.API.Repository;

namespace Request.API.Services.Commands.Menu
{
  public class BulkReviewMenuCommand : IUnitOfWorkCommand<Unit>
  {
    readonly IList<Model.Menu> _menus;
    readonly Guid _userID;
    readonly string _userName;

    public BulkReviewMenuCommand(IList<Model.Menu> menus, Guid userID, string userName)
    {
      _menus = menus;
      _userID = userID;
      _userName = userName;
    }

    public async Task<Unit> Invoke(IUnitOfWork unitOfWork)
    {
      var dbContext = DbContext.From(unitOfWork);
      unitOfWork.BeginTransaction();

      await dbContext.Menu.BulkAddReview(_menus, _userID, _userName);

      unitOfWork.Commit();

      return Unit.Default;
    }
  }
}
