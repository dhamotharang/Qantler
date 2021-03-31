using Core.API;
using Core.API.Repository;
using Request.API.Repository;
using System.Threading.Tasks;

namespace Request.API.Services.Commands.RFA
{
  public class GetRFAByIDCommand : IUnitOfWorkCommand<Model.RFA>
  {
    readonly long _id;

    public GetRFAByIDCommand(long id)
    {
      _id = id;
    }

    public async Task<Model.RFA> Invoke(IUnitOfWork unitOfWork)
    {
      return await DbContext.From(unitOfWork).RFA.GetRFAByID(_id);
    }
  }
}
