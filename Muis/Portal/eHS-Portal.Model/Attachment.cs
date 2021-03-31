using System;

namespace eHS.Portal.Model
{
  public class Attachment
  {
    public long ID { get; set; }

    public Guid FileID { get; set; }

    public string FileName { get; set; }

    public string Extension { get; set; }

    public long Size { get; set; }

    public DateTimeOffset? CreatedOn { get; set; }

    public DateTimeOffset? ModifiedOn { get; set; }

    public bool IsDeleted { get; set; }
  }
}
