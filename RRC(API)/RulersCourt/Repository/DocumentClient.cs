using RulersCourt.Models;
using RulersCourt.Translators;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace RulersCourt.Repository
{
    public class DocumentClient
    {
        public DocumentListModel GetDocument(string connString, int userID, string type, int pageNumber, int pageSize, string creator, string smartSearch, string lang)
        {
            var result = new DocumentListModel();

            SqlParameter[] parama = {
            new SqlParameter("@P_PageNumber", pageNumber),
            new SqlParameter("@P_PageSize", pageSize),
            new SqlParameter("@P_Type", type),
            new SqlParameter("@P_Creator", creator),
            new SqlParameter("@P_SmartSearch", smartSearch),
            new SqlParameter("@P_Method", 0),
            new SqlParameter("@P_Language", lang)
            };

            result.Collection = SqlHelper.ExecuteProcedureReturnData<List<DocumentGetModel>>(connString, "Get_DocumentByID", r => r.TranslateAsCocumentList(), parama);

            SqlParameter[] param = {
            new SqlParameter("@P_PageNumber", pageNumber),
            new SqlParameter("@P_PageSize", pageSize),
            new SqlParameter("@P_Type", type),
            new SqlParameter("@P_Creator", creator),
            new SqlParameter("@P_SmartSearch", smartSearch),
            new SqlParameter("@P_Method", 1),
            new SqlParameter("@P_Language", lang)
            };

            result.Count = SqlHelper.ExecuteProcedureReturnString(connString, "Get_DocumentByID", param);

            return result;
        }

        public string PostDocument(string connString, DocumentPostModel value)
        {
            var result = new DocumentListModel();

            SqlParameter[] parama = {
            new SqlParameter("@P_Type", value.Type),
            new SqlParameter("@P_AttachmentGuid", value.AttachmentGuid),
            new SqlParameter("@P_AttachmentsName", value.AttachmentsName),
            new SqlParameter("@P_CreatedBy", value.CreatedBy),
            new SqlParameter("@P_CreatedDateTime", value.CreatedDateTime)
            };

            return SqlHelper.ExecuteProcedureReturnString(connString, "Save_Documents", parama);
        }

        public string PutDocument(string connString, DocumentPutModel value)
        {
            var result = new DocumentListModel();

            SqlParameter[] parama = {
            new SqlParameter("@P_Type", value.Type),
            new SqlParameter("@P_AttachmentID", value.AttachmentID),
            new SqlParameter("@P_AttachmentGuid", value.AttachmentGuid),
            new SqlParameter("@P_AttachmentsName", value.AttachmentsName),
            new SqlParameter("@P_UpdatedBy", value.UpdatedBy),
            new SqlParameter("@P_UpdatedDateTime", value.UpdatedDateTime)
            };

            return SqlHelper.ExecuteProcedureReturnString(connString, "Save_Documents", parama);
        }

        public string DeleteDocument(string connString, int attachmentId, string userID)
        {
            var result = new DocumentListModel();

            SqlParameter[] parama = {
            new SqlParameter("@P_Type", "Delete"),
            new SqlParameter("@P_AttachmentID", attachmentId),
            new SqlParameter("@P_UpdatedBy", userID)
            };
            return SqlHelper.ExecuteProcedureReturnString(connString, "Save_Documents", parama);
        }
    }
}