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
    public class SubCategoryDataAccess
    {
        internal DataAccessHelper _DataHelper = null;

		private readonly IConfiguration _configuration;

        /// <summary>
        /// Constructor setting configuration
        /// </summary>
        /// <param name="configuration"></param>
        public SubCategoryDataAccess(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<DatabaseResponse> CreateSubCategory(CreateSubCategoryMaster SubCategoryMaster)
        {
            try
            {

                SqlParameter[] parameters =
               {
                    new SqlParameter( "@Name",  SqlDbType.NVarChar ),
                       new SqlParameter( "@Name_Ar",  SqlDbType.NVarChar ),
                    new SqlParameter( "@CategoryId",  SqlDbType.NVarChar ),
                    new SqlParameter( "@CreatedBy",  SqlDbType.Int ),
					new SqlParameter( "@Weight",  SqlDbType.Int )
				};

                parameters[0].Value = SubCategoryMaster.Name;
                parameters[1].Value = SubCategoryMaster.Name_Ar;
                parameters[2].Value = SubCategoryMaster.CategoryId;
                parameters[3].Value = SubCategoryMaster.CreatedBy;
				parameters[4].Value = SubCategoryMaster.Weight;
				_DataHelper = new DataAccessHelper("CreateSubCategory", parameters, _configuration);

                DataTable dt = new DataTable();

                int result = await _DataHelper.RunAsync(dt);

                List<SubCategoryMaster> subCategories = new List<SubCategoryMaster>();

                if (dt != null && dt.Rows.Count > 0)
                {

                    subCategories = (from model in dt.AsEnumerable()
                                  select new SubCategoryMaster()
                                  {
                                      Id = model.Field<int>("Id"),
                                      Name = model.Field<string>("Name"),
                                      Name_Ar = model.Field<string>("Name_Ar"),
                                      CategoryId = model.Field<int>("CategoryId"),
                                      CategoryName = model.Field<string>("CategoryName"),
                                      CreatedBy = model.Field<string>("CreatedBy"),
                                      CreatedOn = model.Field<DateTime>("CreatedOn"),
                                      UpdatedOn = model.Field<DateTime>("UpdatedOn"),
                                      UpdatedBy = model.Field<string>("UpdatedBy"),
                                      Active = model.Field<bool>("Active"),
									  Weight = model.Field<int>("Weight")
								  }).ToList();
                }

                return new DatabaseResponse { ResponseCode = result, Results = subCategories };
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

        public async Task<DatabaseResponse> GetSubCategories()
        {
            try
            {
                _DataHelper = new DataAccessHelper("GetSubCategory", _configuration);

                DataTable dt = new DataTable();

                int result = await _DataHelper.RunAsync(dt);

                List<SubCategoryMaster> subCategories = new List<SubCategoryMaster>();

                if (dt != null && dt.Rows.Count > 0)
                {

                    subCategories = (from model in dt.AsEnumerable()
                                  select new SubCategoryMaster()
                                  {
                                      Id = model.Field<int>("Id"),
                                      Name = model.Field<string>("Name"),
                                      Name_Ar = model.Field<string>("Name_Ar"),
                                      CategoryId = model.Field<int>("CategoryId"),
                                      CategoryName = model.Field<string>("CategoryName"),
                                      CreatedBy = model.Field<string>("CreatedBy"),
                                      CreatedOn = model.Field<DateTime>("CreatedOn"),
                                      UpdatedOn = model.Field<DateTime>("UpdatedOn"),
                                      UpdatedBy = model.Field<string>("UpdatedBy"),
                                      Active = model.Field<bool>("Active"),
									   Weight = model.Field<int>("Weight")
								  }).ToList();
                }

                return new DatabaseResponse { ResponseCode = result, Results = subCategories };
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

        public async Task<DatabaseResponse> GetSubCategory(int id)
        {
            try
            {

                SqlParameter[] parameters =
               {
                    new SqlParameter( "@Id",  SqlDbType.NVarChar )

                };

                parameters[0].Value = id;

                _DataHelper = new DataAccessHelper("GetSubCategoryById", parameters, _configuration);

                DataTable dt = new DataTable();

                int result = await _DataHelper.RunAsync(dt);

                List<SubCategoryMaster> subCategories = new List<SubCategoryMaster>();

                if (dt != null && dt.Rows.Count > 0)
                {

                    subCategories = (from model in dt.AsEnumerable()
                                  select new SubCategoryMaster()
                                  {
                                      Id = model.Field<int>("Id"),
                                      Name = model.Field<string>("Name"),
                                      Name_Ar = model.Field<string>("Name_Ar"),
                                      CategoryId = model.Field<int>("CategoryId"),
                                      CategoryName = model.Field<string>("CategoryName"),
                                      CreatedBy = model.Field<string>("CreatedBy"),
                                      CreatedOn = model.Field<DateTime>("CreatedOn"),
                                      UpdatedOn = model.Field<DateTime>("UpdatedOn"),
                                      UpdatedBy = model.Field<string>("UpdatedBy"),
                                      Active = model.Field<bool>("Active")

                                  }).ToList();
                }

                return new DatabaseResponse { ResponseCode = result, Results = subCategories };
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

        public async Task<DatabaseResponse> DeleteSubCategory(int id)
        {
            try
            {

                SqlParameter[] parameters =
               {
                    new SqlParameter( "@Id",  SqlDbType.NVarChar )

                };

                parameters[0].Value = id;

                _DataHelper = new DataAccessHelper("DeleteSubCategory", parameters, _configuration);

                DataTable dt = new DataTable();

                int result = await _DataHelper.RunAsync(dt);

                List<SubCategoryMaster> subCategories = new List<SubCategoryMaster>();

                if (dt != null && dt.Rows.Count > 0)
                {

                    subCategories = (from model in dt.AsEnumerable()
                                  select new SubCategoryMaster()
                                  {
                                      Id = model.Field<int>("Id"),
                                      Name = model.Field<string>("Name"),
                                      CategoryId = model.Field<int>("CategoryId"),
                                      CategoryName = model.Field<string>("CategoryName"),
                                      CreatedBy = model.Field<string>("CreatedBy"),
                                      CreatedOn = model.Field<DateTime>("CreatedOn"),
                                      UpdatedOn = model.Field<DateTime>("UpdatedOn"),
                                      UpdatedBy = model.Field<string>("UpdatedBy"),
                                      Active = model.Field<bool>("Active")
                                  }).ToList();
                }

                return new DatabaseResponse { ResponseCode = result, Results = subCategories };
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

        public async Task<DatabaseResponse> UpdateSubCategory(UpdateSubCategoryMaster SubCategoryMaster, int id)
        {
            try
            {

                SqlParameter[] parameters =
               {
                    new SqlParameter( "@Id",  SqlDbType.Int ),
                    new SqlParameter( "@Name",  SqlDbType.NVarChar ),
                    new SqlParameter( "@Name_Ar",  SqlDbType.NVarChar ),
                    new SqlParameter( "@CategoryId",  SqlDbType.NVarChar ),                    
                    new SqlParameter( "@UpdatedBy",  SqlDbType.Int ),
                    new SqlParameter( "@Active",  SqlDbType.Bit ),
					new SqlParameter( "@Weight",  SqlDbType.Int )
				};


                parameters[0].Value = id;
                parameters[1].Value = SubCategoryMaster.Name;
                parameters[2].Value = SubCategoryMaster.Name_Ar;
                parameters[3].Value = SubCategoryMaster.CategoryId;
                parameters[4].Value = SubCategoryMaster.UpdatedBy;
                parameters[5].Value = SubCategoryMaster.Active;
				parameters[6].Value = SubCategoryMaster.Weight;
				_DataHelper = new DataAccessHelper("UpdateSubCategory", parameters, _configuration);

                DataTable dt = new DataTable();

                int result = await _DataHelper.RunAsync(dt);

                List<SubCategoryMaster> subCategories = new List<SubCategoryMaster>();

                if (dt != null && dt.Rows.Count > 0)
                {

                    subCategories = (from model in dt.AsEnumerable()
                                  select new SubCategoryMaster()
                                  {
                                      Id = model.Field<int>("Id"),
                                      Name = model.Field<string>("Name"),
                                      Name_Ar = model.Field<string>("Name_Ar"),
                                      CategoryId = model.Field<int>("CategoryId"),
                                      CategoryName = model.Field<string>("CategoryName"),
                                      CreatedBy = model.Field<string>("CreatedBy"),
                                      CreatedOn = model.Field<DateTime>("CreatedOn"),
                                      UpdatedOn = model.Field<DateTime>("UpdatedOn"),
                                      UpdatedBy = model.Field<string>("UpdatedBy"),
                                      Active = model.Field<bool>("Active"),
									   Weight = model.Field<int>("Weight")
								  }).ToList();
                }

                return new DatabaseResponse { ResponseCode = result, Results = subCategories };
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
