using Core.Model;
using Request.Model;
using System.Collections.Generic;

namespace Request.API.Repository.Mappers
{
  public class ReviewMapper
  {
    readonly Dictionary<long, Review> _reviewCache =
      new Dictionary<long, Review>();

    readonly Dictionary<long, ReviewLineItem> _liCache =
      new Dictionary<long, ReviewLineItem>();
    
    public Review Map(Review review,
      ReviewLineItem lineitem,
      Email email = null)
    {
      if (!_reviewCache.TryGetValue(review.ID, out Review result))
      {
        _reviewCache[review.ID] = review;
        result = review;
      }

      if (    lineitem.ID != 0
          && !_liCache.ContainsKey(lineitem.ID))
      {
        if (result.LineItems == null)
        {
          review.LineItems = new List<ReviewLineItem>();
        }

        result.LineItems.Add(lineitem);
        _liCache[lineitem.ID] = lineitem;
      }

      if ((email?.ID ?? 0L) != 0L)
      {
        result.Email = email;
      }

      return result;
    }
  }
}
