using IdylAPI.Models;
using IdylAPI.Models.Authorize;
using IdylAPI.Models.Master;
using IdylAPI.Models.Syst;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IdylAPI.Services.Interfaces.Syst
{
    public interface ICustomerService
    {
        IEnumerable<Customer> GetByUser(int userNo);
        IEnumerable<Customer> GetAllExclude(int userNo);
        Task<Result> Register(Customer customer);
    }
}
