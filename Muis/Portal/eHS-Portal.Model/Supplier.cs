using System;

namespace eHS.Portal.Model
{
  public class Supplier
  {
    public long ID { get; set; }

    public string Name { get; set; }

    public DateTimeOffset? CreatedOn { get; set; }

    public DateTimeOffset? ModifiedOn { get; set; }

    public bool IsDeleted { get; set; }
  }
}
