using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using eHS.Portal.Client;

namespace eHS.Portal.Services.RFA
{
  public interface IRFAService
  {
    /// <summary>
    /// Submits an RFA.
    /// </summary>
    Task<Model.RFA> Submit(Model.RFA rfa);

    /// <summary>
    /// Retrieve RFA with specified ID.
    /// </summary>
    Task<Model.RFA> GetByID(long id);

    /// <summary>
    /// Discard RFA.
    /// </summary>
    Task Discard(long id);

    /// <summary>
    /// Closed an RFA.
    /// </summary>
    Task Close(long id);

    /// <summary>
    /// Extends RFA due date.
    /// </summary>
    Task Extend(long id, string notes, DateTimeOffset toDate);

    /// <summary>
    /// Retrieve list of RFA based on specified options.
    /// </summary>
    Task<IList<Model.RFA>> ListOfRFA(RFAOptions options);
  }
}
