using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Core.API.Repository;
using Core.Model;
using Dapper;
using Request.API.Repository.Mappers;
using Request.Model;

namespace Request.API.Repository
{
  public class CertificateRepository : ICertificateRepository
  {
    readonly IUnitOfWork _unitOfWork;

    public CertificateRepository(IUnitOfWork unitOfWork)
    {
      _unitOfWork = unitOfWork;
    }

    public async Task<IList<Certificate>> CertificateFilter(CertificateFilter filter)
    {
      var statusList = new DataTable();
      statusList.Columns.Add("Val", typeof(long));

      if (filter.IDs?.Any() ?? false)
      {
        foreach (var id in filter.IDs)
        {
          statusList.Rows.Add(id);
        }
      }

      var param = new DynamicParameters();
      param.Add("@IDs", statusList.AsTableValuedParameter("dbo.BigIntType"));
      param.Add("@PremiseID", filter.PremiseID);
      var mapper = new CertificateMapper();

      return (await SqlMapper.QueryAsync<Certificate>(_unitOfWork.Connection,
        "SelectCertificate",
        new[]
        {
            typeof(Certificate),
            typeof(Premise),
            typeof(Premise),
            typeof(Menu)
        },
        obj =>
        {
          return mapper.Map(obj[0] as Certificate,
            obj[1] as Premise,
            obj[2] as Premise,
            obj[3] as Menu);
        },
        param,
        splitOn: "ID,PremID,MailPremID,MenuID",
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction)).Distinct().ToList();
    }

    public async Task<IList<CertificateBatch>> BatchFilter(CertificateBatchFilter filter)
    {
      var statusList = new DataTable();
      statusList.Columns.Add("Val", typeof(int));

      if (filter.Status?.Any() ?? false)
      {
        foreach (var status in filter.Status)
        {
          statusList.Rows.Add((int)status);
        }
      }

      var param = new DynamicParameters();
      param.Add("@From", filter.From);
      param.Add("@To", filter.To);
      param.Add("@RequestID", filter.RequestID);
      param.Add("@Status", statusList.AsTableValuedParameter("dbo.SmallIntType"));

      return (await SqlMapper.QueryAsync<CertificateBatch>(_unitOfWork.Connection,
        "SelectCertificateBatch",
        param,
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction)).Distinct().ToList();
    }

    public async Task<IList<CertificateBatch>> BatchFullFilter(CertificateBatchFilter filter)
    {
      var statusList = new DataTable();
      statusList.Columns.Add("Val", typeof(int));

      if (filter.Status?.Any() ?? false)
      {
        foreach (var status in filter.Status)
        {
          statusList.Rows.Add((int)status);
        }
      }

      var param = new DynamicParameters();
      param.Add("@From", filter.From);
      param.Add("@To", filter.To);
      param.Add("@RequestID", filter.RequestID);
      param.Add("@Status", statusList.AsTableValuedParameter("dbo.SmallIntType"));

      var mapper = new CertificateBatchMapper();

      return (await SqlMapper.QueryAsync(_unitOfWork.Connection,
        "SelectCertificateBatchFull",
        new[]
        {
            typeof(CertificateBatch),
            typeof(Certificate),
            typeof(Premise),
            typeof(Premise),
            typeof(Comment)
        },
        obj =>
        {
          return mapper.Map(obj[0] as CertificateBatch,
            obj[1] as Certificate,
            obj[2] as Premise,
            obj[3] as Premise,
            obj[4] as Comment);
        },
        param,
        splitOn: "ID,CertificateID,PremID,MailPremID,CommentID",
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction)).Distinct().ToList();
    }

