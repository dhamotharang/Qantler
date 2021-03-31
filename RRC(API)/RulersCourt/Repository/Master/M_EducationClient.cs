using RulersCourt.Models;
using RulersCourt.Translators;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Repository
{
    public class M_EducationClient
    {
        public List<M_EducationModel> GetEducation(string connString, int userID, string lang)
        {
            SqlParameter[] parama = { new SqlParameter("@P_Language", lang) };
            return SqlHelper.ExecuteProcedureReturnData<List<M_EducationModel>>(connString, "Get_M_Education", r => r.TranslateAsEducation(), parama);
        }

        public string PostEducation(string connString, int userID, M_MasterLookupsPostModel education, string lang)
        {
            SqlParameter[] parama = { new SqlParameter("@P_DisplayName", education.DisplayName),
                                    new SqlParameter("@P_ARDisplayName", education.ArDisplayName),
                                    new SqlParameter("@P_DisplayOrder", education.DisplayOrder),
                                    new SqlParameter("@P_CreatedBy", userID),
                                    new SqlParameter("@P_Language", lang) };
            return SqlHelper.ExecuteProcedureReturnString(connString, "Save_M_Education", parama);
        }

        public string PutEducation(string connString, int userID, M_MasterLookupsPutModel education, string lang)
        {
            SqlParameter[] parama = { new SqlParameter("@P_LookupsID", education.LookupsID),
                                    new SqlParameter("@P_DisplayName", education.DisplayName),
                                    new SqlParameter("@P_ARDisplayName", education.ArDisplayName),
                                    new SqlParameter("@P_DisplayOrder", education.DisplayOrder),
                                    new SqlParameter("@P_UpdatedBy", userID),
                                    new SqlParameter("@P_Language", lang) };
            return SqlHelper.ExecuteProcedureReturnString(connString, "Save_M_Education", parama);
        }

        public string DeleteEducation(string connString, int userID, int lookupID)
        {
            SqlParameter[] parama = { new SqlParameter("@P_LookupsID", lookupID),
                                    new SqlParameter("@P_DeleteFlag", true),
                                    new SqlParameter("@P_UpdatedBy", userID) };
            return SqlHelper.ExecuteProcedureReturnString(connString, "Save_M_Education", parama);
        }
    }
}
