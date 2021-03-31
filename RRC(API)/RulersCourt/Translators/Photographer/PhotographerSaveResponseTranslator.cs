using RulersCourt.Models.Protocol.Media.Photographer;
using System.Data.SqlClient;

namespace RulersCourt.Translators.Protocol.Media.Photographer
{
    public static class PhotographerSaveResponseTranslator
    {
        public static PhotographerWorkflowModel TranslateAsPhotographerSaveResponse(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var photographerSave = new PhotographerWorkflowModel();

            if (reader.IsColumnExists("PhotoGrapherID"))
                photographerSave.PhotoGrapherID = SqlHelper.GetNullableInt32(reader, "PhotoGrapherID");

            if (reader.IsColumnExists("ReferenceNumber"))
                photographerSave.ReferenceNumber = SqlHelper.GetNullableString(reader, "ReferenceNumber");

            if (reader.IsColumnExists("CreatorID"))
                photographerSave.CreatorID = SqlHelper.GetNullableInt32(reader, "CreatorID").GetValueOrDefault();

            if (reader.IsColumnExists("FromID"))
                photographerSave.FromID = SqlHelper.GetNullableInt32(reader, "FromID").GetValueOrDefault();

            return photographerSave;
        }

        public static PhotographerWorkflowModel TranslateAsPhotographerSaveResponseList(this SqlDataReader reader)
        {
            var photographerSaveResponse = new PhotographerWorkflowModel();
            while (reader.Read())
            {
                photographerSaveResponse = TranslateAsPhotographerSaveResponse(reader, true);
            }

            return photographerSaveResponse;
        }
    }
}
