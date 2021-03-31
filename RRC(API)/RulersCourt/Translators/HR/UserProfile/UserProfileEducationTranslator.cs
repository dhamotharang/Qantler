using RulersCourt.Models.UserProfile;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Translators.UserProfile
{
    public static class UserProfileEducationTranslator
    {
        public static UserProfileEducationModel TranslateAsGetUserProfileEducation(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var userProfileEducation = new UserProfileEducationModel();

            if (reader.IsColumnExists("Degree"))
                userProfileEducation.Degree = SqlHelper.GetNullableString(reader, "Degree");

            if (reader.IsColumnExists("SchoolCollege"))
                userProfileEducation.SchoolCollege = SqlHelper.GetNullableString(reader, "SchoolCollege");

            if (reader.IsColumnExists("FieldStudy"))
                userProfileEducation.FieldStudy = SqlHelper.GetNullableString(reader, "FieldStudy");

            if (reader.IsColumnExists("TimePeriodFrom"))
                userProfileEducation.TimePeriodFrom = SqlHelper.GetDateTime(reader, "TimePeriodFrom");

            if (reader.IsColumnExists("TimePeriodTo"))
                userProfileEducation.TimePeriodTo = SqlHelper.GetDateTime(reader, "TimePeriodTo");

            if (reader.IsColumnExists("UserProfileEducationId"))
                userProfileEducation.UserProfileEducationID = SqlHelper.GetNullableInt32(reader, "UserProfileEducationId");

            return userProfileEducation;
        }

        public static List<UserProfileEducationModel> UserProfileTranslateAsEducationList(this SqlDataReader reader)
        {
            var userProfileEducationList = new List<UserProfileEducationModel>();
            while (reader.Read())
            {
                userProfileEducationList.Add(TranslateAsGetUserProfileEducation(reader, true));
            }

            return userProfileEducationList;
        }
    }
}
