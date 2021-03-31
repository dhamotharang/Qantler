using System.Threading.Tasks;

namespace Case.API.Repository
{
  public interface IActivityRepository
  {
    /// <summary>
    /// Insert activity.
    /// </summary>
    public Task<long> InsertActivity(Model.Activity data, long caseID);

    /// <summary>
    /// map attachment and activity.
    /// </summary>
    public Task MapActivityAttachments(long activityID, params long[] attachmentIDs);

    /// <summary>
    /// map letter and activity.
    /// </summary>
    public Task MapActivityLetter(long activityID, long letterID);

    /// <summary>
    /// Delete activity by ID.
    /// </summary>
    public Task DeleteByID(long activityID);
  }
}
