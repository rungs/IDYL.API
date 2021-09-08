using Dapper;
using IdylAPI.Models;
using IdylAPI.Models.Authorize;
using IdylAPI.Models.Syst;
using IdylAPI.Services.Interfaces.Authorize;
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
namespace IdylAPI.Services.Repository.Authorize
{
    public class FormVideoRepository : IFormVideoRepository
    {
        private readonly IConfiguration _configuration;
        private readonly string _connStr;
        public FormVideoRepository(IConfiguration configuration)
        {

            _configuration = configuration;
            _connStr = _configuration.GetConnectionString("IDYLConnection");
        }

        public Result RetriveByFormId(string formId)
        {
            Result result = new Result();
            try
            {
                using (SqlConnection conn = new SqlConnection(_connStr))
                {
                    conn.Open();
                    string cmd = $" select * from _systForms_Video where formid = '{formId}'";
                    FormVideo formVideo  = conn.QueryFirstOrDefault<FormVideo>(cmd, null, commandType: Text);

                    result.Data = formVideo;
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
                    string cmd = $" select * from _systForms_Video ";
                    IEnumerable<FormVideo> formVideo = conn.Query<FormVideo>(cmd, null, commandType: Text);

                    result.Data = formVideo;
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

        public Result Insert(FormVideo formVideo)
        {
            Result result = new Result();
            try
            {
                using (SqlConnection conn = new SqlConnection(_connStr))
                {
                    conn.Open();
                    string cmd = $" insert into _systForms_Video (formid, linkvdo) values('{formVideo.FormId}', '{formVideo.LinkVdo}') ";
                    SqlMapper.Execute(conn, cmd, null, commandType: Text);
                    
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
        public Result Update(string formId, FormVideo formVideo)
        {
            Result result = new Result();
            try
            {
                using (SqlConnection conn = new SqlConnection(_connStr))
                {
                    conn.Open();
                    string cmd = $" update _systForms_Video set linkvdo ='{formVideo.LinkVdo}' where formId = '{formVideo.FormId}' ";
                    SqlMapper.Execute(conn, cmd, null, commandType: Text);

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
        public Result Delete(string formId)
        {
            Result result = new Result();
            try
            {
                using (SqlConnection conn = new SqlConnection(_connStr))
                {
                    conn.Open();
                    string cmd = $" delete from _systForms_Video where formid = '{formId}' ";
                    SqlMapper.Execute(conn, cmd, null, commandType: Text);

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
