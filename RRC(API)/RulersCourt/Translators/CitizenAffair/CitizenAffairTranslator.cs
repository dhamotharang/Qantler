using RulersCourt.Models.CitizenAffair;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Translators.CitizenAffair
{
    public static class CitizenAffairTranslator
    {
        public static CitizenAffairWorkflowModel TranslateAsCitizenAffairSaveResponse(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                {
                    return null;
                }

                reader.Read();
            }

            var cASave = new CitizenAffairWorkflowModel();

            if (reader.IsColumnExists("CitizenAffairID"))
            {
                cASave.CitizenAffairID = SqlHelper.GetNullableInt32(reader, "CitizenAffairID");
            }

            if (reader.IsColumnExists("ReferenceNumber"))
            {
                cASave.ReferenceNumber = SqlHelper.GetNullableString(reader, "ReferenceNumber");
            }

            if (reader.IsColumnExists("CreatorID"))
            {
                cASave.CreatorID = SqlHelper.GetNullableInt32(reader, "CreatorID").GetValueOrDefault();
            }

            if (reader.IsColumnExists("FromID"))
            {
                cASave.FromID = SqlHelper.GetNullableInt32(reader, "FromID").GetValueOrDefault();
            }

            if (reader.IsColumnExists("CurrentStatus"))
            {
                cASave.CurrentStatus = SqlHelper.GetNullableInt32(reader, "CurrentStatus").GetValueOrDefault();
            }

            if (reader.IsColumnExists("InternalRequestorID"))
            {
                cASave.InternalRequestorID = SqlHelper.GetNullableInt32(reader, "InternalRequestorID");
            }

            if (reader.IsColumnExists("ExternalRequestEmail"))
            {
                cASave.ExternalRequestEmailID = SqlHelper.GetNullableString(reader, "ExternalRequestEmail");
            }

            return cASave;
        }

        public static CitizenAffairWorkflowModel TranslateAsCitizenAffairSaveResponseList(this SqlDataReader reader)
        {
            var cASaveResponse = new CitizenAffairWorkflowModel();
            while (reader.Read())
            {
                cASaveResponse = TranslateAsCitizenAffairSaveResponse(reader, true);
            }

            return cASaveResponse;
        }

        public static CitizenAffairGetModel TranslateAsGetCitizenAffair(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                {
                    return null;
                }

                reader.Read();
            }

            var citizenAffair = new CitizenAffairGetModel();

            if (reader.IsColumnExists("CitizenAffairID"))
            {
                citizenAffair.CitizenAffairID = SqlHelper.GetNullableInt32(reader, "CitizenAffairID");
            }

            if (reader.IsColumnExists("SourceOU"))
            {
                citizenAffair.SourceOU = SqlHelper.GetNullableString(reader, "SourceOU");
            }

            if (reader.IsColumnExists("SourceName"))
            {
                citizenAffair.SourceName = SqlHelper.GetNullableString(reader, "SourceName");
            }

            if (reader.IsColumnExists("Status"))
            {
                citizenAffair.Status = SqlHelper.GetNullableInt32(reader, "Status");
            }

            if (reader.IsColumnExists("CurrentApproverID"))
            {
                citizenAffair.CurrentApproverID = SqlHelper.GetNullableInt32(reader, "CurrentApproverID");
            }

            if (reader.IsColumnExists("ReferenceNumber"))
            {
                citizenAffair.ReferenceNumber = SqlHelper.GetNullableString(reader, "ReferenceNumber");
            }

            if (reader.IsColumnExists("RequestType"))
            {
                citizenAffair.RequestType = SqlHelper.GetNullableString(reader, "RequestType");
            }

            if (reader.IsColumnExists("ApproverNameID"))
            {
                citizenAffair.ApproverId = SqlHelper.GetNullableInt32(reader, "ApproverNameID");
            }

            if (reader.IsColumnExists("InitalApproverDepartmentID"))
            {
                citizenAffair.ApproverDepartmentId = SqlHelper.GetNullableInt32(reader, "InitalApproverDepartmentID");
            }

            if (reader.IsColumnExists("NotifyUpon"))
            {
                citizenAffair.NotifyUpon = SqlHelper.GetNullableString(reader, "NotifyUpon");
            }

            if (reader.IsColumnExists("InternalRequestorID"))
            {
                citizenAffair.InternalRequestorID = SqlHelper.GetNullableInt32(reader, "InternalRequestorID");
            }

            if (reader.IsColumnExists("InternalRequestorDepartmentID"))
            {
                citizenAffair.InternalRequestorDepartmentID = SqlHelper.GetNullableInt32(reader, "InternalRequestorDepartmentID");
            }

            if (reader.IsColumnExists("ExternalRequestEmailID"))
            {
                citizenAffair.ExternalRequestEmailID = SqlHelper.GetNullableString(reader, "ExternalRequestEmailID");
            }

            if (reader.IsColumnExists("CreatedBy"))
            {
                citizenAffair.CreatedBy = SqlHelper.GetNullableInt32(reader, "CreatedBy");
            }

            if (reader.IsColumnExists("CreatedDateTime"))
            {
                citizenAffair.CreatedDateTime = SqlHelper.GetDateTime(reader, "CreatedDateTime");
            }

            if (reader.IsColumnExists("UpdatedBy"))
            {
                citizenAffair.UpdatedBy = SqlHelper.GetNullableInt32(reader, "UpdatedBy");
            }

            if (reader.IsColumnExists("UpdatedDateTime"))
            {
                citizenAffair.UpdatedDateTime = SqlHelper.GetDateTime(reader, "UpdatedDateTime");
            }

            return citizenAffair;
        }

        public static List<CitizenAffairGetModel> TranslateAsCitizenAffairList(this SqlDataReader reader)
        {
            var citizenAffairList = new List<CitizenAffairGetModel>();
            while (reader.Read())
            {
                citizenAffairList.Add(TranslateAsGetCitizenAffair(reader, true));
            }

            return citizenAffairList;
        }

        public static CitizenAffairPutModel TranslateAsPutCitizenAffair(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                {
                    return null;
                }

                reader.Read();
            }

            var citizenAffairPut = new CitizenAffairPutModel();

            if (reader.IsColumnExists("CitizenAffairID"))
            {
                citizenAffairPut.CitizenAffairID = SqlHelper.GetNullableInt32(reader, "CitizenAffairID");
            }

            if (reader.IsColumnExists("SourceOU"))
            {
                citizenAffairPut.SourceOU = SqlHelper.GetNullableString(reader, "SourceOU");
            }

            if (reader.IsColumnExists("SourceName"))
            {
                citizenAffairPut.SourceName = SqlHelper.GetNullableString(reader, "SourceName");
            }

            if (reader.IsColumnExists("RequestType"))
            {
                citizenAffairPut.RequestType = SqlHelper.GetNullableString(reader, "RequestType");
            }

            if (reader.IsColumnExists("ReferenceNumber"))
            {
                citizenAffairPut.ReferenceNumber = SqlHelper.GetNullableString(reader, "ReferenceNumber");
            }

            if (reader.IsColumnExists("ApproverNameID"))
            {
                citizenAffairPut.ApproverId = SqlHelper.GetNullableInt32(reader, "ApproverNameID");
            }

            if (reader.IsColumnExists("InitalApproverDepartmentID"))
            {
                citizenAffairPut.ApproverDepartmentId = SqlHelper.GetNullableInt32(reader, "InitalApproverDepartmentID");
            }

            if (reader.IsColumnExists("NotifyUpon"))
            {
                citizenAffairPut.NotifyUpon = SqlHelper.GetNullableString(reader, "NotifyUpon");
            }

            if (reader.IsColumnExists("InternalRequestorID"))
            {
                citizenAffairPut.InternalRequestorID = SqlHelper.GetNullableInt32(reader, "InternalRequestorID");
            }

            if (reader.IsColumnExists("InternalRequestorDepartmentID"))
            {
                citizenAffairPut.InternalRequestorDepartmentID = SqlHelper.GetNullableInt32(reader, "InternalRequestorDepartmentID");
            }

            if (reader.IsColumnExists("ExternalRequestEmailID"))
            {
                citizenAffairPut.ExternalRequestEmailID = SqlHelper.GetNullableString(reader, "ExternalRequestEmailID");
            }

            if (reader.IsColumnExists("UpdatedBy"))
            {
                citizenAffairPut.UpdatedBy = SqlHelper.GetNullableInt32(reader, "UpdatedBy");
            }

            if (reader.IsColumnExists("UpdatedDateTime"))
            {
                citizenAffairPut.UpdatedDateTime = SqlHelper.GetDateTime(reader, "UpdatedDateTime");
            }

            return citizenAffairPut;
        }

        public static CitizenAffairPutModel TranslateAsCitizenAffairPutList(this SqlDataReader reader)
        {
            var citizenAffairList = new CitizenAffairPutModel();
            while (reader.Read())
            {
                citizenAffairList = TranslateAsPutCitizenAffair(reader, true);
            }

            return citizenAffairList;
        }

        public static CitizenAffairDashboardListModel TranslateAsCitizenAffairDashboard(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                {
                    return null;
                }

                reader.Read();
            }

            var cADashboardModel = new CitizenAffairDashboardListModel();

            if (reader.IsColumnExists("ReferenceNumber"))
            {
                cADashboardModel.ReferenceNumber = SqlHelper.GetNullableString(reader, "ReferenceNumber");
            }

            if (reader.IsColumnExists("CitizenAffairID"))
            {
                cADashboardModel.CitizenAffairID = SqlHelper.GetNullableInt32(reader, "CitizenAffairID");
            }

            if (reader.IsColumnExists("RequestType"))
            {
                cADashboardModel.RequestType = SqlHelper.GetNullableString(reader, "RequestType");
            }

            if (reader.IsColumnExists("LocationName"))
            {
                cADashboardModel.PersonalName = SqlHelper.GetNullableString(reader, "LocationName");
            }

            if (reader.IsColumnExists("PhoneNumber"))
            {
                cADashboardModel.PhoneNumber = SqlHelper.GetNullableString(reader, "PhoneNumber");
            }

            if (reader.IsColumnExists("RequestDate"))
            {
                cADashboardModel.RequestDate = SqlHelper.GetDateTime(reader, "RequestDate");
            }

            if (reader.IsColumnExists("Status"))
            {
                cADashboardModel.Status = SqlHelper.GetNullableString(reader, "Status");
            }

            if (reader.IsColumnExists("AttendedBy"))
            {
                cADashboardModel.Creator = SqlHelper.GetNullableString(reader, "AttendedBy");
            }

            if (reader.IsColumnExists("AssignedTo"))
            {
                cADashboardModel.AssignedTo = SqlHelper.GetNullableString(reader, "AssignedTo");
            }

            if (reader.IsColumnExists("Reporter"))
            {
                cADashboardModel.Reporter = SqlHelper.GetNullableString(reader, "Reporter");
            }

            return cADashboardModel;
        }

        public static List<CitizenAffairDashboardListModel> TranslateAsCitizenAffairDashboardList(this SqlDataReader reader)
        {
            var cADashboardList = new List<CitizenAffairDashboardListModel>();
            while (reader.Read())
            {
                cADashboardList.Add(TranslateAsCitizenAffairDashboard(reader, true));
            }

            return cADashboardList;
        }

        public static CitizenAffairReportModel TranslateAsCAReport(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                {
                    return null;
                }

                reader.Read();
            }

            var report = new CitizenAffairReportModel();

            if (reader.IsColumnExists("CitizenAffairID"))
            {
                report.CitizenAffairID = SqlHelper.GetNullableInt32(reader, "CitizenAffairID");
            }

            if (reader.IsColumnExists("ReferenceNumber"))
            {
                report.ReferenceNumber = SqlHelper.GetNullableString(reader, "ReferenceNumber");
            }

            if (reader.IsColumnExists("RequestType"))
            {
                report.RequestType = SqlHelper.GetNullableString(reader, "RequestType");
            }

            if (reader.IsColumnExists("PersonalLocationName"))
            {
                report.PersonalLocationName = SqlHelper.GetNullableString(reader, "PersonalLocationName");
            }

            if (reader.IsColumnExists("Status"))
            {
                report.Status = SqlHelper.GetNullableString(reader, "Status");
            }

            if (reader.IsColumnExists("RequestedDateTime"))
            {
                DateTime? date = SqlHelper.GetDateTime(reader, "RequestedDateTime");
                report.RequestedDateTime = date != null ? date.Value.ToString("dd-MM-yyyy").Replace("-", "/") : string.Empty;
            }

            if (reader.IsColumnExists("PhoneNumber"))
            {
                report.PhoneNumber = SqlHelper.GetNullableString(reader, "PhoneNumber");
            }

            return report;
        }

        public static List<CitizenAffairReportModel> TranslateAsCAReportList(this SqlDataReader reader)
        {
            var list = new List<CitizenAffairReportModel>();
            while (reader.Read())
            {
                list.Add(TranslateAsCAReport(reader, true));
            }

            return list;
        }

        public static CitizenHomeModel TranslateasCitizenDashboardcount(this SqlDataReader reader)
        {
            var citizenhomemodel = new CitizenHomeModel();
            while (reader.Read())
            {
                citizenhomemodel = TranslateAsCitizenDashboardCount(reader, true);
            }

            return citizenhomemodel;
        }

        public static CitizenHomeModel TranslateAsCitizenDashboardCount(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                {
                    return null;
                }

                reader.Read();
            }

            var citizenhomemodel = new CitizenHomeModel();

            if (reader.IsColumnExists("New"))
            {
                citizenhomemodel.New = SqlHelper.GetNullableInt32(reader, "New");
            }

            if (reader.IsColumnExists("NeedMoreInfo"))
            {
                citizenhomemodel.NeedMoreInfo = SqlHelper.GetNullableInt32(reader, "NeedMoreInfo");
            }

            if (reader.IsColumnExists("Closed"))
            {
                citizenhomemodel.Closed = SqlHelper.GetNullableInt32(reader, "Closed");
            }

            if (reader.IsColumnExists("MyOwnRequest"))
            {
                citizenhomemodel.MyOwnRequest = SqlHelper.GetNullableInt32(reader, "MyOwnRequest");
            }

            if (reader.IsColumnExists("InProgressRequest"))
            {
                citizenhomemodel.InProgressRequest = SqlHelper.GetNullableInt32(reader, "InProgressRequest");
            }

            return citizenhomemodel;
        }
    }
}
