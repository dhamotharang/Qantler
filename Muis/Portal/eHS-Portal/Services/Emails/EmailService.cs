using System;
using System.Threading.Tasks;
using eHS.Portal.Client;
using eHS.Portal.Model;

namespace eHS.Portal.Services.Emails
{
  public class EmailService : IEmailService
  {
    readonly ApiClient _apiClient;

    public EmailService(ApiClient apiClient)
    {
      _apiClient = apiClient;
    }

    public async Task<Email> GetRequestEmail(long id)
    {
      return await _apiClient.RequestEmailSdk.GetEmail(id);
    }
  }
}
