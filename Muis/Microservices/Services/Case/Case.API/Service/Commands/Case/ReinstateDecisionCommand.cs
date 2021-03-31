using Case.API.Params;
using Case.API.Repository;
using Case.API.Services.Commands.Case.SubCommands;
using Case.Model;
using Core.API;
using Core.API.Repository;
using Core.EventBus;
using Core.Model;
using System.Linq;
using System.Threading.Tasks;

namespace Case.API.Services.Commands.Case
{
  public class ReinstateDecisionCommand : IUnitOfWorkCommand<long>
  {
    readonly long _caseID;
    readonly ReinstateDecisionParam _data;
    readonly Officer _user;
    readonly IEventBus _eventBus;

    public ReinstateDecisionCommand(long caseID, ReinstateDecisionParam data, Officer user, IEventBus eventBus)
    {
      _caseID = caseID;
      _data = data;
      _user = user;
      _eventBus = eventBus;
    }

    public async Task<long> Invoke(IUnitOfWork unitOfWork)
    {
      var dbContext = DbContext.From(unitOfWork);
      unitOfWork.BeginTransaction();

      switch (_data.Sanction.Sanction)
      {
        case Sanction.Suspension:
          await new ReinstateDecisionSuspensionSubCommand(
            dbContext,
            _caseID,
            _user,
            _data)
            .Invoke();
          break;

        case Sanction.Revocation:
          await new ReinstateDecisionRevokeSubCommand(
            dbContext,
            _caseID,
            _user,
            _data)
            .Invoke();
          break;

        case Sanction.Closed:
          await new ReinstateDecisionClosedSubCommand(
            dbContext,
            _caseID,
            _user,
            _data,
            _eventBus)
            .Invoke();
          break;

        case Sanction.Reinstate:
          await new ReinstateDecisionReinstateSubCommand(
            dbContext,
            _caseID,
            _user,
            _data)
            .Invoke();
          break;
      }

      unitOfWork.Commit();

      return _caseID;
    }
  }
}