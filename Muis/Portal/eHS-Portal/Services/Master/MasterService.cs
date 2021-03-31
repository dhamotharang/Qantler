using eHS.Portal.Client;
using eHS.Portal.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eHS.Portal.Services.Master
{
  public class MasterService : IMasterService
  {
    readonly ApiClient _apiClient;

    public MasterService(ApiClient apiClient)
    {
      _apiClient = apiClient;
    }

    public async Task<EmailTemplate> GetEmailTemplate(EmailTemplateType type)
    {
      if (type == EmailTemplateType.RejectionEmail)
      {
        return await _apiClient.RequestEmailSdk.GetTemplate(type);
      }
      else if (type == EmailTemplateType.AuditInspection
        || type == Model.EmailTemplateType.RescheduleAuditInspection)
      {
        return await _apiClient.JobOrderEmailSdk.GetTemplate(type);
      }
      else if (type == EmailTemplateType.ShowCause
        || type == EmailTemplateType.Warning
        || type == EmailTemplateType.Compound
        || type == EmailTemplateType.Suspension
        || type == EmailTemplateType.ImmediateSuspension
        || type == EmailTemplateType.Revocation)
      {
        return await _apiClient.CaseEmailSdk.GetTemplate(type);
      }
      else
      {
        return await _apiClient.IdentityEmailSdk.GetTemplate(type);
      }
    }

    public async Task UpdateEmailTemplate(EmailTemplate emailTemplate)
    {
      if (emailTemplate.Type == EmailTemplateType.RejectionEmail)
      {
        await _apiClient.RequestEmailSdk.UpdateTemplate(emailTemplate);
      }
      else if (emailTemplate.Type == EmailTemplateType.AuditInspection
        || emailTemplate.Type == EmailTemplateType.RescheduleAuditInspection)
      {
        await _apiClient.JobOrderEmailSdk.UpdateTemplate(emailTemplate);
      }
      else if (emailTemplate.Type == EmailTemplateType.ShowCause
        || emailTemplate.Type == EmailTemplateType.Warning
        || emailTemplate.Type == EmailTemplateType.Compound
        || emailTemplate.Type == EmailTemplateType.Suspension
        || emailTemplate.Type == EmailTemplateType.ImmediateSuspension
        || emailTemplate.Type == EmailTemplateType.Revocation)
      {
        await _apiClient.CaseEmailSdk.UpdateTemplate(emailTemplate);
      }
      else
      {
        await _apiClient.IdentityEmailSdk.UpdateTemplate(emailTemplate);
      }
    }

    public async Task<LetterTemplate> GetLetterTemplate(LetterType type)
    {
      return await _apiClient.CaseLetterSdk.GetTemplate(type);
    }

    public async Task UpdateLetterTemplate(LetterTemplate template)
    {
      await _apiClient.CaseLetterSdk.UpdateTemplate(template);
    }

    public Task<IList<Model.Master>> GetMasterList(MasterType type)
    {
      if (type == Model.MasterType.EscalationCategory || type == Model.MasterType.ReinstateReason)
      {
        return _apiClient.RequestMasterSdk.GetMasterList(type);
      }
      else if (type == Model.MasterType.Offence
        || type == Model.MasterType.BreachCategory)
      {
        return _apiClient.CaseMasterSdk.GetMasterList(type);
      }
      else if (type == Model.MasterType.Bank)
      {
        return _apiClient.FinanceMasterSdk.GetMasterList(type);
      }
      else
      {
        return _apiClient.JobOrderMasterSdk.GetMasterList(type);
      }
    }

    public Task<bool> InsertMaster(Model.Master data)
    {
      if (data.Type == Model.MasterType.EscalationCategory
         || data.Type == Model.MasterType.ReinstateReason)
      {
        return _apiClient.RequestMasterSdk.InsertMaster(data);
      }
      else
      {
        return _apiClient.JobOrderMasterSdk.InsertMaster(data);
      }
    }

    public Task<bool> UpdateMaster(Model.Master data)
    {
      if (data.Type == Model.MasterType.EscalationCategory
        || data.Type == Model.MasterType.ReinstateReason)
      {
        return _apiClient.RequestMasterSdk.UpdateMaster(data);
      }
      else
      {
        return _apiClient.JobOrderMasterSdk.UpdateMaster(data);
      }
    }

    public Task<bool> DeleteMaster(Guid id, MasterType type)
    {
      if (type == Model.MasterType.EscalationCategory
        || type == Model.MasterType.ReinstateReason)
      {
        return _apiClient.RequestMasterSdk.DeleteMaster(id);
      }
      else
      {
        return _apiClient.JobOrderMasterSdk.DeleteMaster(id);
      }
    }
  }
}
