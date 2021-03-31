using Core.API;
using Core.API.Provider;
using Core.API.Providers;
using Core.API.Smtp;
using Core.EventBus;
using Core.Model;
using Identity.API.Services.Commands.Identity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Identity.API.Services
{
  public class IdentityService : TransactionalService,
                                 IIdentityService
  {
    readonly ICacheProvider _cacheProvider;
    readonly ISmtpProvider _smtpProvider;

    readonly IEventBus _eventBus;

    public IdentityService(IDbConnectionProvider connectionProvider, ICacheProvider cacheProvider,
      ISmtpProvider smtpProvider, IEventBus eventBus)
         : base(connectionProvider)
    {
      _cacheProvider = cacheProvider;
      _smtpProvider = smtpProvider;

      _eventBus = eventBus;
    }

    public async Task<Model.Identity> GetIdentityByID(Guid id)
    {
      return await Execute(new GetIdentityIDCommand(id));
    }

    public async Task<IList<Model.Identity>> List(IdentityFilter filter)
    {
      return await Execute(new ListOfIdentityCommand(filter));
    }

    public async Task<Model.Identity> CreateIdentity(Model.Identity identity, Officer user)
    {
      return await Execute(new CreateIdentityCommand(identity, user, _smtpProvider));
    }

    public async Task<Model.Identity> UpdateIdentity(Model.Identity identity, Officer user)
    {
      return await Execute(new UpdateIdentityCommand(identity, user, _cacheProvider, _eventBus));
    }

    public async Task ResetPassword(Guid id, Officer user)
    {
      await Execute(new ResetPasswordCommand(id, user, _cacheProvider, _smtpProvider));
    }

    public async Task ForgotPassword(string email)
    {
      await Execute(new ForgotPasswordCommand(email, _cacheProvider, _smtpProvider));
    }

    public async Task<Model.Identity> GetCertificateAuditorToAssign(IdentityFilter filter,
      string ClusterNode)
    {
      return await Execute(new GetCAToAssignAppCommand(filter, ClusterNode));
    }
  }
}
