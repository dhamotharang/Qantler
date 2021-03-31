using RulersCourt.Models.Master;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Translators.Master
{
    public static class M_LeaveTypeTranslator
    {
        public static M_LeaveTypeModel TranslateAsGetLeave(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var leave = new M_LeaveTypeModel();

            if (reader.IsColumnExists("LeaveID"))
                leave.LeaveID = SqlHelper.GetNullableInt32(reader, "LeaveID");

            if (reader.IsColumnExists("LeaveType"))
                leave.LeaveType = SqlHelper.GetNullableString(reader, "LeaveType");

            if (reader.IsColumnExists("DisplayOrder"))
                leave.DisplayOrder = SqlHelper.GetNullableString(reader, "DisplayOrder");

            return leave;
        }

        public static List<M_LeaveTypeModel> TranslateAsLeaveType(this SqlDataReader reader)
        {
            var leave = new List<M_LeaveTypeModel>();
            while (reader.Read())
            {
                leave.Add(TranslateAsGetLeave(reader, true));
            }

            return leave;
        }
    }
}
