using Core.API;
using Core.API.Exceptions;
using Core.API.Repository;
using Core.Model;
using HalalLibrary.API.Repository;
using System.Threading.Tasks;

namespace HalalLibrary.API.Services.Commands.Ingredient
{
  public class UpdateIngredientCommand : IUnitOfWorkCommand<long>
  {
    readonly Model.Ingredient _data;

    public UpdateIngredientCommand(Model.Ingredient data)
    {
      _data = data;
    }

    public async Task<long> Invoke(IUnitOfWork uow)
    {
      var dbContext = DbContext.From(uow);
      uow.BeginTransaction();

      var result = await dbContext.Ingredient.UpdateIngredient(_data);

      if (result == 0)
      {
        var logText = await dbContext.Translation.GetTranslation(Locale.EN, "ValidateIngredient");
        throw new BadRequestException(logText);
      }

      uow.Commit();

      return result;
    }
  }
}
