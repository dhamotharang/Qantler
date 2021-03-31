using Case.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Case.API.Repository
{
  public interface ILetterRepository
  {
    /// <summary>
    /// Retrieve email template base on specified type.
    /// </summary>
    Task<List<LetterTemplate>> GetTemplate(LetterType type);

    /// <summary>
    /// Update email template .
    /// </summary>
    Task UpdateTemplate(LetterTemplate template, Guid id, string userName);

    /// <summary>
    /// Insert letter.
    /// </summary>
    public Task<long> InsertLetter(Letter data);

    /// <summary>
    /// Update letter.
    /// </summary>
    public Task<long> UpdateLetter(Letter data);

    /// <summary>
    /// Get letter by ID.
    /// </summary>
    public Task<Letter> GetLetterByID(long letterID);
  }
}
