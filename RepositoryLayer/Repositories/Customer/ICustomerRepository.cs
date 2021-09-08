using Domain.Interfaces;
using IdylAPI.Models.Master;
using System.Collections.Generic;

namespace IdylAPI.Services.Interfaces.Master
{
    public interface ICustomerRepository : IRepository<Customer>
    {
        IEnumerable<Customer> GetByUser(int userNo);
        IEnumerable<Customer> GetAllExclude(int userNo);
        Customer GetCustomerByUserCompany(int userNo, int companyNo);
        void Register(Customer customer);
    }
}
