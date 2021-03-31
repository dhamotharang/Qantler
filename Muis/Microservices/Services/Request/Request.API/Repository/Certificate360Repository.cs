using Core.API.Repository;
using Dapper;
using Request.API.Repository.Mappers;
using Request.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Core.Model;


namespace Request.API.Repository
{
  public class Certificate360Repository : ICertificate360Repository
  {
    readonly IUnitOfWork _unitOfWork;
    public Certificate360Repository(IUnitOfWork unitOfWork)
    {
      _unitOfWork = unitOfWork;
    }

    public async Task<IList<Certificate360>> GetCertificatesForRenewal
      (int RenewalPeriod, Scheme scheme, SubScheme? subScheme)
    {
      var param = new DynamicParameters();
      param.Add("@RenewalPeriod", RenewalPeriod);
      param.Add("@Scheme", scheme);
      param.Add("@SubScheme", subScheme);

      return (await SqlMapper.QueryAsync<Certificate360>(
        _unitOfWork.Connection,
        "GetCertsForAutoRenewal", param,
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction)).ToList();
    }

    public async Task<IList<Certificate360>> Certificate360Filter(Certificate360Filter filter)
    {
      var tPremiseIDs = new DataTable();
      tPremiseIDs.Columns.Add("Val", typeof(long));

      if (filter.PremiseIDs?.Any() ?? false)
      {
        foreach (var id in filter.PremiseIDs)
        {
          tPremiseIDs.Rows.Add(id);
        }
      }

      var param = new DynamicParameters();
      param.Add("@PremiseIDs", tPremiseIDs.AsTableValuedParameter("dbo.BigIntType"));

      var mapper = new Certificate360Mapper();

      return (await SqlMapper.QueryAsync<Certificate360>(_unitOfWork.Connection,
        "SelectCertificate360",
        new[]
        {
            typeof(Certificate360),
            typeof(Premise),
            typeof(Menu)
        },
        obj =>
        {
          return mapper.Map(obj[0] as Certificate360,
            obj[1] as Premise,
            obj[2] as Menu);
        },
        param,
        splitOn: "ID,PremID,MenuID",
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction)).Distinct().ToList();
    }

    public async Task<Certificate360> GetCertificateByCertNo(string CertificateNo)
    {
      await Task.CompletedTask;

      var param = new DynamicParameters();
      param.Add("@CertificateNo", CertificateNo);

      var mapper = new Certificate360Mapper();

      return (await SqlMapper.QueryAsync(
          _unitOfWork.Connection,
          "GetCertificate360Detail",
          new[]
          {
            typeof(Certificate360),
            typeof(Certificate360History),
            typeof(Menu),
            typeof(Ingredient),
            typeof(Premise),
            typeof(Customer),
            typeof(HalalTeam)
          },
          obj =>
          {
            var cert = obj[0] as Certificate360;
            var certHistory = obj[1] as Certificate360History;
            var certMenu = obj[2] as Menu;
            var cetIng = obj[3] as Ingredient;
            var certPremise = obj[4] as Premise;
            var certCustomer = obj[5] as Customer;
            var certHalalTeam = obj[6] as HalalTeam;

            return mapper.Map(cert, certHistory, certMenu, cetIng,
              certPremise, certCustomer, certHalalTeam);
          },
          param,
          splitOn: "ID,CertificateHistoryID,MenuID," +
          "IngredientID,PremiseID,CustomerID,HalalTeamID",
          commandType: CommandType.StoredProcedure,
          transaction: _unitOfWork.Transaction)).FirstOrDefault();
    }

    public async Task<Certificate360> GetCertificateByCertID(long ID)
    {
      await Task.CompletedTask;

      var param = new DynamicParameters();
      param.Add("@ID", ID);

      var mapper = new Certificate360Mapper();

      return (await SqlMapper.QueryAsync(
          _unitOfWork.Connection,
          "GetCertificate360ByID",
          new[]
          {
            typeof(Certificate360),
            typeof(Premise),
            typeof(HalalTeam)
          },
          obj =>
          {
            var cert = obj[0] as Certificate360;
            var certPremise = obj[1] as Premise;
            var certHalalTeam = obj[2] as HalalTeam;

            return mapper.Map(cert, certPremise, certHalalTeam);
          },
          param,
          splitOn: "ID,PremiseID,HalalTeamID",
          commandType: CommandType.StoredProcedure,
          transaction: _unitOfWork.Transaction)).FirstOrDefault();
    }

    public async Task<IList<Certificate360History>> GetCertificate360History
      (long certificateID)
    {
      var param = new DynamicParameters();
      param.Add("@CertificateID", certificateID);

      return (await SqlMapper.QueryAsync<Certificate360History>(
        _unitOfWork.Connection,
        "GetCertificate360History", param,
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction)).ToList();
    }

