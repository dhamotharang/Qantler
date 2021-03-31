using HalalLibrary.API.Models;
using HalalLibrary.Model;
using System.Threading.Tasks;

namespace HalalLibrary.API.Repository
{
  public interface IIngredientRepository
  {
    public Task<halalLibraryList> Select(long nextRows,
      long offsetRows,
      string name,
      string brand,
      string supplier,
      string certifyingBody,
      RiskCategory? riskCategory,
      IngredientStatus? status,
      string verifiedBy);

    public Task<long> InsertIngredient(Ingredient data);

    public Task<long> UpdateIngredient(Ingredient data);

    public Task DeleteIngredient(long id);

    public Task<bool> CheckIngredient(Ingredient data);

  }
}
