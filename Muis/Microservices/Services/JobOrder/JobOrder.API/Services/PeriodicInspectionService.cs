using Core.API;
using Core.API.Provider;
using Core.Model;
using JobOrder.API.Models;
using JobOrder.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JobOrder.API.Services.Commands.JobOrders;
using Core.EventBus;
using JobOrder.API.Repository;

namespace JobOrder.API.Services
{
  public class PeriodicInspectionService : TransactionalService, IPeriodicInspectionService
  {
    readonly IEventBus _eventBus;

    readonly ISettingsService _settingsService;
    public PeriodicInspectionService(IDbConnectionProvider connectionProvider, IEventBus eventBus,
      ISettingsService settingsService) 
      : base(connectionProvider)
    {
      _eventBus = eventBus;
      _settingsService = settingsService;
    }

    public async Task<Unit> OnCertificateIssued(OnCertificateIssuedParam eventParam)
    {
      return await Execute(new OnCertificateIssuedCommand(
        eventParam, _eventBus, _settingsService));
    }
  }
}
