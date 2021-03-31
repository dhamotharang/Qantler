using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Core.Helpers;
using Microsoft.Extensions.Configuration;
using Core.Models;
using Core.Enums;
using OERService.Models;
using Serilog;

namespace OERService.DataAccess
{
    public class CopyrightDataAccess
    {
        internal DataAccessHelper _DataHelper = null;

		private readonly IConfiguration _configuration;

        /// <summary>
        /// Constructor setting configuration
        /// </summary>
        /// <param name="configuration"></param>
        public CopyrightDataAccess(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<DatabaseResponse> CreateCopyright(CreateCopyrightMaster copyright)
        {
            try
            {

                SqlParameter[] parameters =
                {
                    new SqlParameter( "@Title",  SqlDbType.NVarChar ),
                    new SqlParameter( "@Description",  SqlDbType.Text ),
                    new SqlParameter( "@Title_Ar",  SqlDbType.NVarChar ),
                    new SqlParameter( "@Description_Ar",  SqlDbType.NVarChar ),
                    new SqlParameter( "@CreatedBy",  SqlDbType.Int ) ,
                    new SqlParameter( "@Media",  SqlDbType.NVarChar ),
                    new SqlParameter( "@Protected",  SqlDbType.Bit ),
                    new SqlParameter( "@IsResourceProtect",  SqlDbType.Bit ),
					 new SqlParameter( "@Weight",  SqlDbType.Int )
				};

                parameters[0].Value = copyright.Title;
                parameters[1].Value = copyright.Description;
                parameters[2].Value = copyright.Title_Ar;
                parameters[3].Value = copyright.Description_Ar;
                parameters[4].Value = copyright.CreatedBy;
                parameters[5].Value = copyright.Media;
                parameters[6].Value = copyright.Protected;
                parameters[7].Value = copyright.IsResourceProtect;
				parameters[8].Value = copyright.Weight;
				_DataHelper = new DataAccessHelper("CreateCopyright", parameters, _configuration);

                DataTable dt = new DataTable();

                int result = await _DataHelper.RunAsync(dt);

                List<CopyrightMaster> copyrights = new List<CopyrightMaster>();

                if (dt != null && dt.Rows.Count > 0)
                {

                    copyrights = (from model in dt.AsEnumerable()
                                  select new CopyrightMaster()
                                  {
                                      Id = model.Field<int>("Id"),
                                      Title = model.Field<string>("Title"),
                                      Description = model.Field<string>("Description"),
                                      Title_Ar = model.Field<string>("Title_Ar"),
                                      Description_Ar = model.Field<string>("Description_Ar"),
                                      CreatedBy = model.Field<string>("CreatedBy"),
                                      CreatedOn = model.Field<DateTime>("CreatedOn"),
                                      UpdatedOn = model.Field<DateTime>("UpdatedOn"),
                                      UpdatedBy = model.Field<string>("UpdatedBy"),
                                      Active = model.Field<bool>("Active"),
                                      Protected = model.Field<bool>("Protected"),
                                      IsResourceProtect = model.Field<bool>("IsResourceProtect"),
									  Weight = model.Field<int>("Weight")
								  }).ToList();
                }

                return new DatabaseResponse { ResponseCode = result, Results = copyrights };
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

        public async Task<DatabaseResponse> GetCopyrights()
        {
            try
            {
                _DataHelper = new DataAccessHelper("GetCopyright", _configuration);

                DataTable dt = new DataTable();

                int result = await _DataHelper.RunAsync(dt);

                List<CopyrightMaster> copyrights = new List<CopyrightMaster>();

                if (dt != null && dt.Rows.Count > 0)
                {

                    copyrights = (from model in dt.AsEnumerable()
                                  select new CopyrightMaster()
                                  {
                                      Id = model.Field<int>("Id"),
                                      Title = model.Field<string>("Title"),
                                      Description = model.Field<string>("Description"),
                                      Title_Ar = model.Field<string>("Title_Ar"),
                                      Description_Ar = model.Field<string>("Description_Ar"),
                                      CreatedBy = model.Field<string>("CreatedBy"),
                                      CreatedOn = model.Field<DateTime>("CreatedOn"),
                                      UpdatedOn = model.Field<DateTime>("UpdatedOn"),
                                      UpdatedBy = model.Field<string>("UpdatedBy"),
                                      Active = model.Field<bool>("Active"),
                                      Media = model.Field<string>("Media"),
                                      Protected = model.Field<bool>("Protected"),
                                      IsResourceProtect = model.Field<bool>("IsResourceProtect"),
									  Weight = model.Field<int>("Weight")
								  }).ToList();
                }

                return new DatabaseResponse { ResponseCode = result, Results = copyrights };
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

        public async Task<DatabaseResponse> GetCopyright(int id)
        {
            try
            {

                SqlParameter[] parameters =
               {
                    new SqlParameter( "@Id",  SqlDbType.NVarChar )

                };

                parameters[0].Value = id;

                _DataHelper = new DataAccessHelper("GetCopyrightById", parameters, _configuration);

                DataTable dt = new DataTable();

                int result = await _DataHelper.RunAsync(dt);

                List<CopyrightMaster> copyrights = new List<CopyrightMaster>();

                if (dt != null && dt.Rows.Count > 0)
                {

                    copyrights = (from model in dt.AsEnumerable()
                                  select new CopyrightMaster()
                                  {
                                      Id = model.Field<int>("Id"),
                                      Title = model.Field<string>("Title"),
                                      Description = model.Field<string>("Description"),
                                      Title_Ar = model.Field<string>("Title_Ar"),
                                      Description_Ar = model.Field<string>("Description_Ar"),
                                      CreatedBy = model.Field<string>("CreatedBy"),
                                      CreatedOn = model.Field<DateTime>("CreatedOn"),
                                      UpdatedOn = model.Field<DateTime>("UpdatedOn"),
                                      UpdatedBy = model.Field<string>("UpdatedBy"),
                                      Active = model.Field<bool>("Active"),
                                      Media = model.Field<string>("Media"),
                                      Protected = model.Field<bool>("Protected"),
                                      IsResourceProtect = model.Field<bool>("IsResourceProtect")

                                  }).ToList();
                }

                return new DatabaseResponse { ResponseCode = result, Results = copyrights };
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

        public async Task<DatabaseResponse> DeleteCopyright(int id)
        {
            try
            {

                SqlParameter[] parameters =
               {
                    new SqlParameter( "@Id",  SqlDbType.NVarChar )

                };

                parameters[0].Value = id;

                _DataHelper = new DataAccessHelper("DeleteCopyright", parameters, _configuration);

                DataTable dt = new DataTable();

                int result = await _DataHelper.RunAsync(dt);

                List<CopyrightMaster> copyrights = new List<CopyrightMaster>();

                if (dt != null && dt.Rows.Count > 0)
                {

                    copyrights = (from model in dt.AsEnumerable()
                                  select new CopyrightMaster()
                                  {
                                      Id = model.Field<int>("Id"),
                                      Title = model.Field<string>("Title"),
                                      Description = model.Field<string>("Description"),
                                      CreatedBy = model.Field<string>("CreatedBy"),
                                      CreatedOn = model.Field<DateTime>("CreatedOn"),
                                      UpdatedOn = model.Field<DateTime>("UpdatedOn"),
                                      UpdatedBy = model.Field<string>("UpdatedBy"),
                                      Active = model.Field<bool>("Active")

                                  }).ToList();
                }

                return new DatabaseResponse { ResponseCode = result, Results = copyrights };
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

        public async Task<DatabaseResponse> UpdateCopyright(UpdateCopyrightMaster Copyright, int id)
        {
            try
            {

                SqlParameter[] parameters =
               {
                    new SqlParameter( "@Id",  SqlDbType.Int ),
                    new SqlParameter( "@Title",  SqlDbType.NVarChar ),
                    new SqlParameter( "@Description",  SqlDbType.Text ),
                    new SqlParameter( "@Title_Ar",  SqlDbType.NVarChar ),
                    new SqlParameter( "@Description_Ar",  SqlDbType.NVarChar ),
                    new SqlParameter( "@UpdatedBy",  SqlDbType.Int ),
                    new SqlParameter( "@Active",  SqlDbType.Bit ),
                    new SqlParameter( "@Media",  SqlDbType.NVarChar ),
                    new SqlParameter( "@Protected",  SqlDbType.Bit ),
                    new SqlParameter( "@IsResourceProtect",  SqlDbType.Bit ),
					 new SqlParameter( "@Weight",  SqlDbType.Int )
				};
                parameters[0].Value = id;
                parameters[1].Value = Copyright.Title;
                parameters[2].Value = Copyright.Description;
                parameters[3].Value = Copyright.Title_Ar;
                parameters[4].Value = Copyright.Description_Ar;
                parameters[5].Value = Copyright.UpdatedBy;
                parameters[6].Value = Copyright.Active;
                parameters[7].Value = Copyright.Media;
                parameters[8].Value = Copyright.Protected;
                parameters[9].Value = Copyright.IsResourceProtect;
				parameters[10].Value = Copyright.Weight;
				_DataHelper = new DataAccessHelper("UpdateCopyright", parameters, _configuration);

                DataTable dt = new DataTable();

                int result = await _DataHelper.RunAsync(dt);

                List<CopyrightMaster> copyrights = new List<CopyrightMaster>();

                if (dt != null && dt.Rows.Count > 0)
                {

                    copyrights = (from model in dt.AsEnumerable()
                                  select new CopyrightMaster()
                                  {
                                      Id = model.Field<int>("Id"),
                                      Title = model.Field<string>("Title"),
                                      Description = model.Field<string>("Description"),
                                      Title_Ar = model.Field<string>("Title_Ar"),
                                      Description_Ar = model.Field<string>("Description_Ar"),
                                      CreatedBy = model.Field<string>("CreatedBy"),
                                      CreatedOn = model.Field<DateTime>("CreatedOn"),
                                      UpdatedOn = model.Field<DateTime>("UpdatedOn"),
                                      UpdatedBy = model.Field<string>("UpdatedBy"),
                                      Active = model.Field<bool>("Active"),
                                      Media = model.Field<string>("Media"),
                                      Protected = model.Field<bool>("Protected"),
                                      IsResourceProtect = model.Field<bool>("IsResourceProtect"),
									  Weight = model.Field<int>("Weight")
								  }).ToList();
                }

                return new DatabaseResponse { ResponseCode = result, Results = copyrights };
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
