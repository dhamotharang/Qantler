using RulersCourt.Models;
using RulersCourt.Translators.Letter;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Repository.Letter
{
    public class LetterOutboundOrganizationEntityClient
    {
        public List<OrganisationEntityModel> GetLetterOrgEntity(string connString, int userID)
        {
            SqlParameter[] param = {
                new SqlParameter("@P_UserID", userID),
                new SqlParameter("@P_IsGoverenmentEntity", 1)
            };
            return SqlHelper.ExecuteProcedureReturnData<List<OrganisationEntityModel>>(connString, "Get_OrganizationEntity", r => r.TranslateAsGetOrganizationEntityList(), param);
        }
    }
}
