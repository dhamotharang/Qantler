using RulersCourt.Models.Meeting;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Repository.Meeting
{
    public class MeetingHistoryClient
    {
        public List<MeetingCommunicationHistoryModel> MeetingHistoryLogByMeetingID(string connString, int? meetingID, string lang)
        {
            SqlParameter[] contractIDParam = {
                new SqlParameter("@P_MeetingID", meetingID),
                new SqlParameter("@P_Language", lang),
                };

            List<MeetingCommunicationHistoryModel> circularDetails = new List<MeetingCommunicationHistoryModel>();

            circularDetails = SqlHelper.ExecuteProcedureReturnData<List<MeetingCommunicationHistoryModel>>(connString, "Get_MeetingHistoryByID", r => r.TranslateAsMeetingHistoryLogList(), contractIDParam);

            return circularDetails;
        }

        public List<MeetingCommunicationHistoryModel> MOMHistoryLogByMeetingID(string connString, int meetingID, string lang)
        {
            SqlParameter[] contractIDParam = {
                new SqlParameter("@P_MeetingID", meetingID),
                new SqlParameter("@P_Language", lang),
            };

            List<MeetingCommunicationHistoryModel> circularDetails = new List<MeetingCommunicationHistoryModel>();

            circularDetails = SqlHelper.ExecuteProcedureReturnData<List<MeetingCommunicationHistoryModel>>(connString, "Get_MOMHistoryByID", r => r.TranslateAsMeetingHistoryLogList(), contractIDParam);

            return circularDetails;
        }
    }
}
