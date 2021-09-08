using IdylAPI.Models.Syst;
using IdylAPI.Services.Interfaces.Syst;
using Persistence.Contexts;
using System.Linq;
using System.Threading.Tasks;

namespace IdylAPI.Services.Repository.Syst
{
    public class SystUserCustomerRepository : BaseRepositoryV2<SystUserCustomer>, ISystUserCustomerRepository
    {
        public SystUserCustomerRepository(AppDBContext context) : base(context) { }

        public void Delete(int CustomerNo,  int UserNo)
        {
            SystUserCustomer systUserCustomer = _entities.Where(t => t.CustomerNo == CustomerNo && t.UserNo == UserNo).FirstOrDefault();
            _entities.Remove(systUserCustomer);
        }

        public SystUserCustomer GetByUserCustomer(int userNo, int customerNo)
        {
            return _entities.Where(t => t.CustomerNo == customerNo && t.UserNo == userNo).FirstOrDefault();
        }
    }
}
