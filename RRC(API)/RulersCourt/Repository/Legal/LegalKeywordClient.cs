using RulersCourt.Models;
using RulersCourt.Translators.Legal;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace RulersCourt.Repository.Legal
{
    public class LegalKeywordClient
    {
        public void SaveUser(string connString, List<LegalKeywordsModel> keywords, int? legalId, int? userID)
        {
            string key = string.Join(";", from item in keywords select item.Keywords);
            SqlParameter[] deleteKeywordparam = {
                            new SqlParameter("@P_LegalID", legalId),
                            new SqlParameter("@P_UserID", userID),
                            new SqlParameter("@P_Keywords", key)
            };
            SqlHelper.ExecuteProcedureReturnString(connString, "Save_LegalKeywords", deleteKeywordparam);
        }
    }
}