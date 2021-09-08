using Dapper;
using IdylAPI.Models;
using IdylAPI.Models.Authorize;
using IdylAPI.Models.WO;
using IdylAPI.Services.Interfaces.WO;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using static System.Data.CommandType;

namespace IdylAPI.Services.Repository.WO
{
    public class WOTaskRepository : IWOTaskRepository
    {
        private readonly IConfiguration _configuration;
        private readonly string _connStr;
        public WOTaskRepository(IConfiguration configuration)
        {

            _configuration = configuration;
            _connStr = _configuration.GetConnectionString("IDYLConnection");
        }
        public Result Insert(List<WOTask> wOTasks, User user)
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
                        foreach (var task in wOTasks)
                        {
                            parameters = new DynamicParameters();
                            if (task.WODNo == 0)
                            {
                                //parameters.Add("@WOResourceNo", wOResource.WOResourceNo, DbType.Int32, ParameterDirection.Output);
                                //parameters.Add("@DocDate", wOResource.DocDate);
                                //parameters.Add("@ResTypeCode", wOResource.ResTypeCode);
                                //parameters.Add("@ResTypeFull", wOResource.ResTypeCode);
                                //parameters.Add("@RescNo", wOResource.RescNo);
                                //parameters.Add("@RescName", wOResource.RescName);
                                //parameters.Add("@PlnQtyMH", wOResource.PlnQtyMH);
                                //parameters.Add("@QtyMH", wOResource.QtyMH);
                                //parameters.Add("@UnitCost", wOResource.UnitCost);
                                //parameters.Add("@Amount", wOResource.Amount);
                                //parameters.Add("@DocNo", wOResource.DocNo);
                                //parameters.Add("@CompanyNo", wOResource.CompanyNo);
                                //parameters.Add("@CreatedBy", user.CustomerNo);
                                //parameters.Add("@WONo", wOResource.WONo);
                                //parameters.Add("@EQNo", wOResource.EQNo);
                                //parameters.Add("@IsExternal", wOResource.IsExternal);
                                //parameters.Add("@WODNo", wOResource.WODNo);
                                //parameters.Add("@RescTypeReal", wOResource.ResTypeCode);
                                //parameters.Add("@WarrantyDate", wOResource.WarrantyDate);
                                //parameters.Add("@VendorNo", wOResource.VendorNo);
                                //parameters.Add("@Type", wOResource.Type);
                                //parameters.Add("@IsAddByTask", 0);
                                //parameters.Add("@MHType", wOResource.MHType);
                                //parameters.Add("@CraftTypeNo", wOResource.CraftTypeNo);
                                //parameters.Add("@Qty", wOResource.Qty);
                                //parameters.Add("@MH", wOResource.MH);

                                //conn.Query<int>("sp_WOResource_Insert", parameters, commandType: StoredProcedure, transaction: trans);
                                //wOResource.WOResourceNo = parameters.Get<int>("@WOResourceNo");
                            }
                            else
                            {

                                parameters = new DynamicParameters();
                                parameters.Add("@WODNo", task.WODNo);
                                parameters.Add("@EQNo", task.EQNo);
                                parameters.Add("@ComponentNo", task.ComponentNo);
                                parameters.Add("@Component", task.Component);
                                parameters.Add("@ActNo", task.ActNo);
                                parameters.Add("@Action", task.Action);
                                parameters.Add("@Result", task.Result);
                                parameters.Add("@UpdatedBy", user.CustomerNo);
                                parameters.Add("@IsFinish", task.IsFinish);
                                parameters.Add("@IsAbNormal", task.IsAbNormal);
                                parameters.Add("@OPNo", task.OPNo);
                                parameters.Add("@Type", task.Type);
                                parameters.Add("@UnitCost", task.UnitCost);
                                parameters.Add("@Amount", task.Amount);
                                parameters.Add("@IsExternal", task.IsExternal);
                                parameters.Add("@Qty", task.Qty);
                                parameters.Add("@CraftTypeNo", task.CraftTypeNo);
                                parameters.Add("@Description", task.Description);
                                parameters.Add("@MH", task.MH);
                                parameters.Add("@HeadCraftTypeNo", task.HeadCraftTypeNo);

                                conn.Execute("sp_WOD_Update", parameters, commandType: StoredProcedure, transaction: trans);

                            }
                        }
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

