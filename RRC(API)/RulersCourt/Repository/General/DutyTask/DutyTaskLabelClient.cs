using RulersCourt.Models.DutyTasks;
using RulersCourt.Translators.DutyTask;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Repository.DutyTask
{
    public class DutyTaskLabelClient
    {
        public void SaveUser(string connString, List<DutyTaskLablesModel> lables, int? memoId, int? userID, DateTime? date)
        {
            SqlParameter[] getLabelparam = {
                    new SqlParameter("@P_DutyTaskID", memoId) };
            var oldLabel = SqlHelper.ExecuteProcedureReturnData<List<DutyTaskLablesModel>>(connString, "Get_DutyTaskLabels", r => r.TranslateAsLabelList(), getLabelparam);

            foreach (var item in lables)
            {
                try
                {
                    if (oldLabel.Count > 0)
                    {
                        var temp = false;
                        try
                        {
                            temp = Convert.ToBoolean(oldLabel.Find(e => e.Labels.Equals(item.Labels)).Labels);
                            temp = false;
                        }
                        catch (NullReferenceException)
                        {
                            temp = true;
                        }

                        if (temp)
                        {
                            SqlParameter[] keywordparam = {
                            new SqlParameter("@P_DutyTaskID", memoId),
                            new SqlParameter("@P_UserID", userID),
                            new SqlParameter("@P_Labels", item.Labels),
                            new SqlParameter("@P_Type", 1),
                            new SqlParameter("@P_DateTime", date) };
                            SqlHelper.ExecuteProcedureReturnString(connString, "Save_DutyTaskLabels", keywordparam);
                        }
                    }
                    else
                    {
                        SqlParameter[] keywordparam = {
                            new SqlParameter("@P_DutyTaskID", memoId),
                            new SqlParameter("@P_UserID", userID),
                            new SqlParameter("@P_Labels", item.Labels),
                            new SqlParameter("@P_Type", 1),
                            new SqlParameter("@P_DateTime", date)
                        };
                        SqlHelper.ExecuteProcedureReturnString(connString, "Save_DutyTaskLabels", keywordparam);
                    }
                }
                catch (Exception)
                {
                }
            }

            foreach (var item in oldLabel)
            {
                if (lables.Find(a => a.Labels.Equals(item.Labels)) is null)
                {
                    SqlParameter[] destinationparam = {
                    new SqlParameter("@P_DutyTaskID", memoId),
                    new SqlParameter("@P_UserID", userID),
                    new SqlParameter("@P_Labels", item.Labels),
                    new SqlParameter("@P_Type", 2),
                    new SqlParameter("@P_DateTime", date) };
                    SqlHelper.ExecuteProcedureReturnString(connString, "Save_DutyTaskLabels", destinationparam);
                }
            }
        }
    }
}
