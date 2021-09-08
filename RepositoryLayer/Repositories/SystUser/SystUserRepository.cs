using IdylAPI.Models.Syst;
using IdylAPI.Services.Interfaces.Syst;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using Persistence.Contexts;
using Domain.Interfaces;
using System;
using System.Threading.Tasks;
using static System.Data.CommandType;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Dapper;

namespace IdylAPI.Services.Repository.Company
{
    public class SystUserRepository : BaseRepositoryV2<SystUser>, ISystUserRepository
    {
        protected readonly AppDBContext _context;
        private readonly IConfiguration _configuration;
        private readonly string _connStr;

        protected IApplicationWriteDbConnection _writeDbConnection { get; }
        public SystUserRepository(IConfiguration configuration, AppDBContext context) : base(context)
        {
            _context = context;
            _configuration = configuration;
            _connStr = _configuration.GetConnectionString("IDYLConnection");
        }

        public IEnumerable<SystUser> GetByCompany(int companyNo)
        {
            return _entities.Where(x => x.CompanyNo == companyNo).Include(i => i.UserGroup).AsEnumerable();
        }

        public IEnumerable<SystUser> GetSuperUserByCompany(int companyNo)
        {
            var siteObj = from user in _context.SystUser
                          join site in _context.Site on user.CompanyNo equals site.CompanyNo
                          where site.CompanyNo == companyNo && user.IsSuperUser == true && user.IsDelete == false
                          select user;

            return siteObj;
        }

        public IEnumerable<SystUser> GetUserByProductKey(string productKey)
        {
            var siteObj = from user in _context.SystUser
                          join site in _context.Site on user.CompanyNo equals site.CompanyNo
                          where site.ProductKey == productKey && user.IsDelete == false && site.IsDelete == false
                          select user;

            IEnumerable<SystUser> systUsers = siteObj.Include(t => t.UserGroup);
            int cnt = 1;
            foreach (SystUser item in systUsers)
            {
                item.No = cnt;
                cnt++;
            }
            return systUsers;
        }

        public SystUser GetByUserId(int userId, int companyNo)
        {
            var customers = from user in _context.SystUser
                            join user_customer in _context.SystUserCustomer on user.UserNo equals user_customer.UserNo
                            join customer in _context.Customer on user_customer.CustomerNo equals customer.CustomerNo
                            join usergroup in _context.UserGroup on user.UserGroupId equals usergroup.UserGroupNo
                            where customer.CompanyNo == companyNo && customer.IsDelete == false && user.UserNo == userId
                            select new SystUser
                            {
                                UserNo = user.UserNo,
                                Username = user.Username,
                                ExpiredDate = user.ExpiredDate,
                                ActivateDate = user.ActivateDate,
                                UserGroup = usergroup,
                                Customer = customer,
                                IsActivate = user.IsActivate
                            };


            return customers.FirstOrDefault();
        }

        public IEnumerable<SystUser> CheckUsernameDupicate(string username)
        {
            return _entities.Where(t => t.Username == username);
        }

        public IEnumerable<Models.Master.Customer> CheckEmailDupicate(string email)
        {
            var customers = from customer in _context.Customer
                            where customer.Email == email && customer.IsDelete == false
                            select customer;
            return customers;
        }

        public async Task UpdateUserActiveSite(int CompanyNo, bool IsActive, int UserNo)
        {
            using (SqlConnection conn = new SqlConnection(_connStr))
            {
                conn.Open();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@CompanyNo", CompanyNo);
                parameters.Add("@UserNo", UserNo);
                parameters.Add("@IsActive", IsActive);
                await SqlMapper.ExecuteAsync(conn, "sp_Customer_SubSiteUpdateActive", parameters, commandType: StoredProcedure);
            }
        }

        public IEnumerable<UserView> GetUserView()
        {
            var userlist = from user in _context.SystUser
                           join user_customer in _context.SystUserCustomer on user.UserNo equals user_customer.UserNo
                           join customer in _context.Customer on user_customer.CustomerNo equals customer.CustomerNo
                           join site in _context.Site on customer.CompanyNo equals site.CompanyNo
                           where user.IsDelete == false && site.IsDelete == false
                           select new UserView
                           {
                               Username = user.Username,
                               SubsiteCode = site.SubsiteCode,
                               SubsiteName = site.SubsiteName,
                               CreatedDate = site.CreatedDate,
                               CompanyName = site.CompanyName_TH
                           };

            return userlist;
        }

        public ActivateUser GetUserCustomerById(int userId, int companyNo)
        {
            var customers = from user in _context.SystUser
                            join user_customer in _context.SystUserCustomer on user.UserNo equals user_customer.UserNo
                            join customer in _context.Customer on user_customer.CustomerNo equals customer.CustomerNo
                            join usergroup in _context.UserGroup on user.UserGroupId equals usergroup.UserGroupNo
                            where customer.IsDelete == false && user.UserNo == userId && customer.CustomerCode != "SYS"
                            select new ActivateUser
                            {
                                UserNo = user.UserNo,
                                Username = user.Username,
                                ExpiredDate = user.ExpiredDate,
                                ActivateDate = user.ActivateDate,
                                IsActivate = user.IsActivate,
                                CompanyNo = customer.CompanyNo,
                                FirstName = customer.Firstname,
                                LastName = customer.Lastname,
                                DaysRemaining = user.ExpiredDate.HasValue ? user.ExpiredDate.Value.Subtract(DateTime.Now).Days : null,
                                IsMaintainance = customer.IsMaintainance,
                                SectionNo = customer.SectionNo,
                                CraftTypeNo = customer.CraftTypeNo,
                                IsHeadCraft = customer.IsHeadCraft,
                                IsHeadSection = customer.IsHeadSection,
                                Mobile = customer.Mobile,
                                Email = customer.Email,
                                UserGroupName = usergroup.UserGroupName
                            };

            return customers.FirstOrDefault();
        }
    }
}
