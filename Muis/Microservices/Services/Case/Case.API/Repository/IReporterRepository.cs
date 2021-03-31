using Case.API.Params;
using Case.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Case.API.Repository
{
  public interface IReporterRepository
  {
    /// <summary>
    /// insert reporter
    /// </summary>
    public Task<Guid> InsertReporter(Person person);
  }
}
