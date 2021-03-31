using Case.Model;
using Core.API.Repository;
using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Case.API.Repository
{
  public class PremiseRepository : IPremiseRepository
  {
    readonly IUnitOfWork _unitOfWork;

    public PremiseRepository(IUnitOfWork unitOfWork)
    {
      _unitOfWork = unitOfWork;
    }

    public async Task InsertPremise(Premise premise)
    {
      var param = new DynamicParameters();
      param.Add("@ID", premise.ID);
      param.Add("@IsLocal", premise.IsLocal);
      param.Add("@Name", premise.Name);
      param.Add("@Type", premise.Type);
      param.Add("@Area", premise.Area);
      param.Add("@Schedule", premise.Schedule);
      param.Add("@BlockNo", premise.BlockNo);
      param.Add("@UnitNo", premise.UnitNo);
      param.Add("@FloorNo", premise.FloorNo);
      param.Add("@BuildingName", premise.BuildingName);
      param.Add("@Address1", premise.Address1);
      param.Add("@Address2", premise.Address2);
      param.Add("@City", premise.City);
      param.Add("@Province", premise.Province);
      param.Add("@Country", premise.Country);
      param.Add("@Postal", premise.Postal);
      param.Add("@Longitude", premise.Longitude);
      param.Add("@Latitude", premise.Latitude);

      await SqlMapper.ExecuteAsync(_unitOfWork.Connection,
        "InsertPremise",
        param,
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction);
    }

    public async Task<IList<Premise>> GetPremises(long? caseID)
    {
      var param = new DynamicParameters();
      param.Add("@CaseID", caseID);

      return (await SqlMapper.QueryAsync(_unitOfWork.Connection,
       "GetPremises",
       new[]
       {
            typeof(Premise)
       },
       obj =>
       {
         return obj[0] as Premise;
       },
       param,
       commandType: CommandType.StoredProcedure,
       transaction: _unitOfWork.Transaction)).Distinct().ToList();
    }
  }
}
