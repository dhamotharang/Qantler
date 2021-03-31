using System;
using System.Collections.Generic;

namespace eHS.Portal.Client
{
  public class CaseOptions
  {
    public long? ID { get; set; }

    public IList<Guid> OffenceType { get; set; }

    public long? Source { get; set; }

    public IList<Guid> ManagedBy { get; set; }

    public IList<Guid> AssignedTo { get; set; }

    public IList<long> Status { get; set; }

    public DateTimeOffset? From { get; set; }

    public DateTimeOffset? To { get; set; }
  }
}
