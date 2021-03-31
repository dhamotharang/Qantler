using eHS.Portal.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eHS.Portal.Models.Cluster
{
  public class ClusterModel
  {
    public long ID { get; set; }

    public Model.Cluster Data { get; set; }

    public IList<Model.Cluster> OClusters { get; set; }
  }
}
