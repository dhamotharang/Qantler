using RulersCourt.Models;

namespace RulersCourt.Services
{
    public interface IAuthenticationService
    {
        User Authenticate(string username, string password, WrdUserLoginCredentialsModel wrdUser);
    }
}
