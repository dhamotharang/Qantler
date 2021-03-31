using RulersCourt.Models.UserProfile;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Translators.UserProfile
{
    public static class UserProfileTrainingCertificationsTranslator
    {
        public static UserProfileTrainingCertificationsModel TranslateAsGetUserProfileTrainingCertifications(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var userProfileTrainingCertifications = new UserProfileTrainingCertificationsModel();

            if (reader.IsColumnExists("TrainingName"))
                userProfileTrainingCertifications.TrainingName = SqlHelper.GetNullableString(reader, "TrainingName");

            if (reader.IsColumnExists("StartDate"))
                userProfileTrainingCertifications.StartDate = SqlHelper.GetDateTime(reader, "StartDate");

            if (reader.IsColumnExists("EndDate"))
                userProfileTrainingCertifications.EndDate = SqlHelper.GetDateTime(reader, "EndDate");

            if (reader.IsColumnExists("UserProfileTrainingCertificationsId"))
                userProfileTrainingCertifications.TrainingID = SqlHelper.GetNullableInt32(reader, "UserProfileTrainingCertificationsId");

            if (reader.IsColumnExists("TrainingRequestID"))
                userProfileTrainingCertifications.TrainingRequestID = SqlHelper.GetNullableInt32(reader, "TrainingRequestID");

            return userProfileTrainingCertifications;
        }

        public static List<UserProfileTrainingCertificationsModel> UserProfileTranslateAsTrainingCertificationsList(this SqlDataReader reader)
        {
            var userProfileTrainingCertificationsList = new List<UserProfileTrainingCertificationsModel>();
            while (reader.Read())
            {
                userProfileTrainingCertificationsList.Add(TranslateAsGetUserProfileTrainingCertifications(reader, true));
            }

            return userProfileTrainingCertificationsList;
        }
    }
}
