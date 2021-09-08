using Domain.Entities.PM;
using Domain.Interfaces;
using IdylAPI.Models;
using IdylAPI.Services.Interfaces.Syst;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SocialMedia.Core.Services
{
    public class FreqUnitService : IFreqUnitService
    {
        private readonly IUnitOfWork _unitOfWork;

        public FreqUnitService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<FreqUnit> GetAll()
        {
            return _unitOfWork.FreqUnitRepository.GetAll();
        }
    }
}
