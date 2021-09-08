using Dapper;
using IdylAPI.Models;
using IdylAPI.Models.Authorize;
using IdylAPI.Models.Dashboard;
using IdylAPI.Models.Syst;
using IdylAPI.Services.Interfaces.Authorize;
using IdylAPI.Services.Interfaces.Dashboard;
using IdylAPI.Services.Interfaces.WO;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PAUtility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using static System.Data.CommandType;
namespace IdylAPI.Services.Repository.Dashboard
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly IConfiguration _configuration;
        private readonly string _connStr;
        public DashboardRepository(IConfiguration configuration)
        {

            _configuration = configuration;
            _connStr = _configuration.GetConnectionString("IDYLConnection");
        }

        public Result RetriveBacklog(int siteNo, User user)
        {
            var loginResponse = new LoginResponse { };
            Result result = new Result();
            try
            {
                using (SqlConnection conn = new SqlConnection(_connStr))
                {
                    conn.Open();
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@CompanyNo", siteNo);
                    parameters.Add("@CustomerNo", user.CustomerNo);
                    parameters.Add("@UserNo", user.UserNo);
                    
                    IEnumerable<Backlog> obj = SqlMapper.Query<Backlog>(conn, "msp_Dashboard_Backlog", parameters, commandType: StoredProcedure);

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
