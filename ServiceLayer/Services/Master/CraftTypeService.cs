using Domain.Interfaces;
using IdylAPI.Models.Master;
using IdylAPI.Services.Interfaces.Syst;
using System.Collections.Generic;

namespace IdylAPI.Services.Master
{
    public class CraftTypeService : ICraftTypeService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CraftTypeService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        IEnumerable<CraftType> ICraftTypeService.GetByCompany(int companyNo)
        {
            return _unitOfWork.CraftTypeRepository.GetByCompany(companyNo);
        }
    }
}
