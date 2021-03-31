using RulersCourt.Models;
using RulersCourt.Translators;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Repository
{
    public class M_EventTypeClient
    {
        public List<M_EventTypeModel> GetEventType(string connString, int userID, string lang)
        {
            SqlParameter[] parama = { new SqlParameter("@P_Language", lang) };

            return SqlHelper.ExecuteProcedureReturnData<List<M_EventTypeModel>>(connString, "Get_M_EventType", r => r.TranslateAsEventType(), parama);
        }

        public string PostEventType(string connString, int userID, M_MasterLookupsPostModel eventType, string lang)
        {
            SqlParameter[] parama = {
                new SqlParameter("@P_DisplayName", eventType.DisplayName),
                new SqlParameter("@P_ARDisplayName", eventType.ArDisplayName),
                new SqlParameter("@P_DisplayOrder", eventType.DisplayOrder),
                new SqlParameter("@P_CreatedBy", userID),
                new SqlParameter("@P_Language", lang)
            };
            return SqlHelper.ExecuteProcedureReturnString(connString, "Save_M_EventType", parama);
        }

        public string PutEventType(string connString, int userID, M_MasterLookupsPutModel eventType, string lang)
        {
            SqlParameter[] parama = {
                new SqlParameter("@P_LookupsID", eventType.LookupsID),
                new SqlParameter("@P_DisplayName", eventType.DisplayName),
                new SqlParameter("@P_ARDisplayName", eventType.ArDisplayName),
                new SqlParameter("@P_DisplayOrder", eventType.DisplayOrder),
                new SqlParameter("@P_UpdatedBy", userID),
                new SqlParameter("@P_Language", lang)
            };
            return SqlHelper.ExecuteProcedureReturnString(connString, "Save_M_EventType", parama);
        }

        public string DeleteEventType(string connString, int userID, int lookupID)
        {
            SqlParameter[] parama = {
                new SqlParameter("@P_LookupsID", lookupID),
                new SqlParameter("@P_DeleteFlag", true),
                new SqlParameter("@P_UpdatedBy", userID)
            };
            return SqlHelper.ExecuteProcedureReturnString(connString, "Save_M_EventType", parama);
        }
    }
}
