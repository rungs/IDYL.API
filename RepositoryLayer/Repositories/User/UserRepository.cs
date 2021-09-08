using Dapper;
using IdylAPI.Models;
using IdylAPI.Models.Master;
using IdylAPI.Services.Interfaces.Master;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using static System.Data.CommandType;
namespace IdylAPI.Services.Repository.Master
{
    public class UserRepository : IUserRepository
    {
        private readonly IConfiguration _configuration;
        private readonly string _connStr;
        public UserRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _connStr = _configuration.GetConnectionString("IDYLConnection");
        }

        public Result RetriveAll()
        {
            Result result = new Result();
            try
            {
                using (SqlConnection conn = new SqlConnection(_connStr))
                {
                    conn.Open();

                    string cmd = " select *, Company.CompanyName_TH as companyname from _systUser inner join Company on _systUser.CompanyNo = Company.CompanyNo where isnull(_systUser.IsDelete, 0) = 0  ";
                    IEnumerable<SystUser> sites = SqlMapper.Query<SystUser>(conn, cmd, null, commandType: Text);
                    result.Data = sites;
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
