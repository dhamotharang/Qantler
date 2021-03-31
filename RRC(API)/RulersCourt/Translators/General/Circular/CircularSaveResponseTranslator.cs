using RulersCourt.Models.Circular;
using System.Data.SqlClient;

namespace RulersCourt.Translators.Circular
{
    public static class CircularSaveResponseTranslator
    {
        public static CircularWorkflowModel TranslateAsCircularSaveResponse(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var circularSave = new CircularWorkflowModel();

            if (reader.IsColumnExists("CircularId"))
                circularSave.CircularId = SqlHelper.GetNullableInt32(reader, "CircularId");

            if (reader.IsColumnExists("ReferenceNumber"))
                circularSave.ReferenceNumber = SqlHelper.GetNullableString(reader, "ReferenceNumber");

            if (reader.IsColumnExists("CreatorID"))
                circularSave.CreatorID = SqlHelper.GetNullableInt32(reader, "CreatorID").GetValueOrDefault();

            if (reader.IsColumnExists("FromID"))
                circularSave.FromID = SqlHelper.GetNullableInt32(reader, "FromID").GetValueOrDefault();

            return circularSave;
        }

        public static CircularWorkflowModel TranslateAsCircularSaveResponseList(this SqlDataReader reader)
        {
            var circularSaveResponse = new CircularWorkflowModel();
            while (reader.Read())
            {
                circularSaveResponse = TranslateAsCircularSaveResponse(reader, true);
            }

            return circularSaveResponse;
        }
    }
}
