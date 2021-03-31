using RulersCourt.Models;
using RulersCourt.Translators;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Repository
{
    public class M_EmiratesClient
    {
        public List<M_EmiratesModel> GetEmirates(string connString, int userID, string lang)
        {
            SqlParameter[] parama = { new SqlParameter("@P_Language", lang) };
            return SqlHelper.ExecuteProcedureReturnData<List<M_EmiratesModel>>(connString, "Get_M_Emirates", r => r.TranslateAsEmirates(), parama);
        }

        public string PostEmirates(string connString, int userID, M_MasterLookupsPostModel emirates, string lang)
        {
            SqlParameter[] parama = { new SqlParameter("@P_DisplayName", emirates.DisplayName),
                                    new SqlParameter("@P_ARDisplayName", emirates.ArDisplayName),
                                    new SqlParameter("@P_DisplayOrder", emirates.DisplayOrder),
                                    new SqlParameter("@P_CreatedBy", userID),
                                    new SqlParameter("@P_Language", lang) };
            return SqlHelper.ExecuteProcedureReturnString(connString, "Save_M_Emirates", parama);
        }

        public string PutEmirates(string connString, int userID, M_MasterLookupsPutModel emirates, string lang)
        {
            SqlParameter[] parama = { new SqlParameter("@P_LookupsID", emirates.LookupsID),
                                    new SqlParameter("@P_DisplayName", emirates.DisplayName),
                                    new SqlParameter("@P_ARDisplayName", emirates.ArDisplayName),
                                    new SqlParameter("@P_DisplayOrder", emirates.DisplayOrder),
                                    new SqlParameter("@P_UpdatedBy", userID),
                                    new SqlParameter("@P_Language", lang) };
            return SqlHelper.ExecuteProcedureReturnString(connString, "Save_M_Emirates", parama);
        }

        public string DeleteEmirates(string connString, int userID, int lookupID)
        {
            SqlParameter[] parama = { new SqlParameter("@P_LookupsID", lookupID),
                                    new SqlParameter("@P_DeleteFlag", true),
                                    new SqlParameter("@P_UpdatedBy", userID) };
            return SqlHelper.ExecuteProcedureReturnString(connString, "Save_M_Emirates", parama);
        }
    }
}
