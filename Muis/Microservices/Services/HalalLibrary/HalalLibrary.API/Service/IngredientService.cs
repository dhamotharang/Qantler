using Core.API;
using Core.API.Provider;
using HalalLibrary.API.Models;
using HalalLibrary.API.Service.Command.Ingredient;
using HalalLibrary.API.Services;
using HalalLibrary.API.Services.Commands.Ingredient;
using HalalLibrary.Events;
using HalalLibrary.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HalalLibrary.Service
{
  public class IngredientService : TransactionalService,
                                IIngredientService
  {

    public IngredientService(IDbConnectionProvider connectionProvider)
         : base(connectionProvider)
    {

    }

    public async Task<halalLibraryList> Query(long nextRows,
      long offsetRows,
      string name,
      string brand,
      string supplier,
      string certifyingBody,
      RiskCategory? riskCategory,
      IngredientStatus? status,
      string verifiedBy)
    {
      var result = await Execute(new QueryIngredientCommand(nextRows,
        offsetRows,
        name, brand,
        supplier,
        certifyingBody,
        riskCategory,
        status,
        verifiedBy));

      if (result != null)
      {
        return result;
      }
      else
      {
        return new halalLibraryList() { totalData = 0, data = new List<Ingredient>() };
      }
    }

    public async Task<long> InsertIngredient(Ingredient data)
    {
      return await Execute(new InsertIngredientCommand(data));
    }

    public async Task<long> UpdateIngredient(Ingredient data)
    {
      return await Execute(new UpdateIngredientCommand(data));
    }

    public async Task DeleteIngredient(long id)
    {
      await Execute(new DeleteIngredientCommand(id));
    }

    public async Task<bool> CheckIngredient(Ingredient data)
    {
      return await Execute(new CheckIngredientCommand(data));
    }

    public async Task AddIngredients(NewIngredientEvent data)
    {
      await Execute(new BulkInsertIngredientCommand(data?.Ingredients));
    }
  }
}
