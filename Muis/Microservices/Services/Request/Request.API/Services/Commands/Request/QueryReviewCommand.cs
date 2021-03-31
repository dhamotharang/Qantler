using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.API;
using Core.API.Repository;
using Request.API.Repository;
using Request.Model;

namespace Request.API.Services.Commands.Request
{
  public class QueryReviewCommand : IUnitOfWorkCommand<IList<Review>>
  {
    readonly long[] _requestIDs;

    public QueryReviewCommand(long[] requestIDs)
    {
      _requestIDs = requestIDs;
    }

    public async Task<IList<Review>> Invoke(IUnitOfWork unitOfWork)
    {
      return await DbContext.From(unitOfWork).Request.QueryReview(new ReviewFilter
      {
        RequestIDs = _requestIDs
      });
    }
  }
}
