using RulersCourt.Models.Meeting;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace RulersCourt.Repository.Meeting
{
    public class MeetingRemindMeClient
    {
        public void SaveRemindMeAt(string connString, List<MeetingRemindMeAtModel> internalInvitees, int? meetingId, int? userID, DateTime? date)
        {
            string result = string.Empty;

            if (internalInvitees.Count != 0)
            {
                SqlParameter[] parama = { new SqlParameter("@P_Type", "Delete"),
                                        new SqlParameter("@P_MeetingID", meetingId),
                                        new SqlParameter("@P_UserID", userID),
                                        new SqlParameter("@P_ActionDateTime", date),
                                        new SqlParameter("@P_ID", string.Join(",", from item in internalInvitees select item.MeetingRemindID)) };

                result = SqlHelper.ExecuteProcedureReturnString(connString, "Save_MeetingRemindMeAt", parama);
            }
            else
            {
                SqlParameter[] parama = { new SqlParameter("@P_Type", "Delete"),
                                        new SqlParameter("@P_MeetingID", meetingId),
                                        new SqlParameter("@P_UserID", userID),
                                        new SqlParameter("@P_ActionDateTime", date)
                                     };

                result = SqlHelper.ExecuteProcedureReturnString(connString, "Save_MeetingRemindMeAt", parama);
            }

            foreach (var item in internalInvitees)
            {
                if ((item.MeetingRemindID ?? 0) == 0)
                {
                    SqlParameter[] destinationparam = {
                             new SqlParameter("@P_MeetingRemindMeID", item.MeetingRemindID),
                             new SqlParameter("@P_RemindMeDateTime", item.RemindMeDateTime),
                             new SqlParameter("@P_MeetingID", meetingId),
                             new SqlParameter("@P_Type", "1"),
                             new SqlParameter("@P_UserID", userID),
                             new SqlParameter("@P_ActionDateTime", date)
                             };

                    result = SqlHelper.ExecuteProcedureReturnString(connString, "Save_MeetingRemindMeAt", destinationparam);
                }

                if ((item.MeetingRemindID ?? 0) != 0 && item.RemindMeDateTime != null)
                {
                    SqlParameter[] destinationparam = {
                             new SqlParameter("@P_MeetingRemindMeID", item.MeetingRemindID),
                             new SqlParameter("@P_RemindMeDateTime", item.RemindMeDateTime),
                             new SqlParameter("@P_MeetingID", meetingId),
                             new SqlParameter("@P_DeleteFlag", 0),
                             new SqlParameter("@P_Type", "1"),
                             new SqlParameter("@P_UserID", userID),
                             new SqlParameter("@P_ActionDateTime", date)
                             };

                    result = SqlHelper.ExecuteProcedureReturnString(connString, "Save_MeetingRemindMeAt", destinationparam);
                }
            }
        }
    }
}
