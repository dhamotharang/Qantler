using System;

namespace eHS.Portal.DTO
{
  public enum AuthAction
  {
    None,
    ChangePassword
  }

  public class AuthResponse
  {
    public AuthAction Action { get; set; }

    public Model.Identity Identity { get; set; }
  }
}
