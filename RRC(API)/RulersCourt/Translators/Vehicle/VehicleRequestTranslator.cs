using RulersCourt.Models.Vehicle.VehicleRequest;
using RulersCourt.Models.Vehicle.Vehicles;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Translators.Vehicle
{
    public static class VehicleRequestTranslator
    {
        public static VehicleRequestGetModel TranslateAsGetVehicleRequest(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var vehicleGetModel = new VehicleRequestGetModel();

            if (reader.IsColumnExists("VehicleReqID"))
                vehicleGetModel.VehicleReqID = SqlHelper.GetNullableInt32(reader, "vehicleReqID");

            if (reader.IsColumnExists("ReferenceNumber"))
                vehicleGetModel.ReferenceNumber = SqlHelper.GetNullableString(reader, "ReferenceNumber");

            if (reader.IsColumnExists("RequestType"))
                vehicleGetModel.RequestType = SqlHelper.GetNullableInt32(reader, "RequestType");

            if (reader.IsColumnExists("Requestor"))
                vehicleGetModel.Requestor = SqlHelper.GetNullableInt32(reader, "Requestor");

            if (reader.IsColumnExists("RequestDateTime"))
                vehicleGetModel.RequestDateTime = SqlHelper.GetDateTime(reader, "RequestDateTime");

            if (reader.IsColumnExists("DriverID"))
                vehicleGetModel.DriverID = SqlHelper.GetNullableInt32(reader, "DriverID");

            if (reader.IsColumnExists("TobeDrivenbyDepartmentID"))
                vehicleGetModel.TobeDrivenbyDepartmentID = SqlHelper.GetNullableInt32(reader, "TobeDrivenbyDepartmentID");

            if (reader.IsColumnExists("TobeDrivenbyDriverID"))
                vehicleGetModel.TobeDrivenbyDriverID = SqlHelper.GetNullableInt32(reader, "TobeDrivenbyDriverID");

            if (reader.IsColumnExists("TripTypeID"))
                vehicleGetModel.TripTypeID = SqlHelper.GetNullableInt32(reader, "TripTypeID");

            if (reader.IsColumnExists("TripTypeOthers"))
                vehicleGetModel.TripTypeOthers = SqlHelper.GetNullableString(reader, "TripTypeOthers");

            if (reader.IsColumnExists("Emirates"))
                vehicleGetModel.Emirates = SqlHelper.GetNullableInt32(reader, "Emirates");

            if (reader.IsColumnExists("City"))
                vehicleGetModel.City = SqlHelper.GetNullableInt32(reader, "City");

            if (reader.IsColumnExists("Destination"))
                vehicleGetModel.Destination = SqlHelper.GetNullableInt32(reader, "Destination");

            if (reader.IsColumnExists("DestinationOthers"))
                vehicleGetModel.DestinationOthers = SqlHelper.GetNullableString(reader, "DestinationOthers");

            if (reader.IsColumnExists("TripPeriodFrom"))
                vehicleGetModel.TripPeriodFrom = SqlHelper.GetDateTime(reader, "TripPeriodFrom");

            if (reader.IsColumnExists("TripPeriodTo"))
                vehicleGetModel.TripPeriodTo = SqlHelper.GetDateTime(reader, "TripPeriodTo");

            if (reader.IsColumnExists("VehicleModelID"))
                vehicleGetModel.VehicleModelID = SqlHelper.GetNullableInt32(reader, "VehicleModelID");

            if (reader.IsColumnExists("DestinationOthers"))
                vehicleGetModel.DestinationOthers = SqlHelper.GetNullableString(reader, "DestinationOthers");

            if (reader.IsColumnExists("ApproverDepartment"))
                vehicleGetModel.ApproverDepartment = SqlHelper.GetNullableInt32(reader, "ApproverDepartment");

            if (reader.IsColumnExists("ApproverName"))
                vehicleGetModel.ApproverName = SqlHelper.GetNullableInt32(reader, "ApproverName");

            if (reader.IsColumnExists("ReleaseDateTime"))
                vehicleGetModel.ReleaseDateTime = SqlHelper.GetDateTime(reader, "ReleaseDateTime");

            if (reader.IsColumnExists("LastMileageReading"))
                vehicleGetModel.LastMileageReading = SqlHelper.GetNullableInt32(reader, "LastMileageReading");

            if (reader.IsColumnExists("ReleaseLocationID"))
                vehicleGetModel.ReleaseLocationID = SqlHelper.GetNullableInt32(reader, "ReleaseLocationID");

            if (reader.IsColumnExists("ReturnLocationID"))
                vehicleGetModel.ReturnLocationID = SqlHelper.GetNullableInt32(reader, "ReturnLocationID");

            if (reader.IsColumnExists("ReturnDateTime"))
                vehicleGetModel.ReturnDateTime = SqlHelper.GetDateTime(reader, "ReturnDateTime");

            if (reader.IsColumnExists("CurrentMileageReading"))
                vehicleGetModel.CurrentMileageReading = SqlHelper.GetNullableInt32(reader, "CurrentMileageReading");

            if (reader.IsColumnExists("HavePersonalBelongings"))
                vehicleGetModel.HavePersonalBelongings = SqlHelper.GetBoolean(reader, "HavePersonalBelongings");

            if (reader.IsColumnExists("PersonalBelongingsText"))
                vehicleGetModel.PersonalBelongingsText = SqlHelper.GetNullableString(reader, "PersonalBelongingsText");

            if (reader.IsColumnExists("Status"))
                vehicleGetModel.Status = SqlHelper.GetNullableInt32(reader, "Status");

            if (reader.IsColumnExists("Notes"))
                vehicleGetModel.Notes = SqlHelper.GetNullableString(reader, "Notes");

            if (reader.IsColumnExists("CreatedBy"))
                vehicleGetModel.CreatedBy = SqlHelper.GetNullableInt32(reader, "CreatedBy");

            if (reader.IsColumnExists("UpdatedBy"))
                vehicleGetModel.UpdatedBy = SqlHelper.GetNullableInt32(reader, "UpdatedBy");

            if (reader.IsColumnExists("CreatedDateTime"))
                vehicleGetModel.CreatedDateTime = SqlHelper.GetDateTime(reader, "CreatedDateTime");

            if (reader.IsColumnExists("UpdatedDateTime"))
                vehicleGetModel.UpdatedDateTime = SqlHelper.GetDateTime(reader, "UpdatedDateTime");

            if (reader.IsColumnExists("DeleteFlag"))
                vehicleGetModel.DeleteFlag = SqlHelper.GetBoolean(reader, "DeleteFlag");

            if (reader.IsColumnExists("VehicleID"))
                vehicleGetModel.VehicleID = SqlHelper.GetNullableInt32(reader, "VehicleID");

            if (reader.IsColumnExists("Reason"))
                vehicleGetModel.Reason = SqlHelper.GetNullableString(reader, "Reason");

            return vehicleGetModel;
        }

        public static List<VehicleRequestGetModel> TranslateAsVehicleRequestList(this SqlDataReader reader)
        {
            var vehicleList = new List<VehicleRequestGetModel>();
            while (reader.Read())
            {
                vehicleList.Add(TranslateAsGetVehicleRequest(reader, true));
            }

            return vehicleList;
        }

        public static VehicleRequestPutModel TranslateAsPutVehicle(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var vehicleModel = new VehicleRequestPutModel();

            if (reader.IsColumnExists("VehicleReqID"))
                vehicleModel.VehicleReqID = SqlHelper.GetNullableInt32(reader, "vehicleReqID");

            if (reader.IsColumnExists("RequestType"))
                vehicleModel.RequestType = SqlHelper.GetNullableInt32(reader, "RequestType");

            if (reader.IsColumnExists("Requestor"))
                vehicleModel.Requestor = SqlHelper.GetNullableInt32(reader, "Requestor");

            if (reader.IsColumnExists("RequestDateTime"))
                vehicleModel.RequestDateTime = SqlHelper.GetDateTime(reader, "RequestDateTime");

            if (reader.IsColumnExists("DriverID"))
                vehicleModel.DriverID = SqlHelper.GetNullableInt32(reader, "DriverID");

            if (reader.IsColumnExists("TobeDrivenbyDepartmentID"))
                vehicleModel.TobeDrivenbyDepartmentID = SqlHelper.GetNullableInt32(reader, "TobeDrivenbyDepartmentID");

            if (reader.IsColumnExists("TobeDrivenbyDriverID"))
                vehicleModel.TobeDrivenbyDriverID = SqlHelper.GetNullableInt32(reader, "TobeDrivenbyDriverID");

            if (reader.IsColumnExists("TripTypeID"))
                vehicleModel.TripTypeID = SqlHelper.GetNullableInt32(reader, "TripTypeID");

            if (reader.IsColumnExists("TripTypeOthers"))
                vehicleModel.TripTypeOthers = SqlHelper.GetNullableString(reader, "TripTypeOthers");

            if (reader.IsColumnExists("Emirates"))
                vehicleModel.Emirates = SqlHelper.GetNullableInt32(reader, "Emirates");

            if (reader.IsColumnExists("City"))
                vehicleModel.City = SqlHelper.GetNullableInt32(reader, "City");

            if (reader.IsColumnExists("Destination"))
                vehicleModel.Destination = SqlHelper.GetNullableInt32(reader, "Destination");

            if (reader.IsColumnExists("DestinationOthers"))
                vehicleModel.DestinationOthers = SqlHelper.GetNullableString(reader, "DestinationOthers");

            if (reader.IsColumnExists("TripPeriodFrom"))
                vehicleModel.TripPeriodFrom = SqlHelper.GetDateTime(reader, "TripPeriodFrom");

            if (reader.IsColumnExists("TripPeriodTo"))
                vehicleModel.TripPeriodTo = SqlHelper.GetDateTime(reader, "TripPeriodTo");

            if (reader.IsColumnExists("VehicleModelID"))
                vehicleModel.VehicleModelID = SqlHelper.GetNullableInt32(reader, "VehicleModelID");

            if (reader.IsColumnExists("DestinationOthers"))
                vehicleModel.DestinationOthers = SqlHelper.GetNullableString(reader, "DestinationOthers");

            if (reader.IsColumnExists("ApproverDepartment"))
                vehicleModel.ApproverDepartment = SqlHelper.GetNullableInt32(reader, "ApproverDepartment");

            if (reader.IsColumnExists("ApproverName"))
                vehicleModel.ApproverName = SqlHelper.GetNullableInt32(reader, "ApproverName");

            if (reader.IsColumnExists("ReleaseDateTime"))
                vehicleModel.ReleaseDateTime = SqlHelper.GetDateTime(reader, "ReleaseDateTime");

            if (reader.IsColumnExists("LastMileageReading"))
                vehicleModel.LastMileageReading = SqlHelper.GetNullableInt32(reader, "LastMileageReading");

            if (reader.IsColumnExists("ReleaseLocationID"))
                vehicleModel.ReleaseLocationID = SqlHelper.GetNullableInt32(reader, "ReleaseLocationID");

            if (reader.IsColumnExists("ReturnDateTime"))
                vehicleModel.ReturnDateTime = SqlHelper.GetDateTime(reader, "ReturnDateTime");

            if (reader.IsColumnExists("CurrentMileageReading"))
                vehicleModel.CurrentMileageReading = SqlHelper.GetNullableInt32(reader, "CurrentMileageReading");

            if (reader.IsColumnExists("HavePersonalBelongings"))
                vehicleModel.HavePersonalBelongings = SqlHelper.GetBoolean(reader, "HavePersonalBelongings");

            if (reader.IsColumnExists("PersonalBelongingsText"))
                vehicleModel.PersonalBelongingsText = SqlHelper.GetNullableString(reader, "PersonalBelongingsText");

            if (reader.IsColumnExists("VehicleID"))
                vehicleModel.VehicleID = SqlHelper.GetNullableInt32(reader, "VehicleID");

            if (reader.IsColumnExists("Notes"))
                vehicleModel.Notes = SqlHelper.GetNullableString(reader, "Notes");

            if (reader.IsColumnExists("UpdatedBy"))
                vehicleModel.UpdatedBy = SqlHelper.GetNullableInt32(reader, "UpdatedBy");

            if (reader.IsColumnExists("UpdatedDateTime"))
                vehicleModel.UpdatedDateTime = SqlHelper.GetDateTime(reader, "UpdatedDateTime");

            if (reader.IsColumnExists("DeleteFlag"))
                vehicleModel.DeleteFlag = SqlHelper.GetBoolean(reader, "DeleteFlag");

            if (reader.IsColumnExists("ReturnLocationID"))
                vehicleModel.ReturnLocationID = SqlHelper.GetNullableInt32(reader, "ReturnLocationID");

            if (reader.IsColumnExists("Reason"))
                vehicleModel.Reason = SqlHelper.GetNullableString(reader, "Reason");

            return vehicleModel;
        }

        public static List<VehicleRequestPutModel> TranslateAsPutVehicleList(this SqlDataReader reader)
        {
            var vehicleList = new List<VehicleRequestPutModel>();
            while (reader.Read())
            {
                vehicleList.Add(TranslateAsPutVehicle(reader, true));
            }

            return vehicleList;
        }

        public static VehicleRequestWorkflowModel TranslateAsVehicleSaveResponse(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var vehicleSave = new VehicleRequestWorkflowModel();

            if (reader.IsColumnExists("VehicleReqID"))
                vehicleSave.VehicleReqID = SqlHelper.GetNullableInt32(reader, "VehicleReqID");

            if (reader.IsColumnExists("ReferenceNumber"))
                vehicleSave.ReferenceNumber = SqlHelper.GetNullableString(reader, "ReferenceNumber");

            if (reader.IsColumnExists("CreatorID"))
                vehicleSave.CreatorID = SqlHelper.GetNullableInt32(reader, "CreatorID").GetValueOrDefault();

            if (reader.IsColumnExists("FromID"))
                vehicleSave.FromID = SqlHelper.GetNullableInt32(reader, "FromID").GetValueOrDefault();

            return vehicleSave;
        }

        public static VehicleRequestModel TranslateAsVehicleWorkflowSaveResponse(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var vehicleSave = new VehicleRequestModel();

            if (reader.IsColumnExists("VehicleReqID"))
                vehicleSave.VehicleReqID = SqlHelper.GetNullableInt32(reader, "VehicleReqID");

            if (reader.IsColumnExists("ReferenceNumber"))
                vehicleSave.ReferenceNumber = SqlHelper.GetNullableString(reader, "ReferenceNumber");

            if (reader.IsColumnExists("CreatorID"))
                vehicleSave.CreatorID = SqlHelper.GetNullableInt32(reader, "CreatorID").GetValueOrDefault();

            if (reader.IsColumnExists("FromID"))
                vehicleSave.FromID = SqlHelper.GetNullableInt32(reader, "FromID").GetValueOrDefault();

            return vehicleSave;
        }

        public static VehicleRequestWorkflowModel TranslateAsVehicleSaveResponseList(this SqlDataReader reader)
        {
            var vehicleSaveResponse = new VehicleRequestWorkflowModel();
            while (reader.Read())
            {
                vehicleSaveResponse = TranslateAsVehicleSaveResponse(reader, true);
            }

            return vehicleSaveResponse;
        }

        public static VehicleRequestModel TranslateAsVehicleWorkflowSaveResponseList(this SqlDataReader reader)
        {
            var vehicleSaveResponse = new VehicleRequestModel();
            while (reader.Read())
            {
                vehicleSaveResponse = TranslateAsVehicleWorkflowSaveResponse(reader, true);
            }

            return vehicleSaveResponse;
        }

        public static VehicleRequestDashboardListModel TranslateAsDashboardVehicleRequest(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var vehicleRequestDashboardListModel = new VehicleRequestDashboardListModel();

            if (reader.IsColumnExists("VehicleReqID"))
                vehicleRequestDashboardListModel.VehicleReqID = SqlHelper.GetNullableInt32(reader, "VehicleReqID");

            if (reader.IsColumnExists("ReferenceNumber"))
                vehicleRequestDashboardListModel.ReferenceNumber = SqlHelper.GetNullableString(reader, "ReferenceNumber");

            if (reader.IsColumnExists("RequestType"))
                vehicleRequestDashboardListModel.RequestType = SqlHelper.GetNullableString(reader, "RequestType");

            if (reader.IsColumnExists("Requestor"))
                vehicleRequestDashboardListModel.Requestor = SqlHelper.GetNullableInt32(reader, "Requestor");
            if (reader.IsColumnExists("RequestorName"))
                vehicleRequestDashboardListModel.RequestorName = SqlHelper.GetNullableString(reader, "RequestorName");

            if (reader.IsColumnExists("Status"))
                vehicleRequestDashboardListModel.Status = SqlHelper.GetNullableString(reader, "Status");

            if (reader.IsColumnExists("StatusCode"))
                vehicleRequestDashboardListModel.StatusCode = SqlHelper.GetNullableInt32(reader, "StatusCode");

            if (reader.IsColumnExists("TripDateFrom"))
                vehicleRequestDashboardListModel.TripDateFrom = SqlHelper.GetDateTime(reader, "TripDateFrom");

            if (reader.IsColumnExists("TripDateTo"))
                vehicleRequestDashboardListModel.TripDateTo = SqlHelper.GetDateTime(reader, "TripDateTo");

            if (reader.IsColumnExists("Destination"))
                vehicleRequestDashboardListModel.Destination = SqlHelper.GetNullableString(reader, "Destination");

            if (reader.IsColumnExists("City"))
                vehicleRequestDashboardListModel.City = SqlHelper.GetNullableString(reader, "City");

            if (reader.IsColumnExists("TripTimeFrom"))
                vehicleRequestDashboardListModel.TripTimeFrom = SqlHelper.GetNullableString(reader, "TripTimeFrom");

            if (reader.IsColumnExists("TripTimeTo"))
                vehicleRequestDashboardListModel.TripTimeTo = SqlHelper.GetNullableString(reader, "TripTimeTo");

            if (reader.IsColumnExists("createdby"))
                vehicleRequestDashboardListModel.Createdby = SqlHelper.GetNullableInt32(reader, "createdby");

            return vehicleRequestDashboardListModel;
        }

        public static List<VehicleRequestDashboardListModel> TranslateAsDashboardVehicleList(this SqlDataReader reader)
        {
            var vehicleDashboardList = new List<VehicleRequestDashboardListModel>();
            while (reader.Read())
            {
                vehicleDashboardList.Add(TranslateAsDashboardVehicleRequest(reader, true));
            }

            return vehicleDashboardList;
        }

        public static VehiclePreviewModel TranslateAsVehiclePreview(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var model = new VehiclePreviewModel();

            if (reader.IsColumnExists("VehicleID"))
                model.VehicleID = SqlHelper.GetNullableInt32(reader, "VehicleID");

            if (reader.IsColumnExists("VehicleMake"))
                model.VehicleMake = SqlHelper.GetNullableString(reader, "VehicleMake");

            if (reader.IsColumnExists("VehicleModel"))
                model.VehicleModel = SqlHelper.GetNullableString(reader, "VehicleModel");

            if (reader.IsColumnExists("ReturnTime"))
                model.ReturnTime = SqlHelper.GetNullableString(reader, "ReturnTime");

            if (reader.IsColumnExists("ReleaseMeridiem"))
                model.ReleaseMeridiem = SqlHelper.GetNullableString(reader, "ReleaseMeridiem");

            if (reader.IsColumnExists("ReturnLocation"))
                model.ReturnLocation = SqlHelper.GetNullableString(reader, "ReturnLocation");

            if (reader.IsColumnExists("ReturnDate"))
                model.ReturnDate = SqlHelper.GetNullableString(reader, "ReturnDate");

            if (reader.IsColumnExists("ReleaseTime"))
                model.ReleaseTime = SqlHelper.GetNullableString(reader, "ReleaseTime");

            if (reader.IsColumnExists("ReturnMeridiem"))
                model.ReturnMeridiem = SqlHelper.GetNullableString(reader, "ReturnMeridiem");

            if (reader.IsColumnExists("ReleaseLocation"))
                model.ReleaseLocation = SqlHelper.GetNullableString(reader, "ReleaseLocation");

            if (reader.IsColumnExists("ReleaseDate"))
                model.ReleaseDate = SqlHelper.GetNullableString(reader, "ReleaseDate");

            if (reader.IsColumnExists("PlateNumber"))
                model.PlateNumber = SqlHelper.GetNullableString(reader, "PlateNumber");

            if (reader.IsColumnExists("PlateCode"))
                model.PlateCode = SqlHelper.GetNullableString(reader, "PlateCode");

            if (reader.IsColumnExists("LastMileageOnReturn"))
                model.LastMileageOnReturn = SqlHelper.GetNullableString(reader, "LastMileageOnReturn");

            if (reader.IsColumnExists("LastMileageOnRelease"))
                model.LastMileageOnRelease = SqlHelper.GetNullableString(reader, "LastMileageOnRelease");

            if (reader.IsColumnExists("DriverName"))
                model.DriverName = SqlHelper.GetNullableString(reader, "DriverName");

            if (reader.IsColumnExists("YearofManufacture"))
                model.YearofManufacture = SqlHelper.GetNullableString(reader, "YearofManufacture");

            if (reader.IsColumnExists("ReleasedBy"))
                model.ReleasedBy = SqlHelper.GetNullableString(reader, "ReleasedBy");

            if (reader.IsColumnExists("ReturnedBy"))
                model.ReturnedBy = SqlHelper.GetNullableString(reader, "ReturnedBy");

            if (reader.IsColumnExists("Note"))
                model.Note = SqlHelper.GetNullableString(reader, "Note");

            if (reader.IsColumnExists("PersonalBelonging"))
                model.PersonalBelonging = SqlHelper.GetNullableString(reader, "PersonalBelonging");

            if (reader.IsColumnExists("ReferenceNumber"))
                model.ReferenceNumber = SqlHelper.GetNullableString(reader, "ReferenceNumber");

            return model;
        }

        public static VehiclePreviewModel TranslateAsVehiclePreviewModel(this SqlDataReader reader)
        {
            var model = new VehiclePreviewModel();
            while (reader.Read())
            {
                model = TranslateAsVehiclePreview(reader, true);
            }

            return model;
        }
    }
}
