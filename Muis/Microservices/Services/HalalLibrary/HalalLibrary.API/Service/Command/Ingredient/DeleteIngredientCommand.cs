using Core.API;
using Core.API.Repository;
using Core.Model;
using HalalLibrary.API.Repository;
using System.Threading.Tasks;

namespace HalalLibrary.API.Services.Commands.Ingredient
{
  public class DeleteIngredientCommand : IUnitOfWorkCommand<Unit>
  {
    readonly long _id;

    public DeleteIngredientCommand(long id)
    {
      _id = id;
    }

    public async Task<Unit> Invoke(IUnitOfWork uow)
    {
      var dbContext = DbContext.From(uow);
      uow.BeginTransaction();

      await dbContext.Ingredient.DeleteIngredient(_id);

      uow.Commit();

      return Unit.Default;
    }
  }
}
