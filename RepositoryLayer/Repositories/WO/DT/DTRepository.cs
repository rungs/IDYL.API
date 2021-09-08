using Dapper;
using IdylAPI.Models;

using Microsoft.Extensions.Configuration;
using PAUtility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using static System.Data.CommandType;
namespace IdylAPI.Services.Repository.DT
{
    public class DTRepository
    {
        //private readonly IConfiguration _configuration;
        //private readonly string _connStr;
        //public DTRepository(IConfiguration configuration)
        //{

        //    _configuration = configuration;
        //    _connStr = _configuration.GetConnectionString("IDYLConnection");
        //}

        public Result Update(Models.WO.DT dt, Models.Authorize.User user, SqlConnection conn, SqlTransaction trans)
        {
            Result result = new Result();

            try
            {
                DynamicParameters parameters = new DynamicParameters();
                if (dt.WONo == 0)
                {
                    parameters.Add("@DTNo", dt.DTNo, DbType.Int32, ParameterDirection.Output);
                    parameters.Add("@DTDate", dt.DTDate);
                    parameters.Add("@WONo", dt.WONo);
                    parameters.Add("@EQDown", true);
                    parameters.Add("@PlantDown", true);
                    parameters.Add("@DownTime", dt.DownTime);
                    parameters.Add("@EQNo", dt.EQNo);
                    parameters.Add("@LocationNo", dt.LocationNo);
                    parameters.Add("@CompanyNo", dt.CompanyNo);
                    parameters.Add("@CreatedBy", user.CustomerNo);
                    parameters.Add("@LossAmount", dt.LossAmount);
                    parameters.Add("@LossQty", dt.LossQty);
                    parameters.Add("@DateTimeStop", dt.DateTimeStop);
                    parameters.Add("@DateTimeStopEnd", dt.DateTimeStopEnd);
                    conn.Query<int>("sp_DownTime_Insert", parameters, commandType: StoredProcedure, transaction: trans);
                    dt.DTNo = parameters.Get<int>("@DTNo");

                }
                else
                {
                    parameters = new DynamicParameters();
                    parameters.Add("@WONo", dt.WONo);
                    Models.WO.DT dTOld =  conn.QueryFirst<Models.WO.DT>("sp_DownTime_GetByWONo", parameters, commandType: StoredProcedure, transaction: trans);

                    parameters.Add("@DTNo", dTOld.DTNo);
                    parameters.Add("@DTDate", dTOld.DTDate);
                    parameters.Add("@EQDown", true);
                    parameters.Add("@PlantDown", true);
                    parameters.Add("@DownTime", dTOld.DownTime);
                    parameters.Add("@EQNo", dt.EQNo);
                    parameters.Add("@LocationNo", dt.LocationNo);
                    parameters.Add("@UpdatedBy", user.CustomerNo);
                    parameters.Add("@LossAmount", dTOld.LossAmount);
                    parameters.Add("@LossQty", dTOld.LossQty);
                    parameters.Add("@DateTimeStop", dTOld.DateTimeStop);
                    parameters.Add("@DateTimeStopEnd", dTOld.DateTimeStopEnd);
                    conn.Execute("sp_DownTime_Update", parameters, commandType: StoredProcedure, transaction: trans);
                    
                }
            }
            catch (Exception ex)
            {
                trans.Rollback();
                result.StatusCode = 500;
                result.ErrMsg = ex.Message;
            }

            return result;
        }
    }
}
