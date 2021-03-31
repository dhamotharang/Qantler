using eHS.Portal.Model;
using System.Collections.Generic;

namespace eHS.Portal.DTO
{
  public class ReinstateDecisionParam
  {
    public SanctionInfo Sanction { get; set; }

    public string Note { get; set; }

    public IList<Attachment> Attachment { get; set; }
  }
}
