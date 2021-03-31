using RulersCourt.Models.BabyAddition;
using System.Data.SqlClient;

namespace RulersCourt.Translators.BabyAddition
{
    public static class BabyAdditionSaveResponseTranslator
    {
        public static BabyAdditionWorkflowModel TranslateAsBabyAdditionSaveResponse(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var babyAdditionSave = new BabyAdditionWorkflowModel();

            if (reader.IsColumnExists("BabyAdditionID"))
                babyAdditionSave.BabyAdditionID = SqlHelper.GetNullableInt32(reader, "BabyAdditionID");

            if (reader.IsColumnExists("ReferenceNumber"))
                babyAdditionSave.ReferenceNumber = SqlHelper.GetNullableString(reader, "ReferenceNumber");

            if (reader.IsColumnExists("CreatorID"))
                babyAdditionSave.CreatorID = SqlHelper.GetNullableInt32(reader, "CreatorID").GetValueOrDefault();

            if (reader.IsColumnExists("FromID"))
                babyAdditionSave.FromID = SqlHelper.GetNullableInt32(reader, "FromID").GetValueOrDefault();

            return babyAdditionSave;
        }

        public static BabyAdditionWorkflowModel TranslateAsBabyAdditionSaveResponseList(this SqlDataReader reader)
        {
            var babyAdditionSaveResponse = new BabyAdditionWorkflowModel();
            while (reader.Read())
            {
                babyAdditionSaveResponse = TranslateAsBabyAdditionSaveResponse(reader, true);
            }

            return babyAdditionSaveResponse;
        }
    }
}
