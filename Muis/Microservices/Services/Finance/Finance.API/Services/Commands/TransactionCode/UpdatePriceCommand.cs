using Core.API;
using Core.API.Repository;
using Finance.API.Repository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Finance.API.Services.Commands.TransactionCode
{
  public class UpdatePriceCommand : IUnitOfWorkCommand<bool>
  {
    readonly IList<Model.Price> _priceList;

    readonly Guid _userID;

    readonly string _userName;

    public UpdatePriceCommand(IList<Model.Price> priceList, Guid userID, string userName)
    {
      _priceList = priceList;
      _userID = userID;
      _userName = userName;
    }

    public async Task<bool> Invoke(IUnitOfWork uow)
    {
      return await new DbContext(uow).Transactioncode.UpdatePrice(_priceList, _userID, _userName);
    }
  }
}
