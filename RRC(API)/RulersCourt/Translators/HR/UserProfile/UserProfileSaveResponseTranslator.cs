using RulersCourt.Models.UserProfile;
using System.Data.SqlClient;

namespace RulersCourt.Translators.UserProfile
{
    public static class UserProfileSaveResponseTranslator
    {
        public static UserProfileSaveResponseModel TranslateAsUserProfileSaveResponse(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var userProfileSave = new UserProfileSaveResponseModel();

            if (reader.IsColumnExists("UserProfileId"))
                userProfileSave.UserProfileId = SqlHelper.GetNullableInt32(reader, "UserProfileId");

            if (reader.IsColumnExists("EmployeeCode"))
                userProfileSave.EmployeeCode = SqlHelper.GetNullableString(reader, "EmployeeCode");

            if (reader.IsColumnExists("ReferenceNumber"))
                userProfileSave.ReferenceNumber = SqlHelper.GetNullableString(reader, "ReferenceNumber");

            return userProfileSave;
        }

        public static UserProfileSaveResponseModel TranslateAsUserProfileSaveResponseList(this SqlDataReader reader)
        {
            var userProfileSaveResponse = new UserProfileSaveResponseModel();
            while (reader.Read())
            {
                userProfileSaveResponse = TranslateAsUserProfileSaveResponse(reader, true);
            }

            return userProfileSaveResponse;
        }
    }
}
