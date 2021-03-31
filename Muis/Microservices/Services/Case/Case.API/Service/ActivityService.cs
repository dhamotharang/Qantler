using Case.API.Services.Commands.Case;
using Core.API;
using Core.API.Provider;
using Core.Model;
using System.Threading.Tasks;

namespace Case.API.Service
{
  public class ActivityService : TransactionalService,
                                                   IActivityService
  {

    public ActivityService(IDbConnectionProvider connectionProvider)
        : base(connectionProvider)
    {
    }

    public async Task<long> InsertActivity(Model.Activity data, Officer user, long caseID)
    {
      return await Execute(new InsertActivityCommand(data, user, caseID));
    }
  }
}
