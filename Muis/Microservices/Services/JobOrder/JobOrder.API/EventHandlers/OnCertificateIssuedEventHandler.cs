using Core.EventBus;
using Core.Model;
using JobOrder.API.Services;
using JobOrder.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JobOrder.Model;
using JobOrder.API.Models;

namespace JobOrder.API.EventHandlers
{
  public class OnCertificateIssuedEventHandler : IEventHandler<OnCertificateIssuedEvent>
  {
    readonly IPeriodicInspectionService _periodicInspectionService;

    public OnCertificateIssuedEventHandler(IPeriodicInspectionService periodicInspectionService)
    {
      _periodicInspectionService = periodicInspectionService;
    }

    public async Task Handle(OnCertificateIssuedEvent @event)
    {
      var certificate = new Certificate
      {
        Number = @event.Number,
        Status = CertitifcateStatus.Active,
        Scheme = @event.Scheme,
        SubScheme = @event.SubScheme,
        IssuedOn = @event.IssuedOn,
        StartsFrom = @event.StartsFrom,
        ExpiresOn = @event.ExpiresOn,
        CustomerID = @event.CustomerID,
        PremiseID = @event.Premise.ID
      };

      @event.Premise.Grade = @event.Grade;
      @event.Premise.IsHighPriority = @event.IsHighPriority;

      await _periodicInspectionService.OnCertificateIssued(new OnCertificateIssuedParam
      {
        Certificate = certificate,
        Premise = @event.Premise,
        CustomerID = @event.CustomerID,
        CustomerName = @event.CustomerName
      });
    }
  }
}
