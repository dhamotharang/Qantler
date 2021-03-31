using RulersCourt.Models.ITSupport;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Translators.ITSupportTranslators
{
    public static class ITSupportHistoryLogTranslators
    {
        public static ITSupportHistoryLogModel TranslateITSupportHistoryLogModel(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var iTSupportHistoryLogModel = new ITSupportHistoryLogModel();

            if (reader.IsColumnExists("HistoryID"))
                iTSupportHistoryLogModel.HistoryID = SqlHelper.GetNullableInt32(reader, "HistoryID");

            if (reader.IsColumnExists("ITSupportID"))
                iTSupportHistoryLogModel.ITSupportID = SqlHelper.GetNullableInt32(reader, "ITSupportID");

            if (reader.IsColumnExists("Action"))
                iTSupportHistoryLogModel.Action = SqlHelper.GetNullableString(reader, "Action");

            if (reader.IsColumnExists("Comments"))
                iTSupportHistoryLogModel.Comments = SqlHelper.GetNullableString(reader, "Comments");

            if (reader.IsColumnExists("ActionBy"))
                iTSupportHistoryLogModel.ActionBy = SqlHelper.GetNullableString(reader, "ActionBy");

            if (reader.IsColumnExists("ActionDateTime"))
                iTSupportHistoryLogModel.ActionDateTime = SqlHelper.GetDateTime(reader, "ActionDateTime");

            return iTSupportHistoryLogModel;
        }

        public static List<ITSupportHistoryLogModel> TranslateITSupportHistoryLogModelList(this SqlDataReader reader)
        {
            var iTSupportHistoryLogModelList = new List<ITSupportHistoryLogModel>();
            while (reader.Read())
            {
                iTSupportHistoryLogModelList.Add(TranslateITSupportHistoryLogModel(reader, true));
            }

            return iTSupportHistoryLogModelList;
        }
    }
}
