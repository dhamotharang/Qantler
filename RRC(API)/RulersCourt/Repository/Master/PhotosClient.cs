using RulersCourt.Models.Master.M_Photos;
using RulersCourt.Translators.Master;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace RulersCourt.Repository.Master
{
    public class PhotosClient
    {
        public PhotoResponseModel PostPhoto(string connString, M_PhotoPostModel photo)
        {
            SqlParameter[] parama = { new SqlParameter("@P_AttachmentGuid", photo.AttachmentGuid),
                                    new SqlParameter("@P_AttachmentName", photo.AttachmentName),
                                    new SqlParameter("@P_ExpiryDate", photo.ExpiryDate),
                                    new SqlParameter("@P_CreatedBy", photo.CreatedBy),
                                    new SqlParameter("@P_CreatedDateTime", photo.CreatedDateTime) };
            var result = SqlHelper.ExecuteProcedureReturnData<PhotoResponseModel>(connString, "Save_Photos", r => r.TranslateAsPhotoSaveResponseList(), parama);
            return result;
        }

        public PhotoResponseModel PutPhoto(string connString, M_PhotoPutModel photo)
        {
            SqlParameter[] parama = { new SqlParameter("@P_PhotoID", photo.PhotoID),
                                    new SqlParameter("@P_AttachmentGuid", photo.AttachmentGuid),
                                    new SqlParameter("@P_AttachmentName", photo.AttachmentName),
                                    new SqlParameter("@P_ExpiryDate", photo.ExpiryDate),
                                    new SqlParameter("@P_UpdatedBy", photo.UpdatedBy),
                                    new SqlParameter("@P_UpdatedDateTime", photo.UpdatedDateTime) };
            var result = SqlHelper.ExecuteProcedureReturnData<PhotoResponseModel>(connString, "Save_Photos", r => r.TranslateAsPhotoSaveResponseList(), parama);
            return result;
        }

        public M_PhotoGetModel GetPhotosByID(string connString, int id, int userID)
        {
            M_PhotoGetModel photo = new M_PhotoGetModel();

            SqlParameter[] param = {
                new SqlParameter("@P_PhotoID", id) };
            if (id != 0)
            {
                photo = SqlHelper.ExecuteProcedureReturnData<List<M_PhotoGetModel>>(connString, "Get_PhotosByID", r => r.TranslateAsDesignList(), param).FirstOrDefault();
            }

            userID = photo.CreatedBy.GetValueOrDefault();

            return photo;
        }

        public string DeletePhoto(string connString, int id)
        {
            SqlParameter[] param = {
                new SqlParameter("@P_PhotoID", id)
             };
            return SqlHelper.ExecuteProcedureReturnString(connString, "Delete_PhotosByID", param);
        }

        public M_PhotoListModel GetPhotoList(string connString, int pageNumber, int pageSize, string userID, string description)
        {
            M_PhotoListModel list = new M_PhotoListModel();

            SqlParameter[] param = {
                   new SqlParameter("@P_PageNumber", pageNumber),
                   new SqlParameter("@P_PageSize", pageSize),
                   new SqlParameter("@P_UserID", userID),
                   new SqlParameter("@P_Method", 0),
                   new SqlParameter("@P_Description", description) };
            list.Collection = SqlHelper.ExecuteProcedureReturnData<List<M_PhotoModel>>(connString, "Get_PhotosList", r => r.TranslateAsPhotoAllList(), param);

            SqlParameter[] countparam = {
                   new SqlParameter("@P_PageNumber", pageNumber),
                   new SqlParameter("@P_PageSize", pageSize),
                   new SqlParameter("@P_UserID", userID),
                   new SqlParameter("@P_Method", 1),
                   new SqlParameter("@P_Description", description) };
            list.Count = SqlHelper.ExecuteProcedureReturnString(connString, "Get_PhotosList", countparam);

            return list;
        }

        public M_PhotoListModel GetPhotoAllList(string connString, string userID)
        {
            M_PhotoListModel list = new M_PhotoListModel();

            SqlParameter[] param = {
                   new SqlParameter("@P_UserID", userID),
                   new SqlParameter("@P_Method", 0) };
            list.Collection = SqlHelper.ExecuteProcedureReturnData<List<M_PhotoModel>>(connString, "Get_PhotosAllList", r => r.TranslateAsPhotoAllList(), param);

            SqlParameter[] countparam = {
                   new SqlParameter("@P_UserID", userID),
                   new SqlParameter("@P_Method", 1) };
            list.Count = SqlHelper.ExecuteProcedureReturnString(connString, "Get_PhotosAllList", countparam);

            return list;
        }
    }
}
