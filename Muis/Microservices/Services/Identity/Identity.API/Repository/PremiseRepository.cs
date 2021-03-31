using Core.API.Repository;
using Dapper;
using Identity.API.Repository.Mappers;
using Identity.Model;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.API.Repository
{
  public class PremiseRepository : IPremiseRepository
  {
    readonly IUnitOfWork _unitOfWork;

    public PremiseRepository(IUnitOfWork unitOfWork)
    {
      _unitOfWork = unitOfWork;
    }

    public async Task<IList<Premise>> Select()
    {
      var mapper = new ClusterMappers();

      return (await SqlMapper.QueryAsync(
        _unitOfWork.Connection,
        "GetPremise",
        new[]
        {
        typeof(Premise),
        typeof(Customer)
        },
        obj =>
        {
          var _premise = obj[0] as Premise;
          var _customer = obj[1] as Customer;
          _premise.Customer = _customer;
          return _premise;
        },
        splitOn: "PremiseCustomerID",
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction)).Distinct().ToList();
    }
  }
}
