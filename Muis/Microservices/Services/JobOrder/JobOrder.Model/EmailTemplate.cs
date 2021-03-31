using Core.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobOrder.Model
{
  public enum EmailTemplateType
  {
    AuditInspection = 300,
    RescheduleAuditInspection = 301
  }
  public class EmailTemplate : Email
  {
    public EmailTemplateType Type { get; set; }
  }
}
