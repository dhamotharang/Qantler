using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Request.Model;

namespace Request.API.Repository
{
  public interface IIngredientRepository
  {
    /// <summary>
    /// Query ingredient with specified filters.
    /// </summary>
    /// <param name="options">the filters</param>
    Task<IList<Ingredient>> Query(IngredientFilter filter);

    /// <summary>
    /// Bulk update list of ingredients.
    /// </summary>
    /// <param name="ingredients">the list of ingredients</param>
    /// <param name="userID">the user ID</param>
    /// <param name="userName">the user name</param>
    Task BulkUpdate(IList<Ingredient> ingredients, Guid userID, string userName);

    /// <summary>
    /// Bulk add review to list of ingredients.
    /// </summary>
    /// <param name="ingredients">the list of ingredients</param>
    /// <param name="userID">the user ID</param>
    /// <param name="userName">the user name</param>
    Task BulkAddReview(IList<Ingredient> ingredients, Guid userID, string userName);

    /// <summary>
    /// Add new list of ingredients.
    /// </summary>
    /// <param name="ingredients">the list of ingredients</param>
    /// <param name="userID">the user ID</param>
    /// <param name="userName">the user name</param>
    Task AddIngredients(IList<Ingredient> ingredients, Guid userID, string userName);
  }

  public class IngredientFilter
  {
    public long? RequestID { get; set; }
    public RiskCategory[] RiskCategory { get; set; }
  }
}
