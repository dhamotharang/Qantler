using RulersCourt.Models;
using RulersCourt.Models.Maintenance;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Translators.Maintenance
{
    public static class MaintenanceTranslator
    {
        public static MaintenanceWorkflowModel TranslateAsMaintenanceSaveResponse(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var maintenance = new MaintenanceWorkflowModel();

            if (reader.IsColumnExists("MaintenanceID"))
                maintenance.MaintenanceID = SqlHelper.GetNullableInt32(reader, "MaintenanceID");

            if (reader.IsColumnExists("ReferenceNumber"))
                maintenance.ReferenceNumber = SqlHelper.GetNullableString(reader, "ReferenceNumber");

            if (reader.IsColumnExists("CreatorID"))
                maintenance.CreatorID = SqlHelper.GetNullableInt32(reader, "CreatorID").GetValueOrDefault();

            if (reader.IsColumnExists("FromID"))
                maintenance.FromID = SqlHelper.GetNullableInt32(reader, "FromID").GetValueOrDefault();

            if (reader.IsColumnExists("CurrentStatus"))
            {
                maintenance.CurrentStatus = SqlHelper.GetNullableInt32(reader, "CurrentStatus").GetValueOrDefault();
            }

            return maintenance;
        }

        public static MaintenanceWorkflowModel TranslateAsMaintenanceSaveResponseList(this SqlDataReader reader)
        {
            var maintenanceSaveResponse = new MaintenanceWorkflowModel();
            while (reader.Read())
            {
                maintenanceSaveResponse = TranslateAsMaintenanceSaveResponse(reader, true);
            }

            return maintenanceSaveResponse;
        }

        public static MaintenancePutModel TranslateAsPutMaintenance(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                {
                    return null;
                }

                reader.Read();
            }

            var maintenanceModel = new MaintenancePutModel();

            if (reader.IsColumnExists("MaintenanceID"))
            {
                maintenanceModel.MaintenanceID = SqlHelper.GetNullableInt32(reader, "MaintenanceID");
            }

            if (reader.IsColumnExists("SourceOU"))
            {
                maintenanceModel.SourceOU = SqlHelper.GetNullableString(reader, "SourceOU");
            }

            if (reader.IsColumnExists("SourceName"))
            {
                maintenanceModel.SourceName = SqlHelper.GetNullableString(reader, "SourceName");
            }

            if (reader.IsColumnExists("ApproverDepartment"))
            {
                maintenanceModel.ApproverDepartmentID = SqlHelper.GetNullableInt32(reader, "ApproverDepartment");
            }

            if (reader.IsColumnExists("Details"))
            {
                maintenanceModel.RequestDetails = SqlHelper.GetNullableString(reader, "Details");
            }

            if (reader.IsColumnExists("Subject"))
            {
                maintenanceModel.Subject = SqlHelper.GetNullableString(reader, "Subject");
            }

            if (reader.IsColumnExists("RequestorID"))
            {
                maintenanceModel.RequestorID = SqlHelper.GetNullableInt32(reader, "RequestorID");
            }

            if (reader.IsColumnExists("RequestorDepartmentID"))
            {
                maintenanceModel.RequestorDepartmentID = SqlHelper.GetNullableInt32(reader, "RequestorDepartmentID");
            }

            if (reader.IsColumnExists("Priority"))
            {
                maintenanceModel.Priority = SqlHelper.GetBoolean(reader, "Priority");
            }

            if (reader.IsColumnExists("UpdatedBy"))
            {
                maintenanceModel.UpdatedBy = SqlHelper.GetNullableInt32(reader, "UpdatedBy");
            }

            if (reader.IsColumnExists("UpdatedDateTime"))
            {
                maintenanceModel.UpdatedDateTime = SqlHelper.GetDateTime(reader, "UpdatedDateTime");
            }

            if (reader.IsColumnExists("Action"))
            {
                maintenanceModel.Action = SqlHelper.GetNullableString(reader, "Action");
            }

            if (reader.IsColumnExists("MaintenanceManagerUserID"))
            {
                maintenanceModel.MaintenanceManagerUserID = SqlHelper.GetNullableInt32(reader, "MaintenanceManagerUserID");
            }

            return maintenanceModel;
        }

        public static List<MaintenancePutModel> TranslateAsPutMaintenanceList(this SqlDataReader reader)
        {
            var maintenanceList = new List<MaintenancePutModel>();
            while (reader.Read())
            {
                maintenanceList.Add(TranslateAsPutMaintenance(reader, true));
            }

            return maintenanceList;
        }

        public static MaintenanceGetModel TranslateAsMaintenance(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                {
                    return null;
                }

                reader.Read();
            }

            var maintenanceModel = new MaintenanceGetModel();

            if (reader.IsColumnExists("MaintenanceID"))
            {
                maintenanceModel.MaintenanceID = SqlHelper.GetNullableInt32(reader, "MaintenanceID");
            }

            if (reader.IsColumnExists("ReferenceNumber"))
            {
                maintenanceModel.ReferenceNumber = SqlHelper.GetNullableString(reader, "ReferenceNumber");
            }

            if (reader.IsColumnExists("Subject"))
            {
                maintenanceModel.Subject = SqlHelper.GetNullableString(reader, "Subject");
            }

            if (reader.IsColumnExists("Details"))
            {
                maintenanceModel.RequestDetails = SqlHelper.GetNullableString(reader, "Details");
            }

            if (reader.IsColumnExists("Priority"))
            {
                maintenanceModel.Priority = SqlHelper.GetBoolean(reader, "Priority");
            }

            if (reader.IsColumnExists("SourceOU"))
            {
                maintenanceModel.SourceOU = SqlHelper.GetNullableString(reader, "SourceOU");
            }

            if (reader.IsColumnExists("SourceName"))
            {
                maintenanceModel.SourceName = SqlHelper.GetNullableString(reader, "SourceName");
            }

            if (reader.IsColumnExists("RequestorID"))
            {
                maintenanceModel.RequestorID = SqlHelper.GetNullableInt32(reader, "RequestorID");
            }

            if (reader.IsColumnExists("RequestorDepartmentID"))
            {
                maintenanceModel.RequestorDepartmentID = SqlHelper.GetNullableInt32(reader, "RequestorDepartmentID");
            }

            if (reader.IsColumnExists("ApproverNameID"))
            {
                maintenanceModel.ApproverID = SqlHelper.GetNullableInt32(reader, "ApproverNameID");
            }

            if (reader.IsColumnExists("ApproverDepartment"))
            {
                maintenanceModel.ApproverDepartmentID = SqlHelper.GetNullableInt32(reader, "ApproverDepartment");
            }

            if (reader.IsColumnExists("Status"))
            {
                maintenanceModel.Status = SqlHelper.GetNullableInt32(reader, "Status");
            }

            if (reader.IsColumnExists("MaintenanceManagerUserID"))
            {
                maintenanceModel.MaintenanceManagerUserID = SqlHelper.GetNullableInt32(reader, "MaintenanceManagerUserID");
            }

            if (reader.IsColumnExists("CreatedBy"))
            {
                maintenanceModel.CreatedBy = SqlHelper.GetNullableInt32(reader, "CreatedBy");
            }

            if (reader.IsColumnExists("UpdatedBy"))
            {
                maintenanceModel.UpdatedBy = SqlHelper.GetNullableInt32(reader, "UpdatedBy");
            }

            if (reader.IsColumnExists("CreatedDateTime"))
            {
                maintenanceModel.CreatedDateTime = SqlHelper.GetDateTime(reader, "CreatedDateTime");
            }

            if (reader.IsColumnExists("UpdatedDateTime"))
            {
                maintenanceModel.UpdatedDateTime = SqlHelper.GetDateTime(reader, "UpdatedDateTime");
            }

            return maintenanceModel;
        }

        public static List<MaintenanceGetModel> TranslateAsMaintenanceList(this SqlDataReader reader)
        {
            var maintenanceList = new List<MaintenanceGetModel>();
            while (reader.Read())
            {
                maintenanceList.Add(TranslateAsMaintenance(reader, true));
            }

            return maintenanceList;
        }

        public static MaintenanceHomeDashboardListModel TranslateAsMaintenanceDashboard(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                {
                    return null;
                }

                reader.Read();
            }

            var maintenanceDashboardModel = new MaintenanceHomeDashboardListModel();

            if (reader.IsColumnExists("MaintenanceID"))
            {
                maintenanceDashboardModel.MaintenanceID = SqlHelper.GetNullableInt32(reader, "MaintenanceID");
            }

            if (reader.IsColumnExists("ReferenceNumber"))
            {
                maintenanceDashboardModel.ReferenceNumber = SqlHelper.GetNullableString(reader, "ReferenceNumber");
            }

            if (reader.IsColumnExists("Source"))
            {
                maintenanceDashboardModel.Source = SqlHelper.GetNullableString(reader, "Source");
            }

            if (reader.IsColumnExists("Subject"))
            {
                maintenanceDashboardModel.Subject = SqlHelper.GetNullableString(reader, "Subject");
            }

            if (reader.IsColumnExists("AttendedBy"))
            {
                maintenanceDashboardModel.AttendedBy = SqlHelper.GetNullableString(reader, "AttendedBy");
            }

            if (reader.IsColumnExists("RequestDate"))
            {
                maintenanceDashboardModel.RequestDate = SqlHelper.GetDateTime(reader, "RequestDate");
            }

            if (reader.IsColumnExists("Priority"))
            {
                maintenanceDashboardModel.Priority = SqlHelper.GetNullableString(reader, "Priority");
            }

            if (reader.IsColumnExists("Status"))
            {
                maintenanceDashboardModel.Status = SqlHelper.GetNullableString(reader, "Status");
            }

            if (reader.IsColumnExists("AssignedTo"))
            {
                maintenanceDashboardModel.AssignedTo = SqlHelper.GetNullableString(reader, "AssignedTo");
            }

            return maintenanceDashboardModel;
        }

        public static List<MaintenanceHomeDashboardListModel> TranslateAsMaintenanceDashboardList(this SqlDataReader reader)
        {
            var maintenanceDashboardList = new List<MaintenanceHomeDashboardListModel>();
            while (reader.Read())
            {
                maintenanceDashboardList.Add(TranslateAsMaintenanceDashboard(reader, true));
            }

            return maintenanceDashboardList;
        }

        public static MaintenanceReportModel TranslateAsMaintenanceReport(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var report = new MaintenanceReportModel();

            if (reader.IsColumnExists("ReferenceNumber"))
                report.ReferenceNumber = SqlHelper.GetNullableString(reader, "ReferenceNumber");

            if (reader.IsColumnExists("SourceOU"))
                report.SourceOU = SqlHelper.GetNullableString(reader, "SourceOU");

            if (reader.IsColumnExists("Subject"))
                report.Subject = SqlHelper.GetNullableString(reader, "Subject");

            if (reader.IsColumnExists("Status"))
                report.Status = SqlHelper.GetNullableString(reader, "Status");

            if (reader.IsColumnExists("RequestedDateTime"))
            {
                DateTime? date = SqlHelper.GetDateTime(reader, "RequestedDateTime");
                report.RequestedDateTime = date != null ? date.Value.ToString("dd-MM-yyyy").Replace("-", "/") : string.Empty;
            }

            if (reader.IsColumnExists("AttendedBy"))
                report.AttendedBy = SqlHelper.GetNullableString(reader, "AttendedBy");

            if (reader.IsColumnExists("Priority"))
                report.Priority = SqlHelper.GetNullableString(reader, "Priority");

            return report;
        }

        public static List<MaintenanceReportModel> TranslateAsMaintenanceReportList(this SqlDataReader reader)
        {
            var list = new List<MaintenanceReportModel>();
            while (reader.Read())
            {
                list.Add(TranslateAsMaintenanceReport(reader, true));
            }

            return list;
        }

        public static MaintenanceHomeCountModel TranslateasmaintananceDashboardcount(this SqlDataReader reader)
        {
            var maintanancehomemodel = new MaintenanceHomeCountModel();
            while (reader.Read())
            {
                maintanancehomemodel = TranslateAsmaintananceDashboardCount(reader, true);
            }

            return maintanancehomemodel;
        }

        public static MaintenanceHomeCountModel TranslateAsmaintananceDashboardCount(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                {
                    return null;
                }

                reader.Read();
            }

            var maintanancehomemodel = new MaintenanceHomeCountModel();

            if (reader.IsColumnExists("New"))
            {
                maintanancehomemodel.New = SqlHelper.GetNullableInt32(reader, "New");
            }

            if (reader.IsColumnExists("NeedMoreInfo"))
            {
                maintanancehomemodel.NeedMoreInfo = SqlHelper.GetNullableInt32(reader, "NeedMoreInfo");
            }

            if (reader.IsColumnExists("Closed"))
            {
                maintanancehomemodel.Closed = SqlHelper.GetNullableInt32(reader, "Closed");
            }

            if (reader.IsColumnExists("MyOwnRequest"))
            {
                maintanancehomemodel.MyOwnRequest = SqlHelper.GetNullableInt32(reader, "MyOwnRequest");
            }

            if (reader.IsColumnExists("MyPendingRequest"))
            {
                maintanancehomemodel.MyPendingRequest = SqlHelper.GetNullableInt32(reader, "MyPendingRequest");
            }

            if (reader.IsColumnExists("InProgressRequest"))
            {
                maintanancehomemodel.InProgressRequest = SqlHelper.GetNullableInt32(reader, "InProgressRequest");
            }

            if (reader.IsColumnExists("MyProcessedRequest"))
            {
                maintanancehomemodel.MyProcessedRequest = SqlHelper.GetNullableInt32(reader, "MyProcessedRequest");
            }

            return maintanancehomemodel;
        }
    }
}
