using System;

namespace Identity.API.Dto
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
