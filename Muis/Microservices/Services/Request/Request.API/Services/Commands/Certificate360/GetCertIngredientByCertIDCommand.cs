using Core.API;
using Core.API.Repository;
using Request.API.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace Request.API.Services.Commands.Certificate360
{
  public class GetCertIngredientByCertIDCommand : IUnitOfWorkCommand<IList<Model.Ingredient>>
  {
    readonly long _ID;

    public GetCertIngredientByCertIDCommand(long ID)
    {
      _ID = ID;
    }

    public async Task<IList<Model.Ingredient>> Invoke(IUnitOfWork unitOfWork)
    {
      return await DbContext.From(unitOfWork).Certificate360
        .GetCertificate360Ingredients(_ID);
    }

  }
}