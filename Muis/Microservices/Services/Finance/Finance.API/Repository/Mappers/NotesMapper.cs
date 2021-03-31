using Core.Model;
using Finance.Model;
using System.Collections.Generic;
using System.Linq;

namespace Finance.API.Repository.Mappers
{
  public class NotesMapper
  {
    readonly IDictionary<long, Note> _cache = new Dictionary<long, Note>();

    public Note Map(Note notes, Officer createdBy, Attachment attachment)
    {
      if (!_cache.TryGetValue(notes.ID, out Note result))
      {
        _cache[notes.ID] = notes;

        if (!string.IsNullOrEmpty(createdBy?.Name))
        {
          notes.Officer = createdBy;
        }

        result = notes;
      }

      if (attachment?.ID > 0
        && !(result.Attachments?.Any(e => e.ID == attachment.ID) ?? false))
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
