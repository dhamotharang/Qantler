using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.API;
using Core.API.Provider;
using Request.API.Services.Commands.Ingredient;
using Request.Model;

namespace Request.API.Services
{
  public class IngredientService : TransactionalService,
                                   IIngredientService
  {
    public IngredientService(IDbConnectionProvider connectionProvider)
         : base(connectionProvider)
    {
    }

    public async Task BulkUpdate(IList<Ingredient> ingredients, Guid userID, string userName)
    {
      await Execute(new BulkUpdateIngredientCommand(ingredients, userID, userName));
    }

    public async Task BulkReview(IList<Ingredient> ingredients, Guid userID, string userName)
    {
      await Execute(new BulkReviewIngredientCommand(ingredients, userID, userName));
    }

    public async Task AddIngredients(IList<Ingredient> ingredients, Guid userID, string userName)
    {
      await Execute(new AddIngredientsCommand(ingredients, userID, userName));
    }
  }
}
