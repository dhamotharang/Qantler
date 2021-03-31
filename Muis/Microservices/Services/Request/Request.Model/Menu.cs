using System;
using Core.Model;

namespace Request.Model
{
  public class Menu
  {
    public long ID { get; set; }

    public int Group { get; set; }

    public int Index { get; set; }

    public Scheme? Scheme { get; set; }

    public string Text { get; set; }

    public string SubText { get; set; }

    public bool? Approved { get; set; }

    public string Remarks { get; set; }

    public Guid? ReviewedBy { get; set; }

    public string ReviewedByName { get; set; }

    public DateTimeOffset? ReviewedOn { get; set; }

    public ChangeType ChangeType { get; set; }

    public int LineCount { get; set; }

    public long RequestID { get; set; }

    public DateTimeOffset? CreatedOn { get; set; }

    public DateTimeOffset? ModifiedOn { get; set; }

    public bool IsDeleted { get; set; }

    public bool Undeclared { get; set; }

    #region Generated properties

    public string SchemeText { get; set; }

    #endregion
  }
} 
