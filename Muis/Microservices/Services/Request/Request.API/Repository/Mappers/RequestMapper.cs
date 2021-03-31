using System;
using System.Collections.Generic;
using Request.Model;
using Core.Model;
using System.Linq;

namespace Request.API.Repository.Mappers
{
  public class RequestMapper
  {
    readonly Dictionary<long, Model.Request> _cache = new Dictionary<long, Model.Request>();

    readonly Dictionary<long, RequestLineItem> _lineItemCache =
      new Dictionary<long, RequestLineItem>();

    readonly Dictionary<long, Characteristic> _lineItemCharCache =
      new Dictionary<long, Characteristic>();

    readonly Dictionary<long, HalalTeam> _halalCache =
      new Dictionary<long, HalalTeam>();

    readonly Dictionary<long, Attachment> _attachmentCache =
      new Dictionary<long, Attachment>();

    readonly Dictionary<long, Log> _logCache = new Dictionary<long, Log>();

    readonly Dictionary<long, RFA> _rfaCache = new Dictionary<long, RFA>();

    readonly Dictionary<long, Characteristic> _characteristicsCache =
      new Dictionary<long, Characteristic>();

    readonly Dictionary<long, Review> _reviewCache =
      new Dictionary<long, Review>();

    readonly Dictionary<long, ReviewLineItem> _reviewLineItemCache =
      new Dictionary<long, ReviewLineItem>();

    public Model.Request Map(Model.Request request,
      Code customerCode = null,
      RequestLineItem lineItem = null,
      Premise premise = null,
      RFA rfa = null,
      Characteristic lineItemChar = null)
    {
      return InternalMap(request,
        customerCode,
        lineItem: lineItem,
        premise: premise,
        rfa: rfa,
        lineItemChar: lineItemChar);
    }

    public Model.Request Map(Model.Request request,
      Code customerCode,
      Code groupCode,
      HalalTeam halal,
      Premise premise,
      Characteristic characteristic,
      RFA rfa,
      Log log,
      Attachment attachment,
      RequestLineItem lineItem,
      Review review,
      ReviewLineItem reviewLineItem,
      Characteristic lineItemChar)
    {
      return InternalMap(request,
        customerCode,
        groupCode,
        lineItem,
        lineItemChar,
        halal,
        premise,
        characteristic,
        attachment,
        rfa,
        log,
        review,
        reviewLineItem);
    }

    public Model.Request Map(Model.Request request,
      RequestLineItem lineItem = null,
      Premise premise = null)
    {
      return InternalMap(request,
        lineItem: lineItem,
        premise: premise
       );
    }

    Model.Request InternalMap(Model.Request request,
      Code customerCode = null,
      Code groupCode = null,
      RequestLineItem lineItem = null,
      Characteristic lineItemChar = null,
      HalalTeam halalTeam = null,
      Premise premise = null,
      Characteristic characteristic = null,
      Attachment attachment = null,
      RFA rfa = null,
      Log log = null,
      Review review = null,
      ReviewLineItem reviewLineItem = null)
    {
      if (!_cache.TryGetValue(request.ID, out Model.Request result))
      {
        _cache[request.ID] = request;
        result = request;
      }

      if ((customerCode?.ID ?? 0) > 0)
      {
        result.CustomerCode = customerCode;
      }

      if ((groupCode?.ID ?? 0) > 0)
      {
        result.GroupCode = groupCode;
      }

      RequestLineItem outLineItem = null;
      if ((lineItem?.ID ?? 0L) != 0L
          && !_lineItemCache.TryGetValue(lineItem.ID, out outLineItem))
      {
        if (result.LineItems == null)
        {
          result.LineItems = new List<RequestLineItem>();
        }
        result.LineItems.Add(lineItem);

        _lineItemCache[lineItem.ID] = lineItem;
        outLineItem = lineItem;
      }

      if (outLineItem != null
          && (lineItemChar?.ID ?? 0L) != 0L
          && !_lineItemCharCache.ContainsKey(lineItemChar.ID))
      {
        if (outLineItem.Characteristics == null)
        {
          outLineItem.Characteristics = new List<Characteristic>();
        }

        _lineItemCharCache[lineItemChar.ID] = lineItemChar;
        outLineItem.Characteristics.Add(lineItemChar);
      }

      if ((halalTeam?.ID ?? 0L) != 0L
         && !_halalCache.ContainsKey(halalTeam.ID))
      {
        _halalCache[halalTeam.ID] = halalTeam;

        if (result.Teams == null)
        {
          result.Teams = new List<HalalTeam>();
        }
        result.Teams.Add(halalTeam);
      }

      if ((premise?.ID ?? 0L) != 0L
          && result.Premises?.FirstOrDefault(e => e.ID == premise.ID) == null)
      {
        if (result.Premises == null)
        {
          result.Premises = new List<Premise>();
        }

        result.Premises.Add(premise);
      }

      if ((characteristic?.ID ?? 0L) != 0L
          && !_characteristicsCache.ContainsKey(characteristic.ID))
      {
        _characteristicsCache[characteristic.ID] = characteristic;

        if (result.Characteristics == null)
        {
          result.Characteristics = new List<Characteristic>();
        }
        result.Characteristics.Add(characteristic);
      }

      if ((attachment?.ID ?? 0L) != 0L
          && !_attachmentCache.ContainsKey(attachment.ID))
      {
        _attachmentCache[attachment.ID] = attachment;

        if (result.Attachments == null)
        {
          result.Attachments = new List<Attachment>();
        }
        result.Attachments.Add(attachment);
      }

      if ((rfa?.ID ?? 0L) != 0L
          && !_rfaCache.ContainsKey(rfa.ID))
      {
        _rfaCache[rfa.ID] = rfa;

        if (result.RFAs == null)
        {
          result.RFAs = new List<RFA>();
        }
        result.RFAs.Add(rfa);
      }

      if ((log?.ID ?? 0L) != 0L
          && !_logCache.ContainsKey(log.ID))
      {
        _logCache[log.ID] = log;

        if (result.Logs == null)
        {
          result.Logs = new List<Log>();
        }
        result.Logs.Add(log);
      }


      Review outReview = null;
      if ((review?.ID ?? 0L) != 0L
          && !_reviewCache.TryGetValue(review.ID, out outReview))
      {
        if (result.Reviews == null)
        {
          result.Reviews = new List<Review>();
        }
        result.Reviews.Add(review);

        _reviewCache[review.ID] = review;
        outReview = review;
      }

      if (outReview != null
          && (reviewLineItem?.ID ?? 0L) != 0L
          && !_reviewLineItemCache.ContainsKey(reviewLineItem.ID))
      {
        _reviewLineItemCache[reviewLineItem.ID] = reviewLineItem;

        if (outReview.LineItems == null)
        {
          outReview.LineItems = new List<ReviewLineItem>();
        }
        outReview.LineItems.Add(reviewLineItem);
      }

      return result;
    }
  }
}