    public async Task<IList<Ingredient>> GetCertificate360Ingredients
     (long certificateID)
    {
      var param = new DynamicParameters();
      param.Add("@CertificateID", certificateID);

      return (await SqlMapper.QueryAsync<Ingredient>(
        _unitOfWork.Connection,
        "GetCertificate360Ingredients", param,
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction)).ToList();
    }

    public async Task<IList<Certificate360>> GetCertificate360WithIngredient
      (Certificate360IngredientFilter filter)
    {
      var param = new DynamicParameters();
      param.Add("@Name", filter.Name);
      param.Add("@Brand", filter.BrandName);
      param.Add("@SupplierName", filter.SupplierName);
      param.Add("@CertifyingBody", filter.CertifyingBodyName);

      var mapper = new Certificate360Mapper();

      return (await SqlMapper.QueryAsync(
          _unitOfWork.Connection,
          "GetCertificate360WithIngredient",
          new[]
          {
            typeof(Certificate360),
            typeof(Customer),
            typeof(Code),
            typeof(Code),
            typeof(Officer),
            typeof(Premise)
          },
          obj =>
          {
            var cert = obj[0] as Certificate360;
            var certCustomer = obj[1] as Customer;
            var certCustCode = obj[2] as Code;
            var certCustGroupCode = obj[3] as Code;
            var certCustOfficer = obj[4] as Officer;
            var certPremise = obj[5] as Premise;
            return mapper.Map(cert,
              certCustomer,
              certCustCode,
              certCustGroupCode,
              certCustOfficer,
              certPremise);
          },
          param,
          splitOn: "ID, CustID, CustCodeID, CustCodeGroupID, OfficerID, PremID",
          commandType: CommandType.StoredProcedure,
          transaction: _unitOfWork.Transaction)).ToList();
    }

    public async Task<IList<Menu>> GetCertificate360Menus
     (long certificateID)
    {
      var param = new DynamicParameters();
      param.Add("@CertificateID", certificateID);

      return (await SqlMapper.QueryAsync<Menu>(
        _unitOfWork.Connection,
        "GetCertificate360Menus", param,
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction)).ToList();
    }

    public async Task<long> InsertCertificate360(Certificate360 entity)
    {
      var param = new DynamicParameters();
      param.Add("@Number", entity.Number);
      param.Add("@Status", entity.Status);
      param.Add("@Template", entity.Template);
      param.Add("@Scheme", entity.Scheme);
      param.Add("@SubScheme", entity.SubScheme);
      param.Add("@IssuedOn", entity.IssuedOn);
      param.Add("@ExpiresOn", entity.ExpiresOn);
      param.Add("@SerialNo", entity.SerialNo);
      param.Add("@CustomerID", entity.CustomerID);
      param.Add("@CustomerName", entity.CustomerName);
      param.Add("@RequestorID", entity.RequestorID);
      param.Add("@RequestorName", entity.RequestorName);
      param.Add("@AgentID", entity.AgentID);
      param.Add("@AgentName", entity.AgentName);
      param.Add("@PremiseID", entity.PremiseID);
      param.Add("@ID", dbType: DbType.Int64, direction: ParameterDirection.Output);

      await SqlMapper.ExecuteAsync(_unitOfWork.Connection,
        "InsertCertificate360",
        param,
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction);

      return param.Get<long>("@ID");
    }

    public async Task UpdateCertificate360(Certificate360 entity)
    {
      var param = new DynamicParameters();
      param.Add("@ID", entity.ID);
      param.Add("@Number", entity.Number);
      param.Add("@Status", entity.Status);
      param.Add("@Template", entity.Template);
      param.Add("@Scheme", entity.Scheme);
      param.Add("@SubScheme", entity.SubScheme);
      param.Add("@IssuedOn", entity.IssuedOn);
      param.Add("@ExpiresOn", entity.ExpiresOn);
      param.Add("@SerialNo", entity.SerialNo);
      param.Add("@CustomerID", entity.CustomerID);
      param.Add("@CustomerName", entity.CustomerName);
      param.Add("@RequestorID", entity.RequestorID);
      param.Add("@RequestorName", entity.RequestorName);
      param.Add("@AgentID", entity.AgentID);
      param.Add("@AgentName", entity.AgentName);
      param.Add("@PremiseID", entity.PremiseID);

      await SqlMapper.ExecuteAsync(_unitOfWork.Connection,
        "UpdateCertificate360",
        param,
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction);
    }

