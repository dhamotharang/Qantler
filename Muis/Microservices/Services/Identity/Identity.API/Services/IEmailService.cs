using Identity.Model;
using System;
using System.Threading.Tasks;

namespace Identity.API.Services
{
  public interface IEmailService
  {
    /// <summary>
    /// Retrieve email template based on specified type.
    /// </summary>
    /// <param name="type">the template type</param>
    /// <returns>the email template</returns>
    Task<EmailTemplate> GetTemplate(EmailTemplateType type);

    /// <summary>
    /// Update email template .
    /// </summary>
    /// <param name="id">User ID</param>
    /// <param name="userName">UserName</param>
    /// <param name="emailTemplateDetails">Mail Template</param>
    Task UpdateTemplate(EmailTemplate emailTemplateDetails, Guid id, string userName);
  }
}
