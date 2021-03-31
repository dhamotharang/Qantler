using RulersCourt.Models.UserProfile;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace RulersCourt.Repository.UserProfile
{
    public class UserProfileTrainingCertificationsClient
    {
        public void SaveTrainingCertifications(string connString, List<UserProfileTrainingCertificationsModel> trainingCertifications, int? userProfileId)
        {
            SqlParameter[] removeTrainingCertification = {
                    new SqlParameter("@P_Type", "Delete"),
                    new SqlParameter("@P_UserProfileId", userProfileId),
                    new SqlParameter("@P_UserProfileTrainingCertificationsAllId", string.Join(",", from item in trainingCertifications select item.TrainingID))
            };

            SqlHelper.ExecuteProcedureReturnString(connString, "Save_UserProfileTrainingCertifications", removeTrainingCertification);

            foreach (UserProfileTrainingCertificationsModel item in trainingCertifications)
            {
                SqlParameter[] addEducation = {
                    new SqlParameter("@P_Type", "Add"),
                    new SqlParameter("@P_UserProfileTrainingCertificationsId", item.TrainingID),
                    new SqlParameter("@P_UserProfileId", userProfileId),
                    new SqlParameter("@P_TrainingName", item.TrainingName),
                    new SqlParameter("@P_StartDate", item.StartDate),
                    new SqlParameter("@P_EndDate", item.EndDate)
                };
                string id = SqlHelper.ExecuteProcedureReturnString(connString, "Save_UserProfileTrainingCertifications", addEducation);
                if (item.Attachment != null && item.TrainingRequestID == null)
                    new UserProfileAttachmentClient().PostUserProfileAttachments(connString, "UserProfileTraining", item.Attachment, int.Parse(id), null);
            }
        }
    }
}
