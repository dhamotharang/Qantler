using Core.Model;
using System.Collections.Generic;

namespace Case.API.Params
{
  public class ImmediateSuspensionParam
  {
    public string Months { get; set; }

    public string Notes { get; set; }

    public IList<Attachment> Attachments { get; set; }
  }
}
