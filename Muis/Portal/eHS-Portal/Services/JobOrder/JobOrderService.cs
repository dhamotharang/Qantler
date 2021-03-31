using eHS.Portal.Client;
using eHS.Portal.DTO;
using eHS.Portal.Model;
using eHS.Portal.Models.JobOrder;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eHS.Portal.Services
{
  public class JobOrderService: IJobOrderService
  {
    readonly ApiClient _apiClient;

    public JobOrderService(ApiClient apiClient)
    {
      _apiClient = apiClient;
    }

    public async Task<IList<JobOrder>> Search(JobOrderOptions options)
    {
      return await _apiClient.JobOrderSdk.Query(options);
    }

    public async Task<JobOrder> GetByID(long id)
    {
      return await _apiClient.JobOrderSdk.GetByID(id);
    }
    
    public async Task AddInvitee(long jobID, Officer officer)
    {
      await _apiClient.JobOrderSdk.AddInvitee(jobID, officer);
    }

    public async Task DeleteInvitee(long jobID, Guid officerID)
    {
      await _apiClient.JobOrderSdk.DeleteInvitee(jobID, officerID);
    }

    public async Task Reschedule(long id, RescheduleParam param)
    {
      await _apiClient.JobOrderSdk.Reschedule(id, param);
    }

    public async Task Cancel(long id, CancelParam param)
    {
      await _apiClient.JobOrderSdk.Cancel(id, param);
    }

    public Task<IList<Notes>> GetNotes(long id)
    {
      return _apiClient.JobOrderNotesSdk.GetNotes(id);
    }

    public Task<Notes> AddNotes(long id, Notes notes)
    {
      notes.JobOrderID = id;
      return _apiClient.JobOrderNotesSdk.AddNotes(notes);
    }

    public async Task Schedule(long id, ScheduleParam param)
    {
      await _apiClient.JobOrderSdk.Schedule(id, param);
    }
  }
}