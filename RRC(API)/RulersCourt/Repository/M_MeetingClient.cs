using RulersCourt.Models;
using RulersCourt.Translators;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Repository
{
    public class M_MeetingClient
    {
        public List<M_MeetingModel> GetMeetingType(string connString, int userID, string lang)
        {
            SqlParameter[] parama = { new SqlParameter("@P_Language", lang) };

            return SqlHelper.ExecuteProcedureReturnData<List<M_MeetingModel>>(connString, "Get_M_MeetingType", r => r.TranslateAsMeetingType(), parama);
        }

        public string PostMeetingType(string connString, int userID, M_MasterLookupsPostModel meetingType, string lang)
        {
            SqlParameter[] parama = {
                new SqlParameter("@P_DisplayName", meetingType.DisplayName),
                new SqlParameter("@P_ARDisplayName", meetingType.ArDisplayName),
                new SqlParameter("@P_DisplayOrder", meetingType.DisplayOrder),
                new SqlParameter("@P_CreatedBy", userID),
                new SqlParameter("@P_Language", lang)
            };
            return SqlHelper.ExecuteProcedureReturnString(connString, "Save_M_MeetingType", parama);
        }

        public string PutMeetingType(string connString, int userID, M_MasterLookupsPutModel meetingType, string lang)
        {
            SqlParameter[] parama = {
                new SqlParameter("@P_LookupsID", meetingType.LookupsID),
                new SqlParameter("@P_DisplayName", meetingType.DisplayName),
                new SqlParameter("@P_ARDisplayName", meetingType.ArDisplayName),
                new SqlParameter("@P_DisplayOrder", meetingType.DisplayOrder),
                new SqlParameter("@P_UpdatedBy", userID),
                new SqlParameter("@P_Language", lang)
            };
            return SqlHelper.ExecuteProcedureReturnString(connString, "Save_M_MeetingType", parama);
        }

        public string DeleteMeetingType(string connString, int userID, int lookupID)
        {
            SqlParameter[] parama = {
                new SqlParameter("@P_LookupsID", lookupID),
                new SqlParameter("@P_DeleteFlag", true),
                new SqlParameter("@P_UpdatedBy", userID)
            };
            return SqlHelper.ExecuteProcedureReturnString(connString, "Save_M_MeetingType", parama);
        }
    }
}
