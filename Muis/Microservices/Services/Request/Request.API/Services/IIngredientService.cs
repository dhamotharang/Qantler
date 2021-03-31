using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Request.Model;

namespace Request.API.Services
{
  public interface IIngredientService
  {
    /// <summary>
    /// Bulk update list of ingredients.
    /// </summary>
    /// <param name="ingredients">the list of ingredients</param>
    /// <param name="userID">the user ID</param>
    /// <param name="userName">the user name</param>
    Task BulkUpdate(IList<Ingredient> ingredients, Guid userID, string userName);

    /// <summary>
    /// Bulk review list of ingredients.
    /// </summary>
    /// <param name="ingredients">the list of ingredients</param>
    /// <param name="userID">the user ID</param>
    /// <param name="userName">the user name</param>
    Task BulkReview(IList<Ingredient> ingredients, Guid userID, string userName);

    /// <summary>
    /// Add new list of ingredients.
    /// </summary>
    /// <param name="ingredients">the list of ingredients</param>
    /// <param name="userID">the user ID</param>
    /// <param name="userName">the user name</param>
    Task AddIngredients(IList<Ingredient> ingredients, Guid userID, string userName);
  }
}
