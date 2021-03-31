using Core.API.Repository;
using Core.Model;
using Dapper;
using HalalLibrary.API.Models;
using HalalLibrary.API.Repository.Mappers;
using HalalLibrary.Model;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace HalalLibrary.API.Repository
{
  public class IngredientRepository : IIngredientRepository
  {
    readonly IUnitOfWork _unitOfWork;

    public IngredientRepository(IUnitOfWork unitOfWork)
    {
      _unitOfWork = unitOfWork;
    }

    public async Task<halalLibraryList> Select(long nextRows,
      long offsetRows,
      string name,
      string brand,
      string supplier,
      string certifyingBody,
      RiskCategory? riskCategory,
      IngredientStatus? status,
      string verifiedBy)
    {
      var mapper = new HalalLibraryMapper();

      var param = new DynamicParameters();
      param.Add("@NextRows", nextRows);
      param.Add("@OffsetRows", offsetRows);
      param.Add("@Name", name);
      param.Add("@Brand", brand);
      param.Add("@Supplier", supplier);
      param.Add("@CertifyingBody", certifyingBody);
      param.Add("@RiskCategory", riskCategory);
      param.Add("@Status", status);
      param.Add("@VerifiedBy", verifiedBy);

      return (await SqlMapper.QueryAsync(_unitOfWork.Connection,
        "SelectHalalLibrary",
        new[]
        {
          typeof(long),
          typeof(Ingredient),
          typeof(Supplier),
          typeof(CertifyingBody),
          typeof(Officer)
        },
        obj =>
        {
          return mapper.Map(long.Parse(obj[0].ToString()),
            obj[1] as Ingredient,
            obj[2] as Supplier,
            obj[3] as CertifyingBody,
            obj[4] as Officer);
        },
        param,
        splitOn: "IngredientID,SupplierID,CertifyingBodyID,OfficerID",
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction)).FirstOrDefault();
    }

    public async Task<long> InsertIngredient(Ingredient data)
    {
      var ingredientParam = new DynamicParameters();
      ingredientParam.Add("@Name", data.Name);
      ingredientParam.Add("@Brand", data.Brand);
      ingredientParam.Add("@RiskCategory", data.RiskCategory);
      ingredientParam.Add("@Status", data.Status);
      ingredientParam.Add("@SupplierID", data.SupplierID);
      ingredientParam.Add("@CertifyingBodyID", data.CertifyingBodyID);
      ingredientParam.Add("@VerifiedByID", data.VerifiedByID);
      ingredientParam.Add("@Out", dbType: DbType.Int64, direction: ParameterDirection.Output);

      await SqlMapper.ExecuteAsync(_unitOfWork.Connection,
        "InsertIngredient",
        ingredientParam,
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction);

      return ingredientParam.Get<long>("@Out");
    }

    public async Task<long> UpdateIngredient(Ingredient data)
    {
      var ingredientParam = new DynamicParameters();
      ingredientParam.Add("@ID", data.ID);
      ingredientParam.Add("@Name", data.Name);
      ingredientParam.Add("@Brand", data.Brand);
      ingredientParam.Add("@RiskCategory", data.RiskCategory);
      ingredientParam.Add("@Status", data.Status);
      ingredientParam.Add("@SupplierID", data.SupplierID);
      ingredientParam.Add("@CertifyingBodyID", data.CertifyingBodyID);
      ingredientParam.Add("@VerifiedByID", data.VerifiedByID);
      ingredientParam.Add("@Out", dbType: DbType.Int64, direction: ParameterDirection.Output);

      await SqlMapper.ExecuteAsync(_unitOfWork.Connection,
        "UpdateIngredient",
        ingredientParam,
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction);

      return ingredientParam.Get<long>("@Out");
    }

    public async Task DeleteIngredient(long id)
    {
      var ingredientParam = new DynamicParameters();
      ingredientParam.Add("@ID", id);

      await SqlMapper.ExecuteAsync(_unitOfWork.Connection,
        "DeleteIngredient",
        ingredientParam,
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction);
    }

    public async Task<bool> CheckIngredient(Ingredient data)
    {
      var ingredientParam = new DynamicParameters();
      ingredientParam.Add("@Name", data.Name);
      ingredientParam.Add("@Brand", data.Brand);
      ingredientParam.Add("@SupplierName", data.Supplier?.Name);
      ingredientParam.Add("@CertifyingBody", data.CertifyingBody?.Name);
      ingredientParam.Add("@Result", dbType: DbType.Boolean, direction: ParameterDirection.Output);

      await SqlMapper.ExecuteAsync(_unitOfWork.Connection,
        "CheckIngredient",
        ingredientParam,
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction);

      return ingredientParam.Get<bool>("@Result");
    }
  }
}
