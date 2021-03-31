using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.API;
using Core.API.Repository;
using Core.Model;
using Request.API.Repository;

namespace Request.API.Services.Commands.Ingredient
{
  public class BulkUpdateIngredientCommand : IUnitOfWorkCommand<Unit>
  {
    readonly IList<Model.Ingredient> _ingredients;
    readonly Guid _userID;
    readonly string _userName;

    public BulkUpdateIngredientCommand(IList<Model.Ingredient> ingredients, Guid userID,
      string userName)
    {
      _ingredients = ingredients;
      _userID = userID;
      _userName = userName;
    }

    public async Task<Unit> Invoke(IUnitOfWork unitOfWork)
    {
      var dbContext = DbContext.From(unitOfWork);
      unitOfWork.BeginTransaction();

      await dbContext.Ingredient.BulkUpdate(_ingredients, _userID, _userName);

      unitOfWork.Commit();

      return Unit.Default;
    }
  }
}
