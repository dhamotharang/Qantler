using System;
using System.Collections.Generic;
using JobOrder.Model;

namespace JobOrder.API.Models
{
  public class JobOrderOptions
  {
    public long? ID { get; set; }

    public string Customer { get; set; }

    public string Premise { get; set; }

    public IList<JobOrderStatus> Status { get; set; }

    public IList<JobOrderType> Type { get; set; }

    public IList<Guid> AssignedTo { get; set; }

    public DateTimeOffset? From { get; set; }

    public DateTimeOffset? To { get; set; }
  }
}
