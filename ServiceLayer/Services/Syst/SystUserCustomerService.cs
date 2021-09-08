using Domain.Interfaces;
using IdylAPI.Models.Syst;
using IdylAPI.Services.Interfaces.Syst;
using System.Threading.Tasks;

namespace IdylAPI.Services.Master
{
    public class SystUserCustomerService : ISystUserCustomerService
    {
        private readonly IUnitOfWork _unitOfWork;

        public SystUserCustomerService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Delete(int userNo, int customerNo)
        {
            _unitOfWork.SystUserCustomerRepository.Delete(customerNo, userNo);
            await _unitOfWork.SaveChangesAsync();
        }

        public async  Task<SystUserCustomer> GetByUserCustomer(int userNo, int customerNo)
        {
            return  _unitOfWork.SystUserCustomerRepository.GetByUserCustomer(userNo, customerNo);
        }

        public async Task Insert(SystUserCustomer systUserCustomer )
        {
            await _unitOfWork.SystUserCustomerRepository.Add(systUserCustomer);
            await _unitOfWork.SaveChangesAsync();
        } 
    }
}
