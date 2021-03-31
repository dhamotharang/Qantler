using HalalLibrary.API.Models;
using HalalLibrary.Events;
using HalalLibrary.Model;
using System.Threading.Tasks;

namespace HalalLibrary.API.Services
{
  public interface IIngredientService
  {
    /// <summary>
    /// Gets all ingredient.
    /// </summary>
    /// <returns>The halal library data.</returns>
    /// <param name="nextRows">next set of rows.</param>
    /// <param name="offsetRows">previous set ID.</param>
    Task<halalLibraryList> Query(long nextRows, 
      long offsetRows, 
      string name,
      string brand,
      string supplier,
      string certifyingBody,
      RiskCategory? riskCategory,
      IngredientStatus? status,
      string verifiedBy);

    /// <summary>
    /// Insert ingredient data.
    /// </summary>
    /// <param name="data">Ingredient data</param>
    Task<long> InsertIngredient(Ingredient data);

    /// <summary>
    /// updated  ingredient data.
    /// </summary>
    /// <param name="data">Ingredient data</param>
    Task<long> UpdateIngredient(Ingredient data);

    /// <summary>
    /// delete ingredient data.
    /// </summary>
    /// <param name="id">Ingredient ID</param>
    Task DeleteIngredient(long id);

    /// <summary>
    /// check ingredient combination already exists
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    Task<bool> CheckIngredient(Ingredient data);

    /// <summary>
    /// Bulk insert ingredient data
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    Task AddIngredients(NewIngredientEvent data);

  }
}
