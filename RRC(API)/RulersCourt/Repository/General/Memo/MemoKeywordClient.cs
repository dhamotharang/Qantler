using RulersCourt.Models;
using RulersCourt.Translators;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Repository
{
    public class MemoKeywordClient
    {
        public void SaveUser(string connString, List<MemoKeywordsModel> keyword, int? memoId, int? userID)
        {
            SqlParameter[] getKeywordparam = {
                    new SqlParameter("@P_MemoID", memoId) };
            var oldKeyword = SqlHelper.ExecuteProcedureReturnData<List<MemoKeywordsModel>>(connString, "Get_MemoKeywords", r => r.TranslateAsKeywordsList(), getKeywordparam);

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
                            new SqlParameter("@P_MemoID", memoId),
                            new SqlParameter("@P_UserID", userID),
                            new SqlParameter("@P_Keywords", item.Keywords),
                            new SqlParameter("@P_Type", 1) };
                            SqlHelper.ExecuteProcedureReturnString(connString, "Save_MemoKeywords", keywordparam);
                        }
                    }
                    else
                    {
                        SqlParameter[] keywordparam = {
                            new SqlParameter("@P_MemoID", memoId),
                            new SqlParameter("@P_UserID", userID),
                            new SqlParameter("@P_Keywords", item.Keywords),
                            new SqlParameter("@P_Type", 1) };
                        SqlHelper.ExecuteProcedureReturnString(connString, "Save_MemoKeywords", keywordparam);
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
                    new SqlParameter("@P_MemoID", memoId),
                    new SqlParameter("@P_UserID", userID),
                    new SqlParameter("@P_Keywords", item.Keywords),
                    new SqlParameter("@P_Type", 2) };
                    SqlHelper.ExecuteProcedureReturnString(connString, "Save_MemoKeywords", destinationparam);
                }
            }
        }
    }
}
