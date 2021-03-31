using eHS.Portal.Model;
using System.Collections.Generic;

namespace eHS.Portal.DTO
{
  public class CaseFOCParam
  {
    public Letter Letter { get; set; }

    public IList<Identity> Reviewer { get; set; }
  }
}
