using Core.Base;
using System;
using System.Collections.Generic;
using System.Text;
using JobOrder.Model;
using System.Threading.Tasks;
using Core.Base.Repository;
using Core.Model;
using Core.EventBus;

namespace JobOrder.Jobs.Services.Command
{
  public class CreateJobOrderCommand : IUnitOfWorkCommand<Unit>
  {
    readonly IEventBus _eventBus;
    public CreateJobOrderCommand(IEventBus eventBus)
    {
      _eventBus = eventBus;
    }

    public async Task<Unit> Invoke(IUnitOfWork uow)
    {
      var dbContext = Repository.DbContext.From(uow);
      uow.BeginTransaction();

      var schedulers = await dbContext.PeriodicScheduler.GetSchedulers();
      foreach (PeriodicScheduler schedulerItem in schedulers)
      {
        var certificate = await dbContext.Certificate.Select(new Model.CertificateOptions
        {
          PremiseID = schedulerItem.PremiseID
        });

        var jobOrder = new JobOrder.Model.JobOrder
        {
          Type = JobOrderType.Periodic,
          Status = JobOrderStatus.Draft,
          TargetDate = schedulerItem.NextTargetInspection,
          CustomerID = certificate.Customer.ID,
          Customer = new Customer
          {
            ID = certificate.Customer.ID,
            Name = certificate.Customer.Name,
            Code = certificate.Customer.Code
          },
          LineItems = new List<JobOrder.Model.JobOrderLineItem>
          {
            new JobOrderLineItem {
              Scheme = certificate.Scheme,
              SubScheme = certificate.SubScheme
            }
          },
          Premises = new List<Premise>
          {
            certificate.Premise
          }
        };

        var jobId = await dbContext.JobOrder.InsertJobOrder(jobOrder);

        var logText = await dbContext.Translation.GetTranslation(Locale.EN, "JobOrderCreate");

        var logID = await dbContext.Log.InsertLog(new Log
        {
          Type = LogType.JobOrder,
          Action = logText
        });

        var scheduler = await dbContext.PeriodicScheduler.Select(new Model.PeriodicSchedulerOptions
        {
          PremiseID = certificate.Premise.ID
        });
        await dbContext.PeriodicScheduler.InsertOrReplace(new PeriodicScheduler
        {
          PremiseID = certificate.Premise.ID,
          LastJobID = jobId,
          NextTargetInspection = scheduler[0].NextTargetInspection,
          LastScheduledOn = scheduler[0].LastScheduledOn,
          Status = scheduler[0].Status,
        });

        _eventBus.Publish(new SendNotificationWithPermissionsEvent
        {
          Title = await dbContext.Translation.GetTranslation(Locale.EN, "PeriodicSchedulerTitle"),
          Body = await dbContext.Translation.GetTranslation(Locale.EN, "PeriodicSchedulerBody"),
          Module = "PeriodicInspection",
          RefID = $"{jobId}",
          Permissions = new List<Permission> { Permission.PeriodicInspectionReadWrite }
        });
      }

      uow.Commit();

      return Unit.Default;
    }
  }
}
