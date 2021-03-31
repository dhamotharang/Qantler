using RulersCourt.Models.Legal;
using System.Data.SqlClient;

namespace RulersCourt.Translators.Legal
{
    public static class LegalSaveResponseTranslator
    {
        public static LegalWorkflowModel TranslateAsLegalSaveResponse(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var legalSave = new LegalWorkflowModel();

            if (reader.IsColumnExists("LegalID"))
                legalSave.LegalID = SqlHelper.GetNullableInt32(reader, "LegalID");

            if (reader.IsColumnExists("ReferenceNumber"))
                legalSave.ReferenceNumber = SqlHelper.GetNullableString(reader, "ReferenceNumber");

            if (reader.IsColumnExists("CreatorID"))
                legalSave.CreatorID = SqlHelper.GetNullableInt32(reader, "CreatorID").GetValueOrDefault();

            if (reader.IsColumnExists("FromID"))
                legalSave.FromID = SqlHelper.GetNullableInt32(reader, "FromID").GetValueOrDefault();

            return legalSave;
        }

        public static LegalWorkflowModel TranslateAsLegalSaveResponseList(this SqlDataReader reader)
        {
            var legalSaveResponse = new LegalWorkflowModel();
            while (reader.Read())
            {
                legalSaveResponse = TranslateAsLegalSaveResponse(reader, true);
            }

            return legalSaveResponse;
        }
    }
}