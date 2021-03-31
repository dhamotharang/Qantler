using System;
using System.Collections.Generic;
using Request.Model;

namespace Identity.API.Repository.Mappers
{
  public class ChecklistMapper
  {
    readonly IDictionary<long, ChecklistHistory> _historyCache =
      new Dictionary<long, ChecklistHistory>();
    readonly IDictionary<long, ChecklistCategory> _categoryCache =
      new Dictionary<long, ChecklistCategory>();

    public ChecklistHistory Map(ChecklistHistory history,
      ChecklistCategory category,
      ChecklistItem item)
    {
      if (!_historyCache.TryGetValue(history.ID, out ChecklistHistory result))
      {
        history.Categories = new List<ChecklistCategory>();

        _historyCache[history.ID] = history;
        result = history;
      }

      if ((category?.ID ?? 0L) != 0)
      {
        if (!_categoryCache.TryGetValue(category.ID, out ChecklistCategory categoryCache))
        {
          category.Items = new List<ChecklistItem>();
          _categoryCache[category.ID] = category;
          categoryCache = category;

          result.Categories.Add(category);
        }

        if ((item?.ID ?? 0L) != 0L)
        {
          categoryCache.Items.Add(item);
        }
      }

      return result;
    }
  }
}
