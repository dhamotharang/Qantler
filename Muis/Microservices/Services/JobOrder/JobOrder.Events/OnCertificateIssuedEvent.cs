using Core.EventBus;
using Core.Model;
using JobOrder.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobOrder.Events
{
  public class OnCertificateIssuedEvent : Event
  {
    public string Number { get; set; }

    public Scheme Scheme { get; set; }

    public SubScheme? SubScheme { get; set; }

    public DateTimeOffset? IssuedOn { get; set; }

    public DateTimeOffset? StartsFrom { get; set; }

    public DateTimeOffset? ExpiresOn { get; set; }

    public int? Grade { get; set; }

    public bool IsHighPriority { get; set; }

    public Guid CustomerID { get; set; }

    public string CustomerName { get; set; }

    #region Generated Properties

    public Premise Premise { get; set; }

    #endregion
  }
}
