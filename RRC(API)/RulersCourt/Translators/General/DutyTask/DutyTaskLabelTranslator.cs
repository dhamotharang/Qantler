using RulersCourt.Models.DutyTasks;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Translators.DutyTask
{
    public static class DutyTaskLabelTranslator
    {
        public static DutyTaskLablesModel TranslateAsGetLabel(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var label = new DutyTaskLablesModel();

            if (reader.IsColumnExists("Labels"))
                label.Labels = SqlHelper.GetNullableString(reader, "Labels");

            return label;
        }

        public static List<DutyTaskLablesModel> TranslateAsLabelList(this SqlDataReader reader)
        {
            var labels = new List<DutyTaskLablesModel>();
            while (reader.Read())
            {
                labels.Add(TranslateAsGetLabel(reader, true));
            }

            return labels;
        }
    }
}
