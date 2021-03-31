using RulersCourt.Models.OfficalTaskCompensation.Compensation;
using System.Data.SqlClient;

namespace RulersCourt.Translators.OfficalTaskCompensation.Compensation
{
    public static class CompensationSaveResponseTranslator
    {
        public static CompensationWorkflowModel TranslateAsCompensationSaveResponse(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var compensationSave = new CompensationWorkflowModel();

            if (reader.IsColumnExists("CompensationID"))
                compensationSave.CompensationID = SqlHelper.GetNullableInt32(reader, "CompensationID");

            if (reader.IsColumnExists("ReferenceNumber"))
                compensationSave.ReferenceNumber = SqlHelper.GetNullableString(reader, "ReferenceNumber");

            if (reader.IsColumnExists("CreatorID"))
                compensationSave.CreatorID = SqlHelper.GetNullableInt32(reader, "CreatorID").GetValueOrDefault();

            if (reader.IsColumnExists("FromID"))
                compensationSave.FromID = SqlHelper.GetNullableInt32(reader, "FromID").GetValueOrDefault();

            return compensationSave;
        }

        public static CompensationWorkflowModel TranslateAsCompensationSaveResponseList(this SqlDataReader reader)
        {
            var compensationSaveResponse = new CompensationWorkflowModel();
            while (reader.Read())
            {
                compensationSaveResponse = TranslateAsCompensationSaveResponse(reader, true);
            }

            return compensationSaveResponse;
        }
    }
}