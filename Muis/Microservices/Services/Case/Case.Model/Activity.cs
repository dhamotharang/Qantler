using System;
using System.Collections.Generic;
using Core.Model;

namespace Case.Model
{
  public enum ActivityType
  {
    Default,
    JobOrder,
    Payment,
    ShowCauseLetter,
    FOCLetter,
    SanctionLetter,
    AppealOutcomeLetter
  }

  public class Activity
  {
    public long ID { get; set; }

    public string RefID { get; set; }

    public ActivityType Type { get; set; }

    public string Action { get; set; }

    public string Notes { get; set; }

    public Guid UserID { get; set; }

    public DateTimeOffset? CreatedOn { get; set; }

    public bool IsDeleted { get; set; }

    #region Generated properties

    public string TypeText { get; set; }

    public Officer User { get; set; }

    public IList<Letter> Letters { get; set; }

    public IList<Attachment> Attachments { get; set; }

    #endregion
  }
}
