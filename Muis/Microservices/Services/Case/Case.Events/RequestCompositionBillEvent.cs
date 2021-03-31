using Core.EventBus;
using System;

namespace Case.Events
{
  public class RequestCompositionBillEvent : Event
  {
    public Guid? AccountID { get; set; }

    public string Name { get; set; }

    public decimal Amount { get; set; }

    public long RefID { get; set; }
  }
}
