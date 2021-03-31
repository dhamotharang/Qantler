using System;

namespace Request.Model
{
  public class Comment
  {
    public long ID { get; set; }

    public string Text { get; set; }

    public Guid UserID { get; set; }

    public string UserName { get; set; }

    public DateTimeOffset? CreatedOn { get; set; }
  }
}
