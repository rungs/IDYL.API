using Dapper;
using IdylAPI.Models;
using IdylAPI.Services.Interfaces.Master;
using Microsoft.Extensions.Configuration;
using PAUtility;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using static System.Data.CommandType;
namespace IdylAPI.Services.Repository.Master
{
    public class SystemRepository : ISystemRepository
    {
        private readonly IConfiguration _configuration;
        private readonly string _connStr;
        public SystemRepository(IConfiguration configuration)
        {

            _configuration = configuration;
            _connStr = _configuration.GetConnectionString("IDYLConnection");
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
                    string condition = $" where system.companyno = {whereParameter.SiteNo}";
                    condition += $" and system.isdelete = 0 ";
                    if (!string.IsNullOrEmpty(InputVal.ToString(whereParameter.Filter)))
                    {
                        condition += $" and(system.systemCode like '%{whereParameter.Filter}%'";
                        condition += $" or system.systemName like '%{whereParameter.Filter}%')";
                    }

                    parameters.Add("@WhereSel", condition);
                    parameters.Add("@StartRow", whereParameter.StartRow);
                    parameters.Add("@EndRow", whereParameter.EndRow);

                    IEnumerable<IdylAPI.Models.Master.System> obj = SqlMapper.Query<IdylAPI.Models.Master.System>(conn, "msp_System_Retrive", parameters, commandType: StoredProcedure);
                    result.Data = obj;
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
