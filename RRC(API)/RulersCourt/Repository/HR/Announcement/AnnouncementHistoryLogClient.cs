using RulersCourt.Models.Announcement;
using RulersCourt.Translators.Announcement;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Repository.Announcement
{
    public class AnnouncementHistoryLogClient
    {
        public List<AnnouncementHistoryLogModel> AnnouncementHistoryLogByAnnouncementID(string connString, int announcementID, string lang)
        {
            SqlParameter[] contractIDParam = {
                new SqlParameter("@P_AnnouncementID", announcementID),
                new SqlParameter("@P_Language", lang),
            };

            List<AnnouncementHistoryLogModel> announcementDetails = new List<AnnouncementHistoryLogModel>();

            announcementDetails = SqlHelper.ExecuteProcedureReturnData<List<AnnouncementHistoryLogModel>>(connString, "Get_AnnouncementHistoryByID", r => r.TranslateAsAnnouncementHistoryLogList(), contractIDParam);

            return announcementDetails;
        }
    }
}
