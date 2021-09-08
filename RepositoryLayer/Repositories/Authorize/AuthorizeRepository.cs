using Dapper;
using IdylAPI.Helper;
using IdylAPI.Models;
using IdylAPI.Models.Authorize;
using IdylAPI.Models.Syst;
using IdylAPI.Services.Interfaces.Authorize;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PAUtility;
using Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using static System.Data.CommandType;

namespace IdylAPI.Services.Repository.Authorize
{
    public class AuthorizeRepository : IAuthorizeRepository
    {
        private readonly IConfiguration _configuration;
        private readonly string _connStr;
        private readonly IHostingEnvironment _host;
        private readonly AppDBContext _context;
        public AuthorizeRepository(IConfiguration configuration, IHostingEnvironment host, AppDBContext context)
        {
            _host = host;
            _configuration = configuration;
            _connStr = _configuration.GetConnectionString("IDYLConnection");
            _context = context;
        }

        public string CreateToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim("UserNo", user.UserNo.ToString()),
                new Claim("CustomerNo", user.CustomerNo.ToString()),
                new Claim("CustomerName", user.CustomerName),
                new Claim("Username", user.Username),
                new Claim("SectionNo", InputVal.ToString(user.SectionNo)),
                new Claim("UserGroupId", InputVal.ToString(user.UserGroupId)),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(Convert.ToDouble(_configuration["JwtExpireDays"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JwtIssuer"],
                audience: _configuration["JwtAudience"],
                claims: claims,
                expires: expires,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public Result Login(string username, string password)
        {
            var loginResponse = new LoginResponse { };
            Result result = new Result();
            try
            {
                using (SqlConnection conn = new SqlConnection(_connStr))
                {
                    conn.Open();
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@Email", username);
                    parameters.Add("@Password", password);
                    User user = SqlMapper.QueryFirstOrDefault<User>(conn, "sys_systUser_Login", parameters, commandType: StoredProcedure);

                    string token = CreateToken(user);
                    loginResponse.Token = token;
                    loginResponse.UserLogin = user.CustomerName;
                    loginResponse.SectionNo = user.SectionNo.HasValue ? user.SectionNo.Value: 0;
                    loginResponse.SuperUser = user.IsSuperUser;
                    loginResponse.UserGroupCode = user.UserGroupCode;
                    loginResponse.ServerAddress = new PlatformHelper(_configuration).GetServerAddress(user.Platform, user.CustomerType);

                    string sql = "sys_systPermission_GetByUserNo";
                    parameters = new DynamicParameters();
                    parameters.Add("@UserNo", user.UserNo);
                    loginResponse.Permission = SqlMapper.Query<FormPermission>(conn, sql, parameters, commandType: StoredProcedure);

                    sql = "sys_systPermissionData_Retrive";
                    parameters = new DynamicParameters();
                    parameters.Add("@UserNo", user.UserNo);
                    loginResponse.PermissionData = SqlMapper.Query<FormPermissionData>(conn, sql, parameters, commandType: StoredProcedure);

                    sql = "sys_systPermissionAction_Retrive";
                    parameters = new DynamicParameters();
                    parameters.Add("@UserNo", user.UserNo);
                    loginResponse.PermissionAction = SqlMapper.Query<FormPermissionAction>(conn, sql, parameters, commandType: StoredProcedure);

                    string pPath = _host.ContentRootPath;
                    int pathLevel = InputVal.ToInt(_configuration["PathLevel"]);
                    for (int j = 0; j <= pathLevel; j++)
                    {
                        pPath = Directory.GetParent(pPath).ToString();
                    }

                    loginResponse.UserGroupPermissions = _context.UserGroupPermission.Where(s => s.UserGroupNo == user.UserGroupId)
                       .Include(i => i.UserGroup)
                       .Include(i => i.Form)
                       .Include(i => i.Form.Menu)
                       .ToList().OrderBy(i => i.Form.Menu.OrderNo).ToList();

                    string companyname = user.CompanyName_EN.Replace(" ", "").Trim();
                    pPath += $"{_configuration["AttPath"]}\\[ProductivityAssociatesCo.,Ltd]\\lang\\language.json";
                    //
                    //pPath += $"{_configuration["AttPath"]}\\[{companyname}]\\lang\\language.json";
                    loginResponse.Lang = File.ReadAllText(pPath);

                    loginResponse.UserGroupDefaultInfo.Section = _context.Section.Where(t => t.SectionNo == user.SectionNo).FirstOrDefault();
                    
                    //if ((int)UserGroupEnum.UserGroup.Free == user.UserGroupId)
                    //{
                    //    Site site = _context.Site.Where(t=>t.ProductKey == "FREE").FirstOrDefault();
                    //    loginResponse.UserGroupDefaultInfo.Location = _context.Location.Where(t => t.LocationCode == user.UserFixed).FirstOrDefault();
                    //    loginResponse.UserGroupDefaultInfo.ProblemType = _context.ProblemType.Where(t => t.CompanyNo == site.CompanyNo).Include(t => t.Section).FirstOrDefault();
                    //}

                    loginResponse.CanRegister = InputVal.ToString(_configuration["CanRegister"]);
                    loginResponse.CanReset = InputVal.ToString(_configuration["CanReset"]);

                    result.Data = loginResponse;
                    result.StatusCode = 200;
                }
            }
            catch (Exception ex)
            {
                result.Data = ex.Message;
                result.StatusCode = 500;
                result.ErrMsg = "อีเมล์/รหัสเข้าใช้งานโปรแกรม หรือรหัสผ่านไม่ถูกต้อง";
            }
            return result;
        }

        public Result Logout(int notifyTokenNo)
        {
            var loginResponse = new LoginResponse { };
            Result result = new Result();
            try
            {
                using (SqlConnection conn = new SqlConnection(_connStr))
                {
                    conn.Open();
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@NotifyNo", notifyTokenNo);
                    SqlMapper.Execute(conn, "msp_Logout", parameters, commandType: StoredProcedure);

                    result.StatusCode = 200;
                }
            }
            catch (Exception)
            {
                result.StatusCode = 500;

            }
            return result;
        }

        public Result RetriveUserInfo(User userObj, int siteNo)
        {
            var loginResponse = new LoginResponse { };
            Result result = new Result();
            try
            {
                using (SqlConnection conn = new SqlConnection(_connStr))
                {
                    conn.Open();
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@UserNo", userObj.UserNo);
                    parameters.Add("@CompanyNo", siteNo);
                    User user = SqlMapper.QueryFirstOrDefault<User>(conn, "sp_Customer_GetByUserCompany", parameters, commandType: StoredProcedure);
                  
                    string token = CreateToken(user);
                    user.Token = token;

                    result.Data = user;
                    result.StatusCode = 200;
                }
            }
            catch (Exception)
            {
                result.StatusCode = 500;
                result.ErrMsg = "อีเมล์/รหัสเข้าใช้งานโปรแกรม หรือรหัสผ่านไม่ถูกต้อง";
            }
            return result;
        }

        public Result RefreshToken(string token)
        {
            Result result = new Result();
            try
            {
                using (SqlConnection conn = new SqlConnection(_connStr))
                {
                    conn.Open();
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@Token", token);
                    SqlMapper.Execute(conn, "msp_NotifyToken_Refresh", parameters, commandType: StoredProcedure);

                    result.StatusCode = 200;
                }
            }
            catch (Exception)
            {
                result.StatusCode = 500;

            }
            return result;
        }

        public Result Reset(User user)
        {
            Result result = new Result();
            try
            {
                using (SqlConnection conn = new SqlConnection(_connStr))
                {
                    conn.Open();
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("@UserNo", user.UserNo);
                    
                    SqlMapper.Execute(conn, "sp_ResetDataByUser", parameters, commandType: StoredProcedure);

                    result.Data = user;
                    result.ErrMsg = "Reset ข้อมูลเรียบร้อย";
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
