using Request.Model;
using System;
using System.Collections.Generic;
using Core.Model;

namespace Request.API.Repository.Mappers
{
  public class RFAMapper
  {
    readonly Dictionary<long, RFA> _rfaCache = new Dictionary<long, RFA>();
    readonly Dictionary<long, Log> _rfaLogCache = new Dictionary<long, Log>();
    readonly Dictionary<long, RFALineItem> _liCache = new Dictionary<long, RFALineItem>();
    readonly Dictionary<long, Attachment> _liAttCache = new Dictionary<long, Attachment>();
    readonly Dictionary<long, RFAReply> _repCache = new Dictionary<long, RFAReply>();
    readonly Dictionary<long, Attachment> _repAttCache = new Dictionary<long, Attachment>();

    public RFA Map(RFA rfa,
      RFALineItem li = null,
      Attachment liAtt = null,
      RFAReply rep = null,
      Attachment repAtt = null,
      Log rfaLog = null,
      Model.Request request = null)
    {
      if (!_rfaCache.TryGetValue(rfa.ID, out RFA result))
      {
        if ((request?.ID ?? 0) > 0)
        {
          rfa.Request = request;
        }

        _rfaCache[rfa.ID] = rfa;
        result = rfa;
      }

      if (   (rfaLog?.ID ?? 0) != 0
          && !_rfaLogCache.ContainsKey(rfaLog.ID))
      {
        _rfaLogCache[rfaLog.ID] = rfaLog;

        if (result.Logs == null)
        {
          result.Logs = new List<Log>();
        }
        result.Logs.Add(rfaLog);
      }

      if ((li?.ID ?? 0) != 0)
      {
        if (!_liCache.TryGetValue(li.ID, out RFALineItem liOut))
        {
          _liCache[li.ID] = li;
          liOut = li;

          if (result.LineItems == null)
          {
            result.LineItems = new List<RFALineItem>();
          }
          result.LineItems.Add(li);
        }

        if (    (liAtt?.ID ?? 0) != 0
            && !_liAttCache.ContainsKey(liAtt.ID))
        {
          if (liOut.Attachments == null)
          {
            liOut.Attachments = new List<Attachment>();
          }
          liOut.Attachments.Add(liAtt);
          _liAttCache[liAtt.ID] = liAtt;
        }

        RFAReply repOut = null;
        if (   (rep?.ID ?? 0) != 0
            && !_repCache.TryGetValue(rep.ID, out repOut))
        {
          rep.Attachments = new List<Attachment>();
          _repCache[rep.ID] = rep;
          repOut = rep;

          if (liOut.Replies == null)
          {
            liOut.Replies = new List<RFAReply>();
          }
          liOut.Replies.Add(rep);
        }

        if (   (repAtt?.ID ?? 0) != 0
            && !_repAttCache.ContainsKey(repAtt.ID))
        {
          repOut.Attachments.Add(repAtt);
          _repAttCache[repAtt.ID] = repAtt;
        }
      }

      return result;
    }
  }
}
