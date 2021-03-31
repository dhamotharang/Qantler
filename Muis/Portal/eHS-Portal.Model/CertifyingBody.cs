using System;
namespace eHS.Portal.Model
{
  public enum CertifyingBodyStatus
  {
    Unrecognized,
    Recognized
  }

  public class CertifyingBody
  {
    public long ID { get; set; }

    public string Name { get; set; }

    public CertifyingBodyStatus Status { get; set; }

    public DateTimeOffset? CreatedOn { get; set; }

    public DateTimeOffset? ModifiedOn { get; set; }

    public bool IsDeleted { get; set; }
  }
}
