using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Http.Exceptions;
using eHS.Portal.Client;
using eHS.Portal.Model;

namespace eHS.Portal.Services.Customer
{
  public class CustomerService : ICustomerService
  {
    readonly ApiClient _apiClient;

    public CustomerService(ApiClient apiClient)
    {
      _apiClient = apiClient;
    }

    public async Task<Model.Customer> CreateCustomer(Model.Customer customer)
    {
      return await _apiClient.CustomerSdk.Submit(customer);
    }

    public async Task<IList<Model.Customer>> List()
    {
      return await _apiClient.CustomerSdk.List();
    }

    public async Task<Model.Customer> GetByID(Guid id)
    {
      var result = await _apiClient.CustomerSdk.GetRequestCustomerByID(id);

      if (result == null)
      {
        throw new NotFoundException();
      }

      await FetchCustomerInfo(result);

      return result;
    }

    async Task FetchCustomerInfo(Model.Customer customer)
    {
      var parentCustomer = await _apiClient.CustomerSdk.GetByID(customer.ID);

      if (parentCustomer != null)
      {
        customer.Parent = parentCustomer.Parent;
        customer.ParentID = parentCustomer.ParentID;
        customer.AltID = parentCustomer.AltID;
      }
    }

    public Task<IList<Request>> GetCustomerRecentRequest(Guid id)
    {
      return _apiClient.CustomerSdk.GetCustomerRecentRequest(id);
    }

    public Task<Model.Customer> SetParent(Guid id, Guid parentID)
    {
      return _apiClient.CustomerSdk.SetParent(id, parentID);
    }

    public Task<Model.Customer> SetOfficerInCharge(Guid id, Guid officerID)
    {
      return _apiClient.CustomerSdk.SetOfficerInCharge(id, officerID);
    }

    public Task<Model.Customer> UpdateCode(Guid id, Model.Code code)
    {
      return code.Type == CodeType.Code
        ? _apiClient.CustomerSdk.SetCodeID(id, code.ID)
        : _apiClient.CustomerSdk.SetGroupCodeID(id, code.ID);
    }

    public async Task<IList<Model.Certificate360>> GetCustomers(CustomerOptions options)
    {
      return await _apiClient.CustomerSdk.Query(options);
    }
  }
}
