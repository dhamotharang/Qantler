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
    public class EducationalUseDataAccess
    {
        internal DataAccessHelper _DataHelper = null;

		private readonly IConfiguration _configuration;

        /// <summary>
        /// Constructor setting configuration
        /// </summary>
        /// <param name="configuration"></param>
        public EducationalUseDataAccess(IConfiguration configuration)
        {
            _configuration = configuration;
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

                _DataHelper = new DataAccessHelper("spd_lu_Educational_Use", parameters, _configuration);

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
        public async Task<DatabaseResponse> PutAsync(EducationalUseUpdate educationalUseUpdate)
        {
            try
            {

                SqlParameter[] parameters =
               {
                    new SqlParameter( "@Id",  SqlDbType.Int ),
                    new SqlParameter( "@EducationalUse",  SqlDbType.NVarChar ),
                    new SqlParameter( "@EducationalUse_Ar",  SqlDbType.NVarChar ),
                    new SqlParameter( "@Updatedby",  SqlDbType.Int ),
                    new SqlParameter( "@Active",  SqlDbType.Bit ),
					new SqlParameter( "@Weight",  SqlDbType.Int )
				};

                parameters[0].Value = educationalUseUpdate.Id;
                parameters[1].Value = educationalUseUpdate.Name;
                parameters[2].Value = educationalUseUpdate.Name_Ar;
                parameters[3].Value = educationalUseUpdate.UpdatedBy;
                parameters[4].Value = educationalUseUpdate.Active;
				parameters[5].Value = educationalUseUpdate.Weight;
				_DataHelper = new DataAccessHelper("spu_lu_Educational_Use", parameters, _configuration);

                DataTable dt = new DataTable();

                int result = await _DataHelper.RunAsync(dt);

                List<EducationalUseModel> educationalUseModel = new List<EducationalUseModel>();

                if (dt != null && dt.Rows.Count > 0)
                {

                    educationalUseModel = (from model in dt.AsEnumerable()
                                           select new EducationalUseModel()
                                           {
                                               Id = model.Field<int>("Id"),
                                               Name = model.Field<string>("EducationalUse"),
                                               Name_Ar = model.Field<string>("EducationalUse_Ar"),
                                               CreatedBy = model.Field<string>("CreatedBy"),
                                               CreatedOn = model.Field<DateTime>("CreatedOn"),
                                               UpdatedOn = model.Field<DateTime>("UpdatedOn"),
                                               UpdatedBy = model.Field<string>("UpdatedBy"),
                                               Active = model.Field<bool?>("Active"),
											   Weight = model.Field<int>("Weight")
										   }).ToList();
                }

                return new DatabaseResponse { ResponseCode = result, Results = educationalUseModel };
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
        public async Task<DatabaseResponse> PostAsync(EducationalUseCreate educationalUseCreate)
        {
            try
            {

                SqlParameter[] parameters =
               {
                    new SqlParameter( "@EducationalUse",  SqlDbType.NVarChar ),
                    new SqlParameter( "@EducationalUse_Ar",  SqlDbType.NVarChar ),
                    new SqlParameter( "@CreatedBy",  SqlDbType.Int ),
					new SqlParameter( "@Weight",  SqlDbType.Int )
				};

                parameters[0].Value = educationalUseCreate.Name;
                parameters[1].Value = educationalUseCreate.Name_Ar;
                parameters[2].Value = educationalUseCreate.CreatedBy;
				parameters[3].Value = educationalUseCreate.Weight;
				_DataHelper = new DataAccessHelper("spi_lu_Educational_Use", parameters, _configuration);

                DataTable dt = new DataTable();

                int result = await _DataHelper.RunAsync(dt);

                List<EducationalUseModel> educationalUseModel = new List<EducationalUseModel>();

                if (dt != null && dt.Rows.Count > 0)
                {

                    educationalUseModel = (from model in dt.AsEnumerable()
                                           select new EducationalUseModel()
                                           {
                                               Id = model.Field<int>("Id"),
                                               Name = model.Field<string>("EducationalUse"),
                                               Name_Ar = model.Field<string>("EducationalUse_Ar"),
                                               CreatedBy = model.Field<string>("CreatedBy"),
                                               CreatedOn = model.Field<DateTime>("CreatedOn"),
                                               UpdatedOn = model.Field<DateTime>("UpdatedOn"),
                                               UpdatedBy = model.Field<string>("UpdatedBy"),
                                               Active = model.Field<bool?>("Active"),
											   Weight = model.Field<int>("Weight")
										   }).ToList();
                }

                return new DatabaseResponse { ResponseCode = result, Results = educationalUseModel };
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

                _DataHelper = new DataAccessHelper("sps_EducationalUseById", parameters, _configuration);

                DataTable dt = new DataTable();

                int result = await _DataHelper.RunAsync(dt);

                List<EducationalUseModel> educationalUseModel = new List<EducationalUseModel>();

                if (dt != null && dt.Rows.Count > 0)
                {

                    educationalUseModel = (from model in dt.AsEnumerable()
                                  select new EducationalUseModel()
                                  {
                                      Id = model.Field<int>("Id"),
                                      Name = model.Field<string>("EducationalUse"),
                                      Name_Ar = model.Field<string>("EducationalUse_Ar"),
                                      CreatedBy = model.Field<string>("CreatedBy"),
                                      CreatedOn = model.Field<DateTime>("CreatedOn"),
                                      UpdatedOn = model.Field<DateTime>("UpdatedOn"),
                                      UpdatedBy = model.Field<string>("UpdatedBy"),
                                      Active = model.Field<bool?>("Active")

                                  }).ToList();
                }

                return new DatabaseResponse { ResponseCode = result, Results = educationalUseModel };
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

                _DataHelper = new DataAccessHelper("sps_EducationalUse", _configuration);

                DataTable dt = new DataTable();

                int result = await _DataHelper.RunAsync(dt);

                List<EducationalUseModel> educationalUseModel = new List<EducationalUseModel>();

                if (dt != null && dt.Rows.Count > 0)
                {

                    educationalUseModel = (from model in dt.AsEnumerable()
                                           select new EducationalUseModel()
                                           {
                                               Id = model.Field<int>("Id"),
                                               Name = model.Field<string>("EducationalUse"),
                                               Name_Ar = model.Field<string>("EducationalUse_Ar"),
                                               CreatedBy = model.Field<string>("CreatedBy"),
                                               CreatedOn = model.Field<DateTime>("CreatedOn"),
                                               UpdatedOn = model.Field<DateTime>("UpdatedOn"),
                                               UpdatedBy = model.Field<string>("UpdatedBy"),
                                               Active = model.Field<bool?>("Active"),
											   Weight = model.Field<int>("Weight")
										   }).ToList();
                }

                return new DatabaseResponse { ResponseCode = result, Results = educationalUseModel };
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
