using JobOrder.Model;
using System;
using System.Collections.Generic;

namespace JobOrder.API.Repository.Mappers
{
  public class MasterMapper
  {
    readonly IDictionary<Guid, Master> _masterCache =
      new Dictionary<Guid, Master>();

    public Master Map(Master history)
    {
      if (!_masterCache.TryGetValue(history.ID, out Master result))
      {
        _masterCache[history.ID] = history;
        result = history;
      }
      return result;
    }
  }
}