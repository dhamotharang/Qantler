using System;
using System.Collections.Generic;

namespace eHS.Portal.Models.User
{
  public class DetailsModel
  {
    public Guid ID { get; set; }

    public Model.Identity Data { get; set; }

    public IList<Model.Cluster> Clusters { get; set; }
  }
}
