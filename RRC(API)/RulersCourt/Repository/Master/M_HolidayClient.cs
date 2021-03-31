using RulersCourt.Models;
using RulersCourt.Models.Master;
using RulersCourt.Translators;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Repository
{
    public class M_HolidayClient
    {
        public List<M_HolidayModel> GetHoliday(string connString, int userID, int month, int year)
        {
            List<M_HolidayModel> list = new List<M_HolidayModel>();
            SqlParameter[] parama = { new SqlParameter("@P_Month", month),
                                    new SqlParameter("@P_Year", year) };
            list = SqlHelper.ExecuteProcedureReturnData<List<M_HolidayModel>>(connString, "Get_M_Holidays", r => r.TranslateAsHolidays(), parama);

            return list;
        }

        public List<M_HolidayAttachmentGetModel> GetAttachment(string connString)
        {
            string type = "Holiday";
            SqlParameter param = new SqlParameter("@Type", type);

            return SqlHelper.ExecuteProcedureReturnData<List<M_HolidayAttachmentGetModel>>(connString, "Get_AttachmentByID", r => r.TranslateAsHolidayAttachmentList(), param);
        }

        public string SaveAttachment(string connString, string fileName, string guid)
        {
            SqlParameter[] parama = {
                                        new SqlParameter("@P_Type", "Holiday"),
                                        new SqlParameter("@P_ServiceType", "Holiday"),
                                        new SqlParameter("@P_AttachmentGuid", guid),
                                        new SqlParameter("@P_AttachmentsName", fileName) };

            return SqlHelper.ExecuteProcedureReturnString(connString, "Save_Attachments", parama);
        }

        public string SaveHoliday(string connString, int userID, M_HolidayModel holiday)
        {
            SqlParameter[] parama = { new SqlParameter("@P_Holiday", holiday.Holiday),
                                    new SqlParameter("@P_Message", holiday.Message),
                                    new SqlParameter("@P_HolidayID", holiday.HolidayID),
                                    new SqlParameter("@P_User", userID) };
            return SqlHelper.ExecuteProcedureReturnString(connString, "Save_M_Holidays", parama);
        }

        public string PutHoliday(string connString, int userID, M_HolidayModel holiday, int countryID)
        {
            SqlParameter[] parama = { new SqlParameter("@P_Holiday", holiday.Holiday),
                                    new SqlParameter("@P_Message", holiday.Message),
                                    new SqlParameter("@P_HolidayID", holiday.HolidayID),
                                    new SqlParameter("@P_User", userID) };
            return SqlHelper.ExecuteProcedureReturnString(connString, "Save_M_Holidays", parama);
        }

        public string DeleteHoliday(string connString, int userID, int lookupID)
        {
            SqlParameter[] parama = { new SqlParameter("@P_HolidayID", lookupID),
                                    new SqlParameter("@P_DeleteFlag", true),
                                    new SqlParameter("@P_User", userID) };
            return SqlHelper.ExecuteProcedureReturnString(connString, "Save_M_Holidays", parama);
        }
    }
}