using RulersCourt.Models.OfficalTaskCompensation.Compensation;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Translators.OfficalTaskCompensation.Compensation
{
    public static class CompensationTranslator
    {
        public static CompensationGetModel TranslateAsCompensation(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var compensationModel = new CompensationGetModel();

            if (reader.IsColumnExists("CompensationID"))
                compensationModel.CompensationID = SqlHelper.GetNullableInt32(reader, "CompensationID");

            if (reader.IsColumnExists("ReferenceNumber"))
                compensationModel.ReferenceNumber = SqlHelper.GetNullableString(reader, "ReferenceNumber");

            if (reader.IsColumnExists("OfficialTaskID"))
                compensationModel.OfficialTaskID = SqlHelper.GetNullableInt32(reader, "OfficialTaskID");

            if (reader.IsColumnExists("SourceOU"))
                compensationModel.SourceOU = SqlHelper.GetNullableString(reader, "SourceOU");

            if (reader.IsColumnExists("SourceName"))
                compensationModel.SourceName = SqlHelper.GetNullableString(reader, "SourceName");

            if (reader.IsColumnExists("ApproverDepartmentID"))
                compensationModel.ApproverDepartmentID = SqlHelper.GetNullableString(reader, "ApproverDepartmentID");

            if (reader.IsColumnExists("ApproverNameID"))
                compensationModel.ApproverID = SqlHelper.GetNullableString(reader, "ApproverNameID");

            if (reader.IsColumnExists("OfficialTaskType"))
                compensationModel.OfficialTaskType = SqlHelper.GetNullableString(reader, "OfficialTaskType");

            if (reader.IsColumnExists("StartDate"))
                compensationModel.StartDate = SqlHelper.GetDateTime(reader, "StartDate");

            if (reader.IsColumnExists("EndDate"))
                compensationModel.EndDate = SqlHelper.GetDateTime(reader, "EndDate");

            if (reader.IsColumnExists("NumberofDays"))
                compensationModel.NumberofDays = SqlHelper.GetNullableInt32(reader, "NumberofDays");

            if (reader.IsColumnExists("CompensationDescription"))
                compensationModel.CompensationDescription = SqlHelper.GetNullableString(reader, "CompensationDescription");

            if (reader.IsColumnExists("NeedCompensation"))
                compensationModel.NeedCompensation = SqlHelper.GetBoolean(reader, "NeedCompensation");

            if (reader.IsColumnExists("AssigneeID"))
                compensationModel.AssigneeID = SqlHelper.GetNullableInt32(reader, "AssigneeID");

            if (reader.IsColumnExists("HRManagerUserID"))
                compensationModel.HRManagerUserID = SqlHelper.GetNullableInt32(reader, "HRManagerUserID");

            if (reader.IsColumnExists("CreatedBy"))
                compensationModel.CreatedBy = SqlHelper.GetNullableInt32(reader, "CreatedBy");

            if (reader.IsColumnExists("UpdatedBy"))
                compensationModel.UpdatedBy = SqlHelper.GetNullableInt32(reader, "UpdatedBy");

            if (reader.IsColumnExists("Comments"))
                compensationModel.Comments = SqlHelper.GetNullableString(reader, "Comments");

            if (reader.IsColumnExists("Status"))
                compensationModel.Status = SqlHelper.GetNullableInt32(reader, "Status");

            if (reader.IsColumnExists("CreatedDateTime"))
                compensationModel.CreatedDateTime = SqlHelper.GetDateTime(reader, "CreatedDateTime");

            if (reader.IsColumnExists("UpdatedDateTime"))
                compensationModel.UpdatedDateTime = SqlHelper.GetDateTime(reader, "UpdatedDateTime");

            return compensationModel;
        }

        public static List<CompensationGetModel> TranslateAsCompensationList(this SqlDataReader reader)
        {
            var compensationList = new List<CompensationGetModel>();
            while (reader.Read())
            {
                compensationList.Add(TranslateAsCompensation(reader, true));
            }

            return compensationList;
        }

        public static CompensationPutModel TranslateAsPutCompensation(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var compensationModel = new CompensationPutModel();

            if (reader.IsColumnExists("CompensationID"))
                compensationModel.CompensationID = SqlHelper.GetNullableInt32(reader, "CompensationID");

            if (reader.IsColumnExists("SourceOU"))
                compensationModel.SourceOU = SqlHelper.GetNullableString(reader, "SourceOU");

            if (reader.IsColumnExists("SourceName"))
                compensationModel.SourceName = SqlHelper.GetNullableString(reader, "SourceName");

            if (reader.IsColumnExists("ApproverDepartmentID"))
                compensationModel.ApproverDepartmentID = SqlHelper.GetNullableInt32(reader, "ApproverDepartmentID");

            if (reader.IsColumnExists("ApproverNameID"))
                compensationModel.ApproverID = SqlHelper.GetNullableInt32(reader, "ApproverNameID");

            if (reader.IsColumnExists("OfficialTaskType"))
                compensationModel.OfficialTaskType = SqlHelper.GetNullableString(reader, "OfficialTaskType");

            if (reader.IsColumnExists("StartDate"))
                compensationModel.StartDate = SqlHelper.GetDateTime(reader, "StartDate");

            if (reader.IsColumnExists("EndDate"))
                compensationModel.EndDate = SqlHelper.GetDateTime(reader, "EndDate");

            if (reader.IsColumnExists("NumberofDays"))
                compensationModel.NumberofDays = SqlHelper.GetNullableInt32(reader, "NumberofDays");

            if (reader.IsColumnExists("CompensationDescription"))
                compensationModel.CompensationDescription = SqlHelper.GetNullableString(reader, "CompensationDescription");

            if (reader.IsColumnExists("NeedCompensation"))
                compensationModel.NeedCompensation = SqlHelper.GetBoolean(reader, "NeedCompensation");

            if (reader.IsColumnExists("UpdatedBy"))
                compensationModel.UpdatedBy = SqlHelper.GetNullableInt32(reader, "UpdatedBy");

            if (reader.IsColumnExists("UpdatedDateTime"))
                compensationModel.UpdatedDateTime = SqlHelper.GetDateTime(reader, "UpdatedDateTime");

            if (reader.IsColumnExists("Action"))
                compensationModel.Action = SqlHelper.GetNullableString(reader, "Action");

            if (reader.IsColumnExists("HRManagerUserID"))
                compensationModel.HRManagerUserID = SqlHelper.GetNullableInt32(reader, "HRManagerUserID");

            return compensationModel;
        }

        public static List<CompensationPutModel> TranslateAsPutCompensationList(this SqlDataReader reader)
        {
            var compensationList = new List<CompensationPutModel>();
            while (reader.Read())
            {
                compensationList.Add(TranslateAsPutCompensation(reader, true));
            }

            return compensationList;
        }

        public static CompensationPreviewModel TranslateAsCompensationPreview(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var compensationModel = new CompensationPreviewModel();

            if (reader.IsColumnExists("AssigneeEmployeeID"))
                compensationModel.AssigneeEmployeeID = SqlHelper.GetNullableString(reader, "AssigneeEmployeeID");

            if (reader.IsColumnExists("AssigneeName"))
                compensationModel.AssigneeName = SqlHelper.GetNullableString(reader, "AssigneeName");

            if (reader.IsColumnExists("NoOfDays"))
                compensationModel.NoOfDays = SqlHelper.GetNullableInt32(reader, "NoOfDays");

            if (reader.IsColumnExists("OfficialTaskCreatorDesignation"))
                compensationModel.OfficialTaskCreatorDesignation = SqlHelper.GetNullableString(reader, "OfficialTaskCreatorDesignation");

            if (reader.IsColumnExists("OfficialTaskCreatorName"))
                compensationModel.OfficialTaskCreatorName = SqlHelper.GetNullableString(reader, "OfficialTaskCreatorName");

            if (reader.IsColumnExists("OfficialTaskDescription"))
                compensationModel.OfficialTaskDescription = SqlHelper.GetNullableString(reader, "OfficialTaskDescription");

            if (reader.IsColumnExists("OfficialTaskReferenceNo"))
                compensationModel.OfficialTaskReferenceNo = SqlHelper.GetNullableString(reader, "OfficialTaskReferenceNo");

            if (reader.IsColumnExists("CompensationReferenceNo"))
                compensationModel.CompensationReferenceNo = SqlHelper.GetNullableString(reader, "CompensationReferenceNo");

            if (reader.IsColumnExists("StartDate"))
                compensationModel.StartDate = SqlHelper.GetDateTime(reader, "StartDate");

            if (reader.IsColumnExists("EmployeeDetails"))
                compensationModel.EmployeeDetails = SqlHelper.GetNullableString(reader, "EmployeeDetails");

            return compensationModel;
        }

        public static CompensationPreviewModel TranslateAsCompensationPreviewModel(this SqlDataReader reader)
        {
            var compensationPreviewModel = new CompensationPreviewModel();
            while (reader.Read())
            {
                compensationPreviewModel = TranslateAsCompensationPreview(reader, true);
            }

            return compensationPreviewModel;
        }
    }
}