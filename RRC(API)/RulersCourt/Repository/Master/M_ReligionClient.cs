using RulersCourt.Models;
using RulersCourt.Translators;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Repository
{
    public class M_ReligionClient
    {
        public List<M_ReligionModel> GetReligion(string connString, int userID, string lang)
        {
            SqlParameter[] parama = { new SqlParameter("@P_Language", lang) };
            return SqlHelper.ExecuteProcedureReturnData<List<M_ReligionModel>>(connString, "Get_M_Religion", r => r.TranslateAsReligion(), parama);
        }

        public string PostReligion(string connString, int userID, M_MasterLookupsPostModel religion, string lang)
        {
            SqlParameter[] parama = { new SqlParameter("@P_DisplayName", religion.DisplayName),
                                    new SqlParameter("@P_ARDisplayName", religion.ArDisplayName),
                                    new SqlParameter("@P_DisplayOrder", religion.DisplayOrder),
                                    new SqlParameter("@P_CreatedBy", userID),
                                    new SqlParameter("@P_Language", lang) };
            return SqlHelper.ExecuteProcedureReturnString(connString, "Save_M_Religion", parama);
        }

        public string PutReligion(string connString, int userID, M_MasterLookupsPutModel religion, string lang)
        {
            SqlParameter[] parama = { new SqlParameter("@P_LookupsID", religion.LookupsID),
                                    new SqlParameter("@P_ARDisplayName", religion.ArDisplayName),
                                    new SqlParameter("@P_DisplayName", religion.DisplayName),
                                    new SqlParameter("@P_DisplayOrder", religion.DisplayOrder),
                                    new SqlParameter("@P_UpdatedBy", userID),
                                    new SqlParameter("@P_Language", lang) };
            return SqlHelper.ExecuteProcedureReturnString(connString, "Save_M_Religion", parama);
        }

        public string DeleteReligion(string connString, int userID, int lookupID)
        {
            SqlParameter[] parama = { new SqlParameter("@P_LookupsID", lookupID),
                                    new SqlParameter("@P_DeleteFlag", true),
                                    new SqlParameter("@P_UpdatedBy", userID) };
            return SqlHelper.ExecuteProcedureReturnString(connString, "Save_M_Religion", parama);
        }
    }
}
