using System;

namespace Notification.Model
{
  public class Notification
  {
    public long ID { get; set; }

    public string Title { get; set; }

    public string Preview { get; set; }

    public string Body { get; set; }

    public string Module { get; set; }

    public string RefID { get; set; }

    public Category Category { get; set; }

    public Level Level { get; set; }

    public ContentType ContentType { get; set; }

    public Guid? UserID { get; set; }

    public State State { get; set; }

    public DateTimeOffset? CreatedOn { get; set; }

    public DateTimeOffset? ModifiedOn { get; set; }
  }
}
