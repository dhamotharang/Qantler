using RulersCourt.Models;
using RulersCourt.Models.Master;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Translators
{
    public static class M_HolidayTranslator
    {
        public static M_HolidayModel TranslateAsGetHoliday(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var holiday = new M_HolidayModel();

            if (reader.IsColumnExists("HolidayID"))
                holiday.HolidayID = SqlHelper.GetNullableInt32(reader, "HolidayID");

            if (reader.IsColumnExists("Holiday"))
                holiday.Holiday = SqlHelper.GetDateTime(reader, "Holiday");

            if (reader.IsColumnExists("Message"))
                holiday.Message = SqlHelper.GetNullableString(reader, "Message");

            return holiday;
        }

        public static List<M_HolidayModel> TranslateAsHolidays(this SqlDataReader reader)
        {
            var holidays = new List<M_HolidayModel>();
            while (reader.Read())
            {
                holidays.Add(TranslateAsGetHoliday(reader, true));
            }

            return holidays;
        }

        public static M_HolidayAttachmentGetModel TranslateAsGetHolidayAttachment(this SqlDataReader reader, bool isList = false)
        {
            if (!isList)
            {
                if (!reader.HasRows)
                    return null;
                reader.Read();
            }

            var attachment = new M_HolidayAttachmentGetModel();

            if (reader.IsColumnExists("ServiceID"))
                attachment.HolidayID = SqlHelper.GetNullableInt32(reader, "ServiceID");

            if (reader.IsColumnExists("AttachmentGuid"))
                attachment.AttachmentGuid = SqlHelper.GetNullableString(reader, "AttachmentGuid");

            if (reader.IsColumnExists("AttachmentsName"))
                attachment.AttachmentsName = SqlHelper.GetNullableString(reader, "AttachmentsName");

            return attachment;
        }

        public static List<M_HolidayAttachmentGetModel> TranslateAsHolidayAttachmentList(this SqlDataReader reader)
        {
            var attachmentList = new List<M_HolidayAttachmentGetModel>();
            while (reader.Read())
            {
                attachmentList.Add(TranslateAsGetHolidayAttachment(reader, true));
            }

            return attachmentList;
        }
    }
}
