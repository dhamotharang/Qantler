using System;
using System.Collections.Generic;
using eHS.Portal.Model;

namespace eHS.Portal.Client
{
  public class RequestOptions
  {
    public long? ID { get; set; }

    public Guid? CustomerID { get; set; }
    public string RefID { get; set; }

    public string CustomerCode { get; set; }

    public string Customer { get; set; }

    public string Premise { get; set; }

    public DateTimeOffset? From { get; set; }

    public DateTimeOffset? To { get; set; }

    public IList<RequestStatus> Status { get; set; }

    public IList<RequestType> Type { get; set; }

    public RFAStatus? RFAStatus { get; set; }

    public IList<Guid> AssignedTo { get; set; }

    public EscalateStatus? EscalateStatus { get; set; }

    public IList<RequestStatusMinor> StatusMinor { get; set; }
  }
}
