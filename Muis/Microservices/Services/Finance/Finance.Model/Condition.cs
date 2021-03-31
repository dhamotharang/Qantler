using System;
using Core.Model;

namespace Finance.Model
{
  public enum FieldType
  {
    Area
  }

  public enum Operator
  {
    Equal,
    LessThan,
    GreaterThan,
    LessThanOrEqual,
    GreaterThanOrEqual,
    NotEqual
  }

  public enum Logical
  {
    OR,
    AND,
    NOT
  }

  public class Condition
  {
    public long ID { get; set; }

    public int Index { get; set; }

    public string Value { get; set; }

    public string DataType { get; set; }

    public FieldType Field { get; set; }

    public Operator Operator { get; set; }

    public Logical Logical { get; set; }

    public long TransactionCodeID { get; set; }

    public DateTimeOffset? CreatedOn { get; set; }

    public DateTimeOffset? ModifiedOn { get; set; }

    public bool IsDeleted { get; set; }
  }
}
