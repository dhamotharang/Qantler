using Case.Model;
using Core.EventBus;
using System.Collections.Generic;

namespace Case.Events
{

  public class OnCertificateStatusChangedEvent : Event
  {
    public string Number { get; set; }

    public CertificateStatus? NewStatus { get; set; }

    public CertificateStatus? OldStatus { get; set; }
  }
}
