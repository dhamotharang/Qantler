using Core.API;
using System.Threading.Tasks;
using Core.API.Repository;
using JobOrder.API.Repository;

namespace JobOrder.Services.Commands.Findings
{
  public class SubmitFindingsCommand : IUnitOfWorkCommand<Model.Findings>
  {
    readonly Model.Findings _findings;

    public SubmitFindingsCommand(Model.Findings findings)
    {
      _findings = findings;
    }

    public async Task<Model.Findings> Invoke(IUnitOfWork uow)
    {
      var dbContext = DbContext.From(uow);
      uow.BeginTransaction();

      var id = await dbContext.Findings.InsertFindings(_findings);
      var result = await dbContext.Findings.GetFindingsByID(id);

      uow.Commit();

      return result;
    }
  }
}
