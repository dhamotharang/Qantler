using System;
using System.Collections.Generic;
using eHS.Portal.Model;

namespace eHS.Portal.DTO
{
  public class AppealDecisionParam
  {
    public SanctionInfo SanctionInfo { get; set; }

    public string Notes { get; set; }

    public IList<Attachment> Attachments { get; set; }
  }
}
