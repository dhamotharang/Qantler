using Core.Model;
using JobOrder.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JobOrder.API.Repository
{
  public interface IEmailRepository
  {
    /// <summary>
    /// Insert a new email entity.
    /// </summary>
    /// <param name="entity">the entity to insert</param>
    /// <returns>the ID of the newly insert entity</returns>
    Task<long> Insert(Email entity);

    /// <summary>
    /// Retrieve an email entity with specified ID.
    /// </summary>
    /// <param name="id">the id of the email</param>
    /// <returns>the email instance</returns>
    Task<Email> GetByID(long id);

    /// <summary>
    /// Insert or replace existing email template. By type.
    /// </summary>
    /// <param name="template">the template</param>
    Task InsertOrReplaceTemplate(EmailTemplate template);

    /// <summary>
    /// Retrieve email template base on specified type.
    /// </summary>
    /// <param name="type">the template type</param>
    /// <returns>the template</returns>
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
