using RulersCourt.Models.Contact;
using RulersCourt.Translators.Contact;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Repository.Contact
{
    public class ContactHistroyLogClient
    {
        public List<ContactHistoryLogModel> ContactHistoryLogByContactID(string connString, int contactID)
        {
            SqlParameter contactIDParam = new SqlParameter("@P_ContactID", contactID);

            List<ContactHistoryLogModel> contactDetails = new List<ContactHistoryLogModel>();

            contactDetails = SqlHelper.ExecuteProcedureReturnData<List<ContactHistoryLogModel>>(connString, "Get_ContactHistoryByID", r => r.TranslateAsContactHistoryLogList(), contactIDParam);

            return contactDetails;
        }
    }
}
