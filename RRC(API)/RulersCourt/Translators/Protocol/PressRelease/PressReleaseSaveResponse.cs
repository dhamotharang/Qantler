using RulersCourt.Models.Protocol.Media.PressRelease;
using System.Data.SqlClient;

namespace RulersCourt.Translators.Protocol.PressRelease
{
    public static class PressReleaseSaveResponse
    {
        public static PressReleaseResponseModel TranslateAsDesignSaveResponse(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var designSave = new PressReleaseResponseModel();

            if (reader.IsColumnExists("PressReleaseID"))
                designSave.PressReleaseID = SqlHelper.GetNullableInt32(reader, "PressReleaseID");

            if (reader.IsColumnExists("ReferenceNumber"))
                designSave.ReferenceNumber = SqlHelper.GetNullableString(reader, "ReferenceNumber");

            if (reader.IsColumnExists("CreatorID"))
                designSave.CreatorID = SqlHelper.GetNullableInt32(reader, "CreatorID").GetValueOrDefault();

            if (reader.IsColumnExists("FromID"))
                designSave.FromID = SqlHelper.GetNullableInt32(reader, "FromID").GetValueOrDefault();

            return designSave;
        }

        public static PressReleaseResponseModel TranslateAsPressReleaseSaveResponseList(this SqlDataReader reader)
        {
            var designSaveResponse = new PressReleaseResponseModel();
            while (reader.Read())
            {
                designSaveResponse = TranslateAsDesignSaveResponse(reader, true);
            }

            return designSaveResponse;
        }
    }
}
