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
namespace IdylAPI.Services.Repository.FM
{
    public class FMRepository
    {
        public Result Update(Models.WO.FM obj, Models.Authorize.User user, SqlConnection conn, SqlTransaction trans)
        {
            Result result = new Result();

            try
            {
                DynamicParameters parameters = new DynamicParameters();
                if (obj.WONo == 0)
                {
                    parameters.Add("@FMNo", obj.FMNo, DbType.Int32, ParameterDirection.Output);
                    parameters.Add("@Date", obj.Date);
                    parameters.Add("@MeterRead", obj.MeterRead);
                    parameters.Add("@WOTypeNo", obj.WOTypeNo);
                    parameters.Add("@EQNo", obj.EQNo);
                    parameters.Add("@LocationNo", obj.LocationNo);
                    parameters.Add("@EQTypeNo", obj.EQTypeNo);
                    parameters.Add("@Manuf", obj.Manuf);
                    parameters.Add("@Model", obj.Model);
                    parameters.Add("@ProblemNo", obj.ProblemNo);
                    parameters.Add("@Problem", obj.Problem);
                    parameters.Add("@ComponentNo", obj.ComponentNo);
                    parameters.Add("@Component", obj.Component);
                    parameters.Add("@CauseNo", obj.CauseNo);
                    parameters.Add("@Cause", obj.Cause);
                    parameters.Add("@ActionNo", obj.ActionNo);
                    parameters.Add("@Action", obj.Action);
                    parameters.Add("@WONo", obj.WONo);
                    parameters.Add("@OPNo", obj.OPNo);
                    parameters.Add("@CompanyNo", obj.CompanyNo);
                    parameters.Add("@CreatedBy", user.CustomerNo);
                    conn.Query<int>("sp_FM_Insert", parameters, commandType: StoredProcedure, transaction: trans);
                    obj.FMNo = parameters.Get<int>("@FMNo");

                }
                else
                {
                    parameters = new DynamicParameters();
                    //parameters.Add("@WONo", obj.WONo);
                   
                    parameters.Add("@FMNo", obj.FMNo);
                    parameters.Add("@Date", obj.Date);
                    parameters.Add("@MeterRead", obj.MeterRead);
                    parameters.Add("@WOTypeNo", obj.WOTypeNo);
                    parameters.Add("@EQNo", obj.EQNo);
                    parameters.Add("@LocationNo", obj.LocationNo);
                    parameters.Add("@EQTypeNo", obj.EQTypeNo);
                    parameters.Add("@Manuf", obj.Manuf);
                    parameters.Add("@Model", obj.Model);
                    parameters.Add("@ProblemNo", obj.ProblemNo);
                    parameters.Add("@Problem", obj.Problem);
                    parameters.Add("@ComponentNo", obj.ComponentNo);
                    parameters.Add("@Component", obj.Component);
                    parameters.Add("@CauseNo", obj.CauseNo);
                    parameters.Add("@Cause", obj.Cause);
                    parameters.Add("@ActionNo", obj.ActionNo);
                    parameters.Add("@Action", obj.Action);
                    parameters.Add("@UpdatedBy", obj.Action);
                    parameters.Add("@CompanyNo", obj.CompanyNo);
                    conn.Execute("sp_FM_Update", parameters, commandType: StoredProcedure, transaction: trans);
                    
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
