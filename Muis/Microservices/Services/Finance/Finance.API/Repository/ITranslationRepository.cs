using Core.Model;
using System.Threading.Tasks;

namespace Finance.API.Repository
{
  public interface ITranslationRepository
  {
    Task<string> GetTranslation(Locale locale, string code);
  }
}
