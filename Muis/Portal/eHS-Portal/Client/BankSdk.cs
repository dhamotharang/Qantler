using Core.Http;
using eHS.Portal.Infrastructure.Providers;
using eHS.Portal.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eHS.Portal.Client
{
  public class BankSdk
  {
    readonly string _url;
    readonly IHttpRequestProvider _requestProvider;

    public BankSdk(string url, IHttpRequestProvider requestProvider)
    {
      _url = url;
      _requestProvider = requestProvider;
    }

    public Task<IList<Bank>> List(BankFilter filter)
    {
      return _requestProvider.BuildUpon(_url)
        .Uri("/api/bank/list")
        .AddParam("accountName", filter.AccountName)
        .AddParam("accountNo", filter.AccountNo)
        .AddParam("branchCode", filter.BranchCode)
        .AddParam("bankName", filter.BankName)
        .AddParam("ddaStatus", filter.DDAStatus?.ToString())
        .AddInterceptor(new JsonDeserializerInterceptor())
        .Execute<IList<Bank>>();
    }
  }
}
