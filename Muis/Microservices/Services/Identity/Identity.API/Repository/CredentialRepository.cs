using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Core.API.Repository;
using Dapper;
using Identity.Model;

namespace Identity.API.Repository
{
  public class CredentialRepository : ICredentialRepository
  {
    readonly IUnitOfWork _unitOfWork;

    public CredentialRepository (IUnitOfWork unitOfWork)
    {
      _unitOfWork = unitOfWork;
    }

    public async Task<Credential> GetCredential(Provider provider, string key, string secret)
    {
      var param = new DynamicParameters();
      param.Add("Provider", provider);
      param.Add("Key", key);
      param.Add("Secret", secret);

      return (await SqlMapper.QueryAsync<Credential>(_unitOfWork.Connection,
        "GetCredential",
        param,
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction)).FirstOrDefault();
    }

    public async Task<Credential> GetCredentialByKey(Provider provider, string key)
    {
      var param = new DynamicParameters();
      param.Add("Provider", provider);
      param.Add("Key", key);

      return (await SqlMapper.QueryAsync<Credential>(_unitOfWork.Connection,
        "GetCredentialByKey",
        param,
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction)).FirstOrDefault();
    }

    public async Task InsertCredential(Credential credential)
    {
      var param = new DynamicParameters();
      param.Add("Provider", credential.Provider);
      param.Add("Key", credential.Key);
      param.Add("Secret", credential.Secret);
      param.Add("IsTemporary", credential.IsTemporary);
      param.Add("ExpiresOn", credential.ExpiresOn);
      param.Add("IdentityID", credential.IdentityID);

      await SqlMapper.ExecuteAsync(_unitOfWork.Connection,
        "InsertCredential",
        param,
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction);
    }

    public async Task UpdateCredential(Credential credential)
    {
      var param = new DynamicParameters();
      param.Add("ID", credential.ID);
      param.Add("Provider", credential.Provider);
      param.Add("Key", credential.Key);
      param.Add("Secret", credential.Secret);
      param.Add("IsTemporary", credential.IsTemporary);
      param.Add("ExpiresOn", credential.ExpiresOn);

      await SqlMapper.ExecuteAsync(_unitOfWork.Connection,
        "UpdateCredential",
        param,
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction);
    }
  }
}
