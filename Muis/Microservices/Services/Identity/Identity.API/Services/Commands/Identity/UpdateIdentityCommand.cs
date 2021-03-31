using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.API;
using Core.API.Exceptions;
using Core.API.Providers;
using Core.API.Repository;
using Core.EventBus;
using Core.Model;
using Core.Util;
using Identity.API.Repository;
using Identity.Events;

namespace Identity.API.Services.Commands.Identity
{
  public class UpdateIdentityCommand : IUnitOfWorkCommand<Model.Identity>
  {
    readonly Officer _user;
    readonly Model.Identity _identity;

    readonly ICacheProvider _cacheProvider;

    readonly IEventBus _eventBus;

    public UpdateIdentityCommand(Model.Identity identity, Officer user,
      ICacheProvider cacheProvider, IEventBus eventBus)
    {
      _user = user;
      _identity = identity;

      _cacheProvider = cacheProvider;

      _eventBus = eventBus;
    }

    public async Task<Model.Identity> Invoke(IUnitOfWork uow)
    {
      var dbContext = DbContext.From(uow);

      var existing = await dbContext.Identity.GetIdentityByID(_identity.ID);
      if (existing == null)
      {
        throw new NotFoundException();
      }

      var others = await dbContext.Identity.Query(new IdentityFilter
      {
        Email = _identity.Email
      });

      if (others.FirstOrDefault(e => e.ID != _identity.ID
        && e.Email.ToLower().Equals(_identity.Email.ToLower())) != null)
      {
        throw new BadRequestException(string.Format(
          await dbContext.Translation.GetTranslation(Locale.EN, "IdentityEmailExsits"),
          _identity.Email));
      }

      uow.BeginTransaction();

      var hasChanges = await CreateLog(dbContext, existing, _identity);

      if (hasChanges)
      {
        // To fix permission length when stored to DB
        if ((_identity.Permissions?.Length ?? 0) < (int)Permission.SystemReadWrite)
        {
          PermissionUtil.SetPermission(_identity.Permissions,
          (int)Permission.SystemReadWrite,
          Access.None,
          out string fixedPermissions);

          _identity.Permissions = fixedPermissions;
        }

        await dbContext.Identity.Update(_identity);

        await dbContext.Identity.MapIdentityToClusters(_identity.ID, _identity.Clusters);

        await dbContext.Identity.MapIdentityToRequestTypes(_identity.ID, _identity.RequestTypes);

        // Update credential
        if (!existing.Email.Equals(_identity.Email, StringComparison.InvariantCultureIgnoreCase))
        {
          var credential = await dbContext.Credential.GetCredentialByKey(
            Model.Provider.Default,
            existing.Email);

          credential.Key = _identity.Email.ToLower();

          await dbContext.Credential.UpdateCredential(credential);
        }

        // Remove from cache
        await _cacheProvider.RemoveAsync(_identity.ID.ToString());

        _eventBus.Publish(new OnIdentityChangedEvent
        {
          ID = _identity.ID,
          Name = _identity.Name
        });
      }

      var result = await dbContext.Identity.GetIdentityByID(_identity.ID);

      uow.Commit();

      return result;
    }

    public async Task<bool> CreateLog(DbContext dbContext, Model.Identity old, Model.Identity @new)
    {
      var changes = new List<string>()
      {
        "<b>Change Log:</b>"
      };

      if (!@new.Name.Equals(old.Name, StringComparison.InvariantCultureIgnoreCase))
      {
        changes.Add($"&#8226; <b>Name</b>: {old.Name} to {@new.Name}.");
      }

      if (!@new.Designation.Equals(old.Designation, StringComparison.InvariantCultureIgnoreCase))
      {
        changes.Add($"&#8226; <b>Designation</b>: {old.Designation} to {@new.Designation}.");
      }

      if (!@new.Email.Equals(old.Email, StringComparison.InvariantCultureIgnoreCase))
      {
        changes.Add($"&#8226; <b>Email</b>: {old.Email} to {@new.Email}.");
      }

      if (@new.Status != old.Status)
      {
        changes.Add($"&#8226; <b>Status</b>: {old.Status} to {@new.Status}.");
      }

      if (@new.Role != old.Role)
      {
        changes.Add($"&#8226; <b>Role</b>: {old.Role} to {@new.Role}.");
      }

      var newClusters = @new.Clusters?
        .Where(e => old.Clusters?.FirstOrDefault(o => o.ID == e.ID) == null);
      if (newClusters?.Any() ?? false)
      {
        changes.Add($"&#8226; <b>Added Clusters</b>: {string.Join(", ", newClusters.Select(e => e.District))}.");
      }

      var removedClusters = old.Clusters?
        .Where(o => @new.Clusters?.FirstOrDefault(e => e.ID == o.ID) == null);
      if (removedClusters?.Any() ?? false)
      {
        changes.Add($"&#8226; <b>Removed Clusters</b>: {string.Join(", ", removedClusters.Select(e => e.District))}.");
      }

      var newRequestTypes = @new.RequestTypes?.Where(e => !old.RequestTypes?.Contains(e) ?? true);
      var removedRequestTypes = old.RequestTypes?.Where(e => !@new.RequestTypes?.Contains(e) ?? true);

      if (newRequestTypes?.Any() ?? false)
      {
        changes.Add($"&#8226; <b>Added Request Types</b>: {string.Join(", ", newRequestTypes)}.");
      }

      if (removedRequestTypes?.Any() ?? false)
      {
        changes.Add($"&#8226; <b>Removed Request Types</b>: {string.Join(", ", removedRequestTypes)}.");
      }

      var addedPermissions = new List<Permission>();
      var removedPermissions = new List<Permission>();

      if (!string.IsNullOrEmpty(@new.Permissions))
      {
        for(int i = 0; i < @new.Permissions.Length; i++)
        {
          var newPermission = PermissionUtil.HasPermission(@new.Permissions, i, Access.Active);
          var oldPermission = PermissionUtil.HasPermission(old.Permissions, i, Access.Active);

          if(newPermission != oldPermission)
          {
            if (newPermission)
            {
              addedPermissions.Add((Permission)i);
            }
            else
            {
              removedPermissions.Add((Permission)i);
            }
          }
        }
      }

      if (!string.IsNullOrEmpty(old.Permissions))
      {
        var start = @new.Permissions?.Length ?? 0;
        for(int i = start; i < old.Permissions.Length; i++)
        {
          removedPermissions.Add((Permission)i);
        }
      }

      if (addedPermissions.Any())
      {
        changes.Add($"&#8226; <b>Added Permission</b> {string.Join(", ", addedPermissions)}.");
      }

      if (removedPermissions.Any())
      {
        changes.Add($"&#8226; <b>Removed Permission</b>: {string.Join(", ", removedPermissions)}.");
      }

      var hasChanges = changes.Count() > 1;
      if (hasChanges)
      {
        var logID = await dbContext.Log.InsertLog(new Log
        {
          Action = await dbContext.Translation.GetTranslation(Locale.EN, "IdentityUpdated"),
          Notes = string.Join('\n', changes),
          UserID = _user.ID,
          UserName = _user.Name
        });

        await dbContext.Identity.MapIdentityToLog(_identity.ID, logID);
      }

      return hasChanges;
    }
  }

  enum Changes
  {
    Info,
    Permissions,
    Clusters
  }
}
