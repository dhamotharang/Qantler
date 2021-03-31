using System;
using System.Threading.Tasks;
using Core.API;
using Core.API.Provider;
using Core.Model;
using Request.API.Services.Commands.User;

namespace Request.API.Services
{
  public class UserService : TransactionalService,
                             IUserService
  {
    public UserService(IDbConnectionProvider connectionProvider)
         : base(connectionProvider)
    {
    }

    public Task InsertOrReplaceOfficer(Officer user)
    {
      return Execute(new InsertOrReplaceOfficerCommand(user));
    }
  }
}
