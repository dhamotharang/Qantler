using RulersCourt.Models.OfficalTask;
using RulersCourt.Models.OfficialTask;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Translators.OfficialTaskCompensation.OfficialTask
{
    public static class OfficialTaskTranslator
    {
        public static OfficialTaskGetModel TranslateAsOfficialTask(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var officialTaskModel = new OfficialTaskGetModel();

            if (reader.IsColumnExists("OfficialTaskID"))
                officialTaskModel.OfficialTaskID = SqlHelper.GetNullableInt32(reader, "OfficialTaskID");

            if (reader.IsColumnExists("ReferenceNumber"))
                officialTaskModel.ReferenceNumber = SqlHelper.GetNullableString(reader, "ReferenceNumber");

            if (reader.IsColumnExists("Date"))
                officialTaskModel.Date = SqlHelper.GetDateTime(reader, "Date");

            if (reader.IsColumnExists("SourceOU"))
                officialTaskModel.SourceOU = SqlHelper.GetNullableString(reader, "SourceOU");

            if (reader.IsColumnExists("SourceName"))
                officialTaskModel.SourceName = SqlHelper.GetNullableString(reader, "SourceName");

            if (reader.IsColumnExists("OfficialTaskType"))
                officialTaskModel.OfficialTaskType = SqlHelper.GetNullableString(reader, "OfficialTaskType");

            if (reader.IsColumnExists("StartDate"))
                officialTaskModel.StartDate = SqlHelper.GetDateTime(reader, "StartDate");

            if (reader.IsColumnExists("EndDate"))
                officialTaskModel.EndDate = SqlHelper.GetDateTime(reader, "EndDate");

            if (reader.IsColumnExists("NumberofDays"))
                officialTaskModel.NumberofDays = SqlHelper.GetNullableInt32(reader, "NumberofDays");

            if (reader.IsColumnExists("OfficialTaskDescription"))
                officialTaskModel.OfficialTaskDescription = SqlHelper.GetNullableString(reader, "OfficialTaskDescription");

            if (reader.IsColumnExists("CreatedBy"))
                officialTaskModel.CreatedBy = SqlHelper.GetNullableInt32(reader, "CreatedBy");

            if (reader.IsColumnExists("UpdatedBy"))
                officialTaskModel.UpdatedBy = SqlHelper.GetNullableInt32(reader, "UpdatedBy");

            if (reader.IsColumnExists("Status"))
                officialTaskModel.Status = SqlHelper.GetNullableInt32(reader, "Status");

            if (reader.IsColumnExists("Comments"))
                officialTaskModel.Comments = SqlHelper.GetNullableString(reader, "Comments");

            if (reader.IsColumnExists("CreatedDateTime"))
                officialTaskModel.CreatedDateTime = SqlHelper.GetDateTime(reader, "CreatedDateTime");

            if (reader.IsColumnExists("UpdatedDateTime"))
                officialTaskModel.UpdatedDateTime = SqlHelper.GetDateTime(reader, "UpdatedDateTime");

            if (reader.IsColumnExists("IsCompensationAvailable"))
                officialTaskModel.IsCompensationAvailable = SqlHelper.GetBoolean(reader, "IsCompensationAvailable");

            if (reader.IsColumnExists("IsClosed"))
                officialTaskModel.IsClosed = SqlHelper.GetBoolean(reader, "IsClosed");

            return officialTaskModel;
        }

        public static List<OfficialTaskGetModel> TranslateAsOfficialTaskList(this SqlDataReader reader)
        {
            var officialTaskList = new List<OfficialTaskGetModel>();
            while (reader.Read())
            {
                officialTaskList.Add(TranslateAsOfficialTask(reader, true));
            }

            return officialTaskList;
        }

        public static OfficialTaskPutModel TranslateAsPutOfficialTask(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var officialTaskModel = new OfficialTaskPutModel();

            if (reader.IsColumnExists("OfficialTaskID"))
                officialTaskModel.OfficialTaskID = SqlHelper.GetNullableInt32(reader, "OfficialTaskID");

            if (reader.IsColumnExists("Date"))
                officialTaskModel.Date = SqlHelper.GetDateTime(reader, "Date");

            if (reader.IsColumnExists("SourceOU"))
                officialTaskModel.SourceOU = SqlHelper.GetNullableString(reader, "SourceOU");

            if (reader.IsColumnExists("SourceName"))
                officialTaskModel.SourceName = SqlHelper.GetNullableString(reader, "SourceName");

            if (reader.IsColumnExists("OfficialTaskType"))
                officialTaskModel.OfficialTaskType = SqlHelper.GetNullableString(reader, "OfficialTaskType");

            if (reader.IsColumnExists("StartDate"))
                officialTaskModel.StartDate = SqlHelper.GetDateTime(reader, "StartDate");

            if (reader.IsColumnExists("EndDate"))
                officialTaskModel.EndDate = SqlHelper.GetDateTime(reader, "EndDate");

            if (reader.IsColumnExists("NumberofDays"))
                officialTaskModel.NumberofDays = SqlHelper.GetNullableInt32(reader, "NumberofDays");

            if (reader.IsColumnExists("OfficialTaskDescription"))
                officialTaskModel.OfficialTaskDescription = SqlHelper.GetNullableString(reader, "OfficialTaskDescription");

            if (reader.IsColumnExists("UpdatedBy"))
                officialTaskModel.UpdatedBy = SqlHelper.GetNullableInt32(reader, "UpdatedBy");

            if (reader.IsColumnExists("UpdatedDateTime"))
                officialTaskModel.UpdatedDateTime = SqlHelper.GetDateTime(reader, "UpdatedDateTime");

            if (reader.IsColumnExists("Action"))
                officialTaskModel.Action = SqlHelper.GetNullableString(reader, "Action");

            return officialTaskModel;
        }

        public static List<OfficialTaskPutModel> TranslateAsPutOfficialTaskList(this SqlDataReader reader)
        {
            var officialTaskList = new List<OfficialTaskPutModel>();
            while (reader.Read())
            {
                officialTaskList.Add(TranslateAsPutOfficialTask(reader, true));
            }

            return officialTaskList;
        }
    }
}