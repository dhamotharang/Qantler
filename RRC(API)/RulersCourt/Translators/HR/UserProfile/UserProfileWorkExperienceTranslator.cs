using RulersCourt.Models.UserProfile;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Translators.UserProfile
{
    public static class UserProfileWorkExperienceTranslator
    {
        public static UserProfileWorkExperienceModel TranslateAsGetUserProfileWorkExperience(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var userProfileWorkExperience = new UserProfileWorkExperienceModel();

            if (reader.IsColumnExists("JobTitle"))
                userProfileWorkExperience.JobTitle = SqlHelper.GetNullableString(reader, "JobTitle");

            if (reader.IsColumnExists("Company"))
                userProfileWorkExperience.Company = SqlHelper.GetNullableString(reader, "Company");

            if (reader.IsColumnExists("City"))
                userProfileWorkExperience.City = SqlHelper.GetNullableString(reader, "City");

            if (reader.IsColumnExists("TimePeriodFrom"))
                userProfileWorkExperience.TimePeriodFrom = SqlHelper.GetDateTime(reader, "TimePeriodFrom");

            if (reader.IsColumnExists("TimePeriodTo"))
                userProfileWorkExperience.TimePeriodTo = SqlHelper.GetDateTime(reader, "TimePeriodTo");

            if (reader.IsColumnExists("UserProfileWorkExperienceId"))
                userProfileWorkExperience.WorkExperienceID = SqlHelper.GetNullableInt32(reader, "UserProfileWorkExperienceId");

            return userProfileWorkExperience;
        }

        public static List<UserProfileWorkExperienceModel> UserProfileTranslateAsWorkExperienceList(this SqlDataReader reader)
        {
            var userProfileWorkExperienceList = new List<UserProfileWorkExperienceModel>();
            while (reader.Read())
            {
                userProfileWorkExperienceList.Add(TranslateAsGetUserProfileWorkExperience(reader, true));
            }

            return userProfileWorkExperienceList;
        }
    }
}
