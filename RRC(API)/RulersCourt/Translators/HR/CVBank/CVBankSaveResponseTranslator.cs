using RulersCourt.Models.CVBank;
using System.Data.SqlClient;

namespace RulersCourt.Translators.HR.CVBank
{
    public static class CVBankSaveResponseTranslator
    {
        public static CVBankSaveResponseModel TranslateAsCVBankSaveResponse(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var cVBankSave = new CVBankSaveResponseModel();

            if (reader.IsColumnExists("CVBankId"))
                cVBankSave.CVBankId = SqlHelper.GetNullableInt32(reader, "CVBankId");

            if (reader.IsColumnExists("ReferenceNumber"))
                cVBankSave.ReferenceNumber = SqlHelper.GetNullableString(reader, "ReferenceNumber");

            return cVBankSave;
        }

        public static CVBankSaveResponseModel TranslateAsCVBankSaveResponseList(this SqlDataReader reader)
        {
            var cVBankSaveResponse = new CVBankSaveResponseModel();
            while (reader.Read())
            {
                cVBankSaveResponse = TranslateAsCVBankSaveResponse(reader, true);
            }

            return cVBankSaveResponse;
        }
    }
}
