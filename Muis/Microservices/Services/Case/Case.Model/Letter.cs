using System;
using Core.Model;

namespace Case.Model
{
  public enum LetterType
  {
    ShowCause = 400,
    FOC = 401,
    Warning = 402,
    Compound = 403,
    Suspension = 404,
    ImmediateSuspension = 405,
    Revocation = 406,
    ApprovedAppeal = 407,
    RejectedAppeal = 408
  }

  public enum LetterStatus
  {
    Draft = 100,
    Final = 200
  }

  public class Letter
  {
    public long ID { get; set; }

    public LetterType Type { get; set; }

    public string Body { get; set; }

    public string Keywords { get; set; }

    public long? EmailID { get; set; }

    public LetterStatus Status { get; set; }

    public DateTimeOffset? CreatedOn { get; set; }

    public DateTimeOffset? ModifiedOn { get; set; }

    public bool IsDeleted { get; set; }

    #region Generated properties

    public string TypeText { get; set; }

    public Email Email { get; set; }

    #endregion
  }
}
