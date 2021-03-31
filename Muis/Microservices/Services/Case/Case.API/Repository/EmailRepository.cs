using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Core.API.Repository;
using Core.Model;
using Dapper;
using Case.Model;

namespace Case.API.Repository
{
  public class EmailRepository : IEmailRepository
  {
    readonly IUnitOfWork _unitOfWork;

    public EmailRepository(IUnitOfWork unitOfWork)
    {
      _unitOfWork = unitOfWork;
    }

    public async Task<Email> GetByID(long id)
    {
      var param = new DynamicParameters();
      param.Add("@ID", id);

      return (await SqlMapper.QueryAsync<Email>(_unitOfWork.Connection,
        "GetEmailByID",
        param,
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction)).FirstOrDefault();
    }

    public async Task<long> Insert(Email entity)
    {
      var param = new DynamicParameters();
      param.Add("@To", entity.To);
      param.Add("@From", entity.From);
      param.Add("@Cc", entity.Cc);
      param.Add("@Bcc", entity.Bcc);
      param.Add("@Title", entity.Title);
      param.Add("@Body", entity.Body);
      param.Add("@IsBodyHtml", entity.IsBodyHtml);
      param.Add("@ID", dbType: DbType.Int64, direction: ParameterDirection.Output);

      await SqlMapper.ExecuteAsync(_unitOfWork.Connection,
        "InsertEmail",
        param,
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction);

      return param.Get<long>("@ID");
    }

    public async Task InsertOrReplaceTemplate(EmailTemplate template)
    {
      var param = new DynamicParameters();
      param.Add("@Type", template.Type);
      param.Add("@From", template.From);
      param.Add("@Cc", template.Cc);
      param.Add("@Bcc", template.Bcc);
      param.Add("@Title", template.Title);
      param.Add("@Body", template.Body);
      param.Add("@IsBodyHtml", template.IsBodyHtml);
      param.Add("@ID", dbType: DbType.Int64, direction: ParameterDirection.Output);

      await SqlMapper.ExecuteAsync(_unitOfWork.Connection,
        "InsertOrReplaceEmailTemplate",
        param,
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction);
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
