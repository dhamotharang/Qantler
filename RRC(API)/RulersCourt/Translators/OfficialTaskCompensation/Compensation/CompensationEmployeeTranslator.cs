using RulersCourt.Models.OfficialTaskCompensation.Compensation;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Translators.OfficialTaskCompensation.Compensation
{
    public static class CompensationEmployeeTranslator
    {
        public static CompensationEmployeeGetModel TranslateAsGetEmployeeNameList(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var employeeNameID = new CompensationEmployeeGetModel();

            if (reader.IsColumnExists("EmployeeID"))
                employeeNameID.EmployeeID = SqlHelper.GetNullableInt32(reader, "EmployeeID");

            if (reader.IsColumnExists("EmployeeName"))
                employeeNameID.EmployeeName = SqlHelper.GetNullableString(reader, "EmployeeName");

            if (reader.IsColumnExists("EmployeeCode"))
                employeeNameID.EmployeeCode = SqlHelper.GetNullableString(reader, "EmployeeCode");

            if (reader.IsColumnExists("Grade"))
                employeeNameID.Grade = SqlHelper.GetNullableString(reader, "Grade");

            if (reader.IsColumnExists("EmployeePosition"))
                employeeNameID.EmployeePosition = SqlHelper.GetNullableString(reader, "EmployeePosition");

            if (reader.IsColumnExists("EmployeeDepartment"))
                employeeNameID.EmployeeDepartment = SqlHelper.GetNullableString(reader, "EmployeeDepartment");

            return employeeNameID;
        }

        public static List<CompensationEmployeeGetModel> TranslateAsEmployeeNameList(this SqlDataReader reader)
        {
            var employeeNameList = new List<CompensationEmployeeGetModel>();
            while (reader.Read())
            {
                employeeNameList.Add(TranslateAsGetEmployeeNameList(reader, true));
            }

            return employeeNameList;
        }
    }
}
