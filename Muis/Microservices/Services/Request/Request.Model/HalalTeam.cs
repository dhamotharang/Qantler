using System;

namespace Request.Model
{
  public class HalalTeam
  {
    public long ID { get; set; }

    public string AltID { get; set; }

    public string Name { get; set; }

    public string Designation { get; set; }

    public string Role { get; set; }

    public bool IsCertified { get; set; }

    public DateTimeOffset? JoinedOn { get; set; }

    public ChangeType ChangeType { get; set; }

    public long RequestID { get; set; }

    public DateTimeOffset? CreatedOn { get; set; }

    public DateTimeOffset? ModifiedOn { get; set; }

    public bool IsDeleted { get; set; }
  }
}