        public Result RetriveByWO(int woNo)
        {
            Result result = new Result();
            try
            {
                using (SqlConnection conn = new SqlConnection(_connStr))
                {
                    conn.Open();
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@WONo", woNo);
                    IEnumerable<WOTask> wOTask = conn.Query<WOTask>("sp_WOD_GetByWONo", parameters, commandType: StoredProcedure);

                    result.Data = wOTask;
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

        public Result UpdateAbnormal(WOTask task, User user)
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

                        parameters = new DynamicParameters();
                        parameters.Add("@WONo", task.WONo);
                        Models.WO.WO wOs = conn.QueryFirst<Models.WO.WO>("sp_WO_GetByNo", parameters, commandType: StoredProcedure, transaction: trans);

                        parameters = new DynamicParameters();
                        parameters.Add("@WODNo", task.WODNo);
                        parameters.Add("@EQNo", task.EQNo);
                        parameters.Add("@ComponentNo", task.ComponentNo);
                        parameters.Add("@Component", task.Component);
                        parameters.Add("@ActNo", task.ActNo);
                        parameters.Add("@Action", task.Action);
                        parameters.Add("@Result", task.IsGenWO ? $"{task.Result} {wOs.WOCode}" : task.Result);
                        parameters.Add("@UpdatedBy", user.CustomerNo);
                        parameters.Add("@IsFinish", task.IsFinish);
                        parameters.Add("@IsAbNormal", task.IsAbNormal);
                        parameters.Add("@OPNo", task.OPNo);
                        parameters.Add("@Type", task.Type);
                        parameters.Add("@UnitCost", task.UnitCost);
                        parameters.Add("@Amount", task.Amount);
                        parameters.Add("@IsExternal", task.IsExternal);
                        parameters.Add("@Qty", task.Qty);
                        parameters.Add("@CraftTypeNo", task.CraftTypeNo);
                        parameters.Add("@Description", task.Description);
                        parameters.Add("@MH", task.MH);
                        parameters.Add("@HeadCraftTypeNo", task.HeadCraftTypeNo);
                        conn.Execute("sp_WOD_Update", parameters, commandType: StoredProcedure, transaction: trans);

                        DateTime sysDateTime = DateTime.Now;

                        if (task.IsGenWO)
                        {
                            parameters = new DynamicParameters();
                            parameters.Add("@WONo", wOs.WONo, DbType.Int32, ParameterDirection.Output);
                            parameters.Add("@WOStatusNo", 2);
                            parameters.Add("@WODate", sysDateTime);
                            parameters.Add("@WRDate", sysDateTime);
                            parameters.Add("@EQNo", wOs.EQNo);
                            parameters.Add("@LocationNo", wOs.LocationNo);
                            parameters.Add("@WorkDesc", task.Result);
                            parameters.Add("@SectReq", user.SectionNo);
                            parameters.Add("@Requester", user.CustomerNo);
                            parameters.Add("@CompanyNo", wOs.CompanyNo);
                            parameters.Add("@CreatedBy", user.CustomerNo);
                            parameters.Add("@SectionNo", wOs.SectionNo);
                            parameters.Add("@CraftNo", user.CustomerNo);
                            parameters.Add("@WOTypeNo", 2);
                            parameters.Add("@ProblemTypeNo", null);
                            parameters.Add("@ReqDate", null);

                            conn.Query<int>("sp_WO_Insert", parameters, commandType: StoredProcedure, transaction: trans);
                            wOs.WONo = parameters.Get<int>("@WONo");

                            string cmd = $" update wod set GenWONo ={wOs.WONo} where wodno = {task.WODNo} ";
                            conn.Execute(cmd, commandType: Text, transaction: trans);

                            parameters = new DynamicParameters();
                            parameters.Add("@WONo", wOs.WONo);
                            Models.WO.WO wONew = conn.QueryFirst<Models.WO.WO>("sp_WO_GetByNo", parameters, commandType: StoredProcedure, transaction: trans);

                            cmd = $" update wo set WRCode = '{wOs.WRCode + "," + wONew.WONo}' where wono = {task.WONo}";
                            conn.Execute(cmd, commandType: Text, transaction: trans);
                        }

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
    }
}
