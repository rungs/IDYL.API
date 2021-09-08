using IdylAPI.Models;
using IdylAPI.Models.Syst;
using IdylAPI.Services.Interfaces.Authorize;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Persistence.Contexts;
using System;
using System.Linq;

namespace IdylAPI.Services.Repository.Authorize
{
    public class UserGroupPermissionRepository : IUserGroupPermissionRepository
    {
        private readonly IConfiguration _configuration;
        private readonly string _connStr;
        private readonly AppDBContext _context;
        public UserGroupPermissionRepository(IConfiguration configuration, AppDBContext context)
        {
            _configuration = configuration;
            _connStr = _configuration.GetConnectionString("IDYLConnection");
            _context = context;
        }

        public Result GetByUserGroup(int userGroupNo)
        {
            Result result = new Result();
            result.Data =  _context.UserGroupPermission
                
                .Where(s => s.UserGroupNo == userGroupNo)
                .Include(i => i.UserGroup)
                .Include(i => i.Form)
                .Include(i => i.Form.Menu)
                .ToList().OrderBy(i=>i.Form.Menu.OrderNo);
            return result;
        }

        public Result GetUserGroupAll()
        {
            Result result = new Result();
            result.Data = _context.UserGroup.ToList();
            return result;
        }
   
        public Result UpdateUserGroupPermission(UserGroupPermission userGroupPermission)
        {
            Result result = new Result();
            try
            {
                var res = _context.UserGroupPermission.SingleOrDefault(b => b.UserGroupNo == userGroupPermission.UserGroupNo && b.FormId == userGroupPermission.FormId);
                if (res != null)
                {
                    res.IsView = userGroupPermission.IsView;
                    res.IsEdit = userGroupPermission.IsEdit;
                    res.IsNew = userGroupPermission.IsNew;
                    res.IsDelete = userGroupPermission.IsDelete;
                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                result.ErrMsg = ex.Message;
                result.StatusCode = 500;
            }
            return result;
        }

        //public Result RetriveByFormId(string formId)
        //{
        //    Result result = new Result();
        //    try
        //    {
        //        using (SqlConnection conn = new SqlConnection(_connStr))
        //        {
        //            conn.Open();
        //            string cmd = $" select * from _systForms_Video where formid = '{formId}'";
        //            FormVideo formVideo  = conn.QueryFirstOrDefault<FormVideo>(cmd, null, commandType: Text);

        //            result.Data = formVideo;
        //            result.StatusCode = 200;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        result.StatusCode = 500;
        //        result.ErrMsg = ex.Message;
        //    }
        //    return result;
        //}

        //public Result RetriveAll()
        //{
        //    Result result = new Result();
        //    try
        //    {
        //        using (SqlConnection conn = new SqlConnection(_connStr))
        //        {
        //            conn.Open();
        //            string cmd = $" select * from _systForms_Video ";
        //            IEnumerable<FormVideo> formVideo = conn.Query<FormVideo>(cmd, null, commandType: Text);

        //            result.Data = formVideo;
        //            result.StatusCode = 200;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        result.StatusCode = 500;
        //        result.ErrMsg = ex.Message;
        //    }
        //    return result;
        //}

        //public Result Insert(FormVideo formVideo)
        //{
        //    Result result = new Result();
        //    try
        //    {
        //        using (SqlConnection conn = new SqlConnection(_connStr))
        //        {
        //            conn.Open();
        //            string cmd = $" insert into _systForms_Video (formid, linkvdo) values('{formVideo.FormId}', '{formVideo.LinkVdo}') ";
        //            SqlMapper.Execute(conn, cmd, null, commandType: Text);

        //            result.StatusCode = 200;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        result.StatusCode = 500;
        //        result.ErrMsg = ex.Message;
        //    }
        //    return result;
        //}
        //public Result Update(string formId, FormVideo formVideo)
        //{
        //    Result result = new Result();
        //    try
        //    {
        //        using (SqlConnection conn = new SqlConnection(_connStr))
        //        {
        //            conn.Open();
        //            string cmd = $" update _systForms_Video set linkvdo ='{formVideo.LinkVdo}' where formId = '{formVideo.FormId}' ";
        //            SqlMapper.Execute(conn, cmd, null, commandType: Text);

        //            result.StatusCode = 200;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        result.StatusCode = 500;
        //        result.ErrMsg = ex.Message;
        //    }
        //    return result;
        //}
        //public Result Delete(string formId)
        //{
        //    Result result = new Result();
        //    try
        //    {
        //        using (SqlConnection conn = new SqlConnection(_connStr))
        //        {
        //            conn.Open();
        //            string cmd = $" delete from _systForms_Video where formid = '{formId}' ";
        //            SqlMapper.Execute(conn, cmd, null, commandType: Text);

        //            result.StatusCode = 200;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        result.StatusCode = 500;
        //        result.ErrMsg = ex.Message;
        //    }
        //    return result;
        //}
    }
}
