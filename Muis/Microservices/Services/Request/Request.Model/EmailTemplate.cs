using System;
using Core.Model;

namespace Request.Model
{
  public enum EmailTemplateType
  {
    RejectionEmail = 100
  }

  public class EmailTemplate : Email
  {
    public EmailTemplateType Type { get; set; }
  }
}
