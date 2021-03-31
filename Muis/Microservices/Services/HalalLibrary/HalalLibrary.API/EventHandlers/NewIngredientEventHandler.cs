using Core.EventBus;
using HalalLibrary.API.Services;
using HalalLibrary.Events;
using System.Threading.Tasks;

namespace HalalLibrary.API.EventHandlers
{
  public class NewIngredientEventHandler : IEventHandler<NewIngredientEvent>
  {
    readonly IIngredientService _ingredientService;

    public NewIngredientEventHandler(IIngredientService ingredientService)
    {
      _ingredientService = ingredientService;
    }

    public async Task Handle(NewIngredientEvent @event)
    {
      await _ingredientService.AddIngredients(@event);
    }

  }
}