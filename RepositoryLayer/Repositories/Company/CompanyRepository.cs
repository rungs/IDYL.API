using Dapper;
using Domain.Interfaces;
using IdylAPI.Models.Master;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Persistence.Contexts;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using static System.Data.CommandType;
namespace IdylAPI.Services.Repository.Company
{
    public class CompanyRepository : BaseRepositoryV2<Site>, ICompanyRepository
    {
        protected readonly AppDBContext _context;
        private readonly IConfiguration _configuration;
        private readonly string _connStr;

        public CompanyRepository(IConfiguration configuration, AppDBContext context) : base(context)
        {
            _context = context;
            _configuration = configuration;
            _connStr = _configuration.GetConnectionString("IDYLConnection");
        }

        public Site GetCompanyByProductKey(string productKey)
        {
            return _entities.Where(t => t.ProductKey == productKey && t.IsMainSite == true).FirstOrDefault();
        }

        public IEnumerable<Site> GetCompanyByUserId(int userId)
        {
            var siteObj = from user in _context.SystUser
                          join site in _context.Site on user.CompanyNo equals site.CompanyNo
                          where user.UserNo == userId
                          select site;

            var siteList = from s in _context.Site
                           where s.IsDelete == false && s.ProductKey == siteObj.First().ProductKey
                           select s;

            IEnumerable<Site> sites = siteList;
            for (int i = 0; i < sites.Count(); i++)
            {
                sites.ElementAtOrDefault(i).AttachFiles = _context.AttachFile.Where(t => t.CompanyNo == sites.ElementAtOrDefault(i).CompanyNo).ToList();

            }
            return sites.AsEnumerable();
        }

        public IEnumerable<Site> GetCompanyProductKeyUser(string productkey, int userid)
        {
            using (SqlConnection conn = new SqlConnection(_connStr))
            {
                conn.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@ProductKey", productkey);
                parameters.Add("@UserNo", userid);
                return SqlMapper.Query<Site>(conn, "sp_Company_GetByProductKey", parameters, commandType: StoredProcedure);
            }
        }
    }
}
