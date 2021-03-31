using System;
using System.Collections.Generic;
using Core.EventBus;
using Core.Model;
using Finance.Model;

namespace Finance.Events
{
  public class GenerateBillEvent : Event
  {
    public string RefNo { get; set; }

    public long RequestID { get; set; }

    public string RefID { get; set; }

    public BillType Type { get; set; }

    public BillRequestType RequestType { get; set; }

    public bool Expedite { get; set; }

    public Guid CustomerID { get; set; }

    public string CustomerName { get; set; }

    public IList<SchemeHolder> Schemes { get; set; }

    public DateTimeOffset ReferenceDate { get; set; }
  }

  public class SchemeHolder
  {
    public Scheme Scheme { get; set; }

    public SubScheme? SubScheme { get; set; }

    public int NoOfProducts { get; set; }

    public float Area { get; set; }

    public DateTimeOffset StartsFrom { get; set; }

    public DateTimeOffset ExpiresOn { get; set; }
  }
}
