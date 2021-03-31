using eHS.Portal.Model;
using System;
using System.Collections.Generic;

namespace eHS.Portal.Client
{
  public class JobOrderOptions
  {
    public long? ID { get; set; }

    public string Customer { get; set; }

    public string Premise { get; set; }

    public DateTimeOffset? From { get; set; }

    public DateTimeOffset? To { get; set; }

    public IList<JobOrderStatus> Status { get; set; }

    public IList<JobOrderType> Type { get; set; }

    public IList<Guid> AssignedTo { get; set; }
  }
}
