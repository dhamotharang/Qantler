using Core.API.Exceptions;
using Core.Base;
using Core.Model;
using Request.API.Params;
using Request.API.Repository;
using Request.Model;
using System.Linq;
using System.Threading.Tasks;

namespace Request.API.Services.Commands.Request.SubCommands
{
  public class HC03RenewalSyncSubCommand : SubCommand
  {
    readonly Review _review;
    readonly DbContext _dbContext;
    readonly Officer _officer;

    public HC03RenewalSyncSubCommand(Review review, DbContext dbContext, Officer officer)
    {
      _review = review;
      _dbContext = dbContext;
      _officer = officer;
    }
    public override async Task Execute()
    {
      var request = await _dbContext.Request.GetRequestByID(_review.RequestID);

      var premise = request.Premises.Where(e => e.IsPrimary).FirstOrDefault();

      var renewalRequest = (await _dbContext.Request.Select(new RequestOptions
      {
        PremiseID = premise.ID,
        Status = new RequestStatus[]
        {
          RequestStatus.Draft,
          RequestStatus.Open,
          RequestStatus.PendingReviewApproval,
          RequestStatus.ForInspection,
          RequestStatus.PendingApproval
        },
        Type = new RequestType[]
        {
          RequestType.Renewal
        }
      })).FirstOrDefault();

      if (renewalRequest != null)
      {
        var menus = (await _dbContext.Menu.Query(new MenuFilter
        {
          RequestID = renewalRequest.ID
        }))?
        .Where(e => e.ChangeType != ChangeType.Delete)?
        .Select(e => e.Text.ToLower())?
        .ToList();

        var newMenus = request.Menus.Where(m => !menus.Contains(m.Text.ToLower()))
          .ToList();

        await _dbContext.Menu.AddMenus(newMenus, _officer.ID, _officer.Name);

        var ingredients = (await _dbContext.Ingredient.Query(
            new IngredientFilter { RequestID = renewalRequest.ID }))?
          .Where(e => e.ChangeType != ChangeType.Delete)?
          .Select(e => FormatIngredient(e))?
          .ToList();

        var newIngredients = request.Ingredients.Where
          (
            i => !ingredients.Contains(FormatIngredient(i))
          ).ToList();

        await _dbContext.Ingredient.AddIngredients(newIngredients, _officer.ID, _officer.Name);

      }
    }

    string FormatIngredient(Model.Ingredient i)
    {
      return $"{i.Text}-{i.BrandName ?? ""}-{i.SupplierName ?? ""}".ToLower();
    }
  }
}

