using RulersCourt.Models.Design;
using RulersCourt.Translators.Protocol.Media.Design;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Repository.Protocol.Media.Design
{
    public class DesignHistoryLogClient
    {
        public List<DesignCommunicationHistory> DesignHistoryLogByCircularID(string connString, int designID, string lang)
        {
            SqlParameter[] contractIDParam = {
                new SqlParameter("@P_DesignID", designID),
                new SqlParameter("@P_Language", lang)
            };

            List<DesignCommunicationHistory> circularDetails = new List<DesignCommunicationHistory>();

            circularDetails = SqlHelper.ExecuteProcedureReturnData<List<DesignCommunicationHistory>>(connString, "Get_MediaDesignRequestHistoryByID", r => r.TranslateAsDesignHistoryLogList(), contractIDParam);

            return circularDetails;
        }
    }
}