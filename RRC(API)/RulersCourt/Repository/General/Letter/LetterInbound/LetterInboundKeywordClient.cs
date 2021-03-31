using RulersCourt.Models.Letter.LetterInbound;
using RulersCourt.Translators.Letter.LetterInbound;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Repository.Letter.LetterInbound
{
    public class LetterInboundKeywordClient
    {
        public void SaveUser(string connString, List<LetterInboundKeywordsModel> keyword, int? letterId, int? userID)
        {
            SqlParameter[] getKeywordparam = {
                    new SqlParameter("@P_LetterID", letterId) };
            var oldKeyword = SqlHelper.ExecuteProcedureReturnData<List<LetterInboundKeywordsModel>>(connString, "Get_LetterInboundKeywords", r => r.TranslateAsLetterInboundKeywordsList(), getKeywordparam);

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
                            SqlHelper.ExecuteProcedureReturnString(connString, "Save_LetterInboundKeywords", keywordparam);
                        }
                    }
                    else
                    {
                        SqlParameter[] keywordparam = {
                            new SqlParameter("@P_LetterID", letterId),
                            new SqlParameter("@P_UserID", userID),
                            new SqlParameter("@P_Keywords", item.Keywords),
                            new SqlParameter("@P_Type", 1) };
                        SqlHelper.ExecuteProcedureReturnString(connString, "Save_LetterInboundKeywords", keywordparam);
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
                    SqlHelper.ExecuteProcedureReturnString(connString, "Save_LetterInboundKeywords", destinationparam);
                }
            }
        }
    }
}
