using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Core.Http;
using eHS.Portal.Configs;
using eHS.Portal.DTO;
using eHS.Portal.Extensions;
using eHS.Portal.Model;
using eHS.Portal.Services;
using eHS.Portal.Services.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace eHS.Portal.Controllers
{
  public class HomeController : Controller
  {
    readonly JwtConfig _jwtConfig;
    readonly IAuthService _authService;
    readonly IIdentityService _identityService;

    public HomeController(IOptions<JwtConfig> jwtConfig, IAuthService authService, 
      IIdentityService identityService)
    {
      _jwtConfig = jwtConfig.Value;
      _authService = authService;
      _identityService = identityService;
    }

    public IActionResult Index(string key = null)
    {
      if (User?.Identity?.IsAuthenticated ?? false)
      {
        return RedirectToAction("startup");
      }

      ViewData["error"] = TempData["error"];
      ViewData["key"] = key?.Replace("@muis.gov.sg", "");
      return View();
    }

    public IActionResult Dashboard()
    {
      return View();
    }

    public IActionResult Empty()
    {
      return View();
    }

    public IActionResult Startup()
    {
      if (!User.HasPermission())
      {
        return RedirectToAction("index", "error", new
        {
          Code = 428,
          Message = "No permission granted. Please contact administrator."
        });
      }

      var isMuftiOnly = true;
      foreach (Permission permission in Enum.GetValues(typeof(Permission)))
      {
        if (permission != Permission.MuftiRead
          && permission != Permission.MuftiAcknowledge
          && permission != Permission.MuftiCommentsReadWrite
          && User.HasPermission(permission))
        {
          isMuftiOnly = false;
          break;
        }
      }

      if (isMuftiOnly)
      {
        return RedirectToAction("index", "mufti");
      }

      return RedirectToAction("dashboard");
    }

    [HttpPost]
    public async Task<IActionResult> Auth([FromForm] AuthParam param)
    {
      var suffix = "@muis.gov.sg";

      if (!param.Key.EndsWith(suffix))
      {
        param.Key = $"{param.Key}{suffix}";
      }

      AuthResponse response;
      try
      {
        response = await _authService.Authenticate(param);
      }
      catch
      {
        TempData["error"] = "Invalid credentials!";
        TempData["key"] = param.Key.Replace(suffix, "");

        return RedirectToAction("Index", new {
          key = param.Key.Replace(suffix, "")
        });
      }

      if (response.Action == AuthAction.ChangePassword)
      {
        TempData["model"] = JsonConvert.SerializeObject(param);
        return RedirectToAction("ChangePassword");
      }

      await CreateSession(response.Identity);

      return RedirectToAction("startup");
    }

    public IActionResult ChangePassword()
    {
      if (User?.Identity?.IsAuthenticated ?? false)
      {
        return RedirectToAction("startup");
      }

      var model = JsonConvert.DeserializeObject<AuthParam>(TempData["model"] as string);
      return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> ChangePassword([FromForm] AuthParam param)
    {
      var response = await _authService.Authenticate(param);
      if (response == null)
      {
        TempData["error"] = "Something went wrong. Please try again.";
        return RedirectToAction("Index", new
        {
          param.Key
        });
      }

      await CreateSession(response.Identity);

      return RedirectToAction("startup");
    }

    public IActionResult ForgotPassword()
    {
      return View();
    }

    [HttpPost]
    [Route("api/[controller]/forgot-password")]
    public async Task<string> ForgotPassword(string email)
    {
      if (!email.EndsWith("@muis.gov.sg"))
      {
        email = $"{email}@muis.gov.sg";
      }

      await _identityService.ForgotPassword(email);
      return "Ok";
    }

    [HttpGet]
    public async Task<IActionResult> Signout()
    {
      await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

      return RedirectToAction("index", "home");
    }

    async Task CreateSession(Model.Identity data)
    {

      var claims = new List<Claim>
      {
        new Claim(ClaimTypes.NameIdentifier, data.ID.ToString()),
        new Claim(ClaimTypes.Name, data.Name),
        new Claim(ClaimTypes.Email, data.Email),
        new Claim("Designation", data.Designation),
        new Claim("Permissions", data.Permissions)
      };

      var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfig.Key));
      var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

      var token = new JwtSecurityToken(_jwtConfig.Issuer,
                                       _jwtConfig.Issuer,
                                       claims,
                                       expires: DateTime.Now.AddMonths(3),
                                       signingCredentials: creds);

      var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);

      claims.Add(new Claim("Token", jwtToken));

      var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
      var principal = new ClaimsPrincipal(identity);

      await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
    }
  }
}
