using System;
using Core.Model;

namespace Case.Model
{
  public enum EmailTemplateType
  {
    ShowCause = 400,
    Warning = 401,
    Compound = 402,
    Suspension = 403,
    ImmediateSuspension = 404,
    Revocation = 405
  }

  public class EmailTemplate : Email
  {
    public EmailTemplateType Type { get; set; }
  }
}
