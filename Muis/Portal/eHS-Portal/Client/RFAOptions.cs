using System;
using System.Collections.Generic;
using eHS.Portal.Model;

namespace eHS.Portal.Client
{
  public class RFAOptions
  {
    public long? ID { get; set; }

    public string Customer { get; set; }

    public long? RequestID { get; set; }

    public IList<RFAStatus> Status { get; set; }

    public Guid? RaisedBy { get; set; }

    public DateTimeOffset? CreatedOn { get; set; }

    public DateTimeOffset? DueOn { get; set; }
  }
}
