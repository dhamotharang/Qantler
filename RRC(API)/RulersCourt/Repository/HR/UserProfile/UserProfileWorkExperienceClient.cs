using RulersCourt.Models.UserProfile;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace RulersCourt.Repository.UserProfile
{
    public class UserProfileWorkExperienceClient
    {
        public void SaveWorkExperience(string connString, List<UserProfileWorkExperienceModel> workExperience, int? userProfileId)
        {
            SqlParameter[] removeWorkExperience = {
                    new SqlParameter("@P_Type", "Delete"),
                    new SqlParameter("@P_UserProfileId", userProfileId),
                    new SqlParameter("@P_UserProfileWorkExperienceAllId", string.Join(",", from item in workExperience select item.WorkExperienceID))
            };

            SqlHelper.ExecuteProcedureReturnString(connString, "Save_UserProfileWorkExperience", removeWorkExperience);

            foreach (UserProfileWorkExperienceModel item in workExperience)
            {
                SqlParameter[] addWorkExperience = {
                    new SqlParameter("@P_Type", "Add"),
                    new SqlParameter("@P_UserProfileWorkExperienceId", item.WorkExperienceID),
                    new SqlParameter("@P_UserProfileId", userProfileId),
                    new SqlParameter("@P_JobTitle", item.JobTitle),
                    new SqlParameter("@P_Company", item.Company),
                    new SqlParameter("@P_City", item.City),
                    new SqlParameter("@P_TimePeriodFrom", item.TimePeriodFrom),
                    new SqlParameter("@P_TimePeriodTo", item.TimePeriodTo)
                };

                SqlHelper.ExecuteProcedureReturnString(connString, "Save_UserProfileWorkExperience", addWorkExperience);
            }
        }
    }
}
