using Case.API.Params;
using Case.API.Repository;
using Core.API;
using Core.API.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Case.API.Services.Commands.Case
{
  public class SelectCaseCommand : IUnitOfWorkCommand<IEnumerable<Model.Case>>
  {
    readonly CaseOptions _options;

    public SelectCaseCommand(CaseOptions options)
    {
      _options = options;
    }

    public async Task<IEnumerable<Model.Case>> Invoke(IUnitOfWork unitOfWork)
    {
      return await DbContext.From(unitOfWork).Case.Select(_options);
    }
  }
}
