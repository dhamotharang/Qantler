using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using eHS.Portal.Model;

namespace eHS.Portal.Services.Code
{
  public interface ICodeService
  {
    /// <summary>
    /// Create code instance.
    /// </summary>
    Task<Model.Code> Create(Model.Code code);

    /// <summary>
    /// Generate code string for specified type.
    /// </summary>
    Task<string> Generate(CodeType type);

    /// <summary>
    /// List of code for specified type.
    /// </summary>
    Task<IList<Model.Code>> List(CodeType type);
  }
}
