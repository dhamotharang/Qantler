using Request.Model;
using System;

namespace Request.API.Params
{
  public class RequestOptions
  {
    public long? ID { get; set; }

    public Guid? CustomerID { get; set; }

    public string CustomerCode { get; set; }

    public string Customer { get; set; }

    public long? PremiseID { get; set; }

    public string Premise { get; set; }

    public DateTimeOffset? From { get; set; }

    public DateTimeOffset? To { get; set; }

    public RequestStatus[] Status { get; set; }

    public RequestType[] Type { get; set; }

    public RFAStatus? RFAStatus { get; set; }

    public Guid[] AssignedTo { get; set; }

    public EscalateStatus? EscalateStatus { get; set; }

    public RequestStatusMinor[] StatusMinor { get; set; }
  }
}
