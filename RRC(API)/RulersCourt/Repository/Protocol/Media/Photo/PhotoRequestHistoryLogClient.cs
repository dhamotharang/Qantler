using RulersCourt.Models.Photo;
using RulersCourt.Translators.Protocol.Media.PhotoRequestTranslators;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Repository.Protocol.Media.PhotoRequest
{
    public class PhotoRequestHistoryLogClient
    {
        public List<PhotoCommunicationHistory> PhotoRequestHistoryLogByPhotoID(string connString, int photoID, string lang)
        {
            SqlParameter[] contractIDParam = {
                new SqlParameter("@P_PhotoID", photoID),
                new SqlParameter("@P_Language", lang)
            };
            List<PhotoCommunicationHistory> communicationHistories = new List<PhotoCommunicationHistory>();
            communicationHistories = SqlHelper.ExecuteProcedureReturnData<List<PhotoCommunicationHistory>>(connString, "Get_PhotoRequestCommunicationHistoryById", r => r.TranslateAsPhotoHistoryLogList(), contractIDParam);
            return communicationHistories;
        }
    }
}
