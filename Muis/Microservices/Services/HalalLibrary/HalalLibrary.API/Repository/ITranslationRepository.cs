using System;
using System.Threading.Tasks;
using Core.Model;

namespace HalalLibrary.API.Repository
{
  public interface ITranslationRepository
  {
    Task<string> GetTranslation(Locale locale, string code);
  }
}
