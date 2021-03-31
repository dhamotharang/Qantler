using System;
using System.Collections.Generic;

namespace eHS.Portal.Models.User
{
  public class FormModel
  {
    public bool IsEdit => Data.ID != Guid.Empty;

    public Model.Identity Data { get; set; }

    public IList<Model.Cluster> Clusters { get; set; }
  }
}
