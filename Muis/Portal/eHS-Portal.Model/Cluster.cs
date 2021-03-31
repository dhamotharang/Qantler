using System;
using System.Collections.Generic;
using System.Text;

namespace eHS.Portal.Model
{
  public class Cluster
  {
    public long ID { get; set; }

    public string District { get; set; }

    public string Locations { get; set; }

    public string Color { get; set; }

    public DateTimeOffset? CreatedOn { get; set; }

    public DateTimeOffset? ModifiedOn { get; set; }

    public bool IsDeleted { get; set; }

    #region Generated properties

    public IList<string> Nodes { get; set; }

    public IList<Log> Logs { get; set; }

    public string AllNodes { get; set; }

    #endregion
  }
}
