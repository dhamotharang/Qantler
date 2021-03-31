using RulersCourt.Models.Calendar;
using RulersCourt.Translators.Calendar;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Repository.Calendar
{
    public class CalendarHistoryClient
    {
        public List<CalendarHistoryModel> CalendarHistoryLogByMeetingID(string connString, int id, string lang)
        {
            SqlParameter[] contractIDParam = {
                new SqlParameter("@P_CalendarID", id),
                new SqlParameter("@P_Language", lang)
            };

            List<CalendarHistoryModel> circularDetails = new List<CalendarHistoryModel>();

            circularDetails = SqlHelper.ExecuteProcedureReturnData<List<CalendarHistoryModel>>(connString, "Get_CalendarHistoryByID", r => r.TranslateAsCalendarHistoryLogList(), contractIDParam);

            return circularDetails;
        }
    }
}