    public async Task<long> InsertCertificate360History(Certificate360History entity)
    {
      var param = new DynamicParameters();
      param.Add("@RequestID", entity.RequestID);
      param.Add("@RefID", entity.RefID);
      param.Add("@RequestorID", entity.RequestorID);
      param.Add("@RequestorName", entity.RequestorName);
      param.Add("@AgentID", entity.AgentID);
      param.Add("@AgentName", entity.AgentName);
      param.Add("@Duration", entity.Duration);
      param.Add("@IssuedOn", entity.IssuedOn);
      param.Add("@ExpiresOn", entity.ExpiresOn);
      param.Add("@SerialNo", entity.SerialNo);
      param.Add("@ApprovedOn", entity.ApprovedOn);
      param.Add("@ApprovedBy", entity.ApprovedBy);
      param.Add("@CertificateID", entity.CertificateID);
      param.Add("@ID", dbType: DbType.Int64, direction: ParameterDirection.Output);

      await SqlMapper.ExecuteAsync(_unitOfWork.Connection,
        "InsertCertificate360History",
        param,
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction);

      return param.Get<long>("@ID");
    }

    public async Task DeleteAllCertificate360Menus(long certificateID)
    {
      var param = new DynamicParameters();
      param.Add("@ID", certificateID);

      await SqlMapper.ExecuteAsync(_unitOfWork.Connection,
        "DeleteAllCertificate360Menu",
        param,
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction);
    }

    public async Task DeleteAllCertificate360Ingredients(long certificateID)
    {
      var param = new DynamicParameters();
      param.Add("@ID", certificateID);

      await SqlMapper.ExecuteAsync(_unitOfWork.Connection,
        "DeleteAllCertificate360Ingredients",
        param,
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction);
    }

    public async Task DeleteAllCertificate360Teams(long certificateID)
    {
      var param = new DynamicParameters();
      param.Add("@ID", certificateID);

      await SqlMapper.ExecuteAsync(_unitOfWork.Connection,
        "DeleteAllCertificate360Teams",
        param,
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction);
    }

    public async Task MapCertificate360ToMenus(long certificateID, IList<long> menuIDs)
    {
      var tIDs = new DataTable();
      tIDs.Columns.Add("Val", typeof(long));

      if (menuIDs?.Any() ?? false)
      {
        foreach (var menuID in menuIDs)
        {
          tIDs.Rows.Add(menuID);
        }
      }

      var param = new DynamicParameters();
      param.Add("@ID", certificateID);
      param.Add("@MenuIDs", tIDs.AsTableValuedParameter("dbo.BigIntType"));

      await SqlMapper.ExecuteAsync(_unitOfWork.Connection,
        "MapCertificate360ToMenus",
        param,
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction);
    }

    public async Task MapCertificate360ToIngredients(long certificateID, IList<long> menuIDs)
    {
      var tIDs = new DataTable();
      tIDs.Columns.Add("Val", typeof(long));

      if (menuIDs?.Any() ?? false)
      {
        foreach (var menuID in menuIDs)
        {
          tIDs.Rows.Add(menuID);
        }
      }

      var param = new DynamicParameters();
      param.Add("@ID", certificateID);
      param.Add("@IngredientIDs", tIDs.AsTableValuedParameter("dbo.BigIntType"));

      await SqlMapper.ExecuteAsync(_unitOfWork.Connection,
        "MapCertificate360ToIngredients",
        param,
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction);
    }

    public async Task MapCertificate360ToTeams(long certificateID, IList<long> menuIDs)
    {
      var tIDs = new DataTable();
      tIDs.Columns.Add("Val", typeof(long));

      if (menuIDs?.Any() ?? false)
      {
        foreach (var menuID in menuIDs)
        {
          tIDs.Rows.Add(menuID);
        }
      }

      var param = new DynamicParameters();
      param.Add("@ID", certificateID);
      param.Add("@TeamIDs", tIDs.AsTableValuedParameter("dbo.BigIntType"));

      await SqlMapper.ExecuteAsync(_unitOfWork.Connection,
        "MapCertificate360ToTeams",
        param,
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction);
    }

    public async Task<long> InsertAutoRenewalTriggerLog(string number, DateTimeOffset? expiresOn)
    {
      var param = new DynamicParameters();
      param.Add("@Number", number);
      param.Add("@ExpiresOn", expiresOn);
      param.Add("@ID", dbType: DbType.Int64, direction: ParameterDirection.Output);

      await SqlMapper.ExecuteAsync(_unitOfWork.Connection,
        "InsertAutoRenewalLog",
        param,
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction);

      return param.Get<long>("@ID");
    }
  }
}
