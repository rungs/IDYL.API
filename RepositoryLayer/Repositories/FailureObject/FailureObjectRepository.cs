using Dapper;
using IdylAPI.Models;
using IdylAPI.Models.Master;
using IdylAPI.Services.Interfaces.Master;
using Microsoft.Extensions.Configuration;
using PAUtility;
using Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using static System.Data.CommandType;
namespace IdylAPI.Services.Repository.Master
{
    public class FailureObjectRepository : BaseRepositoryV2<FailureObject>, IFailureObjectRepository
    {
        protected readonly AppDBContext _context;
        private readonly IConfiguration _configuration;
        private readonly string _connStr;

        public FailureObjectRepository(IConfiguration configuration, AppDBContext context) : base(context)
        {
            _context = context;
            _configuration = configuration;
            _connStr = _configuration.GetConnectionString("IDYLConnection");
        }

        //private readonly IConfiguration _configuration;
        //private readonly string _connStr;
        //public FailureObjectRepository(IConfiguration configuration)
        //{

        //    _configuration = configuration;
        //    _connStr = _configuration.GetConnectionString("IDYLConnection");
        //}

        public Result Insert(FailureObject failureObject, Models.Authorize.User user)
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

                        if (failureObject.ObjectNo == 0)
                        {
                            parameters.Add("@ObjectNo", failureObject.ObjectNo, DbType.Int32, ParameterDirection.Output);
                            parameters.Add("@ObjectType", failureObject.ObjectType);
                            parameters.Add("@ObjectCode", failureObject.ObjectCode);
                            parameters.Add("@ObjectName", failureObject.ObjectName);
                            parameters.Add("@CreatedBy", user.CustomerNo);
                            parameters.Add("@CompanyNo", failureObject.CompanyNo);
                          

                            conn.Query<int>("sp_FailureObject_Insert", parameters, commandType: StoredProcedure, transaction: trans);
                            failureObject.ObjectNo = parameters.Get<int>("@ObjectNo");

                            parameters = new DynamicParameters();
                            parameters.Add("@EQTypeNo", failureObject.EQTypeNo);
                            parameters.Add("@ObjectNo", failureObject.ObjectNo);
                            parameters.Add("@ObjectCode", failureObject.ObjectCode);
                            parameters.Add("@CreatedBy", user.CustomerNo);
                           
                            conn.Query<int>("sp_EQType_FailureObject_Insert", parameters, commandType: StoredProcedure, transaction: trans);

                        }
                        
                        trans.Commit();
                        result.Data = failureObject.ObjectNo;
                        result.ErrMsg = "บันทึกเรียบร้อย";
                        result.StatusCode = 200;
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        result.StatusCode = 500;
                        if (ex.Message == "duplicate")
                        {
                            result.ErrMsg = "ไม่สามารถบันทึกได้ เนื่องจากรหัสซ้ำ";
                        }
                        else
                        {
                            result.ErrMsg = ex.Message;
                        }
                    }
                }

                return result;
            }
        }

        public Task<int> InsertBulk(List<FailureObject> failureObject)
        {
            _context.FailureObject.AddRange(failureObject);
            return _context.SaveChangesAsync();
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
                    string condition = $" where me.companyno = {whereParameter.SiteNo}";
                    condition += $" and me.isdelete = 0 ";
                    condition += $" and me.ObjectType ='{whereParameter.Type}' ";
                    if (!string.IsNullOrEmpty(InputVal.ToString(whereParameter.Filter)))
                    {
                        condition += $" and(me.Objectname like '%{whereParameter.Filter}%'";
                        condition += $" or me.Objectcode like '%{whereParameter.Filter}%')";
                    }

                    parameters.Add("@WhereSel", condition);
                    parameters.Add("@StartRow", whereParameter.StartRow);
                    parameters.Add("@EndRow", whereParameter.EndRow);
                    parameters.Add("@EQTypeNo", whereParameter.EQTypeNo.HasValue ? whereParameter.EQTypeNo.Value : 0);

                    IEnumerable<FailureObject> eQs = SqlMapper.Query<FailureObject>(conn, "msp_FailureObject_Retrive", parameters, commandType: StoredProcedure);
                    result.Data = eQs;
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