    public async Task<CertificateBatch> GetCertificateBatchByID(long id)
    {
      var param = new DynamicParameters();
      param.Add("@ID", id);

      var mapper = new CertificateBatchMapper();

      return (await SqlMapper.QueryAsync(_unitOfWork.Connection,
        "GetCertificateBatchByID",
        new[]
        {
            typeof(CertificateBatch),
            typeof(Certificate),
            typeof(Premise),
            typeof(Premise),
            typeof(Comment)
        },
        obj =>
        {
          return mapper.Map(obj[0] as CertificateBatch,
            obj[1] as Certificate,
            obj[2] as Premise,
            obj[3] as Premise,
            obj[4] as Comment);
        },
        param,
        splitOn: "ID,CertificateID,PremID,MailPremID,CommentID",
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction)).FirstOrDefault();
    }

    public async Task<CertificateBatch> GetCertificateBatchByIDFull(long id)
    {
      var param = new DynamicParameters();
      param.Add("@ID", id);

      var mapper = new CertificateBatchMapper();

      return (await SqlMapper.QueryAsync(_unitOfWork.Connection,
        "GetCertificateBatchByIDFull",
        new[]
        {
            typeof(CertificateBatch),
            typeof(Certificate),
            typeof(Premise),
            typeof(Premise),
            typeof(Comment),
            typeof(Menu)
        },
        obj =>
        {
          return mapper.Map(obj[0] as CertificateBatch,
            obj[1] as Certificate,
            obj[2] as Premise,
            obj[3] as Premise,
            obj[4] as Comment,
            obj[5] as Menu);
        },
        param,
        splitOn: "ID,CertificateID,PremID,MailPremID,CommentID,MenuID",
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction)).FirstOrDefault();
    }

    public async Task<CertificateBatch> GetCertificateBatchByIDBasic(long id)
    {
      var param = new DynamicParameters();
      param.Add("@ID", id);

      return (await SqlMapper.QueryAsync<CertificateBatch>(_unitOfWork.Connection,
        "GetCertificateBatchByIDBasic",
        param,
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction)).FirstOrDefault();
    }

    public async Task<CertificateBatch> GetCertificateBatchByCode(string code)
    {
      var param = new DynamicParameters();
      param.Add("@Code", code);

      return (await SqlMapper.QueryAsync<CertificateBatch>(_unitOfWork.Connection,
        "GetCertificateBatchByCode",
        param,
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction)).FirstOrDefault();
    }

    public async Task<long> InsertCertificateBatch(CertificateBatch batch)
    {
      var param = new DynamicParameters();
      param.Add("@Code", batch.Code);
      param.Add("@Description", batch.Description);
      param.Add("@Template", batch.Template);

      param.Add("@ID", dbType: DbType.Int64, direction: ParameterDirection.Output);

      await SqlMapper.QueryAsync<CertificateBatch>(_unitOfWork.Connection,
        "InsertCertificateBatch",
        param,
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction);

      return param.Get<long>("@ID");
    }

    public async Task<long> InsertComment(long batchID, string text, Officer officer)
    {
      var param = new DynamicParameters();
      param.Add("@BatchID", batchID);
      param.Add("@Text", text);
      param.Add("@UserID", officer.ID);
      param.Add("@UserName", officer.Name);
      param.Add("@ID", dbType: DbType.Int64, direction: ParameterDirection.Output);

      await SqlMapper.ExecuteAsync(_unitOfWork.Connection,
        "InsertComment",
        param,
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction);

      return param.Get<long>("@ID");
    }

    public async Task<Comment> GetCommentByID(long id)
    {
      var param = new DynamicParameters();
      param.Add("@ID", id);

      return (await SqlMapper.QueryAsync<Comment>(_unitOfWork.Connection,
        "GetCommentByID",
        param,
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction)).FirstOrDefault();
    }

    public async Task<IList<Comment>> GetCertificateBatchComments(long id)
    {
      var param = new DynamicParameters();
      param.Add("@BatchID", id);

      return (await SqlMapper.QueryAsync<Comment>(_unitOfWork.Connection,
        "SelectComments",
        param,
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction)).ToList();
    }

