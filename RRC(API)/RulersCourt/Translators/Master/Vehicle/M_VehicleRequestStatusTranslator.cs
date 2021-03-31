using RulersCourt.Models.Master.M_Vehicle;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Translators.Master.Vehicle
{
    public static class M_VehicleRequestStatusTranslator
    {
        public static M_VehicleRequestStatusModel TranslateAsGetVehicleRequestStatus(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var vehicleRequestStatusModel = new M_VehicleRequestStatusModel();

            if (reader.IsColumnExists("StatusID"))
                vehicleRequestStatusModel.StatusID = SqlHelper.GetNullableInt32(reader, "StatusID");

            if (reader.IsColumnExists("StatusName"))
                vehicleRequestStatusModel.StatusName = SqlHelper.GetNullableString(reader, "StatusName");

            if (reader.IsColumnExists("DisplayOrder"))
                vehicleRequestStatusModel.DisplayOrder = SqlHelper.GetNullableString(reader, "DisplayOrder");

            return vehicleRequestStatusModel;
        }

        public static List<M_VehicleRequestStatusModel> TranslateAsVehicleRequestStatusModel(this SqlDataReader reader)
        {
            var vehicleRequestStatusModel = new List<M_VehicleRequestStatusModel>();
            while (reader.Read())
            {
                vehicleRequestStatusModel.Add(TranslateAsGetVehicleRequestStatus(reader, true));
            }

            return vehicleRequestStatusModel;
        }
    }
}
