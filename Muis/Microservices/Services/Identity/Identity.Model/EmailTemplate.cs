using System;
using Core.Model;

namespace Identity.Model
{
  public enum EmailTemplateType
  {
    NewAccount = 200,
    ResetPassword = 201
  }

  public class EmailTemplate : Email
  {
    public EmailTemplateType Type { get; set; }
  }
}
