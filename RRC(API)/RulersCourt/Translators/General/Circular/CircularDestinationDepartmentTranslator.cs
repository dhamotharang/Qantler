using RulersCourt.Models.Circular;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Translators.Circular
{
    public static class CircularDestinationDepartmentTranslator
    {
        public static CircularDestinationDepartmentGetModel TranslateAsGetDestinationDepartment(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var destinationDepartment = new CircularDestinationDepartmentGetModel();

            if (reader.IsColumnExists("DepartmentID"))
                destinationDepartment.CircularDestinationDepartmentID = SqlHelper.GetNullableInt32(reader, "DepartmentID");

            if (reader.IsColumnExists("CircularDestinationDepartmentName"))
                destinationDepartment.CircularDestinationDepartmentName = SqlHelper.GetNullableString(reader, "CircularDestinationDepartmentName");

            return destinationDepartment;
        }

        public static List<CircularDestinationDepartmentGetModel> CircularTranslateAsDestinationDepartmentList(this SqlDataReader reader)
        {
            var destinationDepartmentList = new List<CircularDestinationDepartmentGetModel>();
            while (reader.Read())
            {
                destinationDepartmentList.Add(TranslateAsGetDestinationDepartment(reader, true));
            }

            return destinationDepartmentList;
        }
    }
}
