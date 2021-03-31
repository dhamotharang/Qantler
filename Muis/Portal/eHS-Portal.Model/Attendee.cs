using System;
using System.Collections.Generic;
using System.Text;

namespace eHS.Portal.Model
{
  public class Attendee
  {
    public long ID { get; set; }

    public string Name { get; set; }

    public string Designation { get; set; }

    public bool? Start { get; set; }

    public bool? End { get; set; }

    public long JobID { get; set; }

    public DateTimeOffset? CreatedOn { get; set; }

    public DateTimeOffset? ModifiedOn { get; set; }

    public bool IsDeleted { get; set; }
  }
}
