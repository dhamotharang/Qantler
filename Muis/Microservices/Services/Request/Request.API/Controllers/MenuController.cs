using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Request.API.Services;
using Request.Model;

namespace Request.API.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class MenuController : ControllerBase
  {
    readonly IMenuService _menuService;

    public MenuController(IMenuService menuService)
    {
      _menuService = menuService;
    }

    [HttpPut]
    [Route("list")]
    public async Task<string> BulkUpdate(IList<Menu> menus, Guid userID, string userName)
    {
      await _menuService.BulkUpdate(menus, userID, userName);
      return "Ok";
    }

    [HttpPut]
    [Route("list/review")]
    public async Task<string> BulkReview(IList<Menu> menus, Guid userID, string userName)
    {
      await _menuService.BulkReview(menus, userID, userName);
      return "Ok";
    }

    [HttpPost]
    [Route("")]
    public async Task<string> AddMenus([FromBody]IList<Menu> menus, Guid userID, string userName)
    {
      await _menuService.AddMenus(menus, userID, userName);
      return "Ok";
    }
  }
}
