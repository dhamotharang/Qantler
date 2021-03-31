using System;
using System.Collections.Generic;

namespace eHS.Portal.Model
{
  public class Email
  {
    public long ID { get; set; }

    public string From { get; set; }

    public string To { get; set; }

    public string Cc { get; set; }

    public string Bcc { get; set; }

    public string Title { get; set; }

    public string Body { get; set; }

    public bool IsBodyHtml { get; set; }

    public string Keyword { get; set; }

    public DateTimeOffset? CreatedOn { get; set; }

    public DateTimeOffset? ModifiedOn { get; set; }

    public bool IsDeleted { get; set; }

    #region Generated properties

    public IList<EmailAttachment> Attachments { get; set; }

    #endregion
  }

  public class EmailAttachment
  {
    public string Key { get; set; }

    public string Data { get; set; }

    public string Name { get; set; }
  }
}
