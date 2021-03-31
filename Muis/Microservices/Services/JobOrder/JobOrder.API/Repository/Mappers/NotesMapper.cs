using Core.Model;
using JobOrder.Model;
using System.Collections.Generic;

namespace JobOrder.API.Repository.Mappers
{
  public class NotesMapper
  {
    readonly IDictionary<long, Notes> _cache = new Dictionary<long, Notes>();

    public Notes Map ( Notes notes, Officer createdBy, Attachment attachment)
    {
      if (!_cache.TryGetValue(notes.ID, out Notes result))
      {
        _cache[notes.ID] = notes;

        if (!string.IsNullOrEmpty(createdBy?.Name))
        {
          notes.Officer = createdBy;
        }

        result = notes;
      }

      if (attachment?.ID > 0)
      {
        if (result.Attachments == null)
        {
          result.Attachments = new List<Attachment>();
        }

        result.Attachments.Add(attachment);
      }

      return result;
    }
  }
}
