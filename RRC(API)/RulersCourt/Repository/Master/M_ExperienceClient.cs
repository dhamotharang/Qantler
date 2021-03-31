using RulersCourt.Models;
using RulersCourt.Translators;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Repository
{
    public class M_ExperienceClient
    {
        public List<M_ExperienceModel> GetExperience(string connString, int userID, string lang)
        {
            SqlParameter[] parama = { new SqlParameter("@P_Language", lang) };
            return SqlHelper.ExecuteProcedureReturnData<List<M_ExperienceModel>>(connString, "Get_M_Experience", r => r.TranslateAsExperience(), parama);
        }

        public string PostExperience(string connString, int userID, M_MasterLookupsPostModel experience, string lang)
        {
            SqlParameter[] parama = { new SqlParameter("@P_DisplayName", experience.DisplayName),
                                    new SqlParameter("@P_ARDisplayName", experience.ArDisplayName),
                                    new SqlParameter("@P_DisplayOrder", experience.DisplayOrder),
                                    new SqlParameter("@P_CreatedBy", userID),
                                    new SqlParameter("@P_Language", lang) };
            return SqlHelper.ExecuteProcedureReturnString(connString, "Save_M_Experience", parama);
        }

        public string PutExperience(string connString, int userID, M_MasterLookupsPutModel experience, string lang)
        {
            SqlParameter[] parama = { new SqlParameter("@P_LookupsID", experience.LookupsID),
                                    new SqlParameter("@P_DisplayName", experience.DisplayName),
                                    new SqlParameter("@P_ARDisplayName", experience.ArDisplayName),
                                    new SqlParameter("@P_DisplayOrder", experience.DisplayOrder),
                                    new SqlParameter("@P_UpdatedBy", userID),
                                    new SqlParameter("@P_Language", lang) };
            return SqlHelper.ExecuteProcedureReturnString(connString, "Save_M_Experience", parama);
        }

        public string DeleteEmployeeStatus(string connString, int userID, int lookupID)
        {
            SqlParameter[] parama = { new SqlParameter("@P_LookupsID", lookupID),
                                    new SqlParameter("@P_DeleteFlag", true),
                                    new SqlParameter("@P_UpdatedBy", userID) };
            return SqlHelper.ExecuteProcedureReturnString(connString, "Save_M_Experience", parama);
        }
    }
}
