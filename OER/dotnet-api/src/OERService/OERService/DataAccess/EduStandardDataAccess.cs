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
    public class EduStandardDataAccess
    {
        internal DataAccessHelper _DataHelper = null;

		private readonly IConfiguration _configuration;

        /// <summary>
        /// Constructor setting configuration
        /// </summary>
        /// <param name="configuration"></param>
        public EduStandardDataAccess(IConfiguration configuration)
        {
            _configuration = configuration;
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

                _DataHelper = new DataAccessHelper("sps_lu_Educational_StandardById", parameters, _configuration);

                DataTable dt = new DataTable();

                int result = await _DataHelper.RunAsync(dt);

                List<EducationalStandardModel> eduStandard = new List<EducationalStandardModel>();

                if (dt != null && dt.Rows.Count > 0)
                {


                    eduStandard = (from model in dt.AsEnumerable()
                                   select new EducationalStandardModel()
                                   {
                                       Id = model.Field<int>("Id"),
                                       Standard = model.Field<string>("Standard"),
                                       Standard_Ar = model.Field<string>("Standard_Ar"),
                                       CreatedBy = model.Field<string>("CreatedBy"),
                                       CreatedOn = model.Field<DateTime>("CreatedOn"),
                                       UpdatedOn = model.Field<DateTime>("UpdatedOn"),
                                       UpdatedBy = model.Field<string>("UpdatedBy"),
                                       Active = model.Field<bool?>("Active")

                                   }).ToList();
                }

                return new DatabaseResponse { ResponseCode = result, Results = eduStandard };
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

                _DataHelper = new DataAccessHelper("sps_lu_Educational_Standard", _configuration);

                DataTable dt = new DataTable();

                int result = await _DataHelper.RunAsync(dt);

                List<EducationalStandardModel> eduStandard = new List<EducationalStandardModel>();

                if (dt != null && dt.Rows.Count > 0)
                {

                    eduStandard = (from model in dt.AsEnumerable()
                                   select new EducationalStandardModel()
                                   {
                                       Id = model.Field<int>("Id"),
                                       Standard = model.Field<string>("Standard"),
                                       Standard_Ar = model.Field<string>("Standard_Ar"),
                                       CreatedBy = model.Field<string>("CreatedBy"),
                                       CreatedOn = model.Field<DateTime>("CreatedOn"),
                                       UpdatedOn = model.Field<DateTime>("UpdatedOn"),
                                       UpdatedBy = model.Field<string>("UpdatedBy"),
                                       Active = model.Field<bool?>("Active"),
									   Weight = model.Field<int>("Weight")
								   }).ToList();
                }

                return new DatabaseResponse { ResponseCode = result, Results = eduStandard };
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
        public async Task<DatabaseResponse> PutAsync(EducationalStandardUpdate educationalStandardUpdate)
        {
            try
            {

                SqlParameter[] parameters =
               {
                    new SqlParameter( "@Id",  SqlDbType.Int ),
                    new SqlParameter( "@Standard",  SqlDbType.NVarChar ),
                    new SqlParameter( "@Standard_Ar",  SqlDbType.NVarChar ),
                    new SqlParameter( "@Updatedby",  SqlDbType.Int ),
                    new SqlParameter( "@Active",  SqlDbType.Bit ),
					new SqlParameter( "@Weight",  SqlDbType.Int)
				};

                parameters[0].Value = educationalStandardUpdate.Id;
                parameters[1].Value = educationalStandardUpdate.Standard;
                parameters[2].Value = educationalStandardUpdate.Standard_Ar;
                parameters[3].Value = educationalStandardUpdate.UpdatedBy;
                parameters[4].Value = educationalStandardUpdate.Active;
				parameters[5].Value = educationalStandardUpdate.Weight;
				_DataHelper = new DataAccessHelper("spu_lu_Educational_Standard", parameters, _configuration);

                DataTable dt = new DataTable();

                int result = await _DataHelper.RunAsync(dt);


                List<EducationalStandardModel> eduStandard = new List<EducationalStandardModel>();

                if (dt != null && dt.Rows.Count > 0)
                {

                    eduStandard = (from model in dt.AsEnumerable()
                                   select new EducationalStandardModel()
                                   {
                                       Id = model.Field<int>("Id"),
                                       Standard = model.Field<string>("Standard"),
                                       Standard_Ar = model.Field<string>("Standard_Ar"),
                                       CreatedBy = model.Field<string>("CreatedBy"),
                                       CreatedOn = model.Field<DateTime>("CreatedOn"),
                                       UpdatedOn = model.Field<DateTime>("UpdatedOn"),
                                       UpdatedBy = model.Field<string>("UpdatedBy"),
                                       Active = model.Field<bool?>("Active"),
									   Weight = model.Field<int>("Weight")
								   }).ToList();
                }

                return new DatabaseResponse { ResponseCode = result, Results = eduStandard };
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
        public async Task<DatabaseResponse> PostAsync(EducationalStandardCreate educationalStandardCreate)
        {
            try
            {

                SqlParameter[] parameters =
               {
                    new SqlParameter( "@Standard",  SqlDbType.NVarChar ),
                    new SqlParameter( "@Standard_Ar",  SqlDbType.NVarChar ),
                    new SqlParameter( "@CreatedBy",  SqlDbType.Int ),
					 new SqlParameter( "@Weight",  SqlDbType.Int )
				};

                parameters[0].Value = educationalStandardCreate.Standard;
                parameters[1].Value = educationalStandardCreate.Standard_Ar;
                parameters[2].Value = educationalStandardCreate.CreatedBy;
				parameters[3].Value = educationalStandardCreate.Weight;
				_DataHelper = new DataAccessHelper("spi_lu_Educational_Standard", parameters, _configuration);

                DataTable dt = new DataTable();

                int result = await _DataHelper.RunAsync(dt);

                List<EducationalStandardModel> eduStandard = new List<EducationalStandardModel>();

                if (dt != null && dt.Rows.Count > 0)
                {

                    eduStandard = (from model in dt.AsEnumerable()
                                           select new EducationalStandardModel()
                                           {
                                               Id = model.Field<int>("Id"),
                                               Standard = model.Field<string>("Standard"),
                                               Standard_Ar = model.Field<string>("Standard_Ar"),
                                               CreatedBy = model.Field<string>("CreatedBy"),
                                               CreatedOn = model.Field<DateTime>("CreatedOn"),
                                               UpdatedOn = model.Field<DateTime>("UpdatedOn"),
                                               UpdatedBy = model.Field<string>("UpdatedBy"),
                                               Active = model.Field<bool?>("Active"),
											   Weight = model.Field<int>("Weight")
										   }).ToList();
                }

                return new DatabaseResponse { ResponseCode = result, Results = eduStandard };
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

                _DataHelper = new DataAccessHelper("spd_lu_Educational_Standard", parameters, _configuration);

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
