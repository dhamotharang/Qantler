using Core.API;
using Core.API.Exceptions;
using Core.API.Repository;
using Core.Model;
using HalalLibrary.API.Repository;
using HalalLibrary.Events;
using HalalLibrary.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HalalLibrary.API.Service.Command.Ingredient
{
  public class BulkInsertIngredientCommand : IUnitOfWorkCommand<Unit>
  {
    readonly IList<Item> _ingredients;

    public BulkInsertIngredientCommand(IList<Item> ingredients)
    {
      _ingredients = ingredients;
    }

    public async Task<Unit> Invoke(IUnitOfWork uow)
    {
      var dbContext = DbContext.From(uow);
      uow.BeginTransaction();

      foreach (var from in _ingredients)
      {
        var ingredient = new Model.Ingredient()
        {
          Name = from.Name,
          Brand = from.Brand,
          RiskCategory = from.RiskCategory,
          Status = from.Status.HasValue ? from.Status.Value : IngredientStatus.Unverified,
          Supplier = new Supplier()
          {
            Name = from.SupplierName
          },
          CertifyingBody = new CertifyingBody()
          {
            Name = from.CertifyingBodyName
          }
        };

        var hasIngredient = await dbContext.Ingredient.CheckIngredient(ingredient);

        if (hasIngredient)
        {
          if (!string.IsNullOrEmpty(ingredient.CertifyingBody?.Name))
          {
            var certificateBody = await dbContext.CertifyingBody
              .GetCertifyingBodyByName(ingredient.CertifyingBody?.Name);

            if (certificateBody == null)
            {
              var certifyingBodyID = await dbContext.CertifyingBody
                .InsertCertifyingBody(ingredient.CertifyingBody);

              ingredient.CertifyingBodyID = certifyingBodyID;
            }
            else
            {
              ingredient.CertifyingBodyID = certificateBody?.ID;
            }
          }

          if (!string.IsNullOrEmpty(ingredient.Supplier?.Name))
          {
            var supplier = await dbContext.Supplier
              .GetSupplierByName(ingredient.Supplier?.Name);

            if (supplier == null)
            {
              var supplierID = await dbContext.Supplier
                .InsertSupplier(ingredient.Supplier);

              ingredient.SupplierID = supplierID;
            }
            else
            {
              ingredient.SupplierID = supplier?.ID;
            }
          }

          var result = await dbContext.Ingredient.InsertIngredient(ingredient);

          if (result == 0)
          {
            var logText = await dbContext.Translation
              .GetTranslation(Locale.EN, "ValidateIngredient");

            throw new BadRequestException(logText);
          }
        }
      }

      uow.Commit();

      return Unit.Default;
    }
  }
}
