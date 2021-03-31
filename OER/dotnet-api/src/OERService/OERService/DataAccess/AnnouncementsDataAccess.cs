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
    public class AnnouncementsDataAccess
    {
        internal DataAccessHelper _DataHelper = null;

		private readonly IConfiguration _configuration;

        /// <summary>
        /// Constructor setting configuration
        /// </summary>
        /// <param name="configuration"></param>
        public AnnouncementsDataAccess(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<DatabaseResponse> GetUserAnnouncements(int userId, int pageNumber, int pageSize)
        {
            try
            {
                SqlParameter[] parameters =
            {
                    new SqlParameter( "@UserId",  SqlDbType.Int),
                    new SqlParameter( "@PageNo",  SqlDbType.Int),
                    new SqlParameter( "@PageSize",  SqlDbType.Int)


                };

                parameters[0].Value = userId;
                parameters[1].Value = pageNumber;
                parameters[2].Value = pageSize;

                _DataHelper = new DataAccessHelper("sps_AnnouncementsToUser", parameters, _configuration);

                DataTable dt = new DataTable();

                int result = await _DataHelper.RunAsync(dt);


                List<Announcements> announcements = new List<Announcements>();

                if (dt != null && dt.Rows.Count > 0)
                {

                    announcements = (from model in dt.AsEnumerable()
                                     select new Announcements()
                                     {
                                         Id = model.Field<int>("Id"),
                                         CreatedBy = model.Field<string>("FirstName") + " " + model.Field<string>("LastName"),

                                         Text = model.Field<string>("Text"),
                                         Text_Ar = model.Field<string>("Text_Ar"),
                                         Active = model.Field<bool>("Active"),
                                         LastLogin = model.Field<DateTime>("LastLogin"),
                                         CreatedOn = model.Field<DateTime?>("CreatedOn"),
                                         UpdatedOn = model.Field<DateTime?>("UpdatedOn"),
                                     TotalRows = model.Field<Int64>("Totalrows")
                                 }).ToList();
                }

                return new DatabaseResponse { ResponseCode = result, Results = announcements };
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
        public async Task<DatabaseResponse> GetByIdAsync(int Id)
        {
            try
            {

                SqlParameter[] parameters =
               {
                    new SqlParameter( "@Id",  SqlDbType.NVarChar )

                };

                parameters[0].Value = Id;

                _DataHelper = new DataAccessHelper("sps_AnnouncementsById", parameters, _configuration);

                DataTable dt = new DataTable();

                int result = await _DataHelper.RunAsync(dt);
                List<Announcements> announcements = new List<Announcements>();

                if (dt != null && dt.Rows.Count > 0)
                {

                    announcements = (from model in dt.AsEnumerable()
                                     select new Announcements()
                                     {
                                         Id = model.Field<int>("Id"),
                                         Text = model.Field<string>("Text"),
                                         Text_Ar = model.Field<string>("Text_Ar"),
                                         CreatedBy = model.Field<string>("CreatedBy"),
                                         CreatedOn = model.Field<DateTime>("CreatedOn"),
                                         UpdatedOn = model.Field<DateTime>("UpdatedOn"),
                                         UpdatedBy = model.Field<string>("UpdatedBy"),
                                         Active = model.Field<bool?>("Active")

                                     }).ToList();
                }
                return new DatabaseResponse { ResponseCode = result, Results = announcements };
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
        public async Task<DatabaseResponse> DeleteAsync(int Id)
        {
            try
            {

                SqlParameter[] parameters =
               {
                    new SqlParameter( "@Id",  SqlDbType.Int )

                };

                parameters[0].Value = Id;

                _DataHelper = new DataAccessHelper("spd_Announcements", parameters, _configuration);

                DataTable dt = new DataTable();

                int result = await _DataHelper.RunAsync(dt);

                return new DatabaseResponse { ResponseCode = result };
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
        public async Task<DatabaseResponse> PutAsync(AnnouncementsUpdate announcementsUpdate)
        {
            try
            {

                SqlParameter[] parameters =
               {
                    new SqlParameter( "@Id",  SqlDbType.Int ),
                    new SqlParameter( "@Text",  SqlDbType.NVarChar ),
                    new SqlParameter( "@Updatedby",  SqlDbType.Int ),
                    new SqlParameter( "@Active",  SqlDbType.Bit ),
                    new SqlParameter( "@Text_Ar",  SqlDbType.NVarChar)

                };

                parameters[0].Value = announcementsUpdate.Id;
                parameters[1].Value = announcementsUpdate.Text;
                parameters[2].Value = announcementsUpdate.UpdatedBy;
                parameters[3].Value = announcementsUpdate.Active;
                parameters[4].Value = announcementsUpdate.Text_Ar;
                _DataHelper = new DataAccessHelper("spu_Announcements", parameters, _configuration);

                DataTable dt = new DataTable();

                int result = await _DataHelper.RunAsync(dt);


                List<Announcements> announcements = new List<Announcements>();

                if (dt != null && dt.Rows.Count > 0)
                {

                    announcements = (from model in dt.AsEnumerable()
                                     select new Announcements()
                                     {
                                         Id = model.Field<int>("Id"),
                                         Text = model.Field<string>("Text"),
                                         Text_Ar = model.Field<string>("Text_Ar"),
                                         CreatedBy = model.Field<string>("CreatedBy"),
                                         CreatedOn = model.Field<DateTime>("CreatedOn"),
                                         UpdatedOn = model.Field<DateTime>("UpdatedOn"),
                                         UpdatedBy = model.Field<string>("UpdatedBy"),
                                         Active = model.Field<bool?>("Active")

                                     }).ToList();
                }

                return new DatabaseResponse { ResponseCode = result, Results = announcements };
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
        public async Task<DatabaseResponse> GetAsync()
        {
            try
            {

                _DataHelper = new DataAccessHelper("sps_Announcements", _configuration);

                DataTable dt = new DataTable();

                int result = await _DataHelper.RunAsync(dt);


                List<Announcements> announcements = new List<Announcements>();

                if (dt != null && dt.Rows.Count > 0)
                {

                    announcements = (from model in dt.AsEnumerable()
                                select new Announcements()
                                {
                                    Id = model.Field<int>("Id"),
                                    Text = model.Field<string>("Text"),
                                    Text_Ar = model.Field<string>("Text_Ar"),
                                    CreatedBy = model.Field<string>("CreatedBy"),
                                    CreatedOn = model.Field<DateTime>("CreatedOn"),
                                    UpdatedOn = model.Field<DateTime>("UpdatedOn"),
                                    UpdatedBy = model.Field<string>("UpdatedBy"),
                                    Active = model.Field<bool?>("Active")

                                }).ToList();
                }

                return new DatabaseResponse { ResponseCode = result, Results = announcements };
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
        public async Task<DatabaseResponse> PostAsync(AnnouncementsCreate announcementsCreate)
        {
            try
            {
                SqlParameter[] parameters =
            {
                    new SqlParameter( "@Text",  SqlDbType.NVarChar),
                    new SqlParameter( "@CreatedBy",  SqlDbType.Int),
                    new SqlParameter( "@Active",  SqlDbType.Bit),
                    new SqlParameter( "@Text_Ar",  SqlDbType.NVarChar)

                };

                parameters[0].Value = announcementsCreate.Text;
                parameters[1].Value = announcementsCreate.CreatedBy;
                parameters[2].Value = announcementsCreate.Active;
                parameters[3].Value = announcementsCreate.Text_Ar;

                _DataHelper = new DataAccessHelper("spi_Announcements", parameters, _configuration);

                DataTable dt = new DataTable();

                int result = await _DataHelper.RunAsync(dt);

            
                return new DatabaseResponse { ResponseCode = result };
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
