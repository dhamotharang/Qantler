using Core.API;
using Core.API.Repository;
using HalalLibrary.API.Repository;
using System.Threading.Tasks;

namespace HalalLibrary.API.Services.Commands.Ingredient
{
  public class CheckIngredientCommand : IUnitOfWorkCommand<bool>
  {
    readonly Model.Ingredient _data;

    public CheckIngredientCommand(Model.Ingredient data)
    {
      _data = data;
    }

    public async Task<bool> Invoke(IUnitOfWork unitOfWork)
    {
      return await DbContext.From(unitOfWork).Ingredient.CheckIngredient(_data);
    }
  }
}