using eHS.Portal.Client;
using eHS.Portal.Model;
using System.Threading.Tasks;

namespace eHS.Portal.Services.CaseLetter
{
  public class CaseLetterService : ICaseLetterService
  {
    readonly ApiClient _apiClient;

    public CaseLetterService(ApiClient apiClient)
    {
      _apiClient = apiClient;
    }

    public async Task<Letter> GetLetterByID(long letterID)
    {
      var result = await _apiClient.CaseLetterSdk.GetLetterByID(letterID);
      return result;
    }
  }
}
