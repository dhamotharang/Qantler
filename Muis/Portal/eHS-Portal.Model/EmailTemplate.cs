using System;

namespace eHS.Portal.Model
{
  public enum EmailTemplateType
  {
    RejectionEmail = 100,
    NewAccount = 200,
    ResetPassword = 201,
    AuditInspection = 300,
    RescheduleAuditInspection=301,
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
