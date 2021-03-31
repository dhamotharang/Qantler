using RulersCourt.Models;
using RulersCourt.Translators;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Repository
{
    public class M_OfficialTaskRequestClient
    {
        public List<M_OfficialTaskRequestModel> GetOfficialTaskRequest(string connString, int userID, string lang)
        {
            SqlParameter[] parama = { new SqlParameter("@P_Language", lang) };
            return SqlHelper.ExecuteProcedureReturnData<List<M_OfficialTaskRequestModel>>(connString, "Get_M_OfficialTaskRequest", r => r.TranslateAsOfficialTaskRequest(), parama);
        }

        public string PostOfficialTaskRequest(string connString, int userID, M_MasterLookupsPostModel officialTaskRequest, string lang)
        {
            SqlParameter[] parama = { new SqlParameter("@P_DisplayName", officialTaskRequest.DisplayName),
                                    new SqlParameter("@P_ARDisplayName", officialTaskRequest.ArDisplayName),
                                    new SqlParameter("@P_DisplayOrder", officialTaskRequest.DisplayOrder),
                                    new SqlParameter("@P_CreatedBy", userID),
                                    new SqlParameter("@P_Language", lang) };
            return SqlHelper.ExecuteProcedureReturnString(connString, "Save_M_OfficialTaskRequest", parama);
        }

        public string PutOfficialTaskRequest(string connString, int userID, M_MasterLookupsPutModel officialTaskRequest, string lang)
        {
            SqlParameter[] parama = { new SqlParameter("@P_LookupsID", officialTaskRequest.LookupsID),
                                    new SqlParameter("@P_DisplayName", officialTaskRequest.DisplayName),
                                    new SqlParameter("@P_ARDisplayName", officialTaskRequest.ArDisplayName),
                                    new SqlParameter("@P_DisplayOrder", officialTaskRequest.DisplayOrder),
                                    new SqlParameter("@P_UpdatedBy", userID),
                                    new SqlParameter("@P_Language", lang) };
            return SqlHelper.ExecuteProcedureReturnString(connString, "Save_M_OfficialTaskRequest", parama);
        }

        public string DeleteOfficialTaskRequest(string connString, int userID, int lookupID)
        {
            SqlParameter[] parama = { new SqlParameter("@P_LookupsID", lookupID),
                                    new SqlParameter("@P_DeleteFlag", true),
                                    new SqlParameter("@P_UpdatedBy", userID) };
            return SqlHelper.ExecuteProcedureReturnString(connString, "Save_M_OfficialTaskRequest", parama);
        }
    }
}