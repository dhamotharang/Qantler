using System;
using System.Linq;
using System.Threading.Tasks;
using Core.EventBus;
using Finance.API.Services;
using Finance.API.Strategies.Billing;
using Finance.Events;

namespace Finance.API.EventHandlers
{
  public class GenerateBillEventHandler : IEventHandler<GenerateBillEvent>
  {
    readonly IBillService _billService;

    public GenerateBillEventHandler(IBillService billService)
    {
      _billService = billService;
    }

    public async Task Handle(GenerateBillEvent @event)
    {
      await _billService.GenerateBill(new BillRequest
      {
        RefNo = @event.RefNo,
        RequestID = @event.RequestID,
        RefID = @event.RefID,
        Type = @event.Type,
        RequestType = @event.RequestType,
        Expedite = @event.Expedite,
        CustomerID = @event.CustomerID,
        CustomerName = @event.CustomerName,
        LineItems = @event.Schemes?.Select(e => new BillRequestLineItem
        {
          Scheme = e.Scheme,
          SubScheme = e.SubScheme,
          NoOfProducts = e.NoOfProducts,
          Area = e.Area,
          StartsFrom = e.StartsFrom,
          ExpiresOn = e.ExpiresOn
        }).ToList(),
        ReferenceDate = @event.ReferenceDate
      });
    }
  }
}