    public async Task ExecAcknowledgeBatch(long batchID, Officer user)
    {
      var param = new DynamicParameters();
      param.Add("@BatchID", batchID);
      param.Add("@UserID", user.ID.ToString());
      param.Add("@UserName", user.Name);

      await SqlMapper.ExecuteAsync(_unitOfWork.Connection,
        "AcknowledgeCertificateBatch",
        param,
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction);
    }

    public async Task<int> GenerateCertificateSequence()
    {
      var param = new DynamicParameters();
      param.Add("@Result", dbType: DbType.Int32, direction: ParameterDirection.Output);

      await SqlMapper.ExecuteAsync(_unitOfWork.Connection,
        "GenerateCertificateSequence",
        param,
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction);

      return param.Get<int>("@Result");
    }

    public async Task<int> GenerateCertificateSerialNo()
    {
      var param = new DynamicParameters();
      param.Add("@Result", dbType: DbType.Int32, direction: ParameterDirection.Output);

      await SqlMapper.ExecuteAsync(_unitOfWork.Connection,
        "GenerateCertificateSerialNo",
        param,
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction);

      return param.Get<int>("@Result");
    }

    public async Task<Certificate360> GetCertificate360ByNo(string certificateNo)
    {
      var param = new DynamicParameters();
      param.Add("@Number", certificateNo);

      var mapper = new Certificate360Mapper();

      return (await SqlMapper.QueryAsync(_unitOfWork.Connection,
        "GetCertificate360ByNo",
        new[]
        {
            typeof(Certificate360)
        },
        obj =>
        {
          var cert = obj[0] as Certificate360;
          return mapper.Map(cert);
        },
        param,
        splitOn: "ID",
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction)).FirstOrDefault();
    }

    public async Task<long> InsertCertificate(Certificate certificate)
    {
      var param = new DynamicParameters();
      param.Add("@RequestType", certificate.RequestType);
      param.Add("@Number", certificate.Number);
      param.Add("@Status", certificate.Status);
      param.Add("@CodeID", certificate.CodeID);
      param.Add("@Template", certificate.Template);
      param.Add("@IsCTC", certificate.IsCertifiedTrueCopy);
      param.Add("@Scheme", certificate.Scheme);
      param.Add("@SubScheme", certificate.SubScheme);
      param.Add("@IssuedOn", certificate.IssuedOn);
      param.Add("@StartsFrom", certificate.StartsFrom);
      param.Add("@ExpiresOn", certificate.ExpiresOn);
      param.Add("@SerialNo", certificate.SerialNo);
      param.Add("@CustomerID", certificate.CustomerID);
      param.Add("@CustomerName", certificate.CustomerName);
      param.Add("@PremiseID", certificate.PremiseID);
      param.Add("@MailingPremiseID", certificate.MailingPremiseID);
      param.Add("@Remarks", certificate.Remarks);
      param.Add("@RequestID", certificate.RequestID);
      param.Add("@ID", dbType: DbType.Int64, direction: ParameterDirection.Output);

      await SqlMapper.ExecuteAsync(_unitOfWork.Connection,
        "InsertCertificate",
        param,
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction);

      var certificateID = param.Get<long>("@ID");

      if (certificate.Menus?.Any() ?? false)
      {
        foreach (var menu in certificate.Menus)
        {
          var menuParam = new DynamicParameters();
          menuParam.Add("@Group", menu.Group);
          menuParam.Add("@Index", menu.Index);
          menuParam.Add("@Scheme", menu.Scheme);
          menuParam.Add("@Text", menu.Text);
          menuParam.Add("@SubText", menu.SubText);
          menuParam.Add("@ChangeType", menu.ChangeType);
          menuParam.Add("@ID", dbType: DbType.Int64, direction: ParameterDirection.Output);

          await SqlMapper.ExecuteAsync(_unitOfWork.Connection,
            "InsertMenu",
            menuParam,
            commandType: CommandType.StoredProcedure,
            transaction: _unitOfWork.Transaction);

          var menuID = menuParam.Get<long>("@ID");

          var mapParam = new DynamicParameters();
          mapParam.Add("@CertificateID", certificateID);
          mapParam.Add("@MenuID", menuID);

          await SqlMapper.ExecuteAsync(_unitOfWork.Connection,
            "MapMenuToCertificate",
            mapParam,
            commandType: CommandType.StoredProcedure,
            transaction: _unitOfWork.Transaction);
        }
      }

      return certificateID;
    }

