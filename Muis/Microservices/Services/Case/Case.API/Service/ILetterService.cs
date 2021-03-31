using Case.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Case.API.Service
{
  public interface ILetterService
  {
    /// <summary>
    /// Retrieve letter template based on specified type.
    /// </summary>
    Task<LetterTemplate> GetTemplate(LetterType type);

    /// <summary>
    /// Update email template .
    /// </summary>
    Task UpdateTemplate(LetterTemplate template, Guid id, string userName);

    // <summary>
    ///Get Letter by ID.
    /// </summary>
    /// <param name="letterID">long</param>
    Task<Letter> GetLetterByID(long letterID);
  }
}
