using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Finance.API.Repository;
using Finance.Model;

namespace Finance.API.Strategies.Billing
{
  public class BillingContext
  {
    readonly IDictionary<string, TransactionCode> _cache =
      new Dictionary<string, TransactionCode>();

    readonly IDictionary<SettingsType, Settings> _settingsCache =
      new Dictionary<SettingsType, Settings>();

    readonly DbContext _dbContext;

    public BillingContext(DbContext dbContext)
    {
      _dbContext = dbContext;
    }

    public async Task<decimal> GST()
    {
      return decimal.Parse((await GetSettings(SettingsType.GST)).Value);
    }

    public async Task<decimal> Stage1Factor()
    {
      return decimal.Parse((await GetSettings(SettingsType.Stage1Percentage)).Value);
    }

    public async Task<Settings> GetSettings(SettingsType type)
    {
      if (!_settingsCache.TryGetValue(type, out Settings settings))
      {
        settings = await _dbContext.Settings.GetSettingsByType(type);
        _settingsCache[type] = settings;
      }
      return settings;
    }

    public async Task<TransactionCode> GetTransactionCode(string code, DateTimeOffset? refDate)
    {
      if (!_cache.TryGetValue(code, out TransactionCode result))
      {
        result = await _dbContext.Transactioncode.GetTransactionCodeByCode(code, refDate);
        _cache[code] = result;
      }
      return result;
    }

    public async Task<IList<TransactionCode>> GetTransactionCodes(DateTimeOffset? refDate,
      params string[] codes)
    {
      var result = new List<TransactionCode>();

      foreach (var code in codes)
      {
        result.Add(await GetTransactionCode(code, refDate));
      }

      return result;
    }
  }
}
