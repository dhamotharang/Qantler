using Core.Model;
using System.Collections.Generic;

namespace Case.API.Params
{
  public class CaseClose
  {
    public string Notes { get; set; }

    public IList<Attachment> Attachments { get; set; }
  }
}
