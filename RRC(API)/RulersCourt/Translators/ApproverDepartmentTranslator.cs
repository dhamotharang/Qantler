using RulersCourt.Models;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Translators
{
    public static class ApproverDepartmentTranslator
    {
        public static ApproverDeparmentModel TranslateAsApproverDeparment(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var approverDeparmentModel = new ApproverDeparmentModel();

            if (reader.IsColumnExists("DepartmentID"))
                approverDeparmentModel.OrganizationID = SqlHelper.GetNullableInt32(reader, "DepartmentID");

            if (reader.IsColumnExists("DepartmentName"))
                approverDeparmentModel.OrganizationUnits = SqlHelper.GetNullableString(reader, "DepartmentName");

            return approverDeparmentModel;
        }

        public static List<ApproverDeparmentModel> TranslateAsApproverDepartmentList(this SqlDataReader reader)
        {
            var approverDepartmentList = new List<ApproverDeparmentModel>();
            while (reader.Read())
            {
                approverDepartmentList.Add(TranslateAsApproverDeparment(reader, true));
            }

            return approverDepartmentList;
        }
    }
}
