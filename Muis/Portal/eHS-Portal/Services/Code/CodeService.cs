using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using eHS.Portal.Client;
using eHS.Portal.Model;

namespace eHS.Portal.Services.Code
{
  public class CodeService : ICodeService
  {
    readonly ApiClient _apiClient;

    public CodeService(ApiClient apiClient)
    {
      _apiClient = apiClient;
    }

    public Task<Model.Code> Create(Model.Code code)
    {
      return _apiClient.CustomerCodeSdk.Create(code);
    }

    public Task<string> Generate(CodeType type)
    {
      return _apiClient.CustomerCodeSdk.Generate(type);
    }

    public Task<IList<Model.Code>> List(CodeType type)
    {
      return _apiClient.CustomerCodeSdk.List(type);
    }
  }
}
