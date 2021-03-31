using System;
using System.Linq;
using Finance.Model;

namespace Finance.API.Helpers
{
  public static class ConditionEvaluator
  {
    public static bool Evaluate(dynamic val, params Condition[] conditions)
    {
      if (!(conditions?.Any() ?? false))
      {
        return false;
      }

      var result = conditions.Where(e => Operator(val, e)).ToList();

      return result.Count() == conditions.Length;
    }

    static bool Operator(dynamic a, Condition condition)
    {
      var b = ConvertTo(a, condition.Value);

      switch(condition.Operator)
      {
        case Model.Operator.Equal:
          return a == b;
        case Model.Operator.GreaterThan:
          return a > b;
        case Model.Operator.GreaterThanOrEqual:
          return a >= b;
        case Model.Operator.LessThan:
          return a < b;
        case Model.Operator.LessThanOrEqual:
          return a <= b;
        case Model.Operator.NotEqual:
          return a != b;
      }

      return false;
    }

    static dynamic ConvertTo(dynamic @ref, string val)
    {
      return Convert.ChangeType(val, ((object)@ref).GetType());
    }
  }
}
