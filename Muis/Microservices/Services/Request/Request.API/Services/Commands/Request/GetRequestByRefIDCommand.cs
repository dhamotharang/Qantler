using Core.API;
using Core.API.Repository;
using Request.API.Repository;
using System.Threading.Tasks;

namespace Request.API.Services.Commands.Request
{
  public class GetRequestByRefIDCommand : IUnitOfWorkCommand<Model.Request>
  {
    readonly string _refId;

    public GetRequestByRefIDCommand(string refId)
    {
      _refId = refId;
    }

    public async Task<Model.Request> Invoke(IUnitOfWork unitOfWork)
    {
      var dbContext = DbContext.From(unitOfWork);

      var result = await dbContext.Request.GetRequestByRefID(_refId);
      if (result != null)
      {
        result.Ingredients = await dbContext.Ingredient.Query(new IngredientFilter
        {
          RequestID = result.ID
        });

        result.Menus = await dbContext.Menu.Query(new MenuFilter
        {
          RequestID = result.ID
        });
      }
      return result;
    }
  }
}
