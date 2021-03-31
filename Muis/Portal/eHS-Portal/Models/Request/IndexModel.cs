using System;
using System.Collections.Generic;
using eHS.Portal.Model;

namespace eHS.Portal.Models.Request
{
  public class IndexModel
  {
    public IList<Identity> Users { get; set; }

    public Guid? AssignedTo { get; set; }

    public Guid? CustomerId{ get; set; }

    public IList<RequestStatus> DefaultStatuses { get; set; }
  }
}
