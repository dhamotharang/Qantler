using RulersCourt.Models.OfficialTaskCompensation.OfficialTask;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Translators.OfficialTaskCompensation.OfficialTask
{
    public static class OfficialTaskEmployeeTranslator
    {
        public static OfficialTaskEmployeeGetModel TranslateAsGetEmployeeNameList(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var employeeNameID = new OfficialTaskEmployeeGetModel();

            if (reader.IsColumnExists("EmployeeID"))
                employeeNameID.OfficialTaskEmployeeID = SqlHelper.GetNullableInt32(reader, "EmployeeID");

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

        public static List<OfficialTaskEmployeeGetModel> TranslateAsEmployeeNameList(this SqlDataReader reader)
        {
            var employeeNameList = new List<OfficialTaskEmployeeGetModel>();
            while (reader.Read())
            {
                employeeNameList.Add(TranslateAsGetEmployeeNameList(reader, true));
            }

            return employeeNameList;
        }

        public static OfficialTaskEmployeeNameModel TranslateAsEmployeeName(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var employeeName = new OfficialTaskEmployeeNameModel();

            if (reader.IsColumnExists("OfficialTaskEmployeeID"))
                employeeName.OfficialTaskEmployeeID = SqlHelper.GetNullableInt32(reader, "OfficialTaskEmployeeID");

            if (reader.IsColumnExists("OfficialTaskEmployeeName"))
                employeeName.OfficialTaskEmployeeName = SqlHelper.GetNullableString(reader, "OfficialTaskEmployeeName");

            return employeeName;
        }

        public static List<OfficialTaskEmployeeNameModel> TranslateAsEmployeeList(this SqlDataReader reader)
        {
            var employeeList = new List<OfficialTaskEmployeeNameModel>();
            while (reader.Read())
            {
                employeeList.Add(TranslateAsEmployeeName(reader, true));
            }

            return employeeList;
        }
    }
}