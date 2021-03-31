using RulersCourt.Models;
using RulersCourt.Translators;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Repository
{
    public class M_SpecializationClient
    {
        public List<M_SpecializationModel> GetSpecialization(string connString, int userID, string language)
        {
            SqlParameter[] getParama = { new SqlParameter("@P_Language", language) };
            return SqlHelper.ExecuteProcedureReturnData<List<M_SpecializationModel>>(connString, "Get_M_Specialization", r => r.TranslateAsSpecialization(), getParama);
        }

        public string PostSpecialization(string connString, int userID, M_MasterLookupsPostModel specialization, string language)
        {
            SqlParameter[] parama = { new SqlParameter("@P_DisplayName", specialization.DisplayName),
                                    new SqlParameter("@P_ARDisplayName", specialization.ArDisplayName),
                                    new SqlParameter("@P_DisplayOrder", specialization.DisplayOrder),
                                    new SqlParameter("@P_CreatedBy", userID),
                                    new SqlParameter("@P_Language", language) };
            return SqlHelper.ExecuteProcedureReturnString(connString, "Save_M_Specialization", parama);
        }

        public string PutSpecialization(string connString, int userID, M_MasterLookupsPutModel specialization, string language)
        {
            SqlParameter[] parama = { new SqlParameter("@P_LookupsID", specialization.LookupsID),
                                    new SqlParameter("@P_ARDisplayName", specialization.ArDisplayName),
                                    new SqlParameter("@P_DisplayName", specialization.DisplayName),
                                    new SqlParameter("@P_DisplayOrder", specialization.DisplayOrder),
                                    new SqlParameter("@P_UpdatedBy", userID),
                                    new SqlParameter("@P_Language", language) };
            return SqlHelper.ExecuteProcedureReturnString(connString, "Save_M_Specialization", parama);
        }

        public string DeleteSpecialization(string connString, int userID, int lookupID)
        {
            SqlParameter[] parama = { new SqlParameter("@P_LookupsID", lookupID),
                                    new SqlParameter("@P_DeleteFlag", true),
                                    new SqlParameter("@P_UpdatedBy", userID) };
            return SqlHelper.ExecuteProcedureReturnString(connString, "Save_M_Specialization", parama);
        }
    }
}