using Dapper;
using IdylAPI.Models;
using IdylAPI.Models.Master;
using IdylAPI.Models.WO;
using IdylAPI.Services.Interfaces.Master;
using Microsoft.Extensions.Configuration;
using PAUtility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using static System.Data.CommandType;
namespace IdylAPI.Services.Repository.Master
{
    public class ResourceRepository : IResourceRepository
    {
        private readonly IConfiguration _configuration;
        private readonly string _connStr;
        public ResourceRepository(IConfiguration configuration)
        {

            _configuration = configuration;
            _connStr = _configuration.GetConnectionString("IDYLConnection");
        }

        public Result Insert(Resource resource, Models.Authorize.User user)
        {
            Result result = new Result();
            using (SqlConnection conn = new SqlConnection(_connStr))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        DynamicParameters parameters = new DynamicParameters();
                        parameters.Add("@RescNo", resource.RescNo, DbType.Int32, ParameterDirection.Output);
                        parameters.Add("@RescType", resource.RescType);
                        parameters.Add("@RescCode", resource.RescCode);
                        parameters.Add("@RescName", resource.RescName);
                        parameters.Add("@CreatedBy", user.CustomerNo);
                        parameters.Add("@CompanyNo", resource.CompanyNo);
                        parameters.Add("@StockItem", resource.StockItem);
                        parameters.Add("@CostEstimate", resource.CostEstimate);
                        parameters.Add("@MatGroup", resource.MatGroup);
                        parameters.Add("@Location", resource.Location);
                        parameters.Add("@Unit", resource.Unit);
                        parameters.Add("@QtyMin", resource.QtyMin);
                        parameters.Add("@QtyMax", resource.QtyMax);

                        conn.Query<int>("sp_Resource_Insert", parameters, commandType: StoredProcedure, transaction: trans);
                        resource.RescNo = parameters.Get<int>("@RescNo");

                        parameters = new DynamicParameters();
                        WOResource wOResource = new WOResource()
                        {
                            ResTypeCode = resource.RescType,
                            RescNo = resource.RescNo,
                            RescName = resource.RescName,
                            UnitCost = resource.CostEstimate,
                            Amount = resource.Qonhand * resource.CostEstimate,
                            WONo = resource.WONo,
                            Type = resource.DocType,
                            IsExternal = true,
                            CompanyNo = resource.CompanyNo
                        };

                        if (resource.DocType == "P") wOResource.PlnQtyMH = (int)resource.Qonhand;
                        else wOResource.QtyMH = (int)resource.Qonhand;

                        parameters.Add("@WOResourceNo", wOResource.WOResourceNo, DbType.Int32, ParameterDirection.Output);
                        parameters.Add("@DocDate", wOResource.DocDate);
                        parameters.Add("@ResTypeCode", wOResource.ResTypeCode);
                        parameters.Add("@ResTypeFull", wOResource.ResTypeCode);
                        parameters.Add("@RescNo", wOResource.RescNo);
                        parameters.Add("@RescName", wOResource.RescName);
                        parameters.Add("@PlnQtyMH", wOResource.PlnQtyMH);
                        parameters.Add("@QtyMH", wOResource.QtyMH);
                        parameters.Add("@UnitCost", wOResource.UnitCost);
                        parameters.Add("@Amount", wOResource.Amount);
                        parameters.Add("@DocNo", wOResource.DocNo);
                        parameters.Add("@CompanyNo", wOResource.CompanyNo);
                        parameters.Add("@CreatedBy", user.CustomerNo);
                        parameters.Add("@WONo", wOResource.WONo);
                        parameters.Add("@EQNo", wOResource.EQNo);
                        parameters.Add("@IsExternal", wOResource.IsExternal);
                        parameters.Add("@WODNo", wOResource.WODNo);
                        parameters.Add("@RescTypeReal", wOResource.ResTypeCode);
                        parameters.Add("@WarrantyDate", wOResource.WarrantyDate);
                        parameters.Add("@VendorNo", wOResource.VendorNo);
                        parameters.Add("@Type", wOResource.Type);
                        parameters.Add("@IsAddByTask", 0);
                        parameters.Add("@MHType", wOResource.MHType);
                        parameters.Add("@CraftTypeNo", wOResource.CraftTypeNo);
                        parameters.Add("@Qty", wOResource.Qty);
                        parameters.Add("@MH", wOResource.MH);

                        conn.Query<int>("sp_WOResource_Insert", parameters, commandType: StoredProcedure, transaction: trans);
                        wOResource.WOResourceNo = parameters.Get<int>("@WOResourceNo");

                        trans.Commit();
                        result.ErrMsg = "บันทึกเรียบร้อย";
                        result.StatusCode = 200;
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        result.StatusCode = 500;
                        result.ErrMsg = ex.Message;
                    }
                }

                return result;
            }
        }

        public Result Retrive(WhereParameter whereParameter)
        {
            Result result = new Result();
            try
            {
                using (SqlConnection conn = new SqlConnection(_connStr))
                {
                    conn.Open();

                    DynamicParameters parameters = new DynamicParameters();
                    string condition = $" where Resource.companyno = {whereParameter.SiteNo}";
                    condition += $" and Resource.isdelete = 0 ";
                    if (!string.IsNullOrEmpty(InputVal.ToString(whereParameter.Filter)))
                    {
                        condition += $" and(Resource.RescCode like '%{whereParameter.Filter}%'";
                        condition += $" or Resource.RescName like '%{whereParameter.Filter}%')";
                    }

                    parameters.Add("@WhereSel", condition);
                    parameters.Add("@StartRow", whereParameter.StartRow);
                    parameters.Add("@EndRow", whereParameter.EndRow);

                    IEnumerable<Resource> resources = SqlMapper.Query<Resource>(conn, "msp_Resource_Retrive", parameters, commandType: StoredProcedure);
                    result.Data = resources;
                    result.StatusCode = 200;
                }
            }
            catch (Exception ex)
            {
                result.StatusCode = 500;
                result.ErrMsg = ex.Message;
            }
            return result;
        }
    }
}
