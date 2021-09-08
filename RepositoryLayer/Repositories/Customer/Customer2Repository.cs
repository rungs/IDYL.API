using IdylAPI.Models.Master;
using IdylAPI.Services.Interfaces.Master;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;
using System.Collections.Generic;
using System.Linq;
namespace IdylAPI.Services.Repository.Syst
{
    public class CustomerRepository : BaseRepositoryV2<Customer>, ICustomerRepository
    {
        protected readonly AppDBContext _context;
        public CustomerRepository(AppDBContext context) : base(context) {
            _context = context;
        }

        public IEnumerable<Customer> GetAllExclude(int userNo)
        {
            var q = from d in _context.Customer
                    where d.IsDelete == false && !(from o in _context.SystUserCustomer
                                                   where o.UserNo == userNo
                                                   select o.CustomerNo).Contains(d.CustomerNo)
                    select d;

            return q.Include(i => i.Site).AsEnumerable();
        }

        public IEnumerable<Customer> GetByUser(int userNo)
        {
            var q = from d in _context.Customer
                    join dc in _context.SystUserCustomer on d.CustomerNo equals dc.CustomerNo
                    where d.IsDelete == false && dc.UserNo == userNo
                    select d;

            return q.Include(i => i.Site).AsEnumerable();
        }

        public Customer GetCustomerByUserCompany(int userNo, int companyNo)
        {
            var q = from d in _context.Customer
                    join dc in _context.SystUserCustomer on d.CustomerNo equals dc.CustomerNo
                    where d.IsDelete == false && dc.UserNo == userNo && d.CompanyNo == companyNo
                    select d;

            return q.FirstOrDefault();
        }

        public void Register(Customer customer)
        {
            Models.Syst.SystUser systUser = new Models.Syst.SystUser();

            systUser.UserGroupId = 5;
        }
    }
}
