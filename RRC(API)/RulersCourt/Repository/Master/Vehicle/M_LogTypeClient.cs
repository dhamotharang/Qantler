using RulersCourt.Models;
using RulersCourt.Models.Master.M_Vehicle;
using RulersCourt.Translators.Master.Vehicle;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Repository.Master.Vehicle
{
    public class M_LogTypeClient
    {
        public List<M_LogTypeModel> GetLogType(string connString, int userID, string lang)
        {
            SqlParameter[] parama = { new SqlParameter("@P_Language", lang) };
            return SqlHelper.ExecuteProcedureReturnData<List<M_LogTypeModel>>(connString, "Get_M_LogType", r => r.TranslateAsLogType(), parama);
        }

        public string PostLogType(string connString, int userID, M_MasterLookupsPostModel logType, string lang)
        {
            SqlParameter[] parama = { new SqlParameter("@P_DisplayName", logType.DisplayName),
                                    new SqlParameter("@P_ARDisplayName", logType.ArDisplayName),
                                    new SqlParameter("@P_DisplayOrder", logType.DisplayOrder),
                                    new SqlParameter("@P_CreatedBy", userID),
                                    new SqlParameter("@P_Language", lang) };
            return SqlHelper.ExecuteProcedureReturnString(connString, "Save_M_LogType", parama);
        }

        public string PutLogType(string connString, int userID, M_MasterLookupsPutModel designType, string lang)
        {
            SqlParameter[] parama = { new SqlParameter("@P_LookupsID", designType.LookupsID),
                                    new SqlParameter("@P_DisplayName", designType.DisplayName),
                                    new SqlParameter("@P_ARDisplayName", designType.ArDisplayName),
                                    new SqlParameter("@P_DisplayOrder", designType.DisplayOrder),
                                    new SqlParameter("@P_UpdatedBy", userID),
                                    new SqlParameter("@P_Language", lang) };
            return SqlHelper.ExecuteProcedureReturnString(connString, "Save_M_LogType", parama);
        }

        public string DeleteLogType(string connString, int userID, int lookupID)
        {
            SqlParameter[] parama = { new SqlParameter("@P_LookupsID", lookupID),
                                    new SqlParameter("@P_DeleteFlag", true),
                                    new SqlParameter("@P_UpdatedBy", userID) };
            return SqlHelper.ExecuteProcedureReturnString(connString, "Save_M_LogType", parama);
        }
    }
}
