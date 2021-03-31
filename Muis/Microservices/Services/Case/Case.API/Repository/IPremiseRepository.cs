using Case.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Case.API.Repository
{
  public interface IPremiseRepository
  {
    /// <summary>
    /// insert premise
    /// </summary>
    Task InsertPremise(Premise premise);

    /// <summary>
    /// get premise
    /// </summary>
    Task<IList<Premise>> GetPremises(long? caseID);
  }
}
