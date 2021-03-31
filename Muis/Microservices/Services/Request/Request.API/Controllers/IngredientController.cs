using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Request.API.Services;
using Request.Model;

namespace Request.API.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class IngredientController : ControllerBase
  {
    readonly IIngredientService _ingredientService;

    public IngredientController(IIngredientService ingredientService)
    {
      _ingredientService = ingredientService;
    }

    [HttpPut]
    [Route("list")]
    public async Task<string> BulkUpdate(IList<Ingredient> ingredients, Guid userID,
      string userName)
    {
      await _ingredientService.BulkUpdate(ingredients, userID, userName);
      return "Ok";
    }

    [HttpPut]
    [Route("list/review")]
    public async Task<string> BulkReview(IList<Ingredient> ingredients, Guid userID,
      string userName)
    {
      await _ingredientService.BulkReview(ingredients, userID, userName);
      return "Ok";
    }

    [HttpPost]
    [Route("")]
    public async Task<string> AddIngredients([FromBody]IList<Ingredient> ingredients, Guid userID,
      string userName)
    {
      await _ingredientService.AddIngredients(ingredients, userID, userName);
      return "Ok";
    }
  }
}
