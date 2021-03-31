using Core.API;
using Core.API.Repository;
using Request.API.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Request.API.Services.Commands.Certificate360
{
  public class SelectCertCommand : IUnitOfWorkCommand<IList<Model.Certificate360>>
  {
    readonly Certificate360Filter _filter;

    public SelectCertCommand(Certificate360Filter filter)
    {
      _filter = filter;
    }

    public async Task<IList<Model.Certificate360>> Invoke(IUnitOfWork unitOfWork)
    {
      return await DbContext.From(unitOfWork).Certificate360.Certificate360Filter(_filter);
    }
  }
}
