using RulersCourt.Models.Master.M_News;
using RulersCourt.Translators.Master;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace RulersCourt.Repository.Master
{
    public class NewsClient
    {
        public NewsResponseModel PostNews(string connString, M_NewsPostModel news)
        {
            SqlParameter[] parama = { new SqlParameter("@P_News", news.News),
                                    new SqlParameter("@P_ExpiryDate", news.ExpiryDate),
                                    new SqlParameter("@P_CreatedBy", news.CreatedBy),
                                    new SqlParameter("@P_CreatedDateTime", news.CreatedDateTime) };
            var result = SqlHelper.ExecuteProcedureReturnData<NewsResponseModel>(connString, "Save_News", r => r.TranslateAsNewsSaveResponseList(), parama);
            return result;
        }

        public NewsResponseModel PutNews(string connString, M_NewsPutModel news)
        {
            SqlParameter[] parama = { new SqlParameter("@P_NewsID", news.NewsID),
                                    new SqlParameter("@P_News", news.News),
                                    new SqlParameter("@P_ExpiryDate", news.ExpiryDate),
                                    new SqlParameter("@P_UpdatedBy", news.UpdatedBy),
                                    new SqlParameter("@P_UpdatedDateTime", news.UpdatedDateTime) };
            var result = SqlHelper.ExecuteProcedureReturnData<NewsResponseModel>(connString, "Save_News", r => r.TranslateAsNewsSaveResponseList(), parama);
            return result;
        }

        public M_NewsGetModel GetNewsByID(string connString, int id, int userID)
        {
            M_NewsGetModel news = new M_NewsGetModel();

            SqlParameter[] param = {
                new SqlParameter("@P_NewsID", id) };
            if (id != 0)
            {
                news = SqlHelper.ExecuteProcedureReturnData<List<M_NewsGetModel>>(connString, "Get_NewsByID", r => r.TranslateAsNewsList(), param).FirstOrDefault();
            }

            userID = news.CreatedBy.GetValueOrDefault();

            return news;
        }

        public string DeleteNews(string connString, int id)
        {
            SqlParameter[] param = {
                new SqlParameter("@P_NewsID", id)
             };
            return SqlHelper.ExecuteProcedureReturnString(connString, "Delete_NewsByID", param);
        }

        public M_NewslistModel GetNewsList(string connString, int pageNumber, int pageSize, string userID, string description)
        {
            M_NewslistModel list = new M_NewslistModel();

            SqlParameter[] param = {
                   new SqlParameter("@P_PageNumber", pageNumber),
                   new SqlParameter("@P_PageSize", pageSize),
                   new SqlParameter("@P_UserID", userID),
                   new SqlParameter("@P_Method", 0),
                   new SqlParameter("@P_Description", description) };
            list.Collection = SqlHelper.ExecuteProcedureReturnData<List<M_NewsModel>>(connString, "Get_NewsList", r => r.TranslateAsNewsAllList(), param);

            SqlParameter[] countparam = {
                   new SqlParameter("@P_PageNumber", pageNumber),
                   new SqlParameter("@P_PageSize", pageSize),
                   new SqlParameter("@P_UserID", userID),
                   new SqlParameter("@P_Method", 1),
                   new SqlParameter("@P_Description", description) };
            list.Count = SqlHelper.ExecuteProcedureReturnString(connString, "Get_NewsList", countparam);

            return list;
        }

        public M_NewslistModel GetNewsAllList(string connString, string userID)
        {
            M_NewslistModel list = new M_NewslistModel();

            SqlParameter[] param = {
                   new SqlParameter("@P_UserID", userID),
                   new SqlParameter("@P_Method", 0) };
            list.Collection = SqlHelper.ExecuteProcedureReturnData<List<M_NewsModel>>(connString, "Get_NewsAllList", r => r.TranslateAsNewsAllList(), param);

            SqlParameter[] countparam = {
                   new SqlParameter("@P_UserID", userID),
                   new SqlParameter("@P_Method", 1) };
            list.Count = SqlHelper.ExecuteProcedureReturnString(connString, "Get_NewsAllList", countparam);

            return list;
        }
    }
}
