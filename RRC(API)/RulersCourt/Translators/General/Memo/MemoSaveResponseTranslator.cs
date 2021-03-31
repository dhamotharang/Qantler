using RulersCourt.Models;
using System.Data.SqlClient;

namespace RulersCourt.Translators
{
    public static class MemoSaveResponseTranslator
    {
        public static MemoWorkflowModel TranslateAsMemoSaveResponse(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var memoSave = new MemoWorkflowModel();

            if (reader.IsColumnExists("MemoId"))
                memoSave.MemoId = SqlHelper.GetNullableInt32(reader, "MemoId");

            if (reader.IsColumnExists("ReferenceNumber"))
                memoSave.ReferenceNumber = SqlHelper.GetNullableString(reader, "ReferenceNumber");

            if (reader.IsColumnExists("CreatorID"))
                memoSave.CreatorID = SqlHelper.GetNullableInt32(reader, "CreatorID").GetValueOrDefault();

            if (reader.IsColumnExists("FromID"))
                memoSave.FromID = SqlHelper.GetNullableInt32(reader, "FromID").GetValueOrDefault();

            return memoSave;
        }

        public static MemoWorkflowModel TranslateAsMemoSaveResponseList(this SqlDataReader reader)
        {
            var memoSaveResponse = new MemoWorkflowModel();
            while (reader.Read())
            {
                memoSaveResponse = TranslateAsMemoSaveResponse(reader, true);
            }

            return memoSaveResponse;
        }
    }
}