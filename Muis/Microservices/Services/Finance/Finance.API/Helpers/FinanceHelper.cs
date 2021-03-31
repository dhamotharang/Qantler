using System;
using System.Threading.Tasks;
using Finance.API.Repository;

namespace Finance.API.Helpers
{
  public static class FinanceHelper
  {
    public static async Task<decimal> GetGST(DbContext dbContext)
    {
      return decimal.Parse(
        (await dbContext.Settings.GetSettingsByType(Model.SettingsType.GST)).Value);
    }
  }
}
