using RulersCourt.Models.DutyTask;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Translators.DutyTask
{
    public static class DutyTaskResponsibleUserAndDepartmentTranslator
    {
        public static DutyTaskResponsibleUsersModel TranslateAsGetResponsibleUser(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var responsibleUser = new DutyTaskResponsibleUsersModel();

            if (reader.IsColumnExists("TaskResponsibleUsersID"))
                responsibleUser.TaskResponsibleUsersID = SqlHelper.GetNullableInt32(reader, "TaskResponsibleUsersID");

            if (reader.IsColumnExists("TaskResponsibleUsersName"))
                responsibleUser.TaskResponsibleUsersName = SqlHelper.GetNullableString(reader, "TaskResponsibleUsersName");

            return responsibleUser;
        }

        public static List<DutyTaskResponsibleUsersModel> TranslateAsResponsibleUserList(this SqlDataReader reader)
        {
            var responsibleUserList = new List<DutyTaskResponsibleUsersModel>();
            while (reader.Read())
            {
                responsibleUserList.Add(TranslateAsGetResponsibleUser(reader, true));
            }

            return responsibleUserList;
        }

        public static DutyTaskResponsibleDepartmentModel TranslateAsGetResponsibleUserDepartment(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var responsibleUserDepartment = new DutyTaskResponsibleDepartmentModel();

            if (reader.IsColumnExists("TaskResponsibleDepartmentID"))
                responsibleUserDepartment.TaskResponsibleDepartmentID = SqlHelper.GetNullableInt32(reader, "TaskResponsibleDepartmentID");

            if (reader.IsColumnExists("TaskResponsibleDepartmentName"))
                responsibleUserDepartment.TaskResponsibleDepartmentName = SqlHelper.GetNullableString(reader, "TaskResponsibleDepartmentName");

            return responsibleUserDepartment;
        }

        public static List<DutyTaskResponsibleDepartmentModel> TranslateAsResponsibleUserDepartmentList(this SqlDataReader reader)
        {
            var responsibleUserDepartmentList = new List<DutyTaskResponsibleDepartmentModel>();
            while (reader.Read())
            {
                responsibleUserDepartmentList.Add(TranslateAsGetResponsibleUserDepartment(reader, true));
            }

            return responsibleUserDepartmentList;
        }
    }
}
