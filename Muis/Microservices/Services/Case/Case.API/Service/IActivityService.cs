using Core.Model;
using System.Threading.Tasks;

namespace Case.API.Service
{
  public interface IActivityService
  {

    /// <summary>
    /// Insert case.
    /// </summary>
    /// <returns>Case ID</returns>
    /// <param name="data">activity</param>
    /// <param name="user">Officier</param>
    /// <param name="caseID">long</param>
    Task<long> InsertActivity(Model.Activity data, Officer user, long caseID);
  }
}
