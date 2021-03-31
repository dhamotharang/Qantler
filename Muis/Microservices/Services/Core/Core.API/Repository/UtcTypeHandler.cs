using System;
using System.Data;
using Dapper;

namespace Core.API.Repository
{
  public class UtcTypeHandler : SqlMapper.TypeHandler<DateTimeOffset>
  {
    public override void SetValue(IDbDataParameter parameter, DateTimeOffset value)
    {
      parameter.Value = value;
    }

    public override DateTimeOffset Parse(object value)
    {
      return DateTime.SpecifyKind((DateTime)value, DateTimeKind.Utc);
    }
  }
}
