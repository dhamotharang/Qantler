using Case.Model;
using Core.Model;
using System.Collections.Generic;

namespace Case.API.Params
{
  public class CaseFOCParam
  {
    public Letter Letter { get; set; }

    public IList<Identity> Reviewer { get; set; }
  }
}
