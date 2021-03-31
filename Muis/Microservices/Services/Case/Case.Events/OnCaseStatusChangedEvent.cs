using Case.Model;
using Core.EventBus;
using System.Collections.Generic;

namespace Case.Events
{

  public class OnCaseStatusChangedEvent : Event
  {
    public IList<long> PremisesID { get; set; }

    public CaseStatus? NewStatus { get; set; }

    public CaseStatus? OldStatus { get; set; }
  }
}
