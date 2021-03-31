using HalalLibrary.API.Models;
using HalalLibrary.API.Services;
using HalalLibrary.Model;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HalalLibrary.API.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class IngredientController : Controller
  {
    readonly IIngredientService _ingredientservice;

    public IngredientController(IIngredientService ingredientservice)
    {
      _ingredientservice = ingredientservice;
    }

    [HttpGet]
    [Route("query")]
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
      return await _ingredientservice.Query(nextRows, 
        offsetRows, 
        name, 
        brand, 
        supplier, 
        certifyingBody, 
        riskCategory, 
        status,
        verifiedBy);
    }

    [HttpPost]
    [Route("")]
    public async Task<long> Post([FromBody] Ingredient data)
    {
      return await _ingredientservice.InsertIngredient(data);
    }

    [HttpPut]
    [Route("")]
    public async Task<long> Put([FromBody] Ingredient data)
    {
      return await _ingredientservice.UpdateIngredient(data);
    }

    [HttpDelete]
    [Route("")]
    public async Task<bool> Delete(long ID)
    {
      await _ingredientservice.DeleteIngredient(ID);
      return true;
    }
  }
}
