using RulersCourt.Models.Letter;
using RulersCourt.Translators.Letter;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Repository
{
    public class LetterOutboundKeywordClient
    {
        public void SaveUser(string connString, List<LetterOutboundKeywordsModel> keyword, int? letterId, int? userID)
        {
            SqlParameter[] getKeywordparam = {
                    new SqlParameter("@P_LetterID", letterId) };
            var oldKeyword = SqlHelper.ExecuteProcedureReturnData<List<LetterOutboundKeywordsModel>>(connString, "Get_LetterOutboundKeywords", r => r.TranslateAsLetterKeywordsList(), getKeywordparam);

            foreach (var item in keyword)
            {
                try
                {
                    if (oldKeyword.Count > 0)
                    {
                        var temp = false;
                        try
                        {
                            temp = Convert.ToBoolean(oldKeyword.Find(e => e.Keywords.Equals(item.Keywords)).Keywords);
                            temp = false;
                        }
                        catch (NullReferenceException)
                        {
                            temp = true;
                        }

                        if (temp)
                        {
                            SqlParameter[] keywordparam = {
                            new SqlParameter("@P_LetterID", letterId),
                            new SqlParameter("@P_UserID", userID),
                            new SqlParameter("@P_Keywords", item.Keywords),
                            new SqlParameter("@P_Type", 1) };
                            SqlHelper.ExecuteProcedureReturnString(connString, "Save_LetterOutboundKeywords", keywordparam);
                        }
                    }
                    else
                    {
                        SqlParameter[] keywordparam = {
                            new SqlParameter("@P_LetterID", letterId),
                            new SqlParameter("@P_UserID", userID),
                            new SqlParameter("@P_Keywords", item.Keywords),
                            new SqlParameter("@P_Type", 1) };
                        SqlHelper.ExecuteProcedureReturnString(connString, "Save_LetterOutboundKeywords", keywordparam);
                    }
                }
                catch (Exception)
                {
                }
            }

            foreach (var item in oldKeyword)
            {
                if (keyword.Find(a => a.Keywords.Equals(item.Keywords)) is null)
                {
                    SqlParameter[] destinationparam = {
                    new SqlParameter("@P_LetterID", letterId),
                    new SqlParameter("@P_UserID", userID),
                    new SqlParameter("@P_Keywords", item.Keywords),
                    new SqlParameter("@P_Type", 2) };
                    SqlHelper.ExecuteProcedureReturnString(connString, "Save_LetterOutboundKeywords", destinationparam);
                }
            }
        }
    }
}
