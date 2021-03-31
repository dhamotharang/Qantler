using Core.API;
using Core.API.Provider;
using Core.API.Smtp;
using Core.EventBus;
using Core.Model;
using JobOrder.API.Models;
using JobOrder.API.Services.Commands.JobOrders;
using JobOrder.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JobOrder.API.Services
{
  public class JobOrderService : TransactionalService,
                                 IJobOrderService
  {
    readonly IEventBus _eventBus;

    readonly IEmailService _emailService;

    readonly ISmtpProvider _smtpProvider;
    public JobOrderService(IDbConnectionProvider connectionProvider, IEventBus eventBus,
      IEmailService emailService, ISmtpProvider smtpProvider) : base(connectionProvider)
    {
      _eventBus = eventBus;

      _emailService = emailService;

      _smtpProvider = smtpProvider;
    }

    public async Task<IEnumerable<Model.JobOrder>> QueryJobOrder(JobOrderOptions options)
    {
      return await Execute(new QueryJobOrderCommand(options));
    }

    public async Task<Model.JobOrder> Create(ScheduleJobOrderParam param, Guid userID,
      string userName, string userEmail)
    {
      return await Execute(new CreateJobOrderCommand(param, userID, userName, userEmail, _eventBus,
        _emailService, _smtpProvider));
    }

    public async Task<IEnumerable<Model.JobOrder>> GetJobOrders(Guid assignedTo,
      DateTimeOffset? lastupdatedOn)
    {
      return await Execute(new GetJobOrdersCommand(assignedTo, lastupdatedOn));
    }

    public async Task<Model.JobOrder> UpdateJobOrderStatus(long id, JobOrderStatus newStatus)
    {
      return await Execute(new UpdateJobOrderStateCommand(id, newStatus, _eventBus));
    }

    public async Task<Model.JobOrder> GetJobOrderByID(long id)
    {
      return await Execute(new GetJobOrderIDCommand(id));
    }

    public async Task AddInvitee(long jobID, Officer officer, Officer user)
    {
      await Execute(new AddInviteeCommand(jobID, officer, user, _eventBus));
    }

    public async Task DeleteInvitee(long jobID, Guid officerID, Officer user)
    {
      await Execute(new DeleteInviteeCommand(jobID, officerID, user, _eventBus));
    }

    public async Task<IEnumerable<Attendee>> AddAttendees(long id, IList<Attendee> attendees)
    {
      return await Execute(new AddAttendeesCommand(attendees, id));
    }

    public async Task UpdateAttendees(long id, IList<Attendee> attendees)
    {
      await Execute(new UpdateAttendeesCommand(id, attendees));
    }

    public async Task Reschedule(long id, RescheduleParam param, Officer user)
    {
      await Execute(new RescheduleJobOrderCommand(id, param, user, _eventBus, 
        _emailService, _smtpProvider));
    }

    public async Task Cancel(long id, CancelParam param, Officer user)
    {
      await Execute(new CancelJobOrderCommand(id, param, user, _eventBus));
    }

    public async Task Schedule(long id, ScheduleParam param, Officer user)
    {
      await Execute(new ScheduleJobOrderCommand(id, param, user, _eventBus));
    }
  }
}
