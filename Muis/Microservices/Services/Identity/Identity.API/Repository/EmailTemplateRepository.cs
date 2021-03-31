using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Core.API.Repository;
using Dapper;
using Identity.Model;

namespace Identity.API.Repository
{
  public class EmailTemplateRepository : IEmailTemplateRepository
  {
    readonly IUnitOfWork _unitOfWork;

    public EmailTemplateRepository(IUnitOfWork unitOfWork)
    {
      _unitOfWork = unitOfWork;
    }

    public async Task<EmailTemplate> GetTemplate(EmailTemplateType type)
    {
      var param = new DynamicParameters();
      param.Add("@Type", type);

      return (await SqlMapper.QueryAsync<EmailTemplate>(_unitOfWork.Connection,
        "GetEmailTemplate",
        param,
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction)).FirstOrDefault();
    }

    public async Task UpdateTemplate(EmailTemplate emailTemplateDetails,
      Guid id, string userName)
    {
      var param = new DynamicParameters();
      param.Add("@ID", emailTemplateDetails.ID);
      param.Add("@From", emailTemplateDetails.From);
      param.Add("@Cc", emailTemplateDetails.Cc);
      param.Add("@Bcc", emailTemplateDetails.Bcc);
      param.Add("@Title", emailTemplateDetails.Title);
      param.Add("@Body", emailTemplateDetails.Body);
      param.Add("@UserID", id);
      param.Add("@UserName", userName);

      await SqlMapper.ExecuteAsync(_unitOfWork.Connection,
        "UpdateEmailTemplate",
        param,
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction);
    }
  }
}
