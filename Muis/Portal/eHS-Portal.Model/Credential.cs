using System;

namespace eHS.Portal.Model
{
  public class Credential
  {
    public long ID { get; set; }

    public Provider Provider { get; set; }

    public string Key { get; set; }

    public string Secret { get; set; }

    public DateTimeOffset? ExpiresOn { get; set; }

    public Guid IdentityID { get; set; }

    public DateTimeOffset CreatedOn { get; set; }

    public DateTimeOffset ModifiedOn { get; set; }

    public bool IsDeleted { get; set; }
  }
}
