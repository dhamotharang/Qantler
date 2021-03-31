using JobOrder.Model;
using System.Threading.Tasks;
using Core.API;
using Core.API.Provider;
using JobOrder.Services.Commands.Findings;

namespace JobOrder.API.Services
{
  public class FindingsService : TransactionalService,
                                 IFindingsService
  {
    public FindingsService(IDbConnectionProvider connectionProvider)
         : base(connectionProvider)
    {
    }

    public Task<Findings> Submit(Findings findings)
    {
      return Execute(new SubmitFindingsCommand(findings));
    }
  }
}
