using Core.API;
using Core.API.Repository;
using Identity.API.Repository;
using Request.Model;
using System.Threading.Tasks;

namespace Identity.API.Services.Commands.Checklist
{
  public class InsertChecklistCommand : IUnitOfWorkCommand<bool>
  {
    readonly ChecklistHistory _checklist;

    public InsertChecklistCommand(ChecklistHistory checklist)
    {
      _checklist = checklist;
    }

    public async Task<bool> Invoke(IUnitOfWork unitOfWork)
    {
      var dbContext = DbContext.From(unitOfWork);
      unitOfWork.BeginTransaction();

      var result = await dbContext.Checklist.InsertChecklist(_checklist);

      unitOfWork.Commit();

      return result;
    }
  }
}
