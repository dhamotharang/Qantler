using Core.API;
using Core.API.Repository;
using Request.API.Models;
using Request.API.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Request.API.Services.Commands.Certificate360
{
  public class GetCertWithIngredientCommand : IUnitOfWorkCommand<IList<Model.Certificate360>>
  {
    readonly Certificate360IngredientFilter _filter;

    public GetCertWithIngredientCommand(Certificate360IngredientFilter filter)
    {
      _filter = filter;
    }

    public async Task<IList<Model.Certificate360>> Invoke(IUnitOfWork unitOfWork)
    {
      return await DbContext.From(unitOfWork).Certificate360
        .GetCertificate360WithIngredient(_filter);
    }
  }
}
