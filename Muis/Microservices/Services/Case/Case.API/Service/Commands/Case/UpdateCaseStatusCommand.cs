using Case.API.Repository;
using Case.Model;
using Core.API;
using Core.API.Repository;
using Core.Model;
using System.Threading.Tasks;

namespace Case.API.Services.Commands.Case
{
  public class UpdateCaseStatusCommand : IUnitOfWorkCommand<Unit>
  {
    readonly long _caseID;
    readonly CaseStatus _status;
    readonly CaseMinorStatus? _statusMinor;

    public UpdateCaseStatusCommand(long caseID, CaseStatus status,
      CaseMinorStatus? statusMinor = null)
    {
      _caseID = caseID;
      _status = status;
      _statusMinor = statusMinor;
    }

    public async Task<Unit> Invoke(IUnitOfWork unitOfWork)
    {
      var dbContext = DbContext.From(unitOfWork);
      unitOfWork.BeginTransaction();

      await dbContext.Case.UpdateStatus(_caseID, _status, _statusMinor);

      unitOfWork.Commit();

      return Unit.Default;
    }
  }
}
