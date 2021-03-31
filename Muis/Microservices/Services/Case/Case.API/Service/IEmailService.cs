using Core.Model;
using Case.Model;
using System;
using System.Threading.Tasks;

namespace Case.API.Service
{
  public interface IEmailService
  {
    /// <summary>
    /// Insert a new email entity.
    /// </summary>
    /// <param name="entity">the entity to insert</param>
    /// <returns>the email</returns>
    Task<Email> Save(Email entity);

    /// <summary>
    /// Retrieve an email entity with specified ID.
    /// </summary>
    /// <param name="id">the id of the email</param>
    /// <returns>the email instance</returns>
    Task<Email> GetByID(long id);

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
