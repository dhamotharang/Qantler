using Case.API.Repository;
using Core.API;
using Core.API.Repository;
using System.Threading.Tasks;

namespace Case.API.Services.Commands.Case
{
  public class GetCaseByIDCommand : IUnitOfWorkCommand<Model.Case>
  {
    readonly long _id;

    public GetCaseByIDCommand(long id)
    {
      _id = id;
    }

    public async Task<Model.Case> Invoke(IUnitOfWork unitOfWork)
    {
      var dbContext = DbContext.From(unitOfWork);

      var @case = await dbContext.Case.GetByID(_id);
      @case.Activities = await dbContext.Case.GetActivityByCaseID(_id);

      return @case;
    }
  }
}
