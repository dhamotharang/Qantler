using eHS.Portal.Model;
using System.Collections.Generic;

namespace eHS.Portal.Models.Case
{
  public class DetailModel
  {
    public IList<Model.Master> OffenceType { get; set; }

    public IList<Premise> Premises { get; set; }
  }
}
