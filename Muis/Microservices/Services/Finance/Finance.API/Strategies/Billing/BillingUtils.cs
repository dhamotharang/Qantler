using System;
using Finance.API.Extensions;
using Finance.Model;

namespace Finance.API.Strategies.Billing
{
  public class BillingUtils
  {
    public static int CalculateNoOfMonths(DateTime from, DateTime to)
    {
      var yearsDiff = to.Year - from.Year;
      var monthsDiff = to.Month - from.Month;

      return (yearsDiff * 12) + monthsDiff + 1;
    }

    public static (int, string) LineItemSection(BillType type)
    {
      switch(type)
      {
        case BillType.Stage1:
          return (1, "Stage 1");
        case BillType.Stage2:
          return (2, "Stage 2");
      }

      return (0, "");
    }

    /// <summary>
    /// Calculates the amount base on given percentage. The ration determines a factor of the total
    /// amount to be paid based on amount given. Ratio is mostly used for pro-rated fees.
    /// </summary>
    public static decimal CalculateAmount(TransactionCode code, decimal percentage,
      decimal ratio = 1M)
    {
      var amount = code.GetLatestPriceAmount();
      return Math.Min(percentage * amount, ratio * amount);
    }
  }
}
