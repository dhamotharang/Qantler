using Core.API.Repository;
using Dapper;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Case.API.Repository
{
  public class ActivityRepository : IActivityRepository
  {
    readonly IUnitOfWork _unitOfWork;

    public ActivityRepository(IUnitOfWork unitOfWork)
    {
      _unitOfWork = unitOfWork;
    }

    public async Task<long> InsertActivity(Model.Activity data, long caseID)
    {
      var caseParam = new DynamicParameters();

      caseParam.Add("@Type", data.Type);
      caseParam.Add("@RefID", data.RefID);
      caseParam.Add("@Action", data.Action);
      caseParam.Add("@Notes", data.Notes);
      caseParam.Add("@CaseID", caseID);
      caseParam.Add("@UserID", data.User?.ID ?? null);
      caseParam.Add("@Out", dbType: DbType.Int64, direction: ParameterDirection.Output);

      await SqlMapper.ExecuteAsync(_unitOfWork.Connection,
        "InsertActivity",
        caseParam,
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction);

      var activityID = caseParam.Get<long>("@Out");

      return activityID;
    }

    public async Task MapActivityAttachments(long activityID, params long[] attachmentIDs)
    {
      var tAttachmentIDs = new DataTable();
      tAttachmentIDs.Columns.Add("Val", typeof(long));

      if (attachmentIDs?.Any() ?? false)
      {
        foreach (var id in attachmentIDs)
        {
          tAttachmentIDs.Rows.Add(id);
        }
      }

      var param = new DynamicParameters();
      param.Add("@ActivityID", activityID);
      param.Add("@AttachmentIDs", tAttachmentIDs.AsTableValuedParameter("dbo.BigIntType"));

      await SqlMapper.ExecuteAsync(_unitOfWork.Connection,
        "MapActivityAttachments",
        param,
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction);
    }

    public async Task MapActivityLetter(long activityID, long letterID)
    {
      var param = new DynamicParameters();
      param.Add("@ActivityID", activityID);
      param.Add("@LetterID", letterID);

      await SqlMapper.ExecuteAsync(_unitOfWork.Connection,
        "MapActivityLetter",
        param,
        commandType: CommandType.StoredProcedure,
        transaction: _unitOfWork.Transaction);
    }

    public async Task DeleteByID(long activityID)
    {
      await SqlMapper.ExecuteAsync(_unitOfWork.Connection,
        $"UPDATE [Activity] SET [IsDeleted] = 1 WHERE [ID] = {activityID}",
        transaction: _unitOfWork.Transaction);
    }
  }
}
