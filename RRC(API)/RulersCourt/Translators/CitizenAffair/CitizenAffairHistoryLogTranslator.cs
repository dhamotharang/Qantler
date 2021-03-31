using RulersCourt.Models.CitizenAffair;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Translators.CitizenAffair
{
    public static class CitizenAffairHistoryLogTranslator
    {
        public static CitizenAffairHistoryLogModel TranslateAsCAHistoryLog(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var cAHistoryLogModel = new CitizenAffairHistoryLogModel();

            if (reader.IsColumnExists("HistoryID"))
                cAHistoryLogModel.HistoryID = SqlHelper.GetNullableInt32(reader, "HistoryID");

            if (reader.IsColumnExists("CitizenAffairID"))
                cAHistoryLogModel.CitizenAffairID = SqlHelper.GetNullableInt32(reader, "CitizenAffairID");

            if (reader.IsColumnExists("Action"))
                cAHistoryLogModel.Action = SqlHelper.GetNullableString(reader, "Action");

            if (reader.IsColumnExists("Comments"))
                cAHistoryLogModel.Comments = SqlHelper.GetNullableString(reader, "Comments");

            if (reader.IsColumnExists("ActionBy"))
                cAHistoryLogModel.CreatedBy = SqlHelper.GetNullableString(reader, "ActionBy");

            if (reader.IsColumnExists("ActionDateTime"))
                cAHistoryLogModel.CreatedDateTime = SqlHelper.GetDateTime(reader, "ActionDateTime");

            return cAHistoryLogModel;
        }

        public static List<CitizenAffairHistoryLogModel> TranslateAsCAHistoryLogList(this SqlDataReader reader)
        {
            var caHistoryLogList = new List<CitizenAffairHistoryLogModel>();
            while (reader.Read())
            {
                caHistoryLogList.Add(TranslateAsCAHistoryLog(reader, true));
            }

            return caHistoryLogList;
        }
    }
}
