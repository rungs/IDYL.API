using Domain.Interfaces;
using IdylAPI.Models.Syst;
using System.Threading.Tasks;

namespace IdylAPI.Services.Interfaces.Syst
{
    public interface ISystUserCustomerRepository : IRepository<SystUserCustomer>
    {
        public void Delete(int CustomerNo, int UserNo);
        SystUserCustomer GetByUserCustomer(int userNo, int customerNo);
    }
}
