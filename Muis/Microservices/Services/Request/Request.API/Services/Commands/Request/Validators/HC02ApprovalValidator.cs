using Core.API.Exceptions;
using Core.Base;
using Request.API.Params;
using Request.API.Repository;
using Request.Model;
using System.Linq;
using System.Threading.Tasks;

namespace Request.API.Services.Commands.Request.Validators
{
  public class HC02ApprovalValidator : Validator
  {
    readonly Review _review;
    readonly DbContext _dbContext;

    public HC02ApprovalValidator(Review review, DbContext dbContext)
    {
      _review = review;
      _dbContext = dbContext;
    }
    public override async Task Validate()
    {
      var request = await _dbContext.Request.GetRequestByID(_review.RequestID);

      var premise = request.Premises.Where(e => e.IsPrimary).FirstOrDefault();

      var hasPendingRequest = (await _dbContext.Request.Select(new RequestOptions
      {
        PremiseID = premise.ID,
        Status = new RequestStatus[]
        {
          RequestStatus.Draft,
          RequestStatus.Open,
          RequestStatus.PendingReviewApproval,
          RequestStatus.ForInspection,
          RequestStatus.PendingApproval
        },
        Type = new RequestType[]
        {
          RequestType.HC03,
          RequestType.HC04
        }
      })).Any();

      if (hasPendingRequest)
      {
        throw new BadRequestException(string.Format(
         await _dbContext.Transalation.GetTranslation(0, "RequestApproveBlockPendingRequest"),
         $"{_review.RequestID}"));
      }
    }
  }
}
