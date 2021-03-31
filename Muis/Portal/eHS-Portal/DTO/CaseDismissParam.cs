using eHS.Portal.Model;
using System.Collections.Generic;

namespace eHS.Portal.DTO
{
  public class CaseDismissParam
  {
    public string Notes { get; set; }

    public IList<Attachment> Attachments { get; set; }
  }
}
