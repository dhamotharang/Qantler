using Core.Base;
using Core.EventBus;
using Request.API.Repository;
using Request.Events;
using Request.Model;
using System.Linq;
using System.Threading.Tasks;

namespace Request.API.Services.Commands.Request.SubCommands
{
  public class SyncNewIngredientSubCommand : SubCommand
  {
    readonly Review _review;
    readonly DbContext _dbContext;
    readonly IEventBus _eventBus;

    public SyncNewIngredientSubCommand(Review review, DbContext dbContext, IEventBus eventBus)
    {
      _review = review;
      _dbContext = dbContext;
      _eventBus = eventBus;
    }
    public override async Task Execute()
    {
      var approvedLineItems = _review.LineItems.Where(e => e.Approved.Value).ToList();
      var approved = approvedLineItems.Count() >= 1;

      if (approved)
      {
        var ingredients = (await _dbContext.Ingredient.Query(new IngredientFilter
        {
          RequestID = _review.RequestID,
          RiskCategory = new RiskCategory[]
          {
          RiskCategory.NotCategorized
          }
        })).Select(from => new Item
        {
          Name = from.Text,
          Brand = from.BrandName,
          RiskCategory = from.RiskCategory,
          Status = from.Status,
          SupplierName = from.SupplierName,
          CertifyingBodyName = from.CertifyingBodyName,
          ReviewedBy = from.ReviewedBy
        }).ToList();

        if (ingredients.Any())
        {
          _eventBus.Publish(new NewIngredientEvent
          {
            Ingredients = ingredients
          });
        }
      }
    }
  }
}
