using Core.API;
using Core.API.Repository;
using Identity.API.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Identity.API.Services.Commands.Cluster
{
  public class QueryClusterCommand : IUnitOfWorkCommand<IEnumerable<Model.Cluster>>
  {
    public QueryClusterCommand()
    {
    }

    public async Task<IEnumerable<Model.Cluster>> Invoke(IUnitOfWork unitOfWork)
    {
      return await DbContext.From(unitOfWork).Cluster.Select();
    }
  }
}
