using Core.API;
using Core.API.Repository;
using Request.API.Repository;
using System.Threading.Tasks;

namespace Request.API.Services.Commands.Request
{
  public class GetRequestByIDCommand : IUnitOfWorkCommand<Model.Request>
  {
    readonly long _id;

    public GetRequestByIDCommand(long id)
    {
      _id = id;
    }

    public async Task<Model.Request> Invoke(IUnitOfWork unitOfWork)
    {
      var dbContext = DbContext.From(unitOfWork);

      var result = await DbContext.From(unitOfWork).Request.GetRequestByID(_id);

      result.Ingredients = await dbContext.Ingredient.Query(new IngredientFilter
      {
        RequestID = _id
      });

      result.Menus = await dbContext.Menu.Query(new MenuFilter
      {
        RequestID = _id
      });

      return result;
    }
  }
}
