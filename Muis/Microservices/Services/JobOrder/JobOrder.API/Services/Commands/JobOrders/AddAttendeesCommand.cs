using Core.API;
using Core.API.Repository;
using JobOrder.API.Repository;
using JobOrder.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JobOrder.API.Services.Commands.JobOrders
{
  public class AddAttendeesCommand : IUnitOfWorkCommand<IEnumerable<Attendee>>
  {
    readonly long _id;
    readonly IList<Attendee> _attendees;

    public AddAttendeesCommand(IList<Attendee> attendees, long id)
    {
      _id = id;
      _attendees = attendees;

    }

    public async Task<IEnumerable<Attendee>> Invoke(IUnitOfWork uow)
    {
      var dbContext = DbContext.From(uow);
      uow.BeginTransaction();

      await dbContext.JobOrder.AddAttendees(_id, _attendees);
      var result = await dbContext.JobOrder.GetAttendeesByJobID(_id);

      uow.Commit();

      return result;
    }
  }
}
