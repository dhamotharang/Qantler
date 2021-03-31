using System;
using System.Collections.Generic;

namespace Core.API.Smtp
{
  public class Mail
  {
    public string From { get; set; }

    public string[] Recipients { get; set; }

    public string[] Cc { get; set; }

    public string[] Bcc { get; set; }

    public string Subject { get; set; }

    public string Body { get; set; }

    public bool IsBodyHtml { get; set; }

    public IList<MailAttachment> Attachments { get; set; }
  }

  public enum MailAttachmentType
  {
    CID,
    FILE
  }

  public class MailAttachment
  {
    public MailAttachmentType Type { get; set; }

    public string ID { get; set; }

    public string Key { get; set; }

    public string Data { get; set; }

    public string Name { get; set; }
  }
}
