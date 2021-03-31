using Case.API.Repository;
using Case.Model;
using Core.Base;
using Core.EventBus;
using System.Threading.Tasks;

namespace Case.API.Services.Commands.Case.SubCommands
{
  public class DecisionRevokeSubCommand : SubCommand
  {
    readonly DbContext _dbContext;
    readonly long _caseID;

    public DecisionRevokeSubCommand(DbContext dbContext, long caseID)
    {
      _dbContext = dbContext;
      _caseID = caseID;
    }
    public override async Task Execute()
    {
      var @case = await _dbContext.Case.GetBasicByID(_caseID);
      @case.Sanction = Sanction.Revocation;
      @case.Status = CaseStatus.PendingSanctionLetter;
      @case.MinorStatus = null;

      await _dbContext.Case.UpdateCaseInfo(@case);
    }
  }
}
