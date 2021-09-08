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
using System.Linq;
using static System.Data.CommandType;
namespace IdylAPI.Services.Repository.Master
{
    public class SectionRepository : BaseRepositoryV2<Section>, ISectionRepository
    {
        private readonly IConfiguration _configuration;
        private readonly string _connStr;
        private readonly AppDBContext _context;
        public SectionRepository(AppDBContext context) : base(context)
        {
            _connStr = _configuration.GetConnectionString("IDYLConnection");
            _context = context;
        }

        public IEnumerable<Section> GetByCompany(int companyNo)
        {
            return _entities.Where(x => x.CompanyNo == companyNo && x.IsDelete == false).AsEnumerable();
        }

        public Section GetSectionByCode(string code, int companyNo)
        {
            return _entities.Where(x => x.CompanyNo == companyNo && x.IsDelete == false && x.SectionCode == code).FirstOrDefault();
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
                    if (!string.IsNullOrEmpty(InputVal.ToString(whereParameter.Filter)))
                    {
                        condition += $" and(me.sectionname like '%{whereParameter.Filter}%'";
                        condition += $" or me.sectioncode like '%{whereParameter.Filter}%')";
                    }

                    parameters.Add("@WhereSel", condition);
                    parameters.Add("@StartRow", whereParameter.StartRow);
                    parameters.Add("@EndRow", whereParameter.EndRow);

                    IEnumerable<Section> eQs = SqlMapper.Query<Section>(conn, "msp_Section_Retrive", parameters, commandType: StoredProcedure);
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
