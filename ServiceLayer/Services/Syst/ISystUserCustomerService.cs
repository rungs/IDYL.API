using IdylAPI.Models;
using IdylAPI.Models.Authorize;
using IdylAPI.Models.Master;
using IdylAPI.Models.Syst;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IdylAPI.Services.Interfaces.Syst
{
    public interface ISystUserCustomerService
    {
        Task Insert(SystUserCustomer systUserCustomer);
        Task Delete(int userNo, int customerNo);
        Task<SystUserCustomer> GetByUserCustomer(int userNo, int customerNo);
    }
}
