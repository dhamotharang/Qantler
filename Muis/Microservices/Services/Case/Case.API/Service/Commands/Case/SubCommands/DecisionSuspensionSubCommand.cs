using Case.API.Repository;
using Case.Model;
using Core.Base;
using System;
using System.Threading.Tasks;

namespace Case.API.Services.Commands.Case.SubCommands
{
  public class DecisionSuspensionSubCommand : SubCommand
  {
    readonly DbContext _dbContext;
    readonly long _caseID;
    readonly int _suspendedMonth;

    public DecisionSuspensionSubCommand(DbContext dbContext, int suspendedMonth,
      long caseID)
    {
      _dbContext = dbContext;
      _caseID = caseID;
      _suspendedMonth = suspendedMonth;
    }
    public override async Task Execute()
    {
      var @case = await _dbContext.Case.GetBasicByID(_caseID);
      @case.Sanction = Sanction.Suspension;
      @case.Status = CaseStatus.PendingSanctionLetter;
      @case.MinorStatus = null;
      @case.SuspendedFrom = DateTime.UtcNow;

      await _dbContext.Case.UpdateCaseInfo(@case);
    }
  }
}
