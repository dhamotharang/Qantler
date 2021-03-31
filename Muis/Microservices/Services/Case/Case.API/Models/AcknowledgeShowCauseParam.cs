using Case.Model;
using Core.Model;
using System.Collections.Generic;

namespace Case.API.Params
{
  public class AcknowledgeShowCauseParam
  {
    public SanctionInfo Sanction { get; set; }

    public string Note { get; set; }

    public IList<Attachment> Attachment { get; set; }
  }
}
