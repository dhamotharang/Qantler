using Core.Model;
using JobOrder.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JobOrder.API.Repository.Mappers
{
  public class FindingsMapper
  {
    readonly Dictionary<long, Findings> _findingsCache =
      new Dictionary<long, Findings>();

    readonly Dictionary<long, FindingsLineItem> _findingslineitemCache =
      new Dictionary<long, FindingsLineItem>();

    readonly Dictionary<long, Attachment> _attachmentCache =
      new Dictionary<long, Attachment>();

    readonly Dictionary<Guid, Officer> _officerCache =
      new Dictionary<Guid, Officer>();

    readonly Dictionary<long, Attachment> _findingsattachmentCache =
      new Dictionary<long, Attachment>();
    public Findings Map(
      Findings findings,
      Attachment findingsattachment,
      FindingsLineItem findingsLineItem,
      Attachment attachment,
      Officer officer)
    {

      if (!_findingsCache.TryGetValue(findings.ID,
        out Findings result))
      {
        _findingsCache[findings.ID] = findings;
        result = findings;
      }

      if (officer.ID != Guid.Empty
        && !_officerCache.ContainsKey(officer.ID))
      {
        findings.Officer = new Officer();
        _officerCache[officer.ID] = officer;
        findings.Officer = officer;

      }

      if (findingsattachment.ID != 0
        && !_findingsattachmentCache.ContainsKey(findingsattachment.ID))
      {
        findings.Attachment = new Attachment();
        _findingsattachmentCache[findingsattachment.ID] = findingsattachment;
        findings.Attachment = findingsattachment;

      }

      if (findingsLineItem.ID != 0)
      {
        if (!_findingslineitemCache.TryGetValue(
          findingsLineItem.ID,
          out FindingsLineItem liOut))
        {
          findings.LineItems = new List<FindingsLineItem>();
          findingsLineItem.Attachments = new List<Attachment>();
          _findingslineitemCache[findingsLineItem.ID] = findingsLineItem;
          liOut = findingsLineItem;
          findings.LineItems.Add(findingsLineItem);
        }

        if (attachment.ID != 0
            && !_attachmentCache.ContainsKey(attachment.ID))
        {
          liOut.Attachments.Add(attachment);
          _attachmentCache[attachment.ID] = attachment;
        }
      }

      return result;
    }
  }
}
