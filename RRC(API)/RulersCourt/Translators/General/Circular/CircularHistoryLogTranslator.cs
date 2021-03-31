using RulersCourt.Models.Circular;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Translators.Circular
{
    public static class CircularHistoryLogTranslator
    {
        public static CircularHistoryLogModel TranslateAsCircularHistoryLog(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var circularHistoryLogModel = new CircularHistoryLogModel();

            if (reader.IsColumnExists("HistoryID"))
                circularHistoryLogModel.HistoryID = SqlHelper.GetNullableInt32(reader, "HistoryID");

            if (reader.IsColumnExists("CircularID"))
                circularHistoryLogModel.CircularID = SqlHelper.GetNullableInt32(reader, "CircularID");

            if (reader.IsColumnExists("Action"))
                circularHistoryLogModel.Action = SqlHelper.GetNullableString(reader, "Action");

            if (reader.IsColumnExists("Comments"))
                circularHistoryLogModel.Comments = SqlHelper.GetNullableString(reader, "Comments");

            if (reader.IsColumnExists("ActionBy"))
                circularHistoryLogModel.ActionBy = SqlHelper.GetNullableString(reader, "ActionBy");

            if (reader.IsColumnExists("ActionDateTime"))
                circularHistoryLogModel.ActionDateTime = SqlHelper.GetDateTime(reader, "ActionDateTime");

            return circularHistoryLogModel;
        }

        public static List<CircularHistoryLogModel> TranslateAsCircularHistoryLogList(this SqlDataReader reader)
        {
            var circularHistoryLogList = new List<CircularHistoryLogModel>();
            while (reader.Read())
            {
                circularHistoryLogList.Add(TranslateAsCircularHistoryLog(reader, true));
            }

            return circularHistoryLogList;
        }
    }
}
