using Core.API;
using Core.API.Repository;
using Identity.API.Repository;
using System.Threading.Tasks;

namespace Identity.API.Services.Commands.Cluster
{
  public class GetClusterByIDCommand : IUnitOfWorkCommand<Model.Cluster>
  {
    readonly long _id;

    public GetClusterByIDCommand(long id)
    {
      _id = id;
    }

    public async Task<Model.Cluster> Invoke(IUnitOfWork unitOfWork)
    {
      return await DbContext.From(unitOfWork).Cluster.GetClusterByID(_id);
    }
  }
}
