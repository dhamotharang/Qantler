using System;

namespace eHS.Portal.Model
{
  public class File
  {
    public Guid ID { get; set; }

    public string FileName { get; set; }

    public string Directory { get; set; }

    public string Extension { get; set; }

    public long Size { get; set; }

    public DateTimeOffset? CreatedOn { get; set; }
  }
}
