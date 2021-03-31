using RulersCourt.Models.UserProfile;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace RulersCourt.Repository.UserProfile
{
    public class UserProfileEducationClient
    {
        public void SaveEducation(string connString, List<UserProfileEducationModel> education, int? userProfileId)
        {
            SqlParameter[] removeEducation = {
                    new SqlParameter("@P_Type", "Delete"),
                    new SqlParameter("@P_UserProfileId", userProfileId),
                    new SqlParameter("@P_UserProfileEducationAllID", string.Join(",", from item in education select item.UserProfileEducationID))
            };

            SqlHelper.ExecuteProcedureReturnString(connString, "Save_UserProfileEducation", removeEducation);

            foreach (UserProfileEducationModel item in education)
            {
                SqlParameter[] addEducation = {
                    new SqlParameter("@P_Type", "Add"),
                    new SqlParameter("@P_UserProfileEducationID", item.UserProfileEducationID),
                    new SqlParameter("@P_UserProfileId", userProfileId),
                    new SqlParameter("@P_Degree", item.Degree),
                    new SqlParameter("@P_SchoolCollege", item.SchoolCollege),
                    new SqlParameter("@P_FieldStudy", item.FieldStudy),
                    new SqlParameter("@P_TimePeriodFrom", item.TimePeriodFrom),
                    new SqlParameter("@P_TimePeriodTo", item.TimePeriodTo)
                };

                SqlHelper.ExecuteProcedureReturnString(connString, "Save_UserProfileEducation", addEducation);
            }
        }
    }
}
