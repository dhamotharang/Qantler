using System;
using System.Collections.Generic;
using eHS.Portal.Model;

namespace eHS.Portal.Models.RFA
{
  public class IndexModel
  {
    public IList<Identity> Users { get; set; }

    public IList<Model.RFA> Data { get; set; }
  }
}
