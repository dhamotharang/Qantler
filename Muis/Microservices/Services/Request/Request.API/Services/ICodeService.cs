using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Request.Model;

namespace Request.API.Services
{
  public interface ICodeService
  {
    /// <summary>
    /// Return list of code with specified type.
    /// </summary>
    Task<IList<Code>> List(CodeType type);

    /// <summary>
    /// Get code instance based on specified ID.
    /// </summary>
    Task<Code> GetByID(long id);

    /// <summary>
    /// Create or add new code.
    /// </summary>
    Task<Code> Create(Code code);

    /// <summary>
    /// Update code instance.
    /// </summary>
    Task Update(Code code);

    /// <summary>
    /// Generate code for specified type.
    /// </summary>
    Task<string> GenerateCode(CodeType type);
  }
}