    public async Task MapCertificate(long batchID, long requestID, Scheme scheme, long certificateID)
    {
      var param = new DynamicParameters();
      param.Add("@BatchID", batchID);
      param.Add("@RequestID", requestID);
      param.Add("@Scheme", scheme);
      param.Add("@CertificateID", certificateID);

      await SqlMapper.ExecuteAsync(_unitOfWork.Connection,
        "MapCertificate",
        param,
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction);
    }

    public async Task MapCertificateBatchToFile(long batchID, Guid fileID)
    {
      var param = new DynamicParameters();
      param.Add("@BatchID", batchID);
      param.Add("@FileID", fileID);

      await SqlMapper.ExecuteAsync(_unitOfWork.Connection,
        "MapCertificateBatchToFile",
        param,
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction);
    }

    public async Task UpdateCertificateBatchStatus(long batchID, CertificateBatchStatus status)
    {
      var param = new DynamicParameters();
      param.Add("@BatchID", batchID);
      param.Add("@Status", status);

      await SqlMapper.ExecuteAsync(_unitOfWork.Connection,
        "UpdateCertificateBatchStatus",
        param,
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction);
    }

    public async Task<IList<Certificate>> CertDeliveryFilter(CertificateDeliveryFilter filter)
    {
      var param = new DynamicParameters();

      var statusList = new DataTable();
      statusList.Columns.Add("Val", typeof(int));

      if (filter.Status?.Any() ?? false)
      {
        foreach (var status in filter.Status)
        {
          statusList.Rows.Add((int)status);
        }
      }

      param.Add("@CustomerCode", filter.CustomerCode);
      param.Add("@CustomerName", filter.CustomerName);
      param.Add("@Premise", filter.Premise);
      param.Add("@PostalCode", filter.Postal);
      param.Add("@Status", statusList.AsTableValuedParameter("dbo.SmallIntType"));
      param.Add("@IssuedOnFrom", filter.IssuedOnFrom);
      param.Add("@IssuedOnTo", filter.IssuedOnTo);
      param.Add("@SerialNo", filter.SerialNo);

      var mapper = new CertificateMapper();

      return (await SqlMapper.QueryAsync<Certificate>(_unitOfWork.Connection,
        "GetCertificatesForDelivery",
        new[]
        {
                  typeof(Certificate),
                  typeof(Premise),
                  typeof(Premise),
                  typeof(Code)
        },
        obj =>
        {
          return mapper.Map(obj[0] as Certificate,
            obj[1] as Premise,
            obj[2] as Premise,
            obj[3] as Code);
        },
        param,
        splitOn: "ID,PremID,MailPremID,CodeID",
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction)).Distinct().ToList();
    }

    public async Task UpdateCertificateDeliveryStatus(long certificateID, 
      CertificateDeliveryStatus status)
    {
      var param = new DynamicParameters();
      param.Add("@CertificateID", certificateID);
      param.Add("@Status", status);

      await SqlMapper.ExecuteAsync(_unitOfWork.Connection,
        "ExecCertificateDelivery",
        param,
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction);
    }

    public async Task<Certificate> GetByRequestID(long requestID)
    {
      return (await SqlMapper.QueryAsync<Certificate>(_unitOfWork.Connection,
        $"SELECT * FROM [Certificate] WHERE [RequestID] = {requestID}",
        transaction: _unitOfWork.Transaction)).FirstOrDefault();
    }
  }
}
