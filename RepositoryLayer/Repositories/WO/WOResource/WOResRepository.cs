using Dapper;
using IdylAPI.Models;
using IdylAPI.Models.Img;
using IdylAPI.Models.WO;
using IdylAPI.Services.Interfaces.WO;
using Microsoft.Extensions.Configuration;
using PAUtility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using static System.Data.CommandType;
namespace IdylAPI.Services.Repository.WO
{
    public class WOResRepository : IWOResRepository
    {
        private readonly IConfiguration _configuration;
        private readonly string _connStr;
        public WOResRepository(IConfiguration configuration)
        {

            _configuration = configuration;
            _connStr = _configuration.GetConnectionString("IDYLConnection");
        }

        public Result Delete(int id)
        {
            Result result = new Result();
            using (SqlConnection conn = new SqlConnection(_connStr))
            {
                conn.Open();

                try
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@WOResourceNo", id);
                    conn.Execute("sp_WOResource_Delete", parameters, commandType: StoredProcedure);
                    result.ErrMsg = "บันทึกเรียบร้อย";
                    result.StatusCode = 200;
                }
                catch (Exception ex)
                {
                    result.StatusCode = 500;
                    result.ErrMsg = ex.Message;
                }


                return result;
            }
        }
        public Result Insert(List<WOResource> wOResourceList, Models.Authorize.User user)
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
                        foreach (var wOResource in wOResourceList)
                        {
                            parameters = new DynamicParameters();
                            if (wOResource.WOResourceNo == 0)
                            {
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
                                parameters.Add("@MH", wOResource.PlnQtyMH);

                                conn.Query<int>("sp_WOResource_Insert", parameters, commandType: StoredProcedure, transaction: trans);
                                wOResource.WOResourceNo = parameters.Get<int>("@WOResourceNo");

                              
                            }
                            else
                            {
                                parameters.Add("@WOResourceNo", wOResource.WOResourceNo);
                                WOResource wOResourceOld =  conn.QueryFirst<WOResource>("sp_WOResource_GetByNo", parameters, commandType: StoredProcedure, transaction: trans);

                                parameters = new DynamicParameters();
                                parameters.Add("@WOResourceNo", wOResource.WOResourceNo);
                                parameters.Add("@DocDate", wOResourceOld.DocDate);
                                parameters.Add("@ResTypeCode", wOResourceOld.ResTypeCode);
                                parameters.Add("@ResTypeFull", wOResourceOld.ResTypeCode);
                                parameters.Add("@RescNo", wOResource.RescNo);
                                parameters.Add("@RescName", wOResourceOld.RescName);
                                parameters.Add("@PlnQtyMH", wOResource.PlnQtyMH);
                                parameters.Add("@QtyMH", wOResource.QtyMH);
                                parameters.Add("@UnitCost", wOResource.UnitCost);
                                parameters.Add("@Amount", wOResource.Amount);
                                parameters.Add("@DocNo", wOResourceOld.DocNo);
                                parameters.Add("@UpdatedBy", user.CustomerNo);
                                parameters.Add("@EQNo", wOResourceOld.EQNo);
                                parameters.Add("@IsExternal", wOResourceOld.IsExternal);
                                parameters.Add("@RescTypeReal", wOResourceOld.ResTypeCode);
                                parameters.Add("@WarrantyDate", wOResourceOld.WarrantyDate);
                                parameters.Add("@VendorNo", wOResourceOld.VendorNo);
                                parameters.Add("@WODNo", wOResourceOld.WODNo);
                                parameters.Add("@MHType", wOResourceOld.MHType);
                                parameters.Add("@CraftTypeNo", wOResourceOld.CraftTypeNo);
                                parameters.Add("@Qty", wOResourceOld.Qty);
                                parameters.Add("@MH", wOResourceOld.MH);

                                conn.Execute("sp_WOResource_Update", parameters, commandType: StoredProcedure, transaction: trans);

                            }
                        }
                        trans.Commit();

                        if (wOResourceList[0].ResTypeCode == "L" && wOResourceList[0].Type == "P")
                        {
                            new Notify.NotifyRepository(_configuration).Send(wOResourceList[0].WONo, "INPROGRESS[PLAN]", wOResourceList[0].CompanyNo, user.CustomerNo);
                        }
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
        public Result Copy(List<WOResource> wOResourceList, Models.Authorize.User user)
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
                        foreach (var wOResource in wOResourceList)
                        {
                            parameters = new DynamicParameters();
                            parameters.Add("@WOResourceNo", wOResource.WOResourceNo);
                            parameters.Add("@Qty", wOResource.PlnQtyMH);
                            parameters.Add("@DocDate", wOResource.DocDate);
                            conn.Execute("msp_WOResource_Copy", parameters, commandType: StoredProcedure, transaction: trans);
                        }

                        trans.Commit();
                        result.ErrMsg = "คัดลอกเรียบร้อย";
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
    }
}
