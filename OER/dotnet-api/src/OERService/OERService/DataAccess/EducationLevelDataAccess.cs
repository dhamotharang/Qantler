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
    public class EducationLevelDataAccess
    {
        internal DataAccessHelper _DataHelper = null;

		private readonly IConfiguration _configuration;

        /// <summary>
        /// Constructor setting configuration
        /// </summary>
        /// <param name="configuration"></param>
        public EducationLevelDataAccess(IConfiguration configuration)
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

                _DataHelper = new DataAccessHelper("spd_lu_Level", parameters, _configuration);

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
        public async Task<DatabaseResponse> GetByIdAsync(int Id)
        {
            try
            {

                SqlParameter[] parameters =
               {
                    new SqlParameter( "@Id",  SqlDbType.NVarChar )

                };

                parameters[0].Value = Id;

                _DataHelper = new DataAccessHelper("sps_lu_LevelById", parameters, _configuration);

                DataTable dt = new DataTable();

                int result = await _DataHelper.RunAsync(dt);
                List<EducationLevel> eduLevel = new List<EducationLevel>();

                if (dt != null && dt.Rows.Count > 0)
                {

                    eduLevel = (from model in dt.AsEnumerable()
                                select new EducationLevel()
                                {
                                    Id = model.Field<int>("Id"),
                                    Level = model.Field<string>("Level"),
                                    Level_Ar = model.Field<string>("Level_Ar"),
                                    CreatedBy = model.Field<string>("CreatedBy"),
                                    CreatedOn = model.Field<DateTime>("CreatedOn"),
                                    UpdatedOn = model.Field<DateTime>("UpdatedOn"),
                                    UpdatedBy = model.Field<string>("UpdatedBy"),
                                    Active = model.Field<bool?>("Active")

                                }).ToList();
                }

                return new DatabaseResponse { ResponseCode = result, Results = eduLevel };
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

                _DataHelper = new DataAccessHelper("sps_lu_Level", _configuration);

                DataTable dt = new DataTable();

                int result = await _DataHelper.RunAsync(dt);


                List<EducationLevel> eduLevel = new List<EducationLevel>();

                if (dt != null && dt.Rows.Count > 0)
                {

                    eduLevel = (from model in dt.AsEnumerable()
                                select new EducationLevel()
                                {
                                    Id = model.Field<int>("Id"),
                                    Level = model.Field<string>("Level"),
                                    Level_Ar = model.Field<string>("Level_Ar"),
                                    CreatedBy = model.Field<string>("CreatedBy"),
                                    CreatedOn = model.Field<DateTime>("CreatedOn"),
                                    UpdatedOn = model.Field<DateTime>("UpdatedOn"),
                                    UpdatedBy = model.Field<string>("UpdatedBy"),
                                    Active = model.Field<bool?>("Active"),
									Weight = model.Field<int>("Weight")
								}).ToList();
                }

                return new DatabaseResponse { ResponseCode = result, Results = eduLevel };
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
        public async Task<DatabaseResponse> PutAsync(EducationLevelUpdate educationLevelUpdate)
        {
            try
            {

                SqlParameter[] parameters =
               {
                    new SqlParameter( "@Id",  SqlDbType.Int ),
                    new SqlParameter( "@Level",  SqlDbType.NVarChar ),
                    new SqlParameter( "@Level_Ar",  SqlDbType.NVarChar ),
                    new SqlParameter( "@Updatedby",  SqlDbType.Int ),
                    new SqlParameter( "@Active",  SqlDbType.Bit ),
					 new SqlParameter( "@Weight",  SqlDbType.Int )
				};

                parameters[0].Value = educationLevelUpdate.Id;
                parameters[1].Value = educationLevelUpdate.Level;
                parameters[2].Value = educationLevelUpdate.Level_Ar;
                parameters[3].Value = educationLevelUpdate.UpdatedBy;
                parameters[4].Value = educationLevelUpdate.Active;
				parameters[5].Value = educationLevelUpdate.Weight;
				_DataHelper = new DataAccessHelper("spu_lu_Level", parameters, _configuration);

                DataTable dt = new DataTable();

                int result = await _DataHelper.RunAsync(dt);



                List<EducationLevel> eduLevel = new List<EducationLevel>();

                if (dt != null && dt.Rows.Count > 0)
                {

                    eduLevel = (from model in dt.AsEnumerable()
                                select new EducationLevel()
                                {
                                    Id = model.Field<int>("Id"),
                                    Level = model.Field<string>("Level"),
                                    Level_Ar = model.Field<string>("Level_Ar"),
                                    CreatedBy = model.Field<string>("CreatedBy"),
                                    CreatedOn = model.Field<DateTime>("CreatedOn"),
                                    UpdatedOn = model.Field<DateTime>("UpdatedOn"),
                                    UpdatedBy = model.Field<string>("UpdatedBy"),
                                    Active = model.Field<bool?>("Active"),
									Weight = model.Field<int>("Weight")
								}).ToList();
                }

                return new DatabaseResponse { ResponseCode = result, Results = eduLevel };
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
        public async Task<DatabaseResponse> PostAsync(EducationLevelCreate educationLevelCreate)
        {
            try
            {

                SqlParameter[] parameters =
               {
                    new SqlParameter( "@Level",  SqlDbType.NVarChar ),
                    new SqlParameter( "@Level_Ar",  SqlDbType.NVarChar ),
                    new SqlParameter( "@CreatedBy",  SqlDbType.Int ),
					 new SqlParameter( "@Weight",  SqlDbType.Int )
				};

                parameters[0].Value = educationLevelCreate.Level;
                parameters[1].Value = educationLevelCreate.Level_Ar;
                parameters[2].Value = educationLevelCreate.CreatedBy;
				parameters[3].Value = educationLevelCreate.Weight;
				_DataHelper = new DataAccessHelper("spi_lu_Level", parameters, _configuration);

                DataTable dt = new DataTable();

                int result = await _DataHelper.RunAsync(dt);

                List<EducationLevel> eduLevel = new List<EducationLevel>();

                if (dt != null && dt.Rows.Count > 0)
                {

                    eduLevel = (from model in dt.AsEnumerable()
                                   select new EducationLevel()
                                   {
                                       Id = model.Field<int>("Id"),
                                       Level = model.Field<string>("Level"),
                                       Level_Ar = model.Field<string>("Level_Ar"),
                                       CreatedBy = model.Field<string>("CreatedBy"),
                                       CreatedOn = model.Field<DateTime>("CreatedOn"),
                                       UpdatedOn = model.Field<DateTime>("UpdatedOn"),
                                       UpdatedBy = model.Field<string>("UpdatedBy"),
                                       Active = model.Field<bool?>("Active"),
									   Weight = model.Field<int>("Weight")
								   }).ToList();
                }

                return new DatabaseResponse { ResponseCode = result, Results = eduLevel };
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
