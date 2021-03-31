using System;
using System.Threading.Tasks;
using Identity.API.Dto;
using Identity.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace Identity.API.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class AuthController : Controller
  {
    readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
      _authService = authService;
    }

    [HttpPost]
    public async Task<AuthResponse> Post([FromBody] AuthParam param)
    {
      return await _authService.Authenticate(param);
    }
  }
}
