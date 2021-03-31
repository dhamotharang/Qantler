using Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JobOrder.Model;
using JobOrder.API.Services;
using Core.API.Smtp;
using JobOrder.API.Models;
using Core.Base;

namespace JobOrder.API.Helpers
{
  public static class EmailHelper
  {

    public static async Task<Email> GenerateAuditInspectionEmail(JobOrder.Model.JobOrder jobOrder,
      IEmailService emailService, Officer officer, EmailTemplateType templateType, 
      string recipient)
    {
      var template = await emailService.GetTemplate(templateType);

      var ScheduledOn = jobOrder.ScheduledOn.Value.AddHours(8);
      var ScheduledOnTo = jobOrder.ScheduledOnTo.Value.AddHours(8);

      var email = new Email
      {
        From = template.From,
        To = recipient,
        Cc = template.Cc.Replace("{officerEmail}", officer?.Email ?? ""),
        Title = template.Title.Replace("{customer}", jobOrder.Customer.Name),
        Body = template.Body.Replace("{customer}", jobOrder.Customer.Name)
          .Replace("{premise}", PremiseUtil.format(jobOrder.Premises[0]))
          .Replace("{scheduledDate}", ScheduledOn.ToString("dd MMM yyyy"))
          .Replace("{startTime}", ScheduledOn.ToString("hh:mm tt"))
          .Replace("{endTime}", ScheduledOnTo.ToString("hh:mm tt"))
          .Replace("{officerName}", officer?.Name ?? ""),
        IsBodyHtml = template.IsBodyHtml
      };

      return email;
    }
  }
}
