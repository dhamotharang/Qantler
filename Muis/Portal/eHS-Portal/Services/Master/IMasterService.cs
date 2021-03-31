using eHS.Portal.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eHS.Portal.Services.Master
{
  public interface IMasterService
  {
    /// <summary>
    /// Retrieve email template based on specified type.
    /// </summary>
    Task<EmailTemplate> GetEmailTemplate(EmailTemplateType type);

    /// <summary>
    /// update Rejection Email Template
    /// </summary>
    Task UpdateEmailTemplate(Model.EmailTemplate emailTemplate);

    /// <summary>
    /// Get job order master
    /// </summary>
    Task<IList<Model.Master>> GetMasterList(MasterType type);

    /// <summary>
    /// Insert master
    /// </summary>
    Task<bool> InsertMaster(Model.Master data);

    /// <summary>
    /// Update master
    /// </summary>
    Task<bool> UpdateMaster(Model.Master data);

    /// <summary>
    /// Delete master
    /// </summary>
    Task<bool> DeleteMaster(Guid id, MasterType type);

    /// <summary>
    /// Retrieve letter template based on specified type.
    /// </summary>
    Task<LetterTemplate> GetLetterTemplate(LetterType type);

    /// <summary>
    /// update letter template
    /// </summary>
    Task UpdateLetterTemplate(LetterTemplate template);
  }
}
