using RulersCourt.Models.Master.M_Vehicle;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Translators.Master.Vehicle
{
    public static class M_VehicleModelTranslator
    {
        public static M_VehicleModel TranslateAsGetVehicleModel(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var vehicleModel = new M_VehicleModel();

            if (reader.IsColumnExists("VehicleModelID"))
                vehicleModel.VehicleModelID = SqlHelper.GetNullableInt32(reader, "VehicleModelID");

            if (reader.IsColumnExists("VehicleModelName"))
                vehicleModel.VehicleModelName = SqlHelper.GetNullableString(reader, "VehicleModelName");

            if (reader.IsColumnExists("DisplayOrder"))
                vehicleModel.DisplayOrder = SqlHelper.GetNullableString(reader, "DisplayOrder");

            return vehicleModel;
        }

        public static List<M_VehicleModel> TranslateAsVehicleModel(this SqlDataReader reader)
        {
            var vehicleModel = new List<M_VehicleModel>();
            while (reader.Read())
            {
                vehicleModel.Add(TranslateAsGetVehicleModel(reader, true));
            }

            return vehicleModel;
        }
    }
}
