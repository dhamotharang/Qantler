using Core.Model;
using Finance.Model;
using System.Collections.Generic;

namespace Finance.API.Repository.Mappers
{
  public class TransactionCodeMapper
  {
    readonly Dictionary<long, TransactionCode> _cache = new Dictionary<long, TransactionCode>();

    readonly Dictionary<long, Log> _logCache = new Dictionary<long, Log>();

    readonly Dictionary<long, Price> _priceCache = new Dictionary<long, Price>();

    readonly Dictionary<long, Condition> _conditionCache = new Dictionary<long, Condition>();

    public TransactionCode Map(
      TransactionCode transactionCode,
      Condition condition = null,
      Log log = null,
      Price price = null)
    {
      if (!_cache.TryGetValue(transactionCode.ID, out TransactionCode result))
      {
        _cache[transactionCode.ID] = transactionCode;
        result = transactionCode;
      }

      if (   (price?.ID ?? 0) > 0
          && !_priceCache.ContainsKey(price.ID))
      {
        if (result.PriceHistory == null)
        {
          result.PriceHistory = new List<Price>();
        }
        result.PriceHistory.Add(price);
        _priceCache[price.ID] = price;
      }

      if (   (condition?.ID ?? 0) > 0
          && !_conditionCache.ContainsKey(condition.ID))
      {
        if (result.Conditions == null)
        {
          result.Conditions = new List<Condition>();
        }
        _conditionCache[condition.ID] = condition;
        result.Conditions.Add(condition);
      }

      if (   (log?.ID ?? 0) > 0
          && !_logCache.ContainsKey(log.ID))
      {
        if (result.Logs == null)
        {
          result.Logs = new List<Log>();
        }
        _logCache[log.ID] = log;
        result.Logs.Add(log);
      }

      return result;
    }
  }
}
