using RulersCourt.Models;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Translators
{
    public static class M_EmployeeStatusTranslator
    {
        public static M_EmployeeStatusModel TranslateAsGetStatus(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var status = new M_EmployeeStatusModel();

            if (reader.IsColumnExists("EmployeeStatusID"))
                status.EmployeeStatusID = SqlHelper.GetNullableInt32(reader, "EmployeeStatusID");

            if (reader.IsColumnExists("EmployeeStatusName"))
                status.EmployeeStatusName = SqlHelper.GetNullableString(reader, "EmployeeStatusName");

            if (reader.IsColumnExists("DisplayOrder"))
                status.DisplayOrder = SqlHelper.GetNullableString(reader, "DisplayOrder");

            return status;
        }

        public static List<M_EmployeeStatusModel> TranslateAsEmployeeStatus(this SqlDataReader reader)
        {
            var citys = new List<M_EmployeeStatusModel>();
            while (reader.Read())
            {
                citys.Add(TranslateAsGetStatus(reader, true));
            }

            return citys;
        }
    }
}
