using System;
using System.Threading.Tasks;
using Identity.Model;

namespace Identity.API.Repository
{
  public interface IEmailTemplateRepository
  {
    /// <summary>
    /// Retrieve email template base on specified type.
    /// </summary>
    Task<EmailTemplate> GetTemplate(EmailTemplateType type);

    /// <summary>
    /// Update email template base on specified type.
    /// </summary>
    Task UpdateTemplate(EmailTemplate emailTemplateDetails, Guid id, string userName);
  }
}
