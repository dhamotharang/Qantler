using Core.Model;
using Request.Model;
using System;
using System.Collections.Generic;

namespace Request.API.Repository.Mappers
{
  public class SettingsMappers
  {
    readonly Dictionary<long, Settings> dict =
      new Dictionary<long, Settings>();

    readonly Dictionary<long, Log> _log =
      new Dictionary<long, Log>();

    public Settings Map(Settings settings, Log log)
    {
      if (!dict.TryGetValue(settings.ID, out Settings result))
      {
        result = settings;
        result.Logs = new List<Log>();
        dict[settings.ID] = result;
      }

      if (log.ID != 0)
      {
        if (!_log.ContainsKey(log.ID))
        {
          _log[log.ID] = log;
          result.Logs.Add(log);
        }
      }
      return result;
    }
  }
}