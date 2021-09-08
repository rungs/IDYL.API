using Dapper;
using IdylAPI.Helper;
using IdylAPI.Models;
using IdylAPI.Models.Authorize;
using IdylAPI.Models.Master;
using IdylAPI.Services.Interfaces.Master;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using PAUtility;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using static System.Data.CommandType;
namespace IdylAPI.Services.Repository.Master
{
    public class SiteRepository : ISiteRepository
    {
        private readonly IConfiguration _configuration;
        private readonly string _connStr;
        private readonly IHostingEnvironment _host;
        public SiteRepository(IConfiguration configuration, IHostingEnvironment host)
        {
            _host = host;
            _configuration = configuration;
            _connStr = _configuration.GetConnectionString("IDYLConnection");
        }

        public Result Retrive(User user)
        {
            Result result = new Result();
            try
            {
                using (SqlConnection conn = new SqlConnection(_connStr))
                {
                    conn.Open();
                    
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@UserNo", user.UserNo);
                    IEnumerable<Site> sites = SqlMapper.Query<Site>(conn, "msp_Company_GetSubSiteActive", parameters, commandType: StoredProcedure);
                    string pPath = _host.ContentRootPath;
                    foreach (Site item in sites)
                    {
                        item.ServerAddress = new PlatformHelper(_configuration).GetServerAddress(item.Platform, item.CustomerType);
                        item.LogoWidth = InputVal.ToInt(_configuration["LogoWidth"]);
                        item.LogoHeight = InputVal.ToInt(_configuration["LogoHeight"]);
                        item.LogoTop = InputVal.ToInt(_configuration["LogoTop"]);
                        item.LogoLeft = InputVal.ToInt(_configuration["LogoLeft"]);
                    }

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

        public Result RetriveById(int id)
        {
            Result result = new Result();
            try
            {
                using (SqlConnection conn = new SqlConnection(_connStr))
                {
                    conn.Open();

                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@CompanyNo", id);
                    Site sites = SqlMapper.QueryFirst<Site>(conn, "sp_Company_GetByID", parameters, commandType: StoredProcedure);
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

        public Result RetriveAll()
        {
            Result result = new Result();
            try
            {
                using (SqlConnection conn = new SqlConnection(_connStr))
                {
                    conn.Open();

                    string cmd = " select *, CompanyName_TH as CompanyName,  from company where isnull(isdelete,0) = 0 ";
                    IEnumerable<Site> sites = SqlMapper.Query<Site>(conn, cmd, null, commandType: Text);
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
