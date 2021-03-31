using Core.EventBus;
using Core.Model;
using Request.API.Models;
using Request.Events;
using Request.Model;
using System.Collections.Generic;
using System.Linq;

namespace Request.API.Helpers
{
  public static class RequestHelper
  {
    public static bool HasStage2Payment(RequestType type, Scheme? scheme, SubScheme? subScheme)
    {
      if (type == RequestType.HC01
        || type == RequestType.HC02
        || subScheme == SubScheme.Canteen
        || subScheme == SubScheme.ShortTerm
        || scheme == Scheme.Endorsement)
      {
        return false;
      }
      return true;
    }
  }
}
