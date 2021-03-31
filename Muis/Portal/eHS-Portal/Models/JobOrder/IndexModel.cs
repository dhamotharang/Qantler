using System;
using System.Collections.Generic;
using eHS.Portal.Model;

namespace eHS.Portal.Models.JobOrder
{
  public class IndexModel
  {
    public IList<Identity> Users { get; set; }

    public Guid? AssignedTo { get; set; }

    public JobOrderType? DefaultType { get; set; }

    public IList<JobOrderStatus> DefaultStatuses { get; set; }
  }
}