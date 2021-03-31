using eHS.Portal.Model;
using System.Threading.Tasks;

namespace eHS.Portal.Services.CaseLetter
{
  public interface ICaseLetterService
  {
    /// <summary>
    /// Get Letter By ID.
    /// </summary>
    Task<Letter> GetLetterByID(long letterID);
  }
}
