using Domain.Entities.Syst;
using Domain.Interfaces;
using Domain.Interfaces.Services.Syst;
using System.Collections.Generic;

namespace IdylAPI.Services.Master
{
    public class LoginHistoryService : ILoginHistoryService
    {
        private readonly IUnitOfWork _unitOfWork;

        public LoginHistoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<LogInHistory> GetAllLoginHistory()
        {
            return _unitOfWork.LoginHistoryRepository.GetAll();
        } 
    }
}
