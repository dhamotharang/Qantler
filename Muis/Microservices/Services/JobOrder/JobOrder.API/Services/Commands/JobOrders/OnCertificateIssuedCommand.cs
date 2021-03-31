using Core.API;
using Core.API.Repository;
using Core.EventBus;
using Core.Model;
using JobOrder.API.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JobOrder.API.Models;
using JobOrder.Model;
using JobOrder.API.Helpers;

namespace JobOrder.API.Services.Commands.JobOrders
{
  public class OnCertificateIssuedCommand : IUnitOfWorkCommand<Unit>
  {
    readonly OnCertificateIssuedParam _param;

    readonly IEventBus _eventBus;

    readonly ISettingsService _settingsService;
    public OnCertificateIssuedCommand(OnCertificateIssuedParam eventParam,
      IEventBus eventBus, ISettingsService settingsService)
    {
      _param = eventParam;
      _eventBus = eventBus;
      _settingsService = settingsService;
    }

    public async Task<Unit> Invoke(IUnitOfWork unitOfWork)
    {
      var dbContext = DbContext.From(unitOfWork);
      unitOfWork.BeginTransaction();

      var customer = new Customer
      {
        ID = _param.CustomerID,
        Name = _param.CustomerName
      };

      var existingCustomer = await dbContext.Customer.GetCustomerByID(_param.CustomerID);
      if (existingCustomer != null)
      {
        customer.Code = existingCustomer.Code;
        customer.GroupCode = existingCustomer.GroupCode;
      }
      await dbContext.Customer.InsertOrReplace(customer);

      await dbContext.Premise.InsertOrReplace(_param.Premise);

      await dbContext.Certificate.DeleteCertificateByPremise(_param.Premise.ID,
        _param.Certificate.Scheme, _param.Certificate.SubScheme);

      _param.Certificate.Status = CertitifcateStatus.Active;

      await dbContext.Certificate.InsertOrReplace(_param.Certificate);

      var certificates = await dbContext.Certificate.Select(new CertificateOptions
      {
        PremiseID = _param.Premise.ID
      });

      var nextTargetInspection = await PeriodicInspectionHelper.CalculateNextTargetScheduledDate(
        _param.Certificate.IssuedOn, certificates.ToList(), _param.Premise.Grade,
          _param.Premise.IsHighPriority, _settingsService);

      var scheduler = new PeriodicScheduler
      {
        LastScheduledOn = _param.Certificate.IssuedOn.Value,
        NextTargetInspection = nextTargetInspection,
        PremiseID = _param.Premise.ID,
        Status = PreiodicSchedulerStatus.Pending
      };
      await dbContext.PeriodicScheduler.InsertOrReplace(scheduler);

      unitOfWork.Commit();

      return Unit.Default;
    }
  }
}
