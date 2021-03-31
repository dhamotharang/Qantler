using System;
using System.Collections.Generic;
using Case.Model;
using Core.Model;

namespace Case.API.Models
{
  public class AppealDecisionParam
  {
    public SanctionInfo SanctionInfo { get; set; }

    public string Notes { get; set; }

    public IList<Attachment> Attachments { get; set; }
  }
}
