using Dapper;
using IdylAPI.Models;
using IdylAPI.Models.Master;
using IdylAPI.Services.Interfaces.Master;
using Microsoft.Extensions.Configuration;
using PAUtility;
using Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using static System.Data.CommandType;
namespace IdylAPI.Services.Repository.Master
{
    public class ProblemTypeRepository : BaseRepositoryV2<ProblemType>, IProblemTypeRepository
    {
        private readonly IConfiguration _configuration;
        private readonly string _connStr;
        private readonly AppDBContext _context;
        public ProblemTypeRepository(AppDBContext context, IConfiguration configuration) : base(context)
        {
            _configuration = configuration;
            _connStr = _configuration.GetConnectionString("IDYLConnection");
            _context = context;
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
                    string condition = $" where problemType.companyno = {whereParameter.SiteNo}";
                    condition += $" and problemType.isdelete = 0 ";
                    if (!string.IsNullOrEmpty(InputVal.ToString(whereParameter.Filter)))
                    {
                        condition += $" and(problemType.problemTypeCode like '%{whereParameter.Filter}%'";
                        condition += $" or problemType.problemTypeName like '%{whereParameter.Filter}%'";
                        condition += $" or section.sectionname like '%{whereParameter.Filter}%'";
                        condition += $" or section.sectioncode like '%{whereParameter.Filter}%')";
                    }

                    parameters.Add("@WhereSel", condition);
                    parameters.Add("@StartRow", whereParameter.StartRow);
                    parameters.Add("@EndRow", whereParameter.EndRow);

                    IEnumerable<ProblemType> problemTypes = SqlMapper.Query<ProblemType>(conn, "msp_ProblemType_Retrive", parameters, commandType: StoredProcedure);
                    result.Data = problemTypes;
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
