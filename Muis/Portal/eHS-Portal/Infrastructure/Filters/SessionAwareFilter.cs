using System;
using eHS.Portal.Infrastructure.Providers;
using eHS.Portal.Extensions;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Threading.Tasks;
using Core.Http;

namespace eHS.Portal.Infrastructure.Filters
{
  public class SessionAwareFilter : ActionFilterAttribute,
                                    IOrderedFilter
  {
    readonly ICacheProvider _cacheProvider;

    public SessionAwareFilter(ICacheProvider cacheProvider)
    {
      Order = 0;
      _cacheProvider = cacheProvider;
    }

    public override async Task OnActionExecutionAsync(ActionExecutingContext context,
      ActionExecutionDelegate next)
    {
      var id = context.HttpContext.User.GetUserId().ToString();

      var result = await _cacheProvider.GetAsync<string>(id);

      var isUnauthorized = string.IsNullOrEmpty(id)
                        || (await _cacheProvider.GetAsync<string>(id)) == null;

      if (isUnauthorized)
      {
        //throw new UnauthorizedException();
      }

      await base.OnActionExecutionAsync(context, next);
    }
  }
}
