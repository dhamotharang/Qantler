using Core.API;
using Core.API.Exceptions;
using Core.API.Repository;
using Core.Model;
using Request.API.Repository;
using Request.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Request.API.Services.Commands.Request
{
  public class RequestRecommendValidator
  {
    readonly IList<Review> _reviews;

    readonly DbContext _dbContext;

    public RequestRecommendValidator(IList<Review> reviews, DbContext dbContext)
    {
      _reviews = reviews;

      _dbContext = dbContext;
    }

    public async Task Invoke()
    {
      foreach (var review in _reviews)
      {
        var hasPendingRFA = (await _dbContext.RFA.Query(new RFAFilter
        {
          RequestID = review.RequestID,
          Status = new List<RFAStatus> { RFAStatus.Open, RFAStatus.PendingReview }
        })).Any();

        if (hasPendingRFA)
        {
          throw new BadRequestException
            (await _dbContext.Transalation.GetTranslation(Locale.EN, "RequestRecommendRFABlock"));
        }
      }
    }
  }
}
