using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Core.API.Smtp
{
  public class SmtpProvider : ISmtpProvider
  {
    readonly SmtpConfig _config;
    readonly ILogger<SmtpProvider> _logger;

    public SmtpProvider(IOptions<SmtpConfig> settings,
      ILogger<SmtpProvider> logger)
    {
      _config = settings.Value;
      _logger = logger;
    }

    public void Send(Mail mail)
    {
      SmtpClient client = new SmtpClient(_config.Host, _config.Port);
      client.UseDefaultCredentials = _config.UseDefaultCredentials;
      client.EnableSsl = _config.EnableSsl;

      if (!string.IsNullOrEmpty(_config.Username)
          && !string.IsNullOrEmpty(_config.Password))
      {
        client.Credentials = new NetworkCredential(_config.Username, _config.Password);
      }

      MailMessage mailMessage = new MailMessage();
      mailMessage.From = new MailAddress(mail.From);

      foreach (var to in mail.Recipients)
      {
        if (!string.IsNullOrEmpty(to?.Trim()))
        {
          mailMessage.To.Add(to.Trim());
        }
      }

      if (mail.Cc != null)
      {
        foreach (var cc in mail.Cc)
        {
          if (!string.IsNullOrEmpty(cc?.Trim()))
          {
            mailMessage.CC.Add(cc.Trim());
          }
        }
      }

      if (mail.Bcc != null)
      {
        foreach (var bcc in mail.Bcc)
        {
          if (!string.IsNullOrEmpty(bcc?.Trim()))
          {
            mailMessage.Bcc.Add(bcc.Trim());
          }
        }
      }

      if (mail.Attachments?.Any() ?? false)
      {
        foreach (var attachment in mail.Attachments)
        {
          if (attachment.Type == MailAttachmentType.CID)
          {
            var id = Guid.NewGuid().ToString();

            var inline = new Attachment(new MemoryStream(Convert.FromBase64String(attachment.Data)),
              MediaTypeNames.Image.Jpeg)
            {
              ContentId = id
            };

            mailMessage.Attachments.Add(inline);

            mail.Body = mail.Body.Replace(attachment.Key,
              $"cid:{id}",
              StringComparison.InvariantCultureIgnoreCase);
          }
          else if (attachment.Type == MailAttachmentType.FILE)
          {
            var mailAttachment = new Attachment(new MemoryStream(Convert.FromBase64String(attachment.Data)),
              attachment.Name, "application/pdf");
            mailMessage.Attachments.Add(mailAttachment);
          }
        }
      }

      mailMessage.Body = mail.Body;
      mailMessage.IsBodyHtml = mail.IsBodyHtml;
      mailMessage.Subject = mail.Subject;

      try
      {
        client.Send(mailMessage);
      }
      catch (Exception e)
      {
        Console.WriteLine($"Exception: {e.StackTrace}");
        _logger.LogError(e.Message);
        _logger.LogError(e.StackTrace);
      }
    }
  }
}
