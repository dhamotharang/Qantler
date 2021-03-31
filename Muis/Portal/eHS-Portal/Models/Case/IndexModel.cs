using eHS.Portal.Model;
using System.Collections.Generic;

namespace eHS.Portal.Models.Case
{
  public class IndexModel
  {
    public IList<Identity> Users { get; set; }

    public IList<Model.Master> OffenceType { get; set; }
  }
}
