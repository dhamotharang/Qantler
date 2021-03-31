using RulersCourt.Models.DutyTasks;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Translators.DutyTask
{
    public static class DutyTaskTaggedUserIDTranslator
    {
        public static DutyTaskTaggedUserIDModel TranslateAsGetTaggedUserId(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var taggedUsedModel = new DutyTaskTaggedUserIDModel();

            if (reader.IsColumnExists("TaggedUserID"))
                taggedUsedModel.TaggedUsersID = SqlHelper.GetNullableInt32(reader, "TaggedUserID");

            if (reader.IsColumnExists("TaggedUsersName"))
                taggedUsedModel.TaggedUsersName = SqlHelper.GetNullableString(reader, "TaggedUsersName");
            return taggedUsedModel;
        }

        public static List<DutyTaskTaggedUserIDModel> TranslateAsTaggedUserID(this SqlDataReader reader)
        {
            var users = new List<DutyTaskTaggedUserIDModel>();
            while (reader.Read())
            {
                users.Add(TranslateAsGetTaggedUserId(reader, true));
            }

            return users;
        }
    }
}
