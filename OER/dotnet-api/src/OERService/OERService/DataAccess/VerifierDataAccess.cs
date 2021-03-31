using Core.Enums;
using Core.Helpers;
using Core.Models;
using Microsoft.Extensions.Configuration;
using OERService.Models;
using Serilog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace OERService.DataAccess
{
    public class VerifierDataAccess
    {
        internal DataAccessHelper _DataHelper = null;
		private readonly IConfiguration _configuration;

        public VerifierDataAccess(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<DatabaseResponse> GetContentApproval(int pageNumber, int pageSize)
        {
            try
            {
                SqlParameter[] parameters =
            {
                    new SqlParameter( "@PageNo",  SqlDbType.Int),
                    new SqlParameter( "@PageSize",  SqlDbType.Int)


                };
                parameters[0].Value = pageNumber;
                parameters[1].Value = pageSize;

                _DataHelper = new DataAccessHelper("sps_ContentApprovalForVerifiers", parameters, _configuration);

                DataTable dt = new DataTable();

                int result = await _DataHelper.RunAsync(dt);


                List<Content> content = new List<Content>();

                if (dt != null && dt.Rows.Count > 0)
                {

                    content = (from model in dt.AsEnumerable()
                                     select new Content()
                                     {
                                         ContentId = model.Field<int>("ContentId"),
                                         ContentType = model.Field<int>("ContentType"),
                                         Title = model.Field<string>("Title"),
                                         CreatedOn = model.Field<DateTime>("CreatedOn"),
                                         Totalrows = model.Field<Int64>("Totalrows"),
                                         Rownumber = model.Field<Int64>("Rownumber")
                                     }).ToList();
                }

                return new DatabaseResponse { ResponseCode = result, Results = content };
            }

            catch (Exception ex)
            {
                Log.Error(new ExceptionHelper().GetLogString(ex, ErrorLevel.Critical));
                throw;
            }
            finally
            {
                _DataHelper.Dispose();
            }

        }
        public async Task<DatabaseResponse> GetVerifiersReport(int pageNumber, int pageSize, string paramIsSearch, string sortType, int sortField)
        {
            try
            {
                SqlParameter[] parameters =
            {
                    new SqlParameter( "@PageNo",  SqlDbType.Int),
                    new SqlParameter( "@PageSize",  SqlDbType.Int),
                    new SqlParameter( "@Keyword",  SqlDbType.NVarChar ),
                    new SqlParameter( "@SortType",  SqlDbType.NVarChar ),
                    new SqlParameter( "@SortField",  SqlDbType.Int )

                };
                parameters[0].Value = pageNumber;
                parameters[1].Value = pageSize;
                parameters[2].Value = paramIsSearch;
                parameters[3].Value = sortType;
                parameters[4].Value = sortField;

                _DataHelper = new DataAccessHelper("sps_GetVerifierReport", parameters, _configuration);

                DataTable dt = new DataTable();

                int result = await _DataHelper.RunAsync(dt);


                List<VerifiersReport> verifiersReport = new List<VerifiersReport>();

                if (dt != null && dt.Rows.Count > 0)
                {

                    verifiersReport = (from model in dt.AsEnumerable()
                               select new VerifiersReport()
                               {
                                   ApprovedCount = model.Field<int>("ApprovedCount"),
                                   VerifierName = model.Field<string>("Verifier"),
                                   RejectedCount = model.Field<int>("RejectedCount"),
                                   Totalrows = model.Field<int>("Totalrows"),
                                   Rownumber = model.Field<int>("Rownumber")
                               }).ToList();
                }

                return new DatabaseResponse { ResponseCode = result, Results = verifiersReport };
            }

            catch (Exception ex)
            {
                Log.Error(new ExceptionHelper().GetLogString(ex, ErrorLevel.Critical));
                throw;
            }
            finally
            {
                _DataHelper.Dispose();
            }

        }
        public async Task<DatabaseResponse> UpdateContentStatus(ContentUpdate contentUpdate)
        {
            try
            {
                SqlParameter[] parameters =
            {
                    new SqlParameter( "@UserId",  SqlDbType.Int),
                    new SqlParameter( "@ContentId",  SqlDbType.Int),
                    new SqlParameter( "@ContentType",  SqlDbType.Int),
                    new SqlParameter( "@Status",  SqlDbType.Int)


                };
                parameters[0].Value = contentUpdate.UserId;
                parameters[1].Value = contentUpdate.ContentId;
                parameters[2].Value = contentUpdate.ContentType;
                parameters[3].Value = contentUpdate.Status;

                _DataHelper = new DataAccessHelper("spu_ContentStatus", parameters, _configuration);

                DataTable dt = new DataTable();

                int result = await _DataHelper.RunAsync(dt);


                return new DatabaseResponse { ResponseCode = result, Results = null };
            }

            catch (Exception ex)
            {
                Log.Error(new ExceptionHelper().GetLogString(ex, ErrorLevel.Critical));
                throw;
            }
            finally
            {
                _DataHelper.Dispose();
            }

        }
    }
}
