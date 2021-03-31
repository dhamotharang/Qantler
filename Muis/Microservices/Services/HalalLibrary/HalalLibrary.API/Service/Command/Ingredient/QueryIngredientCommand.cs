using Core.API;
using Core.API.Repository;
using HalalLibrary.API.Models;
using HalalLibrary.API.Repository;
using HalalLibrary.Model;
using System.Threading.Tasks;

namespace HalalLibrary.API.Services.Commands.Ingredient
{
  public class QueryIngredientCommand : IUnitOfWorkCommand<halalLibraryList>
  {
    readonly long _nextRows;
    readonly long _offsetRows;
    readonly string _name;
    readonly string _brand;
    readonly string _supplier;
    readonly string _certifyingBody;
    readonly RiskCategory? _riskCategory;
    readonly IngredientStatus? _status;
    readonly string _verifiedBy;
    public QueryIngredientCommand(long nextRows,
      long offsetRows,
      string name,
      string brand,
      string supplier,
      string certifyingBody,
      RiskCategory? riskCategory,
      IngredientStatus? status,
      string verifiedBy)
    {
      _nextRows = nextRows;
      _offsetRows = offsetRows;
      _name = name;
      _brand = brand;
      _supplier = supplier;
      _certifyingBody = certifyingBody;
      _riskCategory = riskCategory;
      _status = status;
      _verifiedBy = verifiedBy;
    }

    public async Task<halalLibraryList> Invoke(IUnitOfWork unitOfWork)
    {
      return await DbContext.From(unitOfWork).Ingredient.Select(_nextRows, 
        _offsetRows, 
        _name, 
        _brand, 
        _supplier, 
        _certifyingBody, 
        _riskCategory, 
        _status, 
        _verifiedBy);
    }
  }
}
