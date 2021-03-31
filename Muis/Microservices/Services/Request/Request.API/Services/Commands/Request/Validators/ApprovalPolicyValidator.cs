using Core.API.Exceptions;
using Core.Base;
using Request.API.Repository;
using Request.Model;
using System.Linq;
using System.Threading.Tasks;

namespace Request.API.Services.Commands.Request.Validators
{
  public class ApprovalPolicyValidator : Validator
  {
    readonly Review _review;
    readonly DbContext _dbContext;

    public ApprovalPolicyValidator(Review review, DbContext dbContext)
    {
      _review = review;
      _dbContext = dbContext;
    }

    public override async Task Validate()
    {
      var actionHistories = (await _dbContext.Request.GetRequestActionHistories(_review.RequestID))
          .OrderByDescending(e => e.CreatedOn);

      var recommendAction = actionHistories.Where(e => e.Action == RequestActionType.Review)
        .FirstOrDefault();

      if (recommendAction?.OfficerID == _review.ReviewerID)
      {
        throw new BadRequestException(string.Format(
          await _dbContext.Transalation.GetTranslation(0, "RequestApproveBlockRecommend"),
          $"{_review.RequestID}"));
      }

      var reassignAction = actionHistories.Where(e => e.Action == RequestActionType.Reassign)
        .FirstOrDefault();

      if (reassignAction?.OfficerID == _review.ReviewerID)
      {
        throw new BadRequestException(string.Format(
          await _dbContext.Transalation.GetTranslation(0, "RequestApproveBlockReassign"),
          $"{_review.RequestID}"));
      }
    }
  }
}
