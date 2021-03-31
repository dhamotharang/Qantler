using RulersCourt.Models.Protocol.Media.Design;
using System.Data.SqlClient;

namespace RulersCourt.Translators.Protocol.Media.Design
{
    public static class DesignSaveResponseModelTranslator
    {
        public static DesignResponseModel TranslateAsDesignSaveResponse(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var designSave = new DesignResponseModel();

            if (reader.IsColumnExists("DesignID"))
                designSave.DesignID = SqlHelper.GetNullableInt32(reader, "DesignID");

            if (reader.IsColumnExists("ReferenceNumber"))
                designSave.ReferenceNumber = SqlHelper.GetNullableString(reader, "ReferenceNumber");

            if (reader.IsColumnExists("CreatorID"))
                designSave.CreatorID = SqlHelper.GetNullableInt32(reader, "CreatorID").GetValueOrDefault();

            if (reader.IsColumnExists("FromID"))
                designSave.FromID = SqlHelper.GetNullableInt32(reader, "FromID").GetValueOrDefault();

            return designSave;
        }

        public static DesignResponseModel TranslateAsDesignSaveResponseList(this SqlDataReader reader)
        {
            var designSaveResponse = new DesignResponseModel();
            while (reader.Read())
            {
                designSaveResponse = TranslateAsDesignSaveResponse(reader, true);
            }

            return designSaveResponse;
        }
    }
}