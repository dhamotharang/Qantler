using System;
using Core.EventBus;
using Request.Model;

namespace Request.Events
{
  public class OnRequestStatusChangedEvent : Event
  {
    public long ID { get; set; }

    public string RefID { get; set; }

    public RequestStatus NewStatus { get; set; }

    public RequestStatus OldStatus { get; set; }
  }
}
