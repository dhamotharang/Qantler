using Core.Model;
using System.Threading.Tasks;

namespace Case.API.Repository
{
  public interface ITranslationRepository
  {
    Task<string> GetTranslation(Locale locale, string code);
  }
}
