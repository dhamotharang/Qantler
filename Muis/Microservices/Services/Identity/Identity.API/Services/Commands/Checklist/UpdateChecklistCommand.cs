using Core.API;
using Core.API.Repository;
using Identity.API.Repository;
using Request.Model;
using System.Threading.Tasks;

namespace Identity.API.Services.Commands.Checklist
{
  public class UpdateChecklistCommand : IUnitOfWorkCommand<bool>
  {
    readonly ChecklistHistory _checklist;

    public UpdateChecklistCommand(ChecklistHistory checklist)
    {
      _checklist = checklist;
    }

    public async Task<bool> Invoke(IUnitOfWork unitOfWork)
    {
      var dbContext = DbContext.From(unitOfWork);
      unitOfWork.BeginTransaction();

      var result = await dbContext.Checklist.UpdateChecklist(_checklist);

      unitOfWork.Commit();

      return result;
    }
  }
}
