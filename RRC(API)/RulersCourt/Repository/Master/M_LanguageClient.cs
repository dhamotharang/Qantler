using RulersCourt.Models;
using RulersCourt.Translators;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Repository
{
    public class M_LanguageClient
    {
        public List<M_LanguageModel> GetLanguage(string connString, int userID, string lang)
        {
            SqlParameter[] parama = { new SqlParameter("@P_Language", lang) };
            return SqlHelper.ExecuteProcedureReturnData<List<M_LanguageModel>>(connString, "Get_M_Language", r => r.TranslateAsLanguage(), parama);
        }

        public string PostLanguage(string connString, int userID, M_MasterLookupsPostModel language, string lang)
        {
            SqlParameter[] parama = { new SqlParameter("@P_DisplayName", language.DisplayName),
                                    new SqlParameter("@P_ARDisplayName", language.ArDisplayName),
                                    new SqlParameter("@P_DisplayOrder", language.DisplayOrder),
                                    new SqlParameter("@P_CreatedBy", userID),
                                    new SqlParameter("@P_Language", lang) };
            return SqlHelper.ExecuteProcedureReturnString(connString, "Save_M_Language", parama);
        }

        public string PutLanguage(string connString, int userID, M_MasterLookupsPutModel language, string lang)
        {
            SqlParameter[] parama = { new SqlParameter("@P_LookupsID", language.LookupsID),
                                    new SqlParameter("@P_DisplayName", language.DisplayName),
                                    new SqlParameter("@P_ARDisplayName", language.ArDisplayName),
                                    new SqlParameter("@P_DisplayOrder", language.DisplayOrder),
                                    new SqlParameter("@P_UpdatedBy", userID),
                                    new SqlParameter("@P_Language", lang) };
            return SqlHelper.ExecuteProcedureReturnString(connString, "Save_M_Language", parama);
        }

        public string DeleteLanguage(string connString, int userID, int lookupID)
        {
            SqlParameter[] parama = { new SqlParameter("@P_LookupsID", lookupID),
                                    new SqlParameter("@P_DeleteFlag", true),
                                    new SqlParameter("@P_UpdatedBy", userID) };
            return SqlHelper.ExecuteProcedureReturnString(connString, "Save_M_Language", parama);
        }
    }
}
