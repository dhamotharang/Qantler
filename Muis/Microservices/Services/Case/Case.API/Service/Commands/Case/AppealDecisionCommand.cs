using System;
using System.Threading.Tasks;
using Case.API.Models;
using Case.API.Repository;
using Case.API.Service.Commands.Case.SubCommands;
using Case.Model;
using Core.API;
using Core.API.Repository;
using Core.EventBus;
using Core.Model;

namespace Case.API.Service.Commands.Case
{
  public class AppealDecisionCommand : IUnitOfWorkCommand<Unit>
  {
    readonly long _caseID;
    readonly AppealDecisionParam _param;
    readonly Officer _user;
    readonly IEventBus _eventBus;

    public AppealDecisionCommand(long caseID, AppealDecisionParam param, Officer user,
      IEventBus eventBus)
    {
      _caseID = caseID;
      _param = param;
      _user = user;
      _eventBus = eventBus;
    }

    public async Task<Unit> Invoke(IUnitOfWork uow)
    {
      var dbContext = DbContext.From(uow);
      uow.BeginTransaction();

      Core.Base.SubCommand subCommand;

      switch (_param.SanctionInfo.Sanction)
      {
        case Sanction.Warning:
          subCommand = new AppealDecisionWarningSubCommand(dbContext, _caseID, _param, _user,
            _eventBus);
          break;
        case Sanction.Compound:
          subCommand = new AppealDecisionCompoundSubCommand(dbContext, _caseID, _param, _user,
            _eventBus);
          break;
        case Sanction.Suspension:
        case Sanction.ImmediateSuspension:
          subCommand = new AppealDecisionSuspensionSubCommand(dbContext, _caseID, _param, _user,
            _eventBus);
          break;
        case Sanction.Reinstate:
          subCommand = new AppealDecisionReinstateSubCommand(dbContext, _caseID, _param, _user,
            _eventBus);
          break;
        default:
          subCommand = new AppealDecisionRejectedSubCommand(dbContext, _caseID, _param, _user,
            _eventBus);
          break;
      }

      if (subCommand != null)
      {
        await subCommand.Execute();
      }


      uow.Commit();

      return Unit.Default;
    }
  }
}
