using Core.API.Exceptions;
using Core.Base;
using Request.API.Repository;
using Request.Model;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Request.API.Services.Commands.Request.Validators
{
  public class AmmendCertificateValidator : Validator
  {
    readonly Review _review;
    readonly DbContext _dbContext;

    public AmmendCertificateValidator(Review review, DbContext dbContext)
    {
      _review = review;
      _dbContext = dbContext;
    }
    public override async Task Validate()
    {
      var approvedLineItems = _review.LineItems.Where(e => e.Approved.Value).ToList();
      var approved = approvedLineItems.Count() >= 1;

      if (approved)
      {
        foreach (var item in approvedLineItems)
        {
          var request = await _dbContext.Request.GetRequestByID(_review.RequestID);

          var lineItem = request.LineItems.FirstOrDefault(e => e.Scheme == item.Scheme);

          if (lineItem == null)
          {
            continue;
          }

          var certificateNo = lineItem?.Characteristics?.FirstOrDefault(e =>
            e.Type == RequestCharType.IssuedCertificate)?.Value;

          if (string.IsNullOrEmpty(certificateNo))
          {
            continue;
          }

          var certificate360 = await _dbContext.Certificate.GetCertificate360ByNo(certificateNo);

          if (string.IsNullOrEmpty(certificateNo))
          {
            continue;
          }

          if (certificate360.Status == CertificateStatus.Suspended
            && certificate360.SuspendedUntil >= DateTime.UtcNow)
          {
            throw new BadRequestException(string.Format(
             await _dbContext.Transalation.GetTranslation(0, "RequestApproveBlockCertiSuspended"),
             $"{_review.RequestID}"));
          }
          else if (certificate360.Status == CertificateStatus.Revoked)
          {
            throw new BadRequestException(string.Format(
             await _dbContext.Transalation.GetTranslation(0, "RequestApproveBlockCertiRevoked"),
             $"{_review.RequestID}"));
          }

        }
      }
    }
  }
}