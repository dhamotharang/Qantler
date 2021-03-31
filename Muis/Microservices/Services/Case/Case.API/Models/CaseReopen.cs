using Core.Model;
using System.Collections.Generic;

namespace Case.API.Params
{
  public class CaseReopen
  {
    public string Notes { get; set; }

    public IList<Attachment> Attachments { get; set; }
  }
}
