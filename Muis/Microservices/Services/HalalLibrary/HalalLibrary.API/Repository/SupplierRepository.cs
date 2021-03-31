using Core.API.Repository;
using Dapper;
using HalalLibrary.Model;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace HalalLibrary.API.Repository
{
  public class SupplierRepository : ISupplierRepository
  {
    readonly IUnitOfWork _unitOfWork;

    public SupplierRepository(IUnitOfWork unitOfWork)
    {
      _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<Supplier>> Select()
    {
      return (await SqlMapper.QueryAsync<Supplier>(_unitOfWork.Connection,
         "SelectSupplier",
         commandType: CommandType.StoredProcedure,
         transaction: _unitOfWork.Transaction)).Distinct().ToList();
    }

    public async Task<Supplier> GetSupplierByName(string name)
    {
      var selectSupplier = $"SELECT * FROM Supplier s " +
        $"WHERE s.[IsDeleted] = 0 AND s.[Name] LIKE '%{name}%'";
      return (await _unitOfWork.Connection.QueryAsync<Supplier>(selectSupplier,
        transaction: _unitOfWork.Transaction)).FirstOrDefault();
    }
    public async Task<long> InsertSupplier(Supplier data)
    {

      var supplierParam = new DynamicParameters();
      supplierParam.Add("@Name", data.Name);
      supplierParam.Add("@ID", dbType: DbType.Int64, direction: ParameterDirection.Output);

      await SqlMapper.ExecuteAsync(_unitOfWork.Connection,
        "InsertSupplier",
        supplierParam,
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction);

      return supplierParam.Get<long>("@ID");
    }
  }
}
