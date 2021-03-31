using RulersCourt.Models;
using RulersCourt.Translators;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace RulersCourt.Repository
{
    public class ShareparticipationUsersClient
    {
        public List<ShareparticipationUsersModel> GetparticipationUsers(string connString, int? serviceID)
        {
            SqlParameter[] getShareUserparam = {
                    new SqlParameter("@P_ServiceId", serviceID) };

            return SqlHelper.ExecuteProcedureReturnData<List<ShareparticipationUsersModel>>(connString, "Get_ShareparticipationUsers", r => r.TranslateAsGetShareUserList(), getShareUserparam);
        }

        public MemoWorkflowModel SaveparticipationUsers(string connString, List<ShareparticipationUsersModel> shareUser, int serviceID, string type, int userID, string comments)
        {
            var result = string.Empty;
            foreach (ShareparticipationUsersModel items in shareUser)
            {
                SqlParameter[] getShareUserparam = {
                new SqlParameter("@P_ServiceId", serviceID),
                new SqlParameter("@P_UserID", items.UserID),
                new SqlParameter("@P_Comments", comments),
                new SqlParameter("@P_CreatorID", userID),
                new SqlParameter("@P_Type", type)
                };

                result = SqlHelper.ExecuteProcedureReturnString(connString, "Save_ShareparticipationUsers", getShareUserparam);
            }

            MemoGetModel memoDetails = new MemoGetModel();

            SqlParameter[] param = {
                new SqlParameter("@P_MemoID", serviceID),
                new SqlParameter("@P_UserID", 0)
            };

            memoDetails = SqlHelper.ExecuteProcedureReturnData<List<MemoGetModel>>(connString, "Get_MemoByID", r => r.TranslateAsMemoList(), param).FirstOrDefault();

            MemoWorkflowModel res = new MemoWorkflowModel();
            res.Action = "Share";
            res.FromID = userID;
            res.MemoId = serviceID;
            res.CreatorID = userID;
            res.ShredUserID = shareUser;
            res.ReferenceNumber = memoDetails.ReferenceNumber;

            return res;
        }
    }
}
