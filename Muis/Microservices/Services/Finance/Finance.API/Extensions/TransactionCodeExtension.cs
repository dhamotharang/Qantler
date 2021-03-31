using System;
using System.Linq;
using Finance.Model;

namespace Finance.API.Extensions
{
  public static class TransactionCodeExtension
  {
    public static decimal GetLatestPriceAmount(this TransactionCode self,
      DateTimeOffset? refDate = null)
    {
      refDate ??= DateTimeOffset.UtcNow;

      return self.PriceHistory?.Where(e => e.EffectiveFrom <= refDate)
        .OrderByDescending(e => e.EffectiveFrom)
        .FirstOrDefault()
        .Amount ?? 0;
    }
  }
}
