using Core.API;
using Core.API.Repository;
using Core.Model;
using JobOrder.API.Repository;
using JobOrder.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JobOrder.API.Services.Commands.JobOrders
{
  public class UpdateAttendeesCommand : IUnitOfWorkCommand<Unit>
  {
    readonly IList<Attendee> _attendees;
    readonly long _id;

    public UpdateAttendeesCommand(long id, IList<Attendee> attendees)
    {
      _id = id;
      _attendees = attendees;
    }

    public async Task<Unit> Invoke(IUnitOfWork uow)
    {
      var dbContext = DbContext.From(uow);
      uow.BeginTransaction();

      await dbContext.JobOrder.UpdateAttendees(_id, _attendees);

      uow.Commit();

      return Unit.Default;
    }
  }

}
