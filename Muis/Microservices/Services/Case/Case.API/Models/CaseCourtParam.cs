using Core.Model;
using System.Collections.Generic;

namespace Case.API.Params
{
  public class CaseCourtParam
  {
    public string Notes { get; set; }

    public IList<Attachment> Attachments { get; set; }
  }
}